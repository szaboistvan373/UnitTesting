using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;
 
namespace TestNinja.UnitTests.Mocking {
    [TestFixture]
    class VideoServiceTests {
        private VideoService _videoService;
        private Mock<IFileReader> _fileReader;

        [SetUp]
        public void SetUp() {
            _fileReader = new Mock<IFileReader>();
            _videoService = new VideoService(_fileReader.Object);
        }

        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnErrorMessage() {
            _fileReader.Setup(fr => fr.Read("video.txt"))
                .Returns("");

            var result = _videoService.ReadVideoTitle();

            Assert.That(result, Does.Contain("error").IgnoreCase);
        }

        [Test]
        [TestCase("{title:'Avengers: Infinity War',id:12,isProcessed:true}", "Avengers: Infinity War")]
        [TestCase("{title:'Avengers: Endgame',id:631,isProcessed:false}", "Avengers: Endgame")]
        public void ReadVideoTitle_FileWitHJson_ReturnTitle(string videoJson, string title) {
            _fileReader.Setup(fr => fr.Read("video.txt")).Returns(videoJson);

            var result = _videoService.ReadVideoTitle();

            Assert.That(result, Is.EqualTo(title));
        }
    }
}
