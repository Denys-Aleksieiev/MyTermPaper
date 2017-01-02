using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Epam_FinalProject_FileManager_BLL.Interfaces;

namespace Epam_FinalProject_FileManager.Controllers
{
    public class FileManageController : Controller
    {
        private IFileService _fileService;
        public FileManageController(IFileService fileService)
        {
            _fileService = fileService;
        }

        // GET: FileManage
        public ActionResult Index()
        {
            return View();
        }
    }
}