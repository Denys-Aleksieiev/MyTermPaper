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
using Microsoft.AspNet.Identity;
using PagedList;

namespace Epam_FinalProject_FileManager.Controllers
{
    [Authorize]
    public class MyStorageController : Controller
    {
        IFileService fileService;
        IUserService userService;
        private int pageSize = 15;

        public MyStorageController(IFileService fileS, IUserService userS)
        {
            fileService = fileS;
            userService = userS;


            ViewBag.Compressions = new SelectList(new List<SelectListItem>
                                    {
                                        new SelectListItem { Text = "None", Value = "0" },
                                        new SelectListItem { Text = "Average", Value = "1"},
                                        new SelectListItem { Text = "High", Value = "2"}
                                    }, "Value", "Text");
        }

        public ActionResult UFiles(string sortOrder, Func<string, IEnumerable<FileEntityDTO>> func, int? page = null, string searchString = null)
        {
            string userId = User.Identity.GetUserId();
            Func<string, IEnumerable<FileEntityDTO>> getUsers = func;
            var files = getUsers.Invoke(userId);
            long filesSize = fileService.GetUserFilesSize(userId);
            long userStorageSize = 0;
            try
            {
                userStorageSize = userService.GetUserById(userId).UserStorageSize;
            }
            catch (Exception)
            {
                Redirect("/Account/LogOff");
            }

            ViewBag.sortOrder = (sortOrder ?? "Date");
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
                        files = files.OrderBy(f => f.UploadDate);
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
                FileViewModel defaultResult = new FileViewModel()
                {
                    UserFiles = files.OrderByDescending(f => f.UploadDate),
                    CurrentFilesSize = filesSize,
                    UserStorageSize = userStorageSize,
                    PageSize = pageSize
                };
                return View("UserFiles", defaultResult);
            }
        }

        [HttpGet]
        public ActionResult UserFiles(string sortOrder, string searchString = null, int? page = null)
        {
            ViewBag.PageTitle = "Personal storage";
            ViewBag.ScriptId = "fileUploadButton";
            return UFiles(sortOrder, fileService.GetAllUserFiles, page, searchString);
        }

        [HttpGet]
        public ActionResult UserDocumentFiles(string sortOrder, int? page = null, string searchString = null)
        {
            ViewBag.PageTitle = "Personal storage: documents";
            ViewBag.ScriptId = "documentFileUploadButton";
            return UFiles(sortOrder, fileService.GetAllUserDocuments, page, searchString);
        }
        [HttpGet]
        public ActionResult UserAudioFiles(string sortOrder, int? page = null, string searchString = null)
        {
            ViewBag.PageTitle = "Personal storage: audios";
            ViewBag.ScriptId = "audioFileUploadButton";
            return UFiles(sortOrder, fileService.GetAllUserAudios, page, searchString);
        }
        [HttpGet]
        public ActionResult UserVideoFiles(string sortOrder, int? page = null, string searchString = null)
        {
            ViewBag.PageTitle = "Personal storage: videos";
            ViewBag.ScriptId = "videoFileUploadButton";
            return UFiles(sortOrder, fileService.GetAllUserVideos, page, searchString);
        }

        [HttpGet]
        public FileStreamResult Download(string fileId)
        {
            var file = fileService.GetFileById(fileId);
            return File(new FileStream(file.FilePath, FileMode.Open), "application/octet-stream", file.FileName);
        }
        [HttpGet]
        [AllowAnonymous]
        public FileStreamResult DownloadPublic(string shareLink)
        {
            var file = fileService.GetFileByShareLink(shareLink);
            return File(new FileStream(file.FilePath, FileMode.Open), "application/octet-stream", file.FileName);
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

        [HttpGet]
        public ContentResult UserStorageSize()
        {
            string userId = User.Identity.GetUserId();
            long filesSize = fileService.GetUserFilesSize(userId);
            long userStorageSize = userService.GetUserById(userId).UserStorageSize;

            double percents = (filesSize * 1.0) / userStorageSize * 100;
            string percentString = percents.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);


            string occupiedSize = Evaluate(filesSize);
            string totalSize = Evaluate(userStorageSize);

            return Content("{\"occupiedSize\":\"" + occupiedSize + "\",\"totalSize\":\""
                + totalSize + "\", \"percent\":\"" + percentString + "\"}", "application/json");
        }

