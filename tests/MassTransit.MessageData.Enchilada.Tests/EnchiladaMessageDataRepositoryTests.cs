using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Enchilada.Infrastructure.Interface;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using FileMode = Enchilada.Infrastructure.FileMode;

namespace MassTransit.MessageData.Enchilada.Tests
{
    [TestFixture]
    public class EnchiladaMessageDataRepositoryTests
    {
        private Mock<IFileNameCreator> _uniqueNameCreator;
        private Fixture _fixture;
        private Mock<IUriCreator> _uriCreator;
        private Mock<IEnchiladaFilesystemResolver> _filesystemResolver;
        private EnchiladaMessageDataRepository _enchiladaMessageDataRepository;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _uniqueNameCreator = new Mock<IFileNameCreator>();
            _uriCreator = new Mock<IUriCreator>();
            _filesystemResolver = new Mock<IEnchiladaFilesystemResolver>();

            _enchiladaMessageDataRepository = new EnchiladaMessageDataRepository(_filesystemResolver.Object, _uniqueNameCreator.Object, _uriCreator.Object);
        }

        [Test]
        public async Task ShouldPassBackStreamFromEnchilada()
        {
            var name = _fixture.Create("name");
            _uniqueNameCreator.Setup(x => x.Create())
                .Returns(name);

            var uri = _fixture.Create<Uri>();
            _uriCreator.Setup(x => x.Create(name))
                .Returns(uri);

            var stream = Mock.Of<Stream>();
            var file = new Mock<IFile>();
            file.Setup(x => x.OpenReadAsync())
                .ReturnsAsync(stream);
            
            _filesystemResolver.Setup(x => x.OpenFileReference(uri.AbsoluteUri))
                .Returns(file.Object);

            var actualStream = await _enchiladaMessageDataRepository.Get(uri, CancellationToken.None);

            Assert.That(actualStream, Is.SameAs(stream));
        }

        [Test]
        public async Task ShouldCopyToEnchiladaStream()
        {
            var name = _fixture.Create("name");
            _uniqueNameCreator.Setup(x => x.Create())
                .Returns(name);

            var uri = _fixture.Create<Uri>();
            _uriCreator.Setup(x => x.Create(name))
                .Returns(uri);

            var stream = new MemoryStream();
            var file = new Mock<IFile>();
            file.Setup(x => x.OpenWriteAsync(FileMode.Overwrite))
                .ReturnsAsync(stream);

            _filesystemResolver.Setup(x => x.OpenFileReference(uri.AbsoluteUri))
                .Returns(file.Object);

            var bytes = _fixture.Create<byte[]>();

            var actualUri = await _enchiladaMessageDataRepository.Put(new MemoryStream(bytes), null, CancellationToken.None);

            Assert.That(actualUri, Is.EqualTo(uri));

            Assert.That(stream.ToArray(), Is.EqualTo(bytes));
        }
    }
}
