using System.Collections.Generic;
using System.IO;
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

        [Test]
        public void ShouldHaveFileExtention()
        {
            var fileName = _fileNameCreator.Create();

            Assert.That(Path.HasExtension(fileName), Is.True);
        }
    }
}
