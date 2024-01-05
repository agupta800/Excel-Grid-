using AsterWebApp.Models.FileBrowser;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AsterWebApp.Controllers
{
    public class FileBrowserController : Controller
    {
        private const string contentFolderRoot = "brokers\\sunsecurities\\editor";
        private const string prettyName = "Images";
        private static readonly string[] foldersToCopy = new[] { "~/wwwroot/brokers/sunsecurities/editor" };
        private const string DefaultFilter = "*.txt,*.doc,*.docx,*.xls,*.xlsx,*.ppt,*.pptx,*.zip,*.rar,*.jpg,*.jpeg,*.gif,*.png";

        private readonly DirectoryBrowser directoryBrowser;
        private readonly ContentInitializer contentInitializer;
        private IWebHostEnvironment _hostingEnviroment;

        public FileBrowserController(IWebHostEnvironment hostingEnviroment)
        {
            directoryBrowser = new DirectoryBrowser(hostingEnviroment);
            contentInitializer = new ContentInitializer(contentFolderRoot, foldersToCopy, prettyName);
            _hostingEnviroment = hostingEnviroment;
        }

        public string ContentPath
        {
            get
            {
                return contentInitializer.CreateUserFolder(_hostingEnviroment);
            }
        }

        private string ToAbsolute(string virtualPath)
        {
            return virtualPath;
            // return VirtualPathUtility.ToAbsolute(virtualPath);;
        }

        private string CombinePaths(string basePath, string relativePath)
        {
            return "VirtualPathUtility.Combine(VirtualPathUtility.AppendTrailingSlash(basePath), relativePath);";
        }

        public virtual bool AuthorizeRead(string path)
        {
            return CanAccess(path);
        }

        protected virtual bool CanAccess(string path)
        {
            return path.StartsWith(ToAbsolute(ContentPath), StringComparison.OrdinalIgnoreCase);
        }

        private string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return ToAbsolute(ContentPath);
            }

            return CombinePaths(ToAbsolute(ContentPath), path);
        }

        public virtual JsonResult Read(string path)
        {
            path = NormalizePath(path);

            if (AuthorizeRead(path))
            {
                try
                {
                    //directoryBrowser.HostingEnviroment = _hostingEnviroment;

                    var result = directoryBrowser
                        .GetContent(path, DefaultFilter)
                        .Select(f => new
                        {
                            name = f.Name,
                            type = f.Type == EntryType.File ? "f" : "d",
                            size = f.Size
                        });

                    return Json(result);
                }
                catch (DirectoryNotFoundException)
                {
                    throw new Exception();
                }
            }

            throw new BadHttpRequestException("Forbidden", 403);
        }

        [HttpPost]
        public virtual ActionResult Destroy(string path, string name, string type)
        {
            path = NormalizePath(path);

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(type))
            {
                path = CombinePaths(path, name);
                if (type.ToLowerInvariant() == "f")
                {
                    DeleteFile(path);
                }
                else
                {
                    DeleteDirectory(path);
                }

                return Json(new object[0]);
            }
            throw new BadHttpRequestException("Forbidden", 403);
        }

        public virtual bool AuthorizeDeleteFile(string path)
        {
            return CanAccess(path);
        }

        public virtual bool AuthorizeDeleteDirectory(string path)
        {
            return CanAccess(path);
        }

        protected virtual void DeleteFile(string path)
        {
            if (!AuthorizeDeleteFile(path))
            {
                throw new BadHttpRequestException("Forbidden", 403);
            }

            var physicalPath = Path.Combine(_hostingEnviroment.WebRootPath, path);
            //Server.MapPath(path);

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }
        }

        protected virtual void DeleteDirectory(string path)
        {
            if (!AuthorizeDeleteDirectory(path))
            {
                throw new BadHttpRequestException("Forbidden", 403);
            }

            var physicalPath = Path.Combine(_hostingEnviroment.WebRootPath, path);
            //Server.MapPath(path);

            if (Directory.Exists(physicalPath))
            {
                Directory.Delete(physicalPath, true);
            }
        }

        public virtual bool AuthorizeCreateDirectory(string path, string name)
        {
            return CanAccess(path);
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Create(string path, FileBrowserEntry entry)
        {
            path = NormalizePath(path);
            var name = entry.Name;

            if (!string.IsNullOrEmpty(name) && AuthorizeCreateDirectory(path, name))
            {
                var physicalPath = Path.Combine(_hostingEnviroment.WebRootPath, name);

                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }

                return Json(new
                {
                    name = entry.Name,
                    type = "d",
                    size = entry.Size
                });
            }

            throw new BadHttpRequestException("Forbidden", 403);
        }


        public virtual bool AuthorizeUpload(string path, IFormFile file)
        {
            return CanAccess(path) && IsValidFile(file.FileName);
        }

        private bool IsValidFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var allowedExtensions = DefaultFilter.Split(',');

            return allowedExtensions.Any(e => e.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase));
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public virtual ActionResult Upload(string path, IFormFile file)
        {
            path = NormalizePath(path);
            var fileName = Path.GetFileName(file.FileName);

            if (AuthorizeUpload(path, file))
            {

                //file.SaveAs(Path.Combine(_hostingEnviroment.WebRootPath, fileName));
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Json(new
                {
                    size = file.Length,
                    name = fileName,
                    type = "f"
                }, "text/plain");
            }

            throw new BadHttpRequestException("Forbidden", 403);
        }

        //[OutputCache(Duration = 360, VaryByParam = "path")]
        public ActionResult File(string fileName)
        {
            var path = NormalizePath(fileName);

            if (AuthorizeFile(path))
            {
                var physicalPath = Path.Combine(_hostingEnviroment.WebRootPath, path);

                if (System.IO.File.Exists(physicalPath))
                {
                    const string contentType = "application/octet-stream";
                    return File(System.IO.File.OpenRead(physicalPath), contentType, fileName);
                }
            }

            throw new BadHttpRequestException("Forbidden", 403);
        }

        public virtual bool AuthorizeFile(string path)
        {
            return CanAccess(path) && IsValidFile(Path.GetExtension(path));
        }
    }
}
