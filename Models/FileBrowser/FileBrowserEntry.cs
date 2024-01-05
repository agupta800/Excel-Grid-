using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AsterWebApp.Models.FileBrowser
{
    public enum EntryType
    {
        File = 0,
        Directory
    }
    public class ImageSize
    {
        public int Height
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }
    }
    public class FileBrowserEntry
    {
        public string Name { get; set; }
        public EntryType Type { get; set; }
        public long Size { get; set; }
    }
}
