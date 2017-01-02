using System;
using System.Collections.Generic;
using Epam_FinalProject_FileManager_BLL.DTO;
using Epam_FinalProject_FileManager_BLL.Interfaces;
using Epam_FinalProject_FileManager_BLL.Services;
using Epam_FinalProject_FileManager_DAL;
using Epam_FinalProject_FileManager_DAL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class FileServiceTest
    {
        IUnitOfWork _database = new TestUnitOfWork();
        private IFileService _fileService;
        private IUserService _userService;
        private FileEntityDTO _file;
        private string userId = "4830dc12 - e379 - 4daf - a658 - 65ca17d11ed3";

        public FileServiceTest()
        {
            _fileService = new FileService(_database);
            _userService = new UserService(_database);
            _file = new FileEntityDTO();
            _file.Id = new Guid();
            _file.Owner = new ApplicationUser();
            _file.FilePath = System.Environment.CurrentDirectory + "/file.txt";
            if (!System.IO.File.Exists(System.Environment.CurrentDirectory + "/file.txt"))
            {
                System.IO.File.Create(System.Environment.CurrentDirectory + "/file.txt");
            }
        }
        [TestMethod]
        public void TestAddFileToUser()
        {
            _fileService.AddFileToUser(_file,userId);
            Assert.IsTrue(_fileService.IsUserHasFile(_file, userId));
        }

        [TestMethod]
        public void TestDeleteFile()
        {
            if (!_fileService.IsUserHasFile(_file, userId))
            {
                _fileService.AddFileToUser(_file,userId);
            }
            _fileService.DeleteFile(_file.Id.ToString());
            Assert.IsFalse(_fileService.IsUserHasFile(_file,userId));
        }

        [TestMethod]
        public void TestIsUserHasFile()
        {
            if (!_fileService.IsUserHasFile(_file, userId))
            {
                _fileService.AddFileToUser(_file, userId);
            }
            Assert.IsTrue(_fileService.IsUserHasFile(_file.Id.ToString(), userId));
        }

        [TestMethod]
        public void TestGetFileById()
        {
            if (!_fileService.IsUserHasFile(_file, userId))
            {
                _fileService.AddFileToUser(_file, userId);
            }
            var file = _fileService.GetFileById(_file.Id.ToString());
            Assert.AreEqual(_file.Id, file.Id);
        }

        [TestMethod]
        public void TestDetermineType()
        {
            FileEntity file = new FileEntity();
            file.FileExtention = ".doc";
            var f = _fileService.DetermineType(file);
            Assert.IsTrue(f.IsDocument);
        }

    }
}
