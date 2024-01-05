using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AsterWebApp.Models.FileBrowser
{
    public class DirectoryBrowser
    {
        private IWebHostEnvironment _hostingEnviroment;
        public IWebHostEnvironment HostingEnviroment { get { return _hostingEnviroment; } }
        public DirectoryBrowser(IWebHostEnvironment environment)
        {
            _hostingEnviroment = environment;
        }
        public IEnumerable<FileBrowserEntry> GetContent(string path, string filter)
        {
            return GetFiles(path, filter).Concat(GetDirectories(path));
        }

        private IEnumerable<FileBrowserEntry> GetFiles(string path, string filter)
        {
            //var directory = new DirectoryInfo(Path.Combine(_hostingEnviroment.WebRootPath, path));
            var directory = new DirectoryInfo(path);

            var extensions = (filter ?? "*").Split(",|;".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

            var lst =
             extensions.SelectMany(directory.GetFiles)
                .Select(file => new FileBrowserEntry
                {
                    Name = file.Name,
                    Size = file.Length,
                    Type = EntryType.File
                });

            var tyoes = lst.ToList();
            return lst;
        }

        private IEnumerable<FileBrowserEntry> GetDirectories(string path)
        {
            var directory = new DirectoryInfo(Path.Combine(_hostingEnviroment.WebRootPath, path));

            return directory.GetDirectories()
                .Select(subDirectory => new FileBrowserEntry
                {
                    Name = subDirectory.Name,
                    Type = EntryType.Directory
                });
        }
    }
}
