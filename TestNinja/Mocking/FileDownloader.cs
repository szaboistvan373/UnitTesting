using System.Net;

namespace TestNinja.Mocking {
    public interface IFileDownloader {
        void DownloadFile(string url, string savePath);
    }

    public class FileDownloader : IFileDownloader {
        public void DownloadFile(string url, string savePath) {
            var client = new WebClient();
            client.DownloadFile(url, savePath);
        }
    }
}