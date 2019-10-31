using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YiZhan.Common.JsonModels;
using YiZhan.Common.YZExtensions;
using YiZhan.DataAccess;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.BusinessOrganization;
using YiZhan.Entities.Notifications;
using YiZhan.Entities.WebSettingManagement;
using YiZhan.Web.App.CommonHelper;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YiZhan.Web.Controllers.Admin
{
    public class AdminController : Controller
    {
        private YZNotification AppNotification { get; set; }
        private string ClientIpAddress => GetClientIpAddress();
        IHttpContextAccessor _httpContextAccessor;
        private readonly IEntityRepository<Notification> _notification;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEntityRepository<UserLoginLog> _userLoginLog;
        private readonly IEntityRepository<SiteConfiguration> _siteConfiguration;

        public AdminController(
            IHttpContextAccessor httpContextAccessor,
            IEntityRepository<Notification> notification,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEntityRepository<UserLoginLog> userLoginLog,
            IEntityRepository<SiteConfiguration> siteConfiguration)
        {
            _httpContextAccessor = httpContextAccessor;
            _notification = notification;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _userLoginLog = userLoginLog;
            _siteConfiguration = siteConfiguration;
            AppNotification = new YZNotification(notification);
        }

        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <returns></returns>
        public string GetCurrUserName()
        {
            try
            {
                var userName = User.Identity.Name;
                if (userName != null)
                {
                    var user = _userManager.FindByNameAsync(userName);
                    var userFullName = user.Result.ChineseFullName;
                    ViewBag.CurrUserName = userFullName == null ? userName : userFullName;
                    return userFullName == null ? userName : userFullName;
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

        public IActionResult Index()
        {
            if (GetCurrUserName() == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return RedirectToAction("Index", "AdminCenter");          
        }

        /// <summary>
        /// 系统管理员登录界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            if (GetCurrUserName() != null)
            {
                return RedirectToAction("Index", "AdminCenter");
            }
            return View();
        }

        /// <summary>
        /// 系统管理员登录界面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(string jsonLoginInformation)
        {
            var loginVM = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginInformation>(jsonLoginInformation);

            if (_HasTheSameUser(loginVM.UserName))
            {
                var user = _userManager.FindByNameAsync(loginVM.UserName).Result;
                var uRole =await _userManager.IsInRoleAsync(user, "Admin");
                if (!uRole)
                {
                    var siteConfiguration= _siteConfiguration.GetAll().FirstOrDefault();
                    if (!siteConfiguration.CanLogin)
                    {
                        return Json(new { result = false,canLogin=false, isAdminRole = false, message = "管理员已经禁用登录！<a href='/'>点此返回首页</a>" });
                    }
                }
                var result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    //var user = _userManager.FindByNameAsync(loginVM.UserName).Result;
                    //判断当前登录用户是否为管理员
                    //if (!await _userManager.IsInRoleAsync(user, "Admin"))
                    //{
                    if (!uRole)
                    {
                    return Json(new { result = true, isAdminRole = false, message = "对不起，您不是系统管理员，请勿进行登录操作！<a href='../../Account/UserCenter'>点此进入我的个人中心</a>" });
                    }
                    else
                    {
                        //正常登录                 
                        var message = "尊敬的管理员,您在[" + DateTime.Now + "]成功登录，登录IP:[" + ClientIpAddress + "]";
                        var notification = new Notification
                        {
                            Receiver = user,
                            Name = "登录提醒",
                            Description = message,
                            Link = "javascript:",
                            IsAbnormal = false,
                            IsRead = false,
                            NotificationSource = NotificationSourceEnum.App
                        };
                        AppNotification.SendNotification(notification);
                        AddUserLoginLog(user);
                        //下面的登录成功后的导航应该具体依赖用户所在的角色组来进行处理的。
                        var returnUrl = Url.Action("Index", "AdminCenter");
                        return Json(new { result = true, isAdminRole = true, message = returnUrl });
                    }
                }
                else
                {
                    return Json(new { result = false, message = "用户名或密码错误。" });
                }
            }
            else
            {
                return Json(new { result = false, message = "该管理员用户不存在！" });
            }
        }

        /// <summary>
        /// 是否存在指定用户名的用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        private bool _HasTheSameUser(string userName) => _userManager.Users.Any(x => x.UserName == userName);
        
        public void AddUserLoginLog(ApplicationUser user)
        {
            var userLoginLog = new UserLoginLog
            {
                Ip = ClientIpAddress,
                Address = string.Empty,
                BrowserInfo = string.Empty,
                User = user
            };
            _userLoginLog.AddAndSave(userLoginLog);
        }

        public string GetClientIpAddress()
        {
            #region 获取客户端真实的IP地址
            GetClientIpAddress getClientIpAddress = new GetClientIpAddress(_httpContextAccessor);
            return getClientIpAddress.UserClientIpAddress;
            #endregion
        }
    }
}
