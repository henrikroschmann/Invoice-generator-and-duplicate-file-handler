using System;

namespace DuplicateFilesFinder.Models
{
    internal class FilesInFolder
    {
        public string FileName { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public string FullPath { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
    }
}
