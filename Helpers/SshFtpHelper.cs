//using Renci.SshNet;

//namespace AsterWebApp.Helpers
//{
//    public class SshFtpHelper
//    {

//        static void Main()
//        {
//            // Replace these values with your own FTP server details
//            string host = "ftp.example.com";
//            string username = "your-username";
//            string password = "your-password";
//            string remoteDirectory = "/remote/directory";
//            string localFilePath = "local-file.txt";
//            string remoteFilePath = "/remote/directory/remote-file.txt";

//            var ftpHelper = new SshFtpHelper(host, username, password);

//            // List directory
//            Console.WriteLine($"Files in {remoteDirectory}:");
//            foreach (var file in ftpHelper.ListDirectory(remoteDirectory))
//            {
//                Console.WriteLine(file);
//            }

//            // Download file
//            ftpHelper.DownloadFile(remoteFilePath, localFilePath);
//            Console.WriteLine($"File downloaded to {localFilePath}");

//            // Upload file
//            ftpHelper.UploadFile(localFilePath, remoteFilePath);
//            Console.WriteLine($"File uploaded to {remoteFilePath}");
//        }

//        private readonly string host;
//        private readonly string username;
//        private readonly string password;

//        public SshFtpHelper(string host, string username, string password)
//        {
//            this.host = host;
//            this.username = username;
//            this.password = password;
//        }

//        public IEnumerable<string> ListDirectory(string remotePath)
//        {
//            using (var client = new SftpClient(host, username, password))
//            {
//                client.Connect();
//                var files = client.ListDirectory(remotePath);

//                foreach (var file in files)
//                {
//                    if (!file.IsDirectory && !file.IsSymbolicLink)
//                    {
//                        yield return file.Name;
//                    }
//                }

//                client.Disconnect();
//            }
//        }

//        public void DownloadFile(string remotePath, string localPath)
//        {
//            using (var client = new SftpClient(host, username, password))
//            {
//                client.Connect();
//                using (var fileStream = File.OpenWrite(localPath))
//                {
//                    client.DownloadFile(remotePath, fileStream);
//                }
//                client.Disconnect();
//            }
//        }

//        public void UploadFile(string localPath, string remotePath)
//        {
//            using (var client = new SftpClient(host, username, password))
//            {
//                client.Connect();
//                using (var fileStream = File.OpenRead(localPath))
//                {
//                    client.UploadFile(fileStream, remotePath);
//                }
//                client.Disconnect();
//            }
//        }
//    }
//}
