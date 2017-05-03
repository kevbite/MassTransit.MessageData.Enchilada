using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NUnit.Framework;

namespace MassTransit.MessageData.Enchilada.Tests
{
    [TestFixture]
    public class GuidFileNameCreatorTests
    {
        private GuidFileNameCreator _fileNameCreator;

        [SetUp]
        public void SetUp()
        {
            _fileNameCreator = new GuidFileNameCreator();
        }

        [Test]
        public void ShouldReturnUniqueFileNames()
        {
            var count = 10;
            var fileNames = new List<string>();

            for (int i = 0; i < count; i++)
            {
                var fileName = _fileNameCreator.Create();
                fileNames.Add(fileName);
            }

            Assert.That(fileNames.Distinct().Count(), Is.EqualTo(count));
        }
    }
}
