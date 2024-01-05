using Aster.Entity;
//using Aster.Entity.EDU;
using AsterWebApp.Models;
using AsterWebApp.Models.FileBrowser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using FileResult = AsterWebApp.Models.FileResult;

namespace AsterWebApp.Controllers
{
    public class MediaFileController : Controller
    {

        private IWebHostEnvironment _hostingEnviroment;
        private readonly LinkGenerator _linkGenerator;
        private readonly ReportContext _context;

        private IConfiguration Configuration;

        public MediaFileController(IWebHostEnvironment environment, LinkGenerator linkGenerator, ReportContext context, IConfiguration configuration)
        {
            _hostingEnviroment = environment;
            _linkGenerator = linkGenerator;
            _context = context;
            Configuration = configuration;
        }

        // GET: MediaFileController
        [HttpGet]
        public async Task<IActionResult> StudentInfo()
        {
            var allData = await _context.StageStudentInfos.ToListAsync();        
            

            return Json(allData);
        }
       

        public ActionResult Index()
        {
            ViewBag.Schools = new SelectList(_context.SchoolMasters.Select(x => new { x.SchoolId, x.Name }).ToList(), "SchoolId", "Name");

            ViewBag.MediaFolders = new SelectList(_context.MediaFolders.Select(x => new { x.Id, x.Name }).ToList(), "Id", "Name");


            //var uploadedFiles = _context.MediaFiles.ToList();

            //return View(uploadedFiles);
            return View();


        }

        [HttpPost]
        public IActionResult Submit(IEnumerable<IFormFile> files)
        {
            if (files != null)
            {
                TempData["UploadedFiles"] = GetFileInfo(files).ToList();
            }

            return RedirectToRoute("demo", new { section = "upload", example = "result" });
        }

        public ActionResult Save(IEnumerable<IFormFile> files, FileUploadRequest uploadEntity)
        {
            // The Name of the Upload component is "files"
            string rootPath = String.Empty;
            string iemisPath = String.Empty;
            long schoolId = 0;
            long.TryParse(uploadEntity.EntityId, out schoolId);

            if (files != null)
            {
                rootPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, MediaHelper.ROOT));
                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }
                if (uploadEntity.Entity != null)
                {
                    if (uploadEntity.Entity == "IEMISFILE")
                    {
                        iemisPath = Path.Combine(rootPath, MediaHelper.IEMIS_FILE);
                        if (!Directory.Exists(iemisPath))
                        {
                            Directory.CreateDirectory(iemisPath);
                        }
                    }
                }
                Guid folderGuid = Guid.NewGuid();
                var mediaFolder = _context.MediaFolders.FirstOrDefault(x => x.Name == uploadEntity.Entity);
                var mediaFiles = new List<MediaFile>();
                if (mediaFolder == null)
                {
                    var dbMediaFolder = new MediaFolder
                    {
                        Id = folderGuid,
                        Name = uploadEntity.Entity,
                        Created = DateTime.Now,
                        Description = uploadEntity.Entity,

                    };


                    dbMediaFolder.SchoolId = schoolId;

                    _context.MediaFolders.Add(dbMediaFolder);
                    _context.SaveChanges();
                }
                else
                {
                    folderGuid = mediaFolder.Id;
                }

