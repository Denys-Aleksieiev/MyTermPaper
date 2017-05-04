using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam_FinalProject_FileManager_BLL.DTO;
using Epam_FinalProject_FileManager_DAL;

namespace Epam_FinalProject_FileManager_BLL.Interfaces
{
    public interface IFileService
    {
        IEnumerable<FileEntityDTO> GetAllUserFiles(string userId);
        IEnumerable<FileEntityDTO> GetAllUserDocuments(string userId);
        IEnumerable<FileEntityDTO> GetAllUserAudios(string userId);
        IEnumerable<FileEntityDTO> GetAllUserVideos(string userId);
        IEnumerable<FileEntityDTO> GetAllUserOtherFiles(string userid);
        void AddFileToUser(FileEntityDTO file, string userId);
        bool DeleteFile(string fileId);
        bool UpdateFile(FileEntityDTO file);
        bool IsUserHasFile(string fileId, string userId);
        bool IsUserHasFile(FileEntityDTO file, string userId);
        string CalculateMD5Hash(string path);
        string GetFileShareLink(string fileId);
        void RemoveFileShareLink(string fileId);
        long GetUserFilesSize(string userId);
        FileEntityDTO GetFileById(string fileId);
        FileEntityDTO GetFileByShareLink(string shareLink);
        FileEntity DetermineType(FileEntity file);
    }
}
