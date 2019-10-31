using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;
using YiZhan.DataAccess;
using YiZhan.Entities.Attachments;
using YiZhan.ViewModels.Attachments;
using YiZhan.Entities.ApplicationOrganization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

/// <summary>
/// ���ڴ����ϴ��ļ�����Ӧ�Ĵ洢����Ŀ�����
/// </summary>
namespace YiZhan.Web.Controllers
{
    public class FilesManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _hostingEnv;
        private readonly IEntityRepository<BusinessFile> _businessFile;

        public FilesManagerController(
             UserManager<ApplicationUser> userManager,
             IHostingEnvironment hostingEnv,
            IEntityRepository<BusinessFile> businessFile)
        {
            this._userManager = userManager;
            this._hostingEnv = hostingEnv;
            this._businessFile = businessFile;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> ForSimple(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return Ok(new { count = files.Count, size, filePath });
        }

        public IActionResult FromFormFiles(List<IFormFile> files)
        {
            long size = 0;
            foreach (var file in files)
            {
                //var fileName = file.FileName;
                var fileName = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"')
                                .Substring(file.FileName.LastIndexOf("\\") + 1);
                fileName = _hostingEnv.WebRootPath + $@"\uploadFiles\{fileName}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(fileName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            //ViewBag.Message = $"{files.Count}���ļ� /{size}�ֽ��ϴ��ɹ�!";
            return Json(new { isOK = true, fileCount = files.Count, size = size });
        }

        public IActionResult FromAjaxFiles()
        {
            return View();
        }

        public IActionResult SaveFromAjaxFiles()
        {
            long size = 0;
            var files = Request.Form.Files;
            foreach (var file in files)
            {
                //var fileName = file.FileName;
                var fileName = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"')
                                .Substring(file.FileName.LastIndexOf("\\") + 1);
                fileName = _hostingEnv.WebRootPath + $@"\UploadFiles\{fileName}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(fileName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            return Json(new { isOK = true, fileCount = files.Count, size = size });
        }

        /// <summary>
        /// �ļ��ϴ�ʾ��
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> FilesForAjaxUpload()
        {
            long size = 0;
            long fileSize = 0;
            var boIsOk = false;
            var files = Request.Form.Files;
            var currUserId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id;
            if (files.Count <= 0)
            {
                return Json(new { isOK = false, fileCount = files.Count, size = size, message = "û��ѡ���κ��ļ�����ѡ���ļ������ύ�ϴ���" });
            }
            foreach (var file in files)
            {
                var currFileName = file.FileName;
                var timeForFile = (DateTime.Now.ToString("yyyyMMddHHmmss") + "_").Trim();
                var fileName = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"')
                                .Substring(file.FileName.LastIndexOf("\\") + 1);
                var newFileName = timeForFile + fileName;
                var boPath = "../../UploadFiles/" + newFileName;
                fileName = _hostingEnv.WebRootPath + $@"\UploadFiles\{newFileName}";
                fileSize = file.Length;
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(fileName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                var businessFile = new BusinessFile
                {
                    Name = currFileName,
                    OriginalFileName = currFileName,
                    Description = "�����ļ��ϴ���demoʾ���ļ�",
                    UploadPath = boPath,
                    FileSize = fileSize,
                    UploaderId = Guid.Parse(currUserId)
                };
                boIsOk = await _businessFile.AddOrEditAndSaveAsyn(businessFile);
            }
            if (boIsOk)
            {
                return Json(new { isOK = true, fileCount = files.Count, size = size, message = "�ϴ��ɹ���" });
            }
            else
            {
                return Json(new { isOK = false, fileCount = files.Count, size = size, message = "�ϴ�ʧ�ܣ�" });
            }
        }

        public async Task<List<BusinessFileVM>> GetFiles()
        {
            var fileListVM = new List<BusinessFileVM>();
            var listList = new List<BusinessFile>();
            listList = await _businessFile.GetAllListAsyn();
            foreach (var item in listList)
            {
                var fileVM = new BusinessFileVM(item);
                fileListVM.Add(fileVM);
            }
            return fileListVM;
        }
    }
}