using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking {
    [TestFixture]
    class VideoServiceTests {
        private VideoService _videoService;
        private Mock<IFileReader> _fileReader;
        private Mock<IVideoRepository> _videoRepository;

        [SetUp]
        public void SetUp() {
            _fileReader = new Mock<IFileReader>();
            _videoRepository = new Mock<IVideoRepository>();

            _videoService = new VideoService(_fileReader.Object, _videoRepository.Object);
        }

        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnErrorMessage() {
            _fileReader.Setup(fr => fr.Read("video.txt"))
                .Returns("");

            var result = _videoService.ReadVideoTitle();

            Assert.That(result, Does.Contain("error").IgnoreCase);
        }

        [Test]
        [TestCase("{title:'Avengers: Infinity War',id:1,isProcessed:true}", "Avengers: Infinity War")]
        [TestCase("{title:'Avengers: Endgame',id:3,isProcessed:false}", "Avengers: Endgame")]
        public void ReadVideoTitle_FileWitHJson_ReturnTitle(string videoJson, string title) {
            _fileReader.Setup(fr => fr.Read("video.txt")).Returns(videoJson);

            var result = _videoService.ReadVideoTitle();

            Assert.That(result, Is.EqualTo(title));
        }

        [Test]
        public void GetUnprocessedVideosAsCsv_AllVideosAreProcessed_ReturnAnEmptyString() {
            _videoRepository.Setup(vr => vr.GetUnprocessedVideos()).Returns(new List<Video>());

            var result = _videoService.GetUnprocessedVideosAsCsv();

            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void GetUnprocessedVideosAsCsv_ManyUnprocessedVideos_ReturnVideoIdsAsString() {
            _videoRepository.Setup(vr => vr.GetUnprocessedVideos()).Returns(new List<Video>() {
                new Video { Id = 9 },
                new Video { Id = 5 },
                new Video { Id = 10 },
                new Video { Id = 25 },
            });

            var result = _videoService.GetUnprocessedVideosAsCsv();

            Assert.That(result, Is.EqualTo("9,5,10,25"));
        }
    }
}