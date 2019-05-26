using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace TestNinja.Mocking {
    public interface IFileReader {
        string Read(string filePath);
    }

    public class FileReader : IFileReader {
        public string Read(string filePath) {
            return File.ReadAllText(filePath);
        }
    }

    public class VideoService {
        private readonly IFileReader _fileReader;
        private IVideoRepository _videoRepository;

        public VideoService(IFileReader fileReader = null, IVideoRepository videoRepository = null) {
            _fileReader = fileReader ?? new FileReader();
            _videoRepository = videoRepository ?? new VideoRepository();
        }

        public string ReadVideoTitle() {
            var str = _fileReader.Read("video.txt");
            var video = JsonConvert.DeserializeObject<Video>(str);

            if (video == null)
                return "Error parsing the video.";

            return video.Title;
        }

        public string GetUnprocessedVideosAsCsv() {
            var videoIds = new List<int>();

            var videos = _videoRepository.GetUnprocessedVideos();

            foreach (var v in videos) {
                videoIds.Add(v.Id);
            }

            return string.Join(",", videoIds);
        }
    }

    public interface IVideoRepository {
        IEnumerable<Video> GetUnprocessedVideos();
    }

    class VideoRepository : IVideoRepository {
        public IEnumerable<Video> GetUnprocessedVideos() {
            using (var context = new VideoContext()) {
                var videos =
                    (from video in context.Videos
                     where !video.IsProcessed
                     select video).ToList();

                return videos;
            }
        }
    }

    public class Video {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsProcessed { get; set; }
    }

    public class VideoContext : DbContext {
        public DbSet<Video> Videos { get; set; }
    }
}