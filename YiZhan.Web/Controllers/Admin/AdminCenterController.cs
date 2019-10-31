using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YiZhan.Common.JsonModels;
using YiZhan.DataAccess;
using YiZhan.Entities;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.Notifications;
using YiZhan.Entities.WebSettingManagement;
using YiZhan.ViewModels.ApplicationOrganization;
using YiZhan.ViewModels.Notifications;
using YiZhan.ViewModels.WebSettingManagement;
using YiZhan.Web.App.CommonHelper;

namespace YiZhan.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminCenterController : Controller
    {
        private YZNotification AppNotification { get; set; }
        private IHostingEnvironment _hostingEnv;
        private readonly IEntityRepository<Notification> _notification;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEntityRepository<Advertisement> _advertisement;
        private readonly IEntityRepository<BusinessImage> _businessImage;
        private readonly IEntityRepository<SiteSetting> _siteSetting;
        private readonly IEntityRepository<SiteNotice> _siteNotice;
        private readonly IEntityRepository<UserFeedback> _userFeedback;
        private readonly IEntityRepository<SiteConfiguration> _siteConfiguration;

        public AdminCenterController(
            IHostingEnvironment hostingEnv,
            IEntityRepository<Notification> notification,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IEntityRepository<Advertisement> advertisement,
            IEntityRepository<BusinessImage> businessImage,
            IEntityRepository<SiteSetting> siteSetting,
            IEntityRepository<SiteNotice> siteNotice,
            IEntityRepository<UserFeedback> userFeedback,
        IEntityRepository<SiteConfiguration> siteConfiguration
            )
        {
            _advertisement = advertisement;
            _hostingEnv = hostingEnv;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _businessImage = businessImage;
            _notification = notification;
            _siteSetting = siteSetting;
            _siteNotice = siteNotice;
            _userFeedback = userFeedback;
            _siteConfiguration = siteConfiguration;
            AppNotification = new YZNotification(notification);
        }

        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(await GetCurrUserName()))
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        
      
        
        /// <summary>
        /// 获取网站的基础信息设置
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SiteSetting()
        {
            var query = await _siteSetting.GetAllAsyn();
            var siteSetting = query.FirstOrDefault();
            var viewModel = new SiteSettingVM();
            if (siteSetting != null)
            {
                viewModel = new SiteSettingVM(siteSetting);
                viewModel.Logo = _businessImage.FindBy(x => x.RelevanceObjectId == siteSetting.Id).FirstOrDefault();
                //GetSiteConfiguration();
            }
            return View(viewModel);
        }

        /// <summary>
        /// 获取网站配置的模态框
        /// </summary>
        /// <returns></returns>

        public SiteConfigurationVM GetSiteConfiguration()
        {
            var siteConfiguration = _siteConfiguration.GetAll().FirstOrDefault();
            var siteConfigurationVM = new SiteConfigurationVM(siteConfiguration);
            ViewBag.SiteConfiguration = siteConfigurationVM;
            return siteConfigurationVM;
        }

        /// <summary>
        /// 系统管理员批量发送消息通知
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SendNotifications([Bind("Name,Description,Link")]NotificationVM boVM)
        {
            if (string.IsNullOrEmpty(boVM.Name))
            {
                return Json(new { result = false, message = "发送失败，消息名称不允许为空！" });
            }
            if (string.IsNullOrEmpty(boVM.Description))
            {
                return Json(new { result = false, message = "发送失败，消息内容不允许为空！" });
            }
            var users = _userManager.Users;
            if (users.Count() > 0)
            {
                var sendCount = 0;
                var link = string.IsNullOrEmpty(boVM.Link) ? "javascript:" : boVM.Link;

                foreach (var user in users)
                {
                    var notification = new Notification
                    {
                        Receiver = user,
                        Name = boVM.Name.Replace(" ", ""),
                        Description = boVM.Description.Replace(" ", ""),
                        Link = link.Replace(" ", ""),
                        IsAbnormal = false,
                        IsRead = false,
                        NotificationSource = NotificationSourceEnum.AppUser
                    };
                    AppNotification.SendNotification(notification);
                    sendCount++;
                }
                if (sendCount > 0 && sendCount < users.Count())
                {
                    var sendFail = users.Count() - sendCount;
                    return Json(new { result = false, message = "成功发送[ " + sendCount + "条 ]，失败[ " + sendFail + "条 ]" });
                }
                return Json(new { result = true, message = "全部发送成功！" });
            }
            return Json(new { result = false, message = "全部发送失败！" });
        }
              

        /// <summary>
        /// 管理员获取已发送消息列表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult GetNotifications()
        {
            var viewModel = AppNotification.AdminGetNotifications();
            return PartialView("../../Views/AdminCenter/_ADCenterNotifications", viewModel);
        }

        public async Task<IActionResult> Advertisements()
        {
            var entities = await _advertisement.GetAllListAsyn();
            var advertisementVMs = new List<AdvertisementVM>();
            int counter = 0;
            foreach (var item in entities)
            {
                var advertisementVM = new AdvertisementVM(item);
                advertisementVM.Image = _businessImage.GetSingleBy(x => x.RelevanceObjectId == item.Id);
                advertisementVM.OrderNumber = (++counter).ToString();
                advertisementVMs.Add(advertisementVM);
            }
            return View(advertisementVMs);
        }


        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                var loginUrl = Url.Action("Login", "Admin");
                await _signInManager.SignOutAsync();
                return Json(new { status = true, message = "", url = loginUrl });
            }
            catch (Exception)
            {
                return Json(new { status = true, message = "退出失败，请尝试刷新浏览器！", url = "" });
            }
        }




        /// <summary>
        /// 管理员个人中心页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> MyProfile()
        {
            return View(GetUserVM());
        }


        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ApplicationUser GetUser()
        {
            var user = this._userManager.FindByNameAsync(User.Identity.Name).Result;
            return user;
        }

        /// <summary>
        /// 获取用户的视图模型
        /// 说明：包括用户头像
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ApplicationUserVM GetUserVM()
        {
            var userVM = new ApplicationUserVM(GetUser());
            var avatar = _businessImage.GetAll().FirstOrDefault(i => i.RelevanceObjectId == Guid.Parse(GetUser().Id) && i.Type == ImageType.Avatars);
            userVM.Avatar = avatar;                //可以直接用这个 包含了头像的所有信息
            userVM.AvatarPath = avatar.UploadPath; //也可以用这个但是包含了头像的路径
            return userVM;
        }

        /// <summary>
        /// 获取用户头像
        /// 说明：使用 ViewBag.Avatar 在视图替换img路径
        /// </summary>
        public void GetUserAvatar()
        {
            var avatarPath = this._businessImage.GetAll().FirstOrDefault(i => i.RelevanceObjectId == Guid.Parse(GetUser().Id)).UploadPath;
            if (avatarPath != string.Empty)
            {
                ViewBag.Avatar = avatarPath;
            }
            else
            {
                ViewBag.Avatar = null;
            }
        }

        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetCurrUserName()
        {
            try
            {
                var userName = User.Identity.Name;
                if (userName != null)
                {
                    var user = await _userManager.FindByNameAsync(userName);
                    var userFullName = user.ChineseFullName;
                    ViewBag.CurrUserName = string.IsNullOrEmpty(userFullName) ? userName : userFullName;
                    GetUserAvatar();
                    return string.IsNullOrEmpty(userFullName) ? userName : userFullName;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
