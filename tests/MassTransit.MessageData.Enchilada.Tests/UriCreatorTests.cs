using System;
using NUnit.Framework;

namespace MassTransit.MessageData.Enchilada.Tests
{
    [TestFixture]
    public class UriCreatorTests
    {
        [Test]
        public void ShouldReturnCorrectUri()
        {
            var baseUri = new Uri("enchilada://blob_storage");

            var uri = new UriCreator(baseUri).Create("image.jpg");

            Assert.That(uri, Is.EqualTo(new Uri("enchilada://blob_storage/image.jpg")));
        }
    }
}
