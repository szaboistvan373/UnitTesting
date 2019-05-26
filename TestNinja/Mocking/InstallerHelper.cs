using System.Net;

namespace TestNinja.Mocking {
    public class InstallerHelper {
        private string _setupDestinationFile;

        public IFileDownloader FileDownloader { private get; set; }

        public InstallerHelper() {
            FileDownloader = new FileDownloader();
        }

        public bool DownloadInstaller(string customerName, string installerName) {
            try {
                FileDownloader.DownloadFile(string.Format("http://example.com/{0}/{1}", customerName, installerName), _setupDestinationFile);

                return true;
            }
            catch (WebException) {
                return false;
            }
        }
    }
}