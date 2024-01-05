//using Renci.SshNet.Common;
//using Renci.SshNet;
//using System.Net.Sockets;
//using Microsoft.Extensions.Hosting;
//using System.Net;

//namespace AsterWebApp.Helpers
//{
//    public interface IFtpProvider
//    {
//    }

//    public class FtpProvider
//    {
//        public void Connect()
//        {
//            string ftpServer = "ftp://ftp.sunsecurities.com";
//            string username = "websiteftp@sunsecurities.com";
//            string password = "yI58OCv7#]F5ti";

//            ListFtpDirectory(ftpServer, username, password);

//            //string host = "ftp.sunsecurities.com";
//            //string username = "websiteftp@sunsecurities.com";
//            //string password = "yI58OCv7#]F5ti";
//            //// Establish connection
//            //using (var client = new SftpClient(host, 21, username, password))
//            //{
//            //    client.Connect();

//            //    // List directories
//            //    var dirs = ListDirectories(client, "/");

//            //    // List files
//            //    var lstfiles = ListFiles(client, "/");

//            //    // Upload a file
//            //    string localFilePath = "local_file.txt";
//            //    string remoteFilePath = "/remote_directory/remote_file.txt";
//            //    UploadFile(client, localFilePath, remoteFilePath);

//            //    // Download a file
//            //    string downloadedFilePath = "downloaded_file.txt";
//            //    DownloadFile(client, remoteFilePath, downloadedFilePath);

//            //    client.Disconnect();
//            //}

//        }
//        static void ListFtpDirectory(string ftpServer, string username, string password)
//        {
//            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServer);
//            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
//            request.Credentials = new NetworkCredential(username, password);

//            try
//            {
//                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
//                {
//                    using (Stream responseStream = response.GetResponseStream())
//                    {
//                        if (responseStream != null)
//                        {
//                            using (StreamReader reader = new StreamReader(responseStream))
//                            {
//                                while (!reader.EndOfStream)
//                                {
//                                    string line = reader.ReadLine();
//                                    Console.WriteLine(line);
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            catch (WebException ex)
//            {
//                Console.WriteLine($"Error: {ex.Status} - {ex.Message}");
//            }
//        }

//        static List<string> ListDirectories(SftpClient client, string path)
//        {
//            var directories = client.ListDirectory(path)
//                .Where(entry => entry.IsDirectory)
//                .Select(entry => entry.Name);

//            List<string> lstDirs = new List<string>();

//            foreach (var directory in directories)
//            {
//                lstDirs.Add(directory);
//            }
//            return lstDirs;
//        }

//        static List<string> ListFiles(SftpClient client, string path)
//        {
//            var files = client.ListDirectory(path)
//                .Where(entry => entry.IsRegularFile)
//                .Select(entry => entry.Name);

//            //Console.WriteLine("Files:");
//            List<string> lstFiles = new List<string>();
//            foreach (var file in files)
//            {
//                lstFiles.Add(file);
//            }
//            return lstFiles;
//        }
//        static void UploadFile(SftpClient client, string localFilePath, string remoteFilePath)
//        {
//            using (var fileStream = new FileStream(localFilePath, FileMode.Open))
//            {
//                client.UploadFile(fileStream, remoteFilePath);
//            }

//            Console.WriteLine("File uploaded successfully.");
//        }

//        static void DownloadFile(SftpClient client, string remoteFilePath, string localFilePath)
//        {
//            using (var fileStream = File.Create(localFilePath))
//            {
//                client.DownloadFile(remoteFilePath, fileStream);
//            }

//            Console.WriteLine("File downloaded successfully.");
//        }

//    }
//}
