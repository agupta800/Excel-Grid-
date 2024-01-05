using System;
using System.Collections.Generic;
using FluentFTP;

namespace AsterWebApp.Helpers
{

    namespace FtpWrapper
    {
        public class FtpClientWrapper
        {
            private readonly FtpClient _ftpClient;

            public FtpClientWrapper(string host, string username, string password)
            {
                _ftpClient = new FtpClient(host, new System.Net.NetworkCredential(username, password));
            }

            public void Connect()
            {
                if (!_ftpClient.IsConnected)
                {
                    _ftpClient.Connect();
                }
            }

            public void Disconnect()
            {
                if (_ftpClient.IsConnected)
                {
                    _ftpClient.Disconnect();
                }
            }

            public List<FtpListItem> GetDirectoryListing(string remotePath = "")
            {
                Connect();
                return _ftpClient.GetListing(remotePath).ToList();
            }

            public void UploadFile(string localPath, string remotePath)
            {
                Connect();
                _ftpClient.UploadFile(localPath, remotePath, FtpRemoteExists.Overwrite, false, FtpVerify.None);
            }

            public void DownloadFile(string remotePath, string localPath)
            {
                Connect();
                _ftpClient.DownloadFile(localPath, remotePath, FtpLocalExists.Overwrite, FtpVerify.None);
            }
        }
    }

}
