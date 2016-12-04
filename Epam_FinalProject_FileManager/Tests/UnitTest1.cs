using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Epam_FinalProject_FileManager;
using Epam_FinalProject_FileManager_BLL.DTO;
using Epam_FinalProject_FileManager_BLL.Interfaces;
using Epam_FinalProject_FileManager_BLL.Services;
using Epam_FinalProject_FileManager_DAL;
using Epam_FinalProject_FileManager_DAL.Interfaces;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Epam_FinalProject_FileManager.Models.AppDbInitializer;

namespace Tests
{
    [TestClass]
    public class FileServiceTest
    {
        readonly IUnitOfWork _database = new EFUnitOfWork();
        readonly IFileService _fileService;
        IUserService _userService;
        private FileEntityDTO _file;
        private Guid _fileId;
        private string userId = "5030dc12 - e379 - 4daf - a658 - 65ca17d11ed3";

        public FileServiceTest()
        {


            _fileService = new FileService(_database);
            _userService = new UserService(_database);
            _file = new FileEntityDTO();
            _fileId = new Guid();
           
        }

        [TestMethod]
        public void TestGetAllUserFiles()
        {
            _file.Id = _fileId;
            _fileService.AddFileToUser(_file, userId);
            var f = _fileService.GetAllUserFiles(userId);
            Assert.Equals(f.ToList()[0].Id,_fileId);
        }

        [TestMethod]
        public void TestGetAllUserDocuments()
        {
            //IEnumerable<FileEntityDTO> GetAllUserDocuments(string userId);
        }

        [TestMethod]
        public void TestGetAllUserAudios()
        {
            //IEnumerable<FileEntityDTO> GetAllUserAudios(string userId);
        }

        [TestMethod]
        public void TestGetAllUserVideos()
        {
            //IEnumerable<FileEntityDTO> GetAllUserVideos(string userId);
        }

        [TestMethod]
        public void TestGetAllUserOtherFiles()
        {
            //IEnumerable<FileEntityDTO> GetAllUserOtherFiles(string userid);
        }

        [TestMethod]
        public void TestAddFileToUser()
        {
            //void AddFileToUser(FileEntityDTO file, string userId);
        }

        [TestMethod]
        public void TestDeleteFile()
        {
            //bool DeleteFile(string fileId);
        }

        [TestMethod]
        public void TestIsUserHasFile()
        {
            //bool IsUserHasFile(string fileId, string userId);
        }

        [TestMethod]
        public void TestGetUserFilesSize()
        {
            //long GetUserFilesSize(string userId);
        }

        [TestMethod]
        public void TestGetFileById()
        {
            //FileEntityDTO GetFileById(string fileId);
        }

        [TestMethod]
        public void TestDetermineType()
        {
            //FileEntity DetermineType(FileEntity file);
        }
    }
}
