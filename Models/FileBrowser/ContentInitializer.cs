namespace AsterWebApp.Models.FileBrowser
{
    public class ContentInitializer
    {
        private string rootFolder;
        private string[] foldersToCopy;
        private string prettyName;

        private IWebHostEnvironment _hostingEnviroment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ContentInitializer(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnviroment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public ContentInitializer(string rootFolder, string[] foldersToCopy, string prettyName)
        {
            this.rootFolder = rootFolder;
            this.foldersToCopy = foldersToCopy;
            this.prettyName = prettyName;
        }

        private string UserID
        {
            get
            {
                var obj = "1";
                //_httpContextAccessor.HttpContext.Session["UserID"];
                //HttpContext.Current.Session["UserID"];
                if (obj == null)
                {
                    //HttpContext.Current.Session["UserID"] = obj = DateTime.Now.Ticks.ToString();
                }
                return (string)obj;
            }
        }

        public string CreateUserFolder(IWebHostEnvironment server)
        {
            //var virtualPath = Path.Combine(rootFolder, Path.Combine("UserFiles", UserID), prettyName);

            var virtualPath = Path.Combine(server.WebRootPath, rootFolder);

            var path = Path.Combine(server.WebRootPath, rootFolder);
            //server.MapPath(virtualPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                foreach (var sourceFolder in foldersToCopy)
                {
                    CopyFolder(Path.Combine(server.WebRootPath, sourceFolder), path);
                    //CopyFolder(server.MapPath(sourceFolder), path);
                }
            }
            return virtualPath;
        }

        private void CopyFolder(string source, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            foreach (var file in Directory.EnumerateFiles(source))
            {
                var dest = Path.Combine(destination, Path.GetFileName(file));
                System.IO.File.Copy(file, dest);
            }

            foreach (var folder in Directory.EnumerateDirectories(source))
            {
                var dest = Path.Combine(destination, Path.GetFileName(folder));
                CopyFolder(folder, dest);
            }
        }
    }
}
