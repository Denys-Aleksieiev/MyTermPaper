using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Epam_FinalProject_FileManager.Models;
using Epam_FinalProject_FileManager_BLL.DTO;
using Epam_FinalProject_FileManager_BLL.Interfaces;
using Epam_FinalProject_FileManager_DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace Epam_FinalProject_FileManager.Controllers
{
    public class PublicStorageController : Controller
    {
        IFileService fileService;
        IUserService userService;
        private int pageSize = 15;

        private string USERID = "4830dc12 - e379 - 4daf - a658 - 65ca17d11ed3";

        public PublicStorageController(IFileService fileS, IUserService userS)
        {
            fileService = fileS;
            userService = userS;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string AnonimOwner)
        {
            if (!string.IsNullOrEmpty(AnonimOwner))
            {
                Session["AnonimOwner"] = AnonimOwner;
                return RedirectToAction("UserFiles");
            }
            else
            {
                ViewBag.Error = "Error: Incorrect name";
                return View();
            }
        }

        [HttpGet]
        public ActionResult UserFiles(string sortOrder, string searchString = null, int? page = null)
        {
            if (Session["AnonimOwner"] == null)
            {

                return RedirectToAction("Index");
            }
            ViewBag.FilterBy = "Default";
            ViewBag.ScriptId = "fileUploadButton";
            return UFiles(sortOrder, fileService.GetAllUserFiles, page, searchString);
        }

        [HttpGet]
        public ActionResult UserDocumentFiles(string sortOrder, string searchString = null, int? page = null)
        {
            if (Session["AnonimOwner"] == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.FilterBy = "Documents";
            ViewBag.ScriptId = "documentFileUploadButton";
            return UFiles(sortOrder, fileService.GetAllUserDocuments, page, searchString);
        }

        [HttpGet]
        public ActionResult UserAudioFiles(string sortOrder, string searchString = null, int? page = null)
        {

            if (Session["AnonimOwner"] == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.FilterBy = "Audio";
            ViewBag.ScriptId = "audioFileUploadButton";
            return UFiles(sortOrder, fileService.GetAllUserAudios, page, searchString);
        }

        [HttpGet]
        public ActionResult UserVideoFiles(string sortOrder, string searchString = null, int? page = null)
        {
            if (Session["AnonimOwner"] == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.FilterBy = "Videos";
            ViewBag.ScriptId = "videoFileUploadButton";
            return UFiles(sortOrder, fileService.GetAllUserVideos, page, searchString);
        }

        [HttpGet]
        public ActionResult UFiles(string sortOrder, Func<string, IEnumerable<FileEntityDTO>> func, int? page = null, string searchString = null)
        {
            Func<string, IEnumerable<FileEntityDTO>> getUsers = func;
            var files = getUsers.Invoke(USERID);
            ViewBag.sortOrder = (sortOrder ?? "Name");
            ViewBag.searchString = "";

            if (!String.IsNullOrEmpty(searchString))
            {
                files = files.Where(f => f.FileName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0);
                ViewBag.searchString = searchString;
            }

            #region Sorting
            switch (sortOrder)
            {
                case "Name":
                    {
                        files = files.OrderBy(f => f.FileName);
                    }
                    break;
                case "NameDesc":
                    {
                        files = files.OrderByDescending(f => f.FileName);
                    }
                    break;
                case "Date":
                    {
                        files = files.OrderBy(f => f.UploadDate);
                    }
                    break;
                case "DateDesc":
                    {
                        files = files.OrderByDescending(f => f.UploadDate);
                    }
                    break;
                case "Size":
                    {
                        files = files.OrderBy(f => f.Size);
                    }
                    break;
                case "SizeDesc":
                    {
                        files = files.OrderByDescending(f => f.Size);
                    }
                    break;
                default:
                    {
                        files = files.OrderBy(f => f.FileName);
                    }
                    break;
            }
            #endregion

            int pageNumber = (page ?? 1);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_UserFilesListPartial", files.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                PublicFileViewModel defaultResult = new PublicFileViewModel()
                {
                    Files = files.OrderBy(f => f.FileName),
                    PageSize = pageSize
                };
                return View("UserFiles", defaultResult);
            }
        }


        [HttpGet]
        public FileStreamResult Download(string fileId)
        {
            var file = fileService.GetFileById(fileId);
            Stream decompressedStream = new FileStream(@"compressed.lzma", FileMode.Open);

            using (var fileStream = new FileStream(file.FilePath, FileMode.Open))
            {
                CompressionTechniques.LZMA.Decompress(fileStream, decompressedStream);
            }

            decompressedStream.Position = 0;

            return File(decompressedStream, "application/octet-stream", file.FileName);
        }
        [HttpGet]
        [AllowAnonymous]
        public FileStreamResult DownloadPublic(string shareLink)
        {
            var file = fileService.GetFileByShareLink(shareLink);
            Stream decompressedStream = new FileStream(@"compressed.lzma", FileMode.Open);

            using (var fileStream = new FileStream(file.FilePath, FileMode.Open))
            {
                CompressionTechniques.LZMA.Decompress(fileStream, decompressedStream);
            }

            decompressedStream.Position = 0;

            return File(decompressedStream, "application/octet-stream", file.FileName);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SharedFile(string shareLink)
        {
            FileEntityDTO file = fileService.GetFileByShareLink(shareLink);
            if (file != null)
            {
                return View("FileDescription", file);
            }
            return View("FileNotFound");
        }

        [HttpGet]
        public ContentResult ShareLink(string fileId)
        {
            string shareLink = fileService.GetFileShareLink(fileId);
            string absoluteLink = Url.RouteUrl("ShareFileAccess", new { shareLink = shareLink }, Request.Url.Scheme);
            return Content("{\"shareLink\":\"" + absoluteLink + "\"}", "application/json");
        }

        [HttpPost]
        public ContentResult DeleteShareLink(string fileId)
        {
            fileService.RemoveFileShareLink(fileId);
            return Content("{\"status\":\"true\",\"message\":\"Внешняя ссылка удалена\"}", "application/json");
        }

        [HttpPost]
        public ContentResult UploadFiles()
        {
            FileStream fileStream = null;

            try
            {
                var r = new List<UploadFileResult>();

                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase hpf = Request.Files[fileName] as HttpPostedFileBase;

                    fileStream = new FileStream(@"compressed.lzma", FileMode.Create);
                    CompressionTechniques.LZMA.Compress(hpf.InputStream, fileStream);

                    if (fileStream != null && fileStream.Length != 0)
                    {
                        FileInfo fileInfo = new FileInfo(hpf.FileName);
                        Guid fileId = Guid.NewGuid();

                        string userFilesPath = Server.MapPath("~/App_Data/UserFiles/");

                        if (!Directory.Exists(Path.Combine(userFilesPath, USERID)))
                        {
                            Directory.CreateDirectory(Path.Combine(userFilesPath, USERID));
                        }

                        if (userService.GetUserById(USERID) == null)
                        {
                            ApplicationDbContext context = new ApplicationDbContext();
                            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
                            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                            var user = new ApplicationUser
                            {
                                Email = "PublicUser@gmail.com",
                                UserName = "PublicUser@gmail.com"
                            };
                            user.Id = USERID;
                            string password = "123456b";
                            var result = userManager.Create(user, password);
                            if (result.Succeeded)
                            {
                                // добавляем для пользователя роль
                                userManager.AddToRole(user.Id, "User");
                            }
                        }

                        string endFilePath = Path.Combine(userFilesPath, USERID, fileId.ToString() + fileInfo.Extension);

                        string tempCatalogPath = Server.MapPath("~/App_Data/UserFiles/Temp");
                        string tempFilePath = Path.Combine(tempCatalogPath, hpf.FileName);

                        using (var tmpStream = System.IO.File.Create(tempFilePath))
                        {
                            fileStream.Position = 0;
                            fileStream.CopyTo(tmpStream);
                        }

                        string fileHash = fileService.CalculateMD5Hash(tempFilePath);

                        FileEntityDTO newFile = new FileEntityDTO()
                        {
                            Id = fileId,
                            FileName = fileInfo.Name,
                            FileExtention = fileInfo.Extension,
                            Size = hpf.ContentLength,
                            FilePath = endFilePath,
                            Hash = fileHash,
                            UploadDate = DateTime.Now,
                            IsPublic = true,
                            AnonimOwner = Session["AnonimOwner"] as string
                        };

                        //Sometimes when user uploads large files
                        //After calculating hash, system doesn't have time to release filestream
                        //and attempt to move file causes Exception Process can't access to file
                        //This code tries several times repeat with  file and after that return message about error
                        int numberOfRetries = 5;
                        int retryTime = 300;
                        int retriesCount = 0;
                        bool isRetrying = true;
                        while (isRetrying && retriesCount < numberOfRetries)
                        {
                            try
                            {
                                System.IO.File.Move(tempFilePath, endFilePath);
                                fileService.AddFileToUser(newFile, USERID);
                                r.Add(new UploadFileResult() { Name = fileInfo.Name, Status = true, Message = "Uploaded" });
                                isRetrying = false;
                            }
                            catch (IOException)
                            {
                                retriesCount++;
                                Thread.Sleep(retryTime);
                            }
                        }
                        if (retriesCount == numberOfRetries)
                        {
                            return Content("{\"name\":\"" + "" + "\",\"status\":\"" + "False" +
                            "\",\"message\":\"" + "uploading error" + "\"}", "application/json");
                        }

                    }
                    return Content("{\"name\":\"" + r[0].Name + "\",\"status\":\"" + r[0].Status.ToString() +
                        "\",\"message\":\"" + r[0].Message + "\"}", "application/json");
                }
            }
            catch (HttpException)
            {
                return Content("{\"name\":\"\",\"status\":\"false\",\"message\":\"Превишен максимальный размер файла\"}", "application/json");
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
            return Content("{\"name\":\"\", \"status\":\"false\",\"message\":\"Неизвестная ошибка\"}", "application/json");
        }


    }
}