using System.Collections.Generic;
using Epam_FinalProject_FileManager_BLL.DTO;
using System.Web.Mvc;

namespace Epam_FinalProject_FileManager.Models
{
    public class PublicFileViewModel
    {
        public IEnumerable<FileEntityDTO> Files { get; set; }
        public int PageSize { get; set; }
    }
    public class FileViewModel
    {
        public IEnumerable<FileEntityDTO> UserFiles;
        public long UserStorageSize { get; set; }
        public long CurrentFilesSize { get; set; }
        public int PageSize { get; set; }
    }

    public class UploadFileResult
    {
        public string Name { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}