        [HttpPost]
        public ContentResult Delete(string fileId)
        {
            if (fileService.IsUserHasFile(fileId, User.Identity.GetUserId()))
            {
                if (fileService.DeleteFile(fileId))
                {
                    return Content("{\"status\":\"true\",\"message\":\"OK\"}", "application/json");
                }
            }
            return Content("{\"status\":\"false\",\"message\":\"Ошибка при удалении файла!\"}", "application/json");
        }

        [HttpPost]
        public ActionResult SetCompression(string selectedCompression)
        {
            // Set to Session here.
            Session["selectedCompression"] = selectedCompression;
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        public ContentResult UploadFiles()
        {
            Stream fileStream = null;

            try
            {
                var r = new List<UploadFileResult>();

                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase hpf = Request.Files[fileName] as HttpPostedFileBase;

                    var compressionOption = (string)Session["selectedCompression"];

                    if (compressionOption == "0")
                    {
                        fileStream = hpf.InputStream;

                    }
                    else if (compressionOption == "1")
                    {
                        fileStream = hpf.InputStream;
                    }
                    else if (compressionOption == "2")
                    {
                        fileStream = new FileStream(@"compressed.lzma", FileMode.Create);
                        CompressionTechniques.LZMA.Compress(hpf.InputStream, fileStream);
                    }

                    if (fileStream != null && fileStream.Length != 0)
                    {
                        FileInfo fileInfo = new FileInfo(hpf.FileName);
                        Guid fileId = Guid.NewGuid();
                        string userId = User.Identity.GetUserId();

                        string endFilePath = Path.Combine(Server.MapPath("~/App_Data/UserFiles/"), userId, fileId.ToString() + fileInfo.Extension);

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
                            Size = fileStream.Length,
                            FilePath = endFilePath,
                            Hash = fileHash,
                            UploadDate = DateTime.Now
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
                                try
                                {
                                    long space = userService.GetUserFreeSpace(userId);
                                    if (space > fileStream.Length)
                                    {
                                        System.IO.File.Move(tempFilePath, endFilePath);
                                        fileService.AddFileToUser(newFile, userId);
                                        r.Add(new UploadFileResult()
                                        {
                                            Name = fileInfo.Name,
                                            Status = true,
                                            Message = "Uploaded"
                                        });
                                    }
                                    else
                                    {
                                        System.IO.File.Delete(tempFilePath);
                                        r.Add(new UploadFileResult()
                                        {
                                            Name = fileInfo.Name,
                                            Status = false,
                                            Message = "Not enough memory"
                                        });
                                    }
                                }
                                catch (Exception)
                                {
                                    Redirect("/Account/LogOff");
                                }
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

        public ActionResult MyAudio(string path, string userid, string name)
        {
            var file = Server.MapPath("~/App_Data/UserFiles/" + userid + "/" + name);
            return File(file, "audio/mp3");
        }

        public ActionResult MyVideo(string path, string userid, string name, string extension)
        {
            string file = String.Empty;
            if (extension == ".mp4")
            {
                file = Server.MapPath("~/App_Data/UserFiles/" + userid + "/" + name);
                return File(file, "video/mp4");
            }
            else if (extension == ".avi")
            {
                file = Server.MapPath("~/App_Data/UserFiles/" + userid + "/" + name);
                return File(file, "video/avi");
            }
            else if (extension == ".ogg")
            {
                file = Server.MapPath("~/App_Data/UserFiles/" + userid + "/" + name);
                return File(file, "video/ogg");
            }

            return File(file, "video/mp4");

        }

        public FileResult GetFile(string userid, string name)
        {
            // Force the pdf document to be displayed in the browser
            Response.AppendHeader("Content-Disposition", "inline; filename=" + name + ";");


            string file = Server.MapPath("~/App_Data/UserFiles/" + userid + "/" + name);
            return File(file, System.Net.Mime.MediaTypeNames.Application.Pdf, name);
        }

        public new FileContentResult File(string path, string extension)
        {
            var fileContents = System.IO.File.ReadAllBytes(path);
            var mimeType = "application/pdf";
            if (extension == ".docx")
                return new FileContentResult(fileContents, "documentfile.docx");

            return new FileContentResult(fileContents, mimeType);
        }

        private string Evaluate(long count)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (count == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(count);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 2);
            return (Math.Sign(count) * num).ToString() + suf[place];
        }
    }
}