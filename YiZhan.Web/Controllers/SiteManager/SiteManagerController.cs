using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YiZhan.Common.JsonModels;
using YiZhan.Common.YZExtensions;
using YiZhan.DataAccess;
using YiZhan.Entities;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.Notifications;
using YiZhan.Entities.WebSettingManagement;
using YiZhan.ViewModels.WebSettingManagement;
using YiZhan.Web.App.CommonHelper;

namespace YiZhan.Web.Controllers.SiteManager
{
    public class SiteManagerController : Controller
    {
        public YZNotification AppNotification { get; set; }
        private IHostingEnvironment _hostingEnv;
        private string ClientIpAddress => GetClientIpAddress();
        IHttpContextAccessor _httpContextAccessor;
        private readonly IEntityRepository<FriendshipLink> _friendshipLink;
        private readonly IEntityRepository<SiteSetting> _siteSetting;
        private readonly IEntityRepository<SiteNotice> _siteNotice;
        private readonly IEntityRepository<UserFeedback> _userFeedback;
        private readonly IEntityRepository<SiteConfiguration> _siteConfiguration;
        private readonly IEntityRepository<BusinessImage> _businessImage;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEntityRepository<Notification> _notification;
        private readonly IEntityRepository<Advertisement> _advertisement;

        public SiteManagerController(
             IHostingEnvironment hostingEnv,
             IHttpContextAccessor httpContextAccessor,
             IEntityRepository<FriendshipLink> friendshipLink,
             IEntityRepository<SiteSetting> siteSetting,
             IEntityRepository<SiteNotice> siteNotice,
             IEntityRepository<UserFeedback> userFeedback,
            IEntityRepository<SiteConfiguration> siteConfiguration,
             IEntityRepository<BusinessImage> businessImage,
            UserManager<ApplicationUser> userManager,
            IEntityRepository<Advertisement> advertisement,
            IEntityRepository<Notification> notification)
        {
            this._hostingEnv = hostingEnv;
            this._httpContextAccessor = httpContextAccessor;
            this._friendshipLink = friendshipLink;
            this._siteSetting = siteSetting;
            this._siteNotice = siteNotice;
            this._userFeedback = userFeedback;
            this._siteConfiguration = siteConfiguration;
            this._businessImage = businessImage;
            this._userManager = userManager;
            this._notification = notification;
            this._advertisement = advertisement;
            AppNotification = new YZNotification(notification);
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取网站客服代码
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetCustomerService()
        {
            var siteSetting = await GetSiteSetting();
            var siteConfiguration = await GetSiteConfiguration();
            if (siteConfiguration.IsOpenCS)
            {
                if (siteSetting.CustomerService.Trim() == string.Empty)
                {
                    return Json(new { result = false });
                }
                var customerService = siteSetting.CustomerService;
                return Json(new { result = true, csCode = customerService });
            }
            else
            {
                return Json(new { result = false });
            }
        }

        /// <summary>
        /// 获取网站统计代码（此处推荐使用百度统计）
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetStatistics()
        {
            var siteSetting = await GetSiteSetting();
            if (siteSetting.Statistics.Trim() == string.Empty)
            {
                return Json(new { result = false });
            }
            var statisticsVal = siteSetting.Statistics;
            return Json(new { result = true, statistics = statisticsVal });
        }


        #region 友情链接管理

        /// <summary>
        /// 获取网站的友情链接
        /// </summary>
        /// <returns></returns> 
        public async Task<List<FriendshipLinkVM>> GetFriendshipLinks()
        {
            var friendshipLinksVM = new List<FriendshipLinkVM>();
            var friendshipLinks = await _friendshipLink.GetAllListAsyn();
            if (friendshipLinks.Count() > 0)
            {
                var sort = 0;
                foreach (var friendshipLink in friendshipLinks)
                {
                    var friendshipLinkVM = new FriendshipLinkVM(friendshipLink) { OrderNumber = (++sort).ToString() };
                    friendshipLinksVM.Add(friendshipLinkVM);
                }
            }
            return friendshipLinksVM;
        }

        /// <summary>
        /// 友情链接管理页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FriendshipLinks()
        {
            var friendshipLinks = await GetFriendshipLinks();
            return View("../../Views/AdminCenter/FriendshipLinks", friendshipLinks);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<FriendshipLinkVM> GetFriendshipLink(Guid id)
        {
            var model = await _friendshipLink.GetSingleAsyn(id);
            var friendshipLinkVM = new FriendshipLinkVM(model);
            return friendshipLinkVM;
        }


        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteFriendshipLink(Guid id)
        { 
            var isHas = _friendshipLink.HasInstance(id);
            if (isHas)
            {
                var r = await _friendshipLink.DeleteAndSaveAsyn(id);
                if (r.DeleteSatus)
                    return Json(new { isOk = true, message = "删除成功！" });
                else
                    return Json(new { isOk = false, message = "删除失败！" });
            }
            return Json(new { isOk = false, message = "删除失败！" });
        }

        /// <summary>
        /// 更新或编辑友情链接
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateFriendshipLinks(FriendshipLinkInput f)
        {
            FriendshipLink model = null;
            if (f.IsNew)
            {
                model = new FriendshipLink
                {
                    Name = f.Name,
                    Link = f.Link,
                    Description = f.Description,
                    IsBlank = f.IsBlank
                };
            }
            else
            {
                model = await _friendshipLink.GetSingleAsyn(f.Id);
                if (model != null)
                {
                    model.Name = f.Name;
                    model.Link = f.Link;
                    model.Description = f.Description;
                    model.IsBlank = f.IsBlank;
                }
                else
                {
                    return Json(new { isOk = false, message = "数据不存在，更新失败！" });
                }
            }

            var result = await _friendshipLink.AddOrEditAndSaveAsyn(model);
            if (!result)
            {
                return Json(new { isOk = false, message = "操作失败！" });
            }
            return Json(new { isOk = true, message = "操作成功！" });
        }

        #endregion

        /// <summary>
        /// 获取网站的邮箱地址
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetYiZhanEmail()
        {
            var query = await GetSiteSetting();
            ViewBag.YiZhanEmail = query.SiteEmail;
            return string.IsNullOrEmpty(query.SiteEmail) ? "管理员未设置邮箱" : query.SiteEmail;
        }

        /// <summary>
        /// 保存网站的基本信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SetSiteSetting(SiteSettingVM input)
        {
            var strList = new List<string> { input.Name, input.Suffix };
            if (StrIsNull(strList))
            {
                return Json(new { result = false, message = "请填写必填项！" });
            }
            var siteSetting = await GetSiteSetting();
            siteSetting.Name = input.Name;
            siteSetting.Suffix = input.Suffix;
            siteSetting.DomainName = input.DomainName;
            siteSetting.KeyWords = input.KeyWords;
            siteSetting.Description = input.Description;
            siteSetting.Copyright = input.Copyright;
            siteSetting.SiteEmail = input.SiteEmail;
            siteSetting.ICP = input.ICP;
            siteSetting.Statistics = input.Statistics;
            siteSetting.CustomerService = input.CustomerService.Trim();
            _siteSetting.EditAndSave(siteSetting);
            if (siteSetting.CustomerService.Trim() == string.Empty)
            {
                var siteConfiguration = await GetSiteConfiguration();
                if (siteConfiguration.IsOpenCS)
                {
                    siteConfiguration.IsOpenCS = false;
                    _siteConfiguration.EditAndSave(siteConfiguration);
                }
            }
            return Json(new { result = true });
        }

        /// <summary>
        /// 内部获取网站设置
        /// </summary>
        /// <returns></returns>
        private async Task<SiteSetting> GetSiteSetting()
        {
            var query = await _siteSetting.GetAllAsyn();
            var siteSetting = query.FirstOrDefault();
            return siteSetting;
        }

        /// <summary>
        /// 全站获取网站信息
        /// </summary>
        /// <returns></returns>
        public async Task<SiteSettingVM> GetSiteSettings()
        {
            var viewModel = new SiteSettingVM(await GetSiteSetting());
            var images = await _businessImage.GetAllAsyn();
            if (images.Count() > 0)
            {
                var image = images.FirstOrDefault(x => x.RelevanceObjectId == viewModel.Id);
                if (image != null)
                {
                    viewModel.LogoPath = string.IsNullOrEmpty(image.UploadPath) ? string.Empty : image.UploadPath;
                }
            }
            return viewModel;
        }

        /// <summary>
        /// 内部获取网站配置
        /// </summary>
        /// <returns></returns>
        private async Task<SiteConfiguration> GetSiteConfiguration()
        {
            var siteConfigurations = await _siteConfiguration.GetAllAsyn();
            var siteConfiguration = siteConfigurations.FirstOrDefault();
            return siteConfiguration;
        }

        /// <summary>
        /// 更新网站的配置
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SetSiteConfiguration(SiteConfigurationVM input)
        {
            var siteConfiguration = await GetSiteConfiguration();
            var siteSetting = await GetSiteSetting();
            if (input.IsOpenCS)
            {
                if (siteSetting.CustomerService == string.Empty
                    && siteConfiguration.IsOpenCS != input.IsOpenCS)
                {
                    return Json(new { result = false, message = "设置失败，网站未设置客服代码！" });
                }
            }
            if (siteConfiguration != null)
            {
                siteConfiguration.CanRegister = input.CanRegister;
                siteConfiguration.CanLogin = input.CanLogin;
                siteConfiguration.IsOpenAd = input.IsOpenAd;
                siteConfiguration.IsOpenCode = input.IsOpenCode;
                siteConfiguration.IsOpenCS = input.IsOpenCS;
                _siteConfiguration.EditAndSave(siteConfiguration);
                return Json(new { result = true });
            }
            return Json(new { result = false, message = "设置失败！" });
        }

        public bool StrIsNull(List<string> strList)
        {
            bool result = false;
            foreach (var item in strList)
            {
                if (string.IsNullOrEmpty(item))
                {
                    result = true;
                }
            }
            return result;
        }


        /// <summary>
        /// 上传LOGO
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public async Task<IActionResult> UploadLOGO(Guid id)
        {
            try
            {
                var image = Request.Form.Files.First();
                var currImageName = image.FileName;
                var timeForFile = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-").Trim();
                string extensionName = currImageName.Substring(currImageName.LastIndexOf("."));
                var imageName = ContentDispositionHeaderValue
                                .Parse(image.ContentDisposition)
                                .FileName
                                .Trim('"')
                                .Substring(image.FileName.LastIndexOf("\\") + 1);
                var newImageName = timeForFile + Guid.NewGuid() + extensionName;
                var boPath = "../../images/UploadImages/" + ImageType.Logo.ToString() + "/" + newImageName;
                var imagePath = _hostingEnv.WebRootPath + $@"\images\UploadImages\{ImageType.Logo.ToString()}";
                imageName = _hostingEnv.WebRootPath + $@"\images\UploadImages\{ImageType.Logo.ToString()}\{newImageName}";
                Directory.CreateDirectory(imagePath); //创建目录
                using (FileStream fs = System.IO.File.Create(imageName))
                {
                    image.CopyTo(fs);
                    fs.Flush();
                }
                var query = await _businessImage.FindByAsyn(x => x.RelevanceObjectId == id);
                var logo = query.FirstOrDefault();
                if (logo != null)
                {
                    System.IO.File.Delete(Path.Combine(logo.PhysicalPath, "\\" + logo.Name));
                    logo.UploadPath = boPath;
                    logo.PhysicalPath = imageName;
                }
                else
                {
                    logo = new BusinessImage
                    {
                        Name = newImageName,
                        DisplayName = currImageName,
                        OriginalFileName = currImageName,
                        Description = "网站LOGO",
                        RelevanceObjectId = id,
                        UploaderId = Guid.Empty,
                        UploadPath = boPath,
                        PhysicalPath = imageName,
                        FileSize = image.Length,
                        Type = ImageType.Logo
                    };
                }
                await _businessImage.AddOrEditAndSaveAsyn(logo);
                return Json(new { result = true });
            }
            catch (Exception)
            {
                return Json(new { result = false });
            }
        }

        #region 网站公告代码

        /// <summary>
        /// 获取所有的公告
        /// </summary>
        /// <returns></returns>
        public async Task<List<SiteNoticeVM>> GetSiteNotices()
        {
            var siteNoticesVM = new List<SiteNoticeVM>();
            var attachments = new List<BusinessImage>();
            var query = await _siteNotice.GetAllIncludingAsyn(x => x.Publisher);
            var siteNotices = query.OrderByDescending(x => x.CreateTime);
            var queryImgs = await _businessImage.GetAllAsyn();
            foreach (var siteNotice in siteNotices)
            {
                var siteNoticeVM = new SiteNoticeVM(siteNotice);
                if (queryImgs.Count() > 0)
                {
                    attachments = queryImgs.Where(x => x.RelevanceObjectId == siteNotice.Id).ToList();
                    if (attachments.Count() > 0)
                    {
                        siteNoticeVM.Attachments = attachments;
                    }
                    else
                    {
                        siteNoticeVM.Attachments = new List<BusinessImage>();
                    }
                    siteNoticeVM.OrderNumber = (siteNoticesVM.Count() + 1).ToString();
                }
                else
                {
                    siteNoticeVM.Attachments = new List<BusinessImage>();
                }
                siteNoticesVM.Add(siteNoticeVM);
            }
            return siteNoticesVM;
        }

        /// <summary>
        /// 默认加载第一条公告
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetDefaultSiteNotice()
        {
            var siteNotices = await GetSiteNotices();
            var viewModel = siteNotices[0];
            await GetYiZhanEmail();
            return PartialView("../../Views/Home/_SiteNoticeDetail", viewModel);
        }

        /// <summary>
        /// 获取单条公告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetSiteNotice(Guid id)
        {
            var viewModel = await GetSiteNoticeVM(id);
            await GetYiZhanEmail();
            return PartialView("../../Views/Home/_SiteNoticeDetail", viewModel);
        }

        /// <summary>
        /// 获取公告内容（视图模型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<SiteNoticeVM> GetSiteNoticeVM(Guid id)
        {
            var attachments = new List<BusinessImage>();
            var query = await _siteNotice.GetAllIncludingAsyn(x => x.Publisher);
            var siteNotice = query.FirstOrDefault(x => x.Id == id);
            var siteNoticeVM = new SiteNoticeVM(siteNotice);
            var queryImgs = await _businessImage.GetAllAsyn();
            if (queryImgs.Count() > 0)
            {
                attachments = queryImgs.Where(x => x.RelevanceObjectId == siteNotice.Id).ToList();
                if (attachments.Count() > 0)
                {
                    siteNoticeVM.Attachments = attachments;
                }
                else
                {
                    siteNoticeVM.Attachments = attachments;
                }
            }
            else
            {
                siteNoticeVM.Attachments = attachments;
            }
            return siteNoticeVM;
        }

        /// <summary>
        /// 获取网站的公告（管理员部分）
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SiteNotices()
        {
            //ViewBag.ModalSiteNoticeVM = null;
            var viewModal = await GetSiteNotices();
            return View("../../Views/AdminCenter/SiteNotices", viewModal);
        }

        /// <summary>
        /// 发布公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateSiteNotice(SiteNoticeVM input)
        {
            if (string.IsNullOrEmpty(input.Name) || string.IsNullOrEmpty(input.Description))
            {
                return Json(new { result = false, message = "必填项为空，请检查标题和内容是否已经选择和填写！" });
            }
            var siteNotice = new SiteNotice
            {
                Publisher = GetUser(),
                Name = EncodeFilterHelper.EncodeHtml(input.Name),
                Description = EncodeFilterHelper.EncodeHtml(input.Description)
            };
            var r = await _siteNotice.AddOrEditAndSaveAsyn(siteNotice);
            if (r)
            {
                await UploadNoticeIMG(siteNotice);
                return Json(new { result = true });
            }
            return Json(new { result = false, message = "发布失败，请重试！" });

        }

        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteSiteNotice(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Json(new { result = false, messgae = "删除失败" });
            }
            var r = await _siteNotice.DeleteAndSaveAsyn(id);
            if (r.DeleteSatus)
            {
                return Json(new { result = true });
            }
            return Json(new { result = false, messgae = "删除失败" });
        }

        /// <summary>
        ///  处理公告的图片上传
        /// </summary>
        /// <param name="entity"></param>
        private async Task<bool> UploadNoticeIMG(SiteNotice entity)
        {
            try
            {
                var images = Request.Form.Files;
                if (images.Count() > 0)
                {
                    foreach (var image in images)
                    {
                        if (image != null)
                        {
                            var currImageName = image.FileName;
                            var timeForFile = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-").Trim();
                            string extensionName = currImageName.Substring(currImageName.LastIndexOf("."));
                            var imageName = ContentDispositionHeaderValue
                                            .Parse(image.ContentDisposition)
                                            .FileName
                                            .Trim('"')
                                            .Substring(image.FileName.LastIndexOf("\\") + 1);
                            var newImageName = timeForFile + Guid.NewGuid() + extensionName;
                            var boPath = "../../images/UploadImages/" + ImageType.Notices.ToString() + "/" + newImageName;
                            var imagePath = _hostingEnv.WebRootPath + $@"\images\UploadImages\{ImageType.Notices.ToString()}";
                            imageName = _hostingEnv.WebRootPath + $@"\images\UploadImages\{ImageType.Notices.ToString()}\{newImageName}";

                            Directory.CreateDirectory(imagePath); //创建目录
                            using (FileStream fs = System.IO.File.Create(imageName))
                            {
                                image.CopyTo(fs);
                                fs.Flush();
                            }

                            var noticeIMG = new BusinessImage
                            {
                                Name = newImageName,
                                DisplayName = currImageName,
                                OriginalFileName = currImageName,
                                Description = "站点公告图片",
                                RelevanceObjectId = entity.Id,
                                UploaderId = Guid.Empty,
                                UploadPath = boPath,
                                PhysicalPath = imageName,
                                FileSize = image.Length,
                                Type = ImageType.Notices
                            };

                            await _businessImage.AddOrEditAndSaveAsyn(noticeIMG);
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region 用户反馈代码

        /// <summary>
        /// 获取所有的用户反馈
        /// </summary>
        /// <param name="id">用户的Id 标明可以使用用户Id查询对应的用户提交的记录</param>
        /// <returns></returns>
        public async Task<List<UserFeedbackVM>> GetUserFeedbacks(Guid? id)
        {
            var userFeedbacksVM = new List<UserFeedbackVM>();
            var userFeedbacks = new List<UserFeedback>();
            if (id.HasValue && id.Value != Guid.Empty)
            {
                var query = await _userFeedback.GetAllIncludingAsyn(x => x.FeedUser);
                var result = query.Where(x => x.FeedUser.Id == id.Value.ToString());
                if (result.Count() > 0)
                {
                    userFeedbacks = result.OrderByDescending(x => x.CreateTime).ToList();
                }
            }
            else
            {
                var query = await _userFeedback.GetAllIncludingAsyn(x => x.FeedUser);
                if (query.Count() > 0)
                {
                    userFeedbacks = query.OrderByDescending(x => x.CreateTime).ToList();
                }
            }
            if (userFeedbacks.Count() > 0)
            {
                foreach (var userFeedback in userFeedbacks)
                {
                    var userFeedbackVM = new UserFeedbackVM(userFeedback);
                    userFeedbackVM.OrderNumber = (userFeedbacksVM.Count() + 1).ToString();
                    userFeedbacksVM.Add(userFeedbackVM);
                }
            }
            return userFeedbacksVM;
        }

        /// <summary>
        /// 获取用户反馈记录
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> GetMyFeedbacks()
        {
            var viewModel = await GetUserFeedbacks(Guid.Parse(GetUser().Id));
            return PartialView("../../Views/Account/UserCenterPartialViews/_MyFeedback", viewModel);
        }

        /// <summary>
        /// 获取用户反馈的界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserFeedback()
        {
            var viewModel = await GetUserFeedbacks(Guid.Empty);
            return PartialView("../../Views/AdminCenter/UserFeedback", viewModel);
        }

        /// <summary>
        /// 删除反馈记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteUserFeedback(Guid id)
        {
            var r = await _userFeedback.DeleteAndSaveAsyn(id);
            if (r.DeleteSatus)
            {
                return Json(new { result = true });
            }
            return Json(new { result = false });
        }

        /// <summary>
        /// 确认接收用户的反馈
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ConfirmUserFeedback(Guid id)
        {
            var entity = await GetUserFeedbackentity(id);
            entity.State = true;
            var r = await _userFeedback.AddOrEditAndSaveAsyn(entity);
            if (r)
            {
                if (entity.FeedUser != null)
                {
                    var message = "您于[ " + entity.CreateTime + " ]反馈的[ " + entity.Type + " ]类型的问题[ " + entity.Description + " ]系统管理员已经接收，感谢您的反馈！";
                    var notification = new Notification
                    {
                        Receiver = entity.FeedUser,
                        Name = "反馈回执",
                        Description = message,
                        Link = "javascript:",
                        IsAbnormal = false,
                        IsRead = false,
                        NotificationSource = NotificationSourceEnum.App
                    };
                    AppNotification.SendNotification(notification);
                }

                return Json(new { result = true });
            }
            return Json(new { result = false });
        }

        /// <summary>
        /// 获取用户反馈（直接实体）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserFeedback> GetUserFeedbackentity(Guid id)
        {
            var query = await _userFeedback.GetAllIncludingAsyn(x => x.FeedUser);
            var userFeedback = query.Where(x => x.Id == id).FirstOrDefault();
            return userFeedback;
        }

        /// <summary>
        /// 提交用户反馈
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitFeedback(UserFeedbackVM input)
        {
            var strList = new List<string> { input.Description };
            if (StrIsNull(strList) || input.Type == UserFeedbackType.未选择)
            {
                return Json(new { result = false, contentNull = true, message = "必填项为空，请检查类型和描述是否已经选择和填写！" });
            }
            var user = GetUser();
            var userFeedback = new UserFeedback
            {
                Link = EncodeFilterHelper.EncodeHtmlForLink(input.Link),
                Description = EncodeFilterHelper.EncodeHtml(input.Description),
                ContactWay = EncodeFilterHelper.EncodeHtml(input.ContactWay),
                Type = input.Type,
                FeedbackIPAddress = GetClientIpAddress(),
                FeedUser = user == null ? null : user
            };
            var r = await _userFeedback.AddOrEditAndSaveAsyn(userFeedback);
            if (r)
            {
                return Json(new { result = true });
            }
            return Json(new { result = false, message = "提交失败！" });
        }

        #endregion

        #region 网站广告和轮播图代码

        /// <summary>
        /// 获取单条广告进行编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetAdvertisement(Guid id)
        {
            var entity = await _advertisement.GetSingleAsyn(id);
            var viewModel = new AdvertisementVM(entity);
            viewModel.Image = _businessImage.GetSingleBy(x => x.RelevanceObjectId == entity.Id);
            return PartialView("../../Views/AdminCenter/_EditAdvertisementPartialModal", viewModel);
        }

        /// <summary>
        /// 编辑公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditAdvertisement(AdvertisementVM input)
        {
            var entity = await _advertisement.GetSingleAsyn(input.Id);
            entity.Link = input.Link;
            var r = await _advertisement.AddOrEditAndSaveAsyn(entity);
            if (r)
            {
                UploadAdvertisementIMG(entity);
                return Json(new { result = true });
            }
            return Json(new { result = false, messages = "失败的操作！" });
        }

        /// <summary>
        ///  处理广告的图片上传
        /// </summary>
        /// <param name="entity"></param>
        private void UploadAdvertisementIMG(IEntity entity)
        {
            try
            {
                var image = Request.Form.Files.First();
                if (image != null)
                {
                    var objectImg = _businessImage.FindBy(x => x.RelevanceObjectId == entity.Id).FirstOrDefault();
                    var currImageName = image.FileName;
                    var timeForFile = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-").Trim();
                    string extensionName = currImageName.Substring(currImageName.LastIndexOf("."));
                    var imageName = ContentDispositionHeaderValue
                                    .Parse(image.ContentDisposition)
                                    .FileName
                                    .Trim('"')
                                    .Substring(image.FileName.LastIndexOf("\\") + 1);
                    var newImageName = timeForFile + Guid.NewGuid() + extensionName;
                    var boPath = "../../images/UploadImages/" + objectImg.Type.ToString() + "/" + newImageName;
                    var imagePath = _hostingEnv.WebRootPath + $@"\images\UploadImages\{objectImg.Type.ToString()}";
                    imageName = _hostingEnv.WebRootPath + $@"\images\UploadImages\{objectImg.Type.ToString()}\{newImageName}";

                    Directory.CreateDirectory(imagePath); //创建目录
                    using (FileStream fs = System.IO.File.Create(imageName))
                    {
                        image.CopyTo(fs);
                        fs.Flush();
                    }
                    if (objectImg.PhysicalPath != null)
                    {
                        System.IO.File.Delete(Path.Combine(objectImg.PhysicalPath, "\\" + objectImg.Name));
                    }
                    objectImg.Name = newImageName;
                    objectImg.PhysicalPath = imagePath;
                    objectImg.UploadPath = boPath;
                    _businessImage.AddOrEditAndSave(objectImg);
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion

        /// <summary>
        /// 获取用户名(Ajax加载显示)
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetUserNameOrFullName()
        {
            try
            {
                var userName = User.Identity.Name;
                if (userName != null)
                {
                    var user = await _userManager.FindByNameAsync(userName);
                    var userFullName = user.ChineseFullName;
                    var result = string.IsNullOrEmpty(userFullName) ? userName : userFullName;
                    return Json(new { name = result });
                }
                else
                {
                    return Json(new { name = string.Empty });
                }
            }
            catch (Exception)
            {
                return Json(new { name = string.Empty });
            }
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>

        public ApplicationUser GetUser()
        {
            try
            {
                var userName = User.Identity.Name;
                if (userName == "")
                {
                    return null;
                }
                return this._userManager.FindByNameAsync(userName).Result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region 获取用户信息代码

        /// <summary>
        /// 根据Id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetUserById(string id)
        {
            try
            {
                if (id == string.Empty)
                {
                    return null;
                }
                var user = await _userManager.FindByIdAsync(id);
                return user == null ? null : user;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 获取用户IP地址
        /// </summary>
        /// <returns></returns>
        public string GetClientIpAddress()
        {
            #region 获取客户端真实的IP地址
            GetClientIpAddress getClientIpAddress = new GetClientIpAddress(_httpContextAccessor);
            return getClientIpAddress.UserClientIpAddress;
            #endregion
        }
    }
}
