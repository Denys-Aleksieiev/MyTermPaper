using Epam_FinalProject_FileManager_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam_FinalProject_FileManager_DAL.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _context;

        public FileRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<FileEntity> Files
        {
            get { return _context.FileEntities; }
        }

        public IQueryable<FileEntity> GetAllImages
        {
            get
            {
                var images = from image in _context.FileEntities
                    where image.IsImage.Equals(true)
                    select image;

                return images;
            }
        }

        public IQueryable<FileEntity> GetAllVideos
        {
            get
            {
                var videos = from video in _context.FileEntities
                             where video.IsVideo.Equals(true)
                             select video;

                return videos;
            }
        }

        public IQueryable<FileEntity> GetAllDocuments
        {
            get
            {
                var documents = from document in _context.FileEntities
                             where document.IsDocument.Equals(true)
                             select document;

                return documents;
            }
        }

        public void AddFileToUser(FileEntity fileEntity, string userId)
        {
            var user = _context.Users.Find(userId);
            user.UserFiles.Add(fileEntity);
            _context.SaveChanges();
        }

        public FileEntity GetFileById(string fileId)
        {
            Guid fId = Guid.Parse(fileId);
            return _context.FileEntities.Find(fId);
        }
        public FileEntity GetFileByShareLink(Guid shareLink)
        {
            return _context.FileEntities.FirstOrDefault(f => f.ShareLink == shareLink);
        }
        public bool DeleteFileById(string fileId)
        {
            Guid fId = Guid.Parse(fileId);
            var fileToRemove = _context.FileEntities.SingleOrDefault(f => f.Id == fId);
            if (fileToRemove != null)
            {
                _context.FileEntities.Remove(fileToRemove);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void UpdateShareLink(string fileId, Guid? newShareLink)
        {
            var file = GetFileById(fileId);
            file.ShareLink = newShareLink;
            _context.SaveChanges();
        }

        public IEnumerable<FileEntity> GetAllPublicFiles()
        {
            var files = from file in _context.FileEntities
                where file.IsPublic.Equals(true)
                select file;

            return files;
        }
    }
}