                foreach (IFormFile file in files)
                {

                    if (file.Length > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string filePath = Path.Combine(iemisPath, fileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        var mediaFileGuid = Guid.NewGuid();

                        var mediaFile = new MediaFile
                        {
                            Id = mediaFileGuid,
                            EntityName = uploadEntity.Entity,
                            AltText = file.Name,
                            Title = file.Name,
                            Filename = fileName,
                            VirtualPath = filePath,
                            ContentType = file.ContentType,
                            SchoolId = schoolId,
                            Size = file.Length,
                            Type = 3,
                            FolderId = folderGuid,
                            RecordStatus = 1,
                            Description = uploadEntity.Entity,
                            DisplayOrder = 9,
                            Created = DateTime.MaxValue
                            

                        };
                        _context.MediaFiles.Add(mediaFile);
                        _context.SaveChanges();
                        mediaFiles.Add(mediaFile);

                        //Read the connection string for the Excel file.
                        string conString = Configuration.GetConnectionString("ExcelConnectionString");

                        if (string.IsNullOrWhiteSpace(conString))
                        {
                            conString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=YES'";
                        }

                        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                        {
                            DataTable dt = new DataTable();
                            conString = string.Format(conString, filePath);

                            OleDbConnection excelConnection = new OleDbConnection(conString);
                            excelConnection.Open();

                            DataTable sheetTable = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            List<SheetViewModel> sheetNames = new List<SheetViewModel>();
                            foreach (DataRow row in sheetTable.Rows)
                            {
                                string sheetName = row["TABLE_NAME"].ToString();
                                sheetName = sheetName.Replace("\\", "/");
                                sheetName = sheetName.Replace(" ", "");
                                sheetName = sheetName.Replace("$", "");
                                sheetName = sheetName.Replace("'", "");
                                sheetName = sheetName.Replace("@", "");
                                sheetName = sheetName.Replace("(", "");
                                sheetName = sheetName.Replace(")", "");

                                SheetViewModel model = new SheetViewModel
                                {
                                    OriginalSheetName = row["TABLE_NAME"].ToString(),
                                    TableSheetName = "Stage_" + sheetName
                                };
                                sheetNames.Add(model);

                            }

                            string connectionString = Configuration.GetConnectionString("DefaultConnection");

                            SqlConnection sqlConnection = new SqlConnection(connectionString);
                            sqlConnection.Open();

                            foreach (var item in sheetNames)
                            {
                                // Create an OleDbDataAdapter to read the data from the current sheet
                                OleDbDataAdapter dataAdapter = new($"SELECT * FROM [{item.OriginalSheetName}]", excelConnection);

                                // Create a DataTable to hold the data from the current sheet
                                DataTable innersheetTable = new DataTable();
                                dataAdapter.Fill(innersheetTable);

                                // Check if the destination table exists
                                bool tableExists;
                                using (SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{item.TableSheetName}'", sqlConnection))
                                {
                                    tableExists = (int)command.ExecuteScalar() > 0;
                                }

                                // If the destination table doesn't exist, create it
                                if (!tableExists)
                                {

                                    //string sheetName = "Sheet1";
                                    DataTable schemaTable = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, item.OriginalSheetName, null });


                                    // Retrieve the schema of the source DataTable to create the destination table
                                    //DataTable sourceTable = innersheetTable; // Assuming the first table in the DataSet

                                    StringBuilder sqlBuilder = new StringBuilder();
                                    sqlBuilder.AppendLine("CREATE TABLE " + item.TableSheetName + " (");

                                    //string outerColumnName = "[SchoolID]";
                                    //string outerDataType = "NVARCHAR(500)";
                                    //sqlBuilder.AppendLine($"{outerColumnName} {outerDataType},");

                                    foreach (DataRow row in schemaTable.Rows)
                                    {
                                        string columnName = "[" + row["COLUMN_NAME"].ToString() + "]";
                                        string dataType = "NVARCHAR(MAX)";
                                        sqlBuilder.AppendLine($"{columnName} {dataType},");
                                    }

                                    // Remove the trailing comma and close the CREATE TABLE statement
                                    sqlBuilder.Length -= 3;
                                    sqlBuilder.AppendLine(");");

                                    string createTableScript = sqlBuilder.ToString();

                                    SqlCommand command = new SqlCommand(createTableScript, sqlConnection);
                                    command.ExecuteNonQuery();

                                }

                                // Create a DataTable to hold the data from the current sheet
                                DataTable dataTable = new DataTable();
                                dataAdapter.Fill(dataTable);

                                // Create a SqlBulkCopy object and set the destination table name
                                SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection);
                                bulkCopy.DestinationTableName = item.TableSheetName;

                                // Define the column mappings based on the column names in the DataTable
                                foreach (DataColumn column in dataTable.Columns)
                                {
                                    string colName = column.ColumnName;
                                    bulkCopy.ColumnMappings.Add(colName, colName);
                                }
                                // Write the data from the DataTable to the SQL Server database
                                bulkCopy.WriteToServer(dataTable);
                            }
                            excelConnection.Close();
                            sqlConnection.Close();
                        }
                    }
                }
            }
            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = _hostingEnviroment.WebRootPath + "/App_Data" + fileName;

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        // System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public void AppendToFile(string fullPath, Stream content)
        {
            try
            {
                using (FileStream stream = new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (content)
                    {
                        content.CopyTo(stream);
                    }
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        public ActionResult ChunkSave(IEnumerable<IFormFile> files, string metaData, FileUploadRequest uploadEntity)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ChunkMetaData));

            if (serializer is null)
            {
                throw new Exception("Not Found!");
            }

            if (metaData == null)
            {
                return Save(files, uploadEntity);
            }

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(metaData));
            ChunkMetaData? somemetaData = serializer.ReadObject(ms) as ChunkMetaData;
            string path = String.Empty;

            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (IFormFile file in files)
                {
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, MediaHelper.ROOT));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    if (uploadEntity.Entity != null)
                    {
                        if (uploadEntity.Entity == "IEMISFILE")
                        {
                            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, MediaHelper.IEMIS_FILE));
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                        }
                    }

                    var fileName = Path.GetFileName(somemetaData.FileName);
                    var physicalPath = Path.Combine(path, fileName);
                    AppendToFile(physicalPath, file.OpenReadStream());

                }
            }

            if (somemetaData is null)
            {
                throw new Exception("No Metadata!");
            }

            FileResult fileBlob = new FileResult();
            fileBlob.uploaded = somemetaData.TotalChunks - 1 <= somemetaData.ChunkIndex;
            fileBlob.fileUid = somemetaData.UploadUid;

            if (fileBlob.uploaded)
            {
                Guid folderGuid = Guid.NewGuid();
                var mediaFolder = _context.MediaFolders.FirstOrDefault(x => x.Name == uploadEntity.Entity);

                var mediaFiles = new List<Aster.Entity.MediaFile>();

                if (mediaFolder == null)
                {
                    var dbMediaFolder = new MediaFolder { Id = folderGuid, Name = uploadEntity.Entity, DisplayOrder=1,Created = DateTime.Now, Description = uploadEntity.Entity, SchoolId = 9, VirtualPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, MediaHelper.ROOT)), ParentId = folderGuid, };
                    _context.MediaFolders.Add(dbMediaFolder);
                    _context.SaveChanges();
                }
                else
                {
                    folderGuid = mediaFolder.Id;
                }

                var mediaFileGuid = Guid.NewGuid();

                var mediaFile = new MediaFile
                {
                    Id = mediaFileGuid,
                    EntityName = uploadEntity.Entity,
                    EntityId = uploadEntity.EntityId,
                    // AltText = schoolCourseDetails.Title,
                    //Title = schoolCourseDetails.Title,
                    Filename = somemetaData.FileName,
                    VirtualPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, MediaHelper.ROOT)),
                    ContentType = somemetaData.ContentType,
                    SchoolId = 9,
                    Size = somemetaData.TotalFileSize,
                    Type = 3,
                    FolderId = folderGuid
                };

                mediaFile.Filename = somemetaData.FileName;
                mediaFile.ContentType = somemetaData.ContentType;
                mediaFile.Size = somemetaData.TotalFileSize;
                mediaFile.Description = somemetaData.UploadUid;
                _context.MediaFiles.Add(mediaFile);
                _context.SaveChanges();
                mediaFiles.Add(mediaFile);

            }

            return Json(fileBlob);
        }

        private IEnumerable<string> GetFileInfo(IEnumerable<IFormFile> files)
        {
            return
                from a in files
                where a != null
                select string.Format("{0} ({1} bytes)", Path.GetFileName(a.FileName), a.Length);
        }

        public async Task<IActionResult> DownloadFileFromFileSystem(Guid id)
        {

            var file = await _context.MediaFiles.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            byte[] imageBytes = System.IO.File.ReadAllBytes(file.VirtualPath);
            return File(imageBytes, file.ContentType);
        }
        // Add a method to your MediaFileController to get data from Excel


        [HttpPost]
        public async Task<IActionResult> DeleteFileFromFileSystem([FromBody] Guid id)
        {
            var file = await _context.MediaFiles.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file != null)
            {
                _context.MediaFiles.Remove(file);
                _context.SaveChanges();
                return Json("OK");
            }
            else
            {
                return Json("File not found");
            }
        }
    }
}
