using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam_FinalProject_FileManager_DAL;
using Epam_FinalProject_FileManager_DAL.Interfaces;

namespace UnitTests
{
    class TestFileRepository : IFileRepository
    {
        List<FileEntity> _context = new List<FileEntity>();
        private string userId = "4830dc12 - e379 - 4daf - a658 - 65ca17d11ed3";

        public IEnumerable<FileEntity> Files
        {
            get { return _context; }
        }

        public IQueryable<FileEntity> GetAllImages
        {
            get
            {
                var images = from image in _context
                             where image.IsImage.Equals(true)
                             select image;

                return images as IQueryable<FileEntity>;
            }
        }

        public IQueryable<FileEntity> GetAllVideos
        {
            get
            {
                var videos = from video in _context
                             where video.IsVideo.Equals(true)
                             select video;

                return videos as IQueryable<FileEntity>;
            }
        }

        public IQueryable<FileEntity> GetAllDocuments
        {
            get
            {
                var documents = from document in _context
                                where document.IsDocument.Equals(true)
                                select document;

                return documents as IQueryable<FileEntity>;
            }
        }

        public void AddFileToUser(FileEntity fileEntity, string userId)
        {
            if (this.userId == userId)
            {
                if (fileEntity.Owner != null) fileEntity.Owner.Id = userId;
                _context.Add(fileEntity);
            }
        }

        public FileEntity GetFileById(string fileId)
        {
            Guid fId = Guid.Parse(fileId);
            foreach(var file in _context)
            {
                if (file.Id == fId)
                    return file;
            }
            return null;
        }
        public FileEntity GetFileByShareLink(Guid shareLink)
        {
            foreach (var file in _context)
            {
                if (file.ShareLink == shareLink)
                    return file;
            }
            return null;

           
        }
        public bool DeleteFileById(string fileId)
        {
            Guid fId = Guid.Parse(fileId);
            for (int i = 0; i < _context.Count; i++)
            {
                if (_context[i].Id == fId)
                {
                    _context.RemoveAt(i);
                    return true;
                }

            }
            return false;
        }

        public void UpdateShareLink(string fileId, Guid? newShareLink)
        {
            var file = GetFileById(fileId);
            file.ShareLink = newShareLink;
        }

        public IEnumerable<FileEntity> GetAllPublicFiles()
        {
            var files = from file in _context
                        where file.IsPublic.Equals(true)
                        select file;

            return files;
        }
    }
}
