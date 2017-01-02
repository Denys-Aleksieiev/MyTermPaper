using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam_FinalProject_FileManager_DAL.Interfaces
{
    public interface IFileRepository
    {
        IEnumerable<FileEntity> Files { get; }
        IQueryable<FileEntity> GetAllImages { get; }
        IQueryable<FileEntity> GetAllVideos { get; }
        IQueryable<FileEntity> GetAllDocuments { get; }
        void AddFileToUser(FileEntity fileDescription, string userId);
        FileEntity GetFileById(string fileId);
        FileEntity GetFileByShareLink(Guid shareLink);
        bool DeleteFileById(string fileId);
        void UpdateShareLink(string fileId, Guid? newShareLink);
        IEnumerable<FileEntity> GetAllPublicFiles();

    }
}
