using Epam_FinalProject_FileManager_BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using AutoMapper;
using Epam_FinalProject_FileManager_BLL.DTO;
using Epam_FinalProject_FileManager_DAL;
using Epam_FinalProject_FileManager_DAL.Interfaces;

namespace Epam_FinalProject_FileManager_BLL.Services
{
    public class FileService : IFileService
    {
        readonly IUnitOfWork _database;

        public FileService(IUnitOfWork database)
        {
            _database = database;
        }

        public IEnumerable<FileEntityDTO> GetAllUserFiles(string userId)
        {
            var list = _database.Files.Files;
            var newList = new List<FileEntity>();
            
            foreach (var file in list.ToList())
            {
                if (file.Owner.Id == userId)
                {
                    var f = DetermineType(file);
                    newList.Add(f);
                }
            }
            Mapper.CreateMap<FileEntity, FileEntityDTO>();
            return Mapper.Map<List<FileEntity>, List<FileEntityDTO>>(newList);
        }

        public IEnumerable<FileEntityDTO> GetAllUserDocuments(string userId)
        {
            var list = _database.Files.Files.ToList();
            var allowedExtensions = new[] { ".doc", ".xlsx", ".txt", ".jpeg", ".docx", ".log", ".msg", ".odt", ".pages", ".rtf", ".tex", ".txt", ".wpd", ".wps", ".3dm", ".3ds", ".max", ".obj", ".bmp", ".gif", ".jpg", ".png", ".psd", ".tga", ".thm", ".tif", ".tiff", ".yuv", ".ai", ".eps", ".ps", ".svg", ".indd", ".pct", ".pdf", ".xlr", ".xls", ".xlsx", ".dwg", ".dxf", ".gpx", ".kml", ".kmz", ".asp", ".aspx", ".cer", ".cfm", ".csr", ".css", ".htm", ".html", ".js", ".jsp", ".php", ".rss", ".xhtml", ".c", ".class", ".cpp", ".cs", ".dtd", ".fla", ".h", ".java", ".lua", ".m", ".pl", ".py", ".sh", ".sln", ".swift", ".vcxproj", ".xcodeproj", ".cfg", ".ini", ".prf"};
            var newList = new List<FileEntity>();
            foreach (var file in list)
            {
                var extension = Path.GetExtension(file.FileName);
                if (file.Owner.Id == userId && allowedExtensions.Contains(extension))
                {
                    file.IsDocument = true;
                    newList.Add(file);
                }
            }
            Mapper.CreateMap<FileEntity, FileEntityDTO>();
            return Mapper.Map<List<FileEntity>, List<FileEntityDTO>>(newList);
        }

        public IEnumerable<FileEntityDTO> GetAllUserAudios(string userId)
        {
            var list = _database.Files.Files.ToList();
            var allowedExtensions = new[] { ".aif", ".iff", ".m3u", ".m4a", ".mid", ".mp3", ".mpa", ".ra", ".wav", ".wma"};
            var newList = new List<FileEntity>();
            foreach (var file in list)
            {
                var extension = Path.GetExtension(file.FileName);
                if (file.Owner.Id == userId && allowedExtensions.Contains(extension))
                {
                    file.IsAudio = true;
                    newList.Add(file);
                }
            }
            Mapper.CreateMap<FileEntity, FileEntityDTO>();
            return Mapper.Map<List<FileEntity>, List<FileEntityDTO>>(newList);
        }

        public IEnumerable<FileEntityDTO> GetAllUserVideos(string userId)
        {
            var list = _database.Files.Files.ToList();
            var allowedExtensions = new[] { ".3g2", ".3gp", ".asf", ".asx", ".avi", ".flv", ".m4v", ".mov", ".mp4", ".mpg", ".rm", ".srt", ".swf", ".vob", ".wmv"};
            var newList = new List<FileEntity>();
            foreach (var file in list)
            {
                var extension = Path.GetExtension(file.FileName);
                if (file.Owner.Id == userId && allowedExtensions.Contains(extension))
                {
                    file.IsVideo = true;
                    newList.Add(file);
                }
            }
            Mapper.CreateMap<FileEntity, FileEntityDTO>();
            return Mapper.Map<List<FileEntity>, List<FileEntityDTO>>(newList);
        }

        public IEnumerable<FileEntityDTO> GetAllUserOtherFiles(string userid)
        {
           var list = _database.Files.Files.ToList();
            var newList = new List<FileEntity>();
            foreach (var file in list)
            {
                var f = DetermineType((file));

                if (!(f.IsAudio || f.IsDocument || f.IsImage))
                {
                    newList.Add(f);
                } 
            }

            Mapper.CreateMap<FileEntity, FileEntityDTO>();
            return Mapper.Map<List<FileEntity>, List<FileEntityDTO>>(newList);
        } 

