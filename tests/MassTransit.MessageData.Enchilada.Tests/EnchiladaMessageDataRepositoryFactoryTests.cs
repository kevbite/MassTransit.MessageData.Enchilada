using System;
using Enchilada.Infrastructure.Interface;
using Moq;
using NUnit.Framework;

namespace MassTransit.MessageData.Enchilada.Tests
{
    [TestFixture]
    public class EnchiladaMessageDataRepositoryFactoryTests
    {
        [Test]
        public void ShouldReturnEnchiladaMessageDataRepository()
        {
            var factory = new EnchiladaMessageDataRepositoryFactory();

            var repository = factory.Create(Mock.Of<IEnchiladaFilesystemResolver>(), new Uri("wat:foo"));

            Assert.That(repository, Is.TypeOf<EnchiladaMessageDataRepository>());
        }
    }
}
