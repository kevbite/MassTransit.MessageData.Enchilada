using System;
using Enchilada.Configuration;
using Enchilada.Infrastructure.Interface;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace MassTransit.MessageData.Enchilada.Tests
{
    [TestFixture]
    public class EnchiladaMessageDataRepositoryFactoryExtentionsTests
    {
        private Mock<IEnchiladaMessageDataRepositoryFactory> _factory;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _factory = new Mock<IEnchiladaMessageDataRepositoryFactory>();
        }

        [Test]
        public void ShouldHaveSingleAdapterNameAsBaseUri()
        {
            var adapterName = _fixture.Create("name");
            var adapter = new Mock<IEnchiladaAdapterConfiguration>();
            adapter.Setup(x => x.AdapterName)
                .Returns(adapterName);

            _factory.Object.Create(adapter.Object);

            _factory.Verify(x => x.Create(It.IsAny<IEnchiladaFilesystemResolver>(), new Uri($"enchilada://{adapterName}")));
        }

        [Test]
        public void ShouldReturnRepository()
        {
            var adapterName = _fixture.Create("name");
            var adapter = new Mock<IEnchiladaAdapterConfiguration>();
            adapter.Setup(x => x.AdapterName)
                .Returns(adapterName);

            var dataRepository = new EnchiladaMessageDataRepository(null, null, null);
            _factory.Setup(x => x.Create(It.IsAny<IEnchiladaFilesystemResolver>(), new Uri($"enchilada://{adapterName}")))
                .Returns(dataRepository);

            var repository = _factory.Object.Create(adapter.Object);

            Assert.That(repository, Is.SameAs(dataRepository));
        }
    }
}
