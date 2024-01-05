//using System;
//using System.IO;
//using System.Net;
//namespace AsterWebApp.Helpers
//{
//    public class FtpHelper
//    {
//        private readonly string ftpHost;
//        private readonly string ftpUsername;
//        private readonly string ftpPassword;

//        public FtpHelper(string host, string username, string password)
//        {
//            ftpHost = host;
//            ftpUsername = username;
//            ftpPassword = password;
//        }

//        public string[] ListDirectories(string path = "/")
//        {
//            return GetDirectoryListing(path, WebRequestMethods.Ftp.ListDirectory);
//        }

//        public string[] ListFiles(string path = "/")
//        {
//            return GetDirectoryListing(path, WebRequestMethods.Ftp.ListDirectoryDetails);
//        }

//        public void UploadFile(string localFilePath, string remotePath)
//        {
//            using (var client = GetFtpWebRequest(remotePath, WebRequestMethods.Ftp.UploadFile) as FtpWebRequest)
//            {
//                using (var fileStream = File.OpenRead(localFilePath))
//                using (var ftpStream = client.GetRequestStream())
//                {
//                    fileStream.CopyTo(ftpStream);
//                }
//            }
//        }

//        public void DownloadFile(string remoteFilePath, string localPath)
//        {
//            using (var client = GetFtpWebRequest(remoteFilePath, WebRequestMethods.Ftp.DownloadFile) as FtpWebRequest)
//            {
//                using (var response = client.GetResponse() as FtpWebResponse)
//                using (var ftpStream = response.GetResponseStream())
//                using (var fileStream = File.Create(localPath))
//                {
//                    ftpStream.CopyTo(fileStream);
//                }
//            }
//        }

//        private string[] GetDirectoryListing(string path, string method)
//        {
//            using (var client = GetFtpWebRequest(path, method) as FtpWebRequest)
//            {
//                using (var response = client.GetResponse() as FtpWebResponse)
//                using (var reader = new StreamReader(response.GetResponseStream()))
//                {
//                    return reader.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
//                }
//            }
//        }

//        private WebRequest GetFtpWebRequest(string path, string method)
//        {
//            var uri = new Uri($"ftp://{ftpHost}/{path}");
//            var request = WebRequest.Create(uri) as FtpWebRequest;

//            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
//            request.Method = method;

//            return request;
//        }
//    }

//}
