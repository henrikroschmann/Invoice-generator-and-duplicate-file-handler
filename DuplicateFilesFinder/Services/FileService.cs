using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DuplicateFilesFinder.Models;

namespace DuplicateFilesFinder.Services
{
    internal static class FileService
    {
        /// <summary>
        /// Open folder browser and add files to list of FilsInFolders
        /// </summary>
        /// <returns></returns>
        internal static List<FilesInFolder> OpenFile()
        {
            var folder = "";
            var openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                folder = openFileDlg.SelectedPath;
            }

            var data = new List<FilesInFolder>();
            try
            {
                var dicFileList = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);
                data.AddRange(dicFileList.Select(element => new FilesInFolder
                {
                    FileName = Path.GetFileNameWithoutExtension(element),
                    FullPath = Path.GetFullPath(element),
                    ModifiedDateTime = File.GetLastWriteTime(element), Size = new FileInfo(element).Length,
                    Type = Path.GetExtension(element)
                }));

                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}