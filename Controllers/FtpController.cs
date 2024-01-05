//using Microsoft.AspNetCore.Mvc;
//using Renci.SshNet;

//namespace AsterWebApp.Controllers
//{
//    public class FtpController : Controller
//    {
//        private const string FtpHost = "ftp.example.com";
//        private const string FtpUsername = "your-ftp-username";
//        private const string FtpPassword = "your-ftp-password";
//        private const int FtpPort = 22; // SFTP port

//        [HttpGet("list")]
//        public IActionResult ListFtpDirectory()
//        {
//            using (var client = new SftpClient(FtpHost, FtpPort, FtpUsername, FtpPassword))
//            {
//                client.Connect();

//                var directoryItems = client.ListDirectory("/");
//                var fileList = directoryItems.Where(item => item.IsRegularFile).Select(item => item.Name);
//                var directoryList = directoryItems.Where(item => item.IsDirectory).Select(item => item.Name);

//                client.Disconnect();

//                return Ok(new { Files = fileList, Directories = directoryList });
//            }
//        }

//        [HttpPost("upload")]
//        public IActionResult UploadFile(IFormFile file)
//        {
//            if (file == null || file.Length <= 0)
//            {
//                return BadRequest("Invalid file");
//            }

//            using (var client = new SftpClient(FtpHost, FtpPort, FtpUsername, FtpPassword))
//            {
//                client.Connect();

//                using (var fileStream = file.OpenReadStream())
//                {
//                    client.UploadFile(fileStream, $"/{file.FileName}");
//                }

//                client.Disconnect();
//            }

//            return Ok("File uploaded successfully");
//        }

//        [HttpGet("download/{fileName}")]
//        public IActionResult DownloadFile(string fileName)
//        {
//            using (var client = new SftpClient(FtpHost, FtpPort, FtpUsername, FtpPassword))
//            {
//                client.Connect();

//                using (var stream = new MemoryStream())
//                {
//                    client.DownloadFile($"/{fileName}", stream);
//                    stream.Position = 0;

//                    return File(stream, "application/octet-stream", fileName);
//                }
//            }
//        }
//    }
//}
