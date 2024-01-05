using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AsterWebApp.Models
{
    public class FileDbContext : DbContext
    {
        public FileDbContext(DbContextOptions<FileDbContext> options) : base(options)
        {
        }

        public DbSet<ChunkMetaData> Chunks { get; set; }
        public DbSet<FileResult> Files { get; set; }
        public DbSet<FileUploadRequest> FileUploads { get; set; }
       
      
    }
}