        public FileEntity DetermineType(FileEntity file)
        {
            var documentExtensions = new[] { ".doc", ".xlsx", ".txt", ".jpeg", ".docx", ".log", ".msg", ".odt", ".pages", ".rtf", ".tex", ".txt", ".wpd", ".wps", ".3dm", ".3ds", ".max", ".obj", ".bmp", ".gif", ".jpg", ".png", ".psd", ".tga", ".thm", ".tif", ".tiff", ".yuv", ".ai", ".eps", ".ps", ".svg", ".indd", ".pct", ".pdf", ".xlr", ".xls", ".xlsx", ".dwg", ".dxf", ".gpx", ".kml", ".kmz", ".asp", ".aspx", ".cer", ".cfm", ".csr", ".css", ".htm", ".html", ".js", ".jsp", ".php", ".rss", ".xhtml", ".c", ".class", ".cpp", ".cs", ".dtd", ".fla", ".h", ".java", ".lua", ".m", ".pl", ".py", ".sh", ".sln", ".swift", ".vcxproj", ".xcodeproj", ".cfg", ".ini", ".prf" };
            var audioExtensions = new[] { ".aif", ".iff", ".m3u", ".m4a", ".mid", ".mp3", ".mpa", ".ra", ".wav", ".wma" };
            var videoExtensions = new[] { ".3g2", ".3gp", ".asf", ".asx", ".avi", ".flv", ".m4v", ".mov", ".mp4", ".mpg", ".rm", ".srt", ".swf", ".vob", ".wmv" };


            var extension = file.FileExtention;
            if (documentExtensions.Contains(extension))
            {
                file.IsDocument = true;
            }
            else if (audioExtensions.Contains(extension))
            {
                file.IsAudio = true;
            }
            else if (videoExtensions.Contains(extension))
            {
                file.IsVideo = true;
            }

            return file;
        }

        public void AddFileToUser(FileEntityDTO file, string userId)
        {
            Mapper.CreateMap<FileEntityDTO, FileEntity>();
            _database.Files.AddFileToUser(Mapper.Map<FileEntityDTO, FileEntity>(file), userId);
        }

        public bool DeleteFile(string fileId)
        {
            FileEntity deletedFile = _database.Files.GetFileById(fileId);
            bool isFileDeleted = true;
            try
            {
                File.Delete(deletedFile.FilePath);
            }
            catch (IOException)
            {
                isFileDeleted = false;
            }
            bool isDbFileDeleted = false;
            if (isFileDeleted)
            {
                isDbFileDeleted = _database.Files.DeleteFileById(fileId);
            }
            return isDbFileDeleted && isFileDeleted;
        }

        public bool UpdateFile(FileEntityDTO file)
        {
            Mapper.CreateMap<FileEntityDTO, FileEntity>();
            var fileEntity = Mapper.Map<FileEntityDTO, FileEntity>(file);
            return _database.Files.UpdateFile(fileEntity);
        }

        public bool IsUserHasFile(string fileId, string userId)
        {
            return _database.Files.GetFileById(fileId).Owner.Id == userId;
        }
        public bool IsUserHasFile(FileEntityDTO file, string userId)
        {
                var files = _database.Files.Files.Where(f => f.Owner != null && f.Owner.Id == userId);
                return files.Any(f => f.Hash == file.Hash);
      
        }

        public string CalculateMD5Hash(string filename)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 8192);
            md5.ComputeHash(stream);
            stream.Close();
            return Convert.ToBase64String(md5.Hash);
        }
        public string GetFileShareLink(string fileId)
        {
            var file = _database.Files.GetFileById(fileId);
            if (file.ShareLink != null)
            {
                return file.ShareLink.Value.ToString("N");
            }
            else
            {
                Guid newShareLink = Guid.NewGuid();
                _database.Files.UpdateShareLink(fileId, newShareLink);
                return newShareLink.ToString("N");
            }
        }
        public void RemoveFileShareLink(string fileId)
        {
            _database.Files.UpdateShareLink(fileId, null);
        }
        public long GetUserFilesSize(string userId)
        {
            var list = GetAllUserFiles(userId);

            long sum = 0;
            foreach (var file in list)
            {
                sum += file.Size;
            }
            return sum;
        }

        public FileEntityDTO GetFileById(string fileId)
        {
            Mapper.CreateMap<FileEntity, FileEntityDTO>();
            return Mapper.Map<FileEntity, FileEntityDTO>(_database.Files.GetFileById(fileId));
        }
        public FileEntityDTO GetFileByShareLink(string shareLink)
        {
            Guid shareId;
            if (!string.IsNullOrEmpty(shareLink) && Guid.TryParse(shareLink, out shareId))
            {
                Mapper.CreateMap<FileEntity, FileEntityDTO>();
                return Mapper.Map<FileEntity, FileEntityDTO>(_database.Files.GetFileByShareLink(shareId));
            }
            return null;
        }
    }
}
