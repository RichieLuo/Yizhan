using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.DataAccess;
using Microsoft.AspNetCore.Identity;
using YiZhan.Common.JsonModels;
using YiZhan.ViewModels.ApplicationOrganization;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.BusinessManagement.User;
using YiZhan.Entities.Notifications;
using YiZhan.Web.App.CommonHelper;
using YiZhan.Entities.BusinessOrganization;
using Microsoft.AspNetCore.Http;
using YiZhan.Common.YZExtensions;
using YiZhan.ViewModels.BusinessManagement;
using YiZhan.Entities.BusinessManagement.Commodities;
using YiZhan.Entities.WebSettingManagement;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YiZhan.Web.Controllers.Account
{
    public class AccountController : Controller
    {
        private string addDefaultPass = "$Ta!233@666";
        private YZNotification AppNotification { get; set; }
        private string ClientIpAddress => GetClientIpAddress();
        private readonly IEntityRepository<UserLoginLog> _userLoginLog;
        private readonly IEntityRepository<Notification> _notification;
        IHttpContextAccessor _httpContextAccessor;
        private IHostingEnvironment _hostingEnv;
        private readonly IEntityRepository<BusinessImage> _businessImage;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEntityRepository<Person> _peson;
        private readonly IEntityRepository<YZ_UserVisitorLog> _userVisitorLog;
        private readonly IEntityRepository<YZ_Commodity> _commodity;
        private readonly IEntityRepository<SiteConfiguration> _siteConfiguration;

        public AccountController(
            IEntityRepository<UserLoginLog> userLoginLog,
            IEntityRepository<Notification> notification,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnv,
            IEntityRepository<BusinessImage> businessImage,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IEntityRepository<Person> person,
            IEntityRepository<YZ_UserVisitorLog> userVisitorLog,
            IEntityRepository<YZ_Commodity> commodity,
           IEntityRepository<SiteConfiguration> siteConfiguration)
        {
            _userLoginLog = userLoginLog;
            _notification = notification;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnv = hostingEnv;
            _businessImage = businessImage;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _peson = person;
            _userVisitorLog = userVisitorLog;
            _commodity = commodity;
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
                //IsShowAddDefaultDataBtn();
                var userName = User.Identity.Name;
                if (userName != null)
                {
                    var user = GetUser();
                    var userFullName = user.ChineseFullName;
                    ViewBag.CurrUserName = userFullName == null ? userName : userFullName;
                    GetUserAvatar();
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

        /// <summary>
        /// 根据已经用户已经登录的情况，判断是否已经可以获取用户的登录信息
        /// </summary>
        /// <returns>用户名</returns>
        [Authorize]
        public IActionResult GetUserName()
        {
            var userName = User.Identity.Name;
            return Json(new { UserName = userName });
        }

        /// <summary>
        /// 用户个人中心页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult UserCenter()
        {
            GetCurrUserName();
            UserInfoIntegrity();

            return View(GetUserVM());
        }

        /// <summary>
        /// 用户中心首页
        /// 说明：显示一些图表之类的统计
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult UserCenterIndex()
        {
            var commodityListForIndexVM = new List<YZ_CommodityVM>();
            var commoditiesForIndex = this._commodity
                .GetAllIncluding(x => x.Category, x => x.AscriptionUser)
                .Where(x => x.State == YZ_CommodityState.OnSale)
                .OrderByDescending(c => c.AddTime).ToList();
            if (commoditiesForIndex.Count() > 0)
            {
                foreach (var commodity in commoditiesForIndex)
                {
                    var commodityImageForIndex = _businessImage.FindBy(m => m.RelevanceObjectId == commodity.Id).FirstOrDefault(m => m.Type == ImageType.CommodityCover);
                    var commodityImagesForIndex = new List<BusinessImage>();
                    commodityImagesForIndex.Add(commodityImageForIndex);
                    var commodityForIndexVM = new YZ_CommodityVM(commodity);
                    commodityForIndexVM.Images = commodityImagesForIndex;
                    commodityListForIndexVM.Add(commodityForIndexVM);
                }
                commodityListForIndexVM.OrderByDescending(m => m.AddTime);
            }

            ViewBag.LatestRelease = commodityListForIndexVM;
            var Commodies = GetWhatYouLike();
            var obj = Commodies.Count() < 0 ? commodityListForIndexVM : Commodies;

            return PartialView("../../Views/Account/UserCenterPartialViews/_UserCenterIndex", obj);
            // return PartialView("../../Views/Account/UserCenterPartialViews/_UserCenterIndex", new List<YZ_CommodityVM>());
        }

        private List<YZ_CommodityVM> GetWhatYouLike()
        {
            //根据用户最近浏览的商品类别找到类似的商品     
            var commodityLisForLiketVM = new List<YZ_CommodityVM>();
            var visitorLog = _userVisitorLog.GetAllIncluding(x => x.Category).OrderByDescending(x => x.LookTime)
                .Where(x => x.UserIdOrIp == GetUser().Id).FirstOrDefault();

            commodityLisForLiketVM = GetCommodityVMForGuessYouLike(visitorLog);

            //总共是4条数据
            return commodityLisForLiketVM.Count() == 0 ? null : commodityLisForLiketVM;
        }

        private List<YZ_CommodityVM> GetCommodityVMForGuessYouLike(YZ_UserVisitorLog visitorLog)
        {
            var commodityLisForLiketVM = new List<YZ_CommodityVM>();
            var listCount = 0;
            var commodityForLikeList = _commodity.GetAllIncluding(x => x.Category).Where(x => x.Category == visitorLog.Category).ToList();
            if (commodityForLikeList.Count() > 0)
            {
                foreach (var commodityForLikeItem in commodityForLikeList)
                {
                    if (listCount >= 4) { break; }
                    var commodityImageForLike = _businessImage.FindBy(m => m.RelevanceObjectId == commodityForLikeItem.Id).FirstOrDefault(m => m.Type == ImageType.CommodityCover);
                    var commodityImagesForLike = new List<BusinessImage>();
                    commodityImagesForLike.Add(commodityImageForLike);
                    var commodityForLikeVM = new YZ_CommodityVM(commodityForLikeItem);
                    commodityForLikeVM.Images = commodityImagesForLike;
                    commodityLisForLiketVM.Add(commodityForLikeVM);
                    listCount++;
                }
            }
            return commodityLisForLiketVM;
        }

        /// <summary>
        ///用户的个人信息 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            var query = await _userLoginLog.GetAllIncludingAsyn(x => x.User);            
            if (query.Count() > 0)
            {
                var userLoginLogs = query.Where(x => x.User == GetUser()).OrderBy(x => x.loginTime).ToList();
                var lastLoginIp = "";
                if (userLoginLogs.Count() > 1)
                {
                    var testData = userLoginLogs.Skip(userLoginLogs.Count() - 2).FirstOrDefault();
                    lastLoginIp = userLoginLogs.Skip(userLoginLogs.Count() - 2).FirstOrDefault().Ip;
                }
                else
                {
                    lastLoginIp = userLoginLogs.FirstOrDefault().Ip;
                }
                ViewBag.LastLoginIp = lastLoginIp;
            }          
            return PartialView("../../Views/Account/UserCenterPartialViews/_MyProfile", GetUserVM());
        }


        /// <summary>
        /// 加载修改头像界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult ChangeAvatar()
        {
            return PartialView("../../Views/Account/UserCenterPartialViews/_ChangeAvatar", GetUserVM());
        }

        /// <summary>
        /// 保存头像
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SaveChangeAvatar()
        {
            try
            {
                var image = Request.Form.Files.First();
                if (image == null)
                {
                    return Json(new { isOK = false, message = "没有选择头像，请选择头像后再保存。" });
                }
                var currImageName = image.FileName;
                var timeForFile = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-").Trim();
                string extensionName = currImageName.Substring(currImageName.LastIndexOf("."));
                var imageName = ContentDispositionHeaderValue
                                .Parse(image.ContentDisposition)
                                .FileName
                                .Trim('"')
                                .Substring(image.FileName.LastIndexOf("\\") + 1);
                var newImageName = timeForFile + GetUser().Id + extensionName;
                var boPath = "../../images/UploadImages/" + ImageType.Avatars.ToString() + "/" + newImageName;
                var imagePath = _hostingEnv.WebRootPath + $@"\images\UploadImages\{ImageType.Avatars.ToString()}";
                imageName = _hostingEnv.WebRootPath + $@"\images\UploadImages\{ImageType.Avatars.ToString()}\{newImageName}";

                Directory.CreateDirectory(imagePath); //创建目录
                using (FileStream fs = System.IO.File.Create(imageName))
                {
                    image.CopyTo(fs);
                    fs.Flush();
                }
                var avatar = _businessImage.GetAll().FirstOrDefault(a => a.RelevanceObjectId == Guid.Parse(GetUser().Id) && a.Type == ImageType.Avatars);

                if (avatar.PhysicalPath != string.Empty)
                {
                    System.IO.File.Delete(avatar.PhysicalPath);
                }
                avatar.UploaderId = Guid.Parse(GetUser().Id);
                avatar.UploadPath = boPath;
                avatar.PhysicalPath = imageName;
                avatar.FileSize = image.Length;
                await _businessImage.AddOrEditAndSaveAsyn(avatar);
                return Json(new { isOK = true, message = "修改成功！", url = avatar.UploadPath });
            }
            catch (Exception)
            {
                return Json(new { isOK = false, message = "修改失败" });
            }
        }

        /// <summary>
        /// 加载消息通知界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult MyNotifications()
        {
            var thisUser = GetUser();
            ViewBag.IsAdmin = thisUser.UserName == "admin" ? true : false;
            var viewModel = AppNotification.GetNotifications(thisUser);
            return PartialView("../../Views/Account/UserCenterPartialViews/_MyNotifications", viewModel);
        }

        /// <summary>
        /// 删除消息通知
        /// </summary>
        /// <param name="noticesId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteNotification(Guid noticesId)
        {
            AppNotification.DeleteNotification(noticesId);
            var noReadCount = await GetNotificationsCount();
            return Json(new { result = true, count = noReadCount });
        }

        /// <summary>
        /// 批量删除消息通知
        /// </summary>
        /// <param name="noticesId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteNotifications(Guid[] noticesId)
        {
            AppNotification.DeleteNotifications(noticesId);
            var noReadCount = await GetNotificationsCount();
            return Json(new { result = true, count = noReadCount });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SetNotificationIsRead(Guid noticesId)
        {
            AppNotification.SetNotificationIsRead(noticesId);
            var noReadCount = await GetNotificationsCount();
            return Json(new { result = true, noReadCount });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SetNotificationsIsRead(Guid[] noticesId)
        {
            AppNotification.SetNotificationsIsRead(noticesId);
            var noReadCount = await GetNotificationsCount();
            return Json(new { result = true, count = noReadCount });
        }

        /// <summary>
        /// 加载用户修改密码界面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult ChangePassword()
        {
            return PartialView("../../Views/Account/UserCenterPartialViews/_ChangePassword");
        }

        /// <summary>
        /// 保存密码修改
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<JsonResult> SaveChangePassword([Bind("Password,ConfirmPassword")]ApplicationUserVM boVM)
        {
            try
            {
                if (boVM.Password == null || boVM.ConfirmPassword == null)
                {
                    return Json(new { result = false, message = "密码修改存在空值，请检查！" });
                }
                if (boVM.Password != boVM.ConfirmPassword)
                {
                    return Json(new { result = false, message = "新密码两次输入不相同，请检查！" });
                }
                // 获取重置密码的令牌
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(GetUser());
                // 重置密码
                await _userManager.ResetPasswordAsync(GetUser(), resetToken, boVM.Password);

                var notification = new Notification
                {
                    Receiver = GetUser(),
                    Name = "修改密码",
                    Description = "您修改了密码，请牢记您的新密码！",
                    Link = "javascript:",
                    IsAbnormal = false,
                    IsRead = false,
                    NotificationSource = NotificationSourceEnum.App
                };
                AppNotification.SendNotification(notification);
                return Json(new { result = true, message = "密码修改成功！" });
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "系统出现异常，暂时无法修改密码，请反馈。" });
            }

        }

        /// <summary>
        /// 获取用户的信息完整度
        /// </summary>
        /// <returns></returns>
        protected virtual int UserInfoIntegrity()
        {
            int integrity = 20;
            var entity = GetUser();
            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.MobileNumber))
                {
                    integrity += 16;
                }
                if (!string.IsNullOrEmpty(entity.QQNumber))
                {
                    integrity += 16;
                }
                if (!string.IsNullOrEmpty(entity.Email))
                {
                    integrity += 16;
                }
                if (!string.IsNullOrEmpty(entity.Description))
                {
                    integrity += 16;
                }
                if (!string.IsNullOrEmpty(entity.ChineseFullName))
                {
                    integrity += 16;
                }
            }
            ViewBag.UserInfoIntegrity = integrity;
            return integrity;
        }

        /// <summary>
        /// 修改资料
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<JsonResult> SaveChangeProfile([Bind("Name,MobileNumber,QQNumber,EMail,Description")]ApplicationUserVM boVM)
        {
            if (string.IsNullOrEmpty(boVM.EMail))
            {
                return Json(new { result = false, message = "电子邮件必须填写！" });
            }
            //var entity = await _userManager.FindByIdAsync(boVM.Id.ToString());
            var thisUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (thisUser != null)
            {
                thisUser.ChineseFullName = EncodeFilterHelper.EncodeHtmlToNull(boVM.Name);
                thisUser.MobileNumber = boVM.MobileNumber;
                thisUser.Email = EncodeFilterHelper.EncodeHtml(boVM.EMail);
                thisUser.QQNumber = boVM.QQNumber;
                thisUser.Description = EncodeFilterHelper.EncodeHtmlToNull(boVM.Description);

                var update = await _userManager.UpdateAsync(thisUser);
                if (update.Succeeded)
                {
                    var thisIntegrity = UserInfoIntegrity() + "%";
                    var thisUsername = GetCurrUserName();
                    return Json(new { result = true, integrity = thisIntegrity, username = thisUsername });
                }
                return Json(new { result = false, message = "修改失败" });
            }
            return Json(new { result = false, message = "修改失败" });
        }

        /// <summary>
        /// 查看大头像
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult UserBigAvatarPreviewModal()
        {
            return PartialView("../../Views/Account/UserCenterPartialViews/_UserBigAvatarPreviewModal", GetUserVM());
        }

        #region 用户注册和登录相关操作

        /// <summary>
        /// 获取消息通知数量
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> NotificationsCount()
        {
            var noReadCount = await GetNotificationsCount();
            return Json(new { count = noReadCount });
        }

        private async Task<int> GetNotificationsCount()
        {
            var notifications = await _notification
                   .GetAllIncludingAsyn(x => x.Receiver);
            var notificationList = notifications.Where(x => x.Receiver == GetUser() && x.IsRead == false).ToList();
            var noReadCount = notificationList.Count();
            return noReadCount;
        }


        /// <summary>
        /// 已经用作了登录界面
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            if (GetCurrUserName() != null)
            {
                return RedirectToAction("UserCenter");
            }
            //获取用户名
            GetCurrUserName();
            ViewBag.IsOpenCode = GetSiteConfiguration().IsOpenCode;
            return View();
        }

        /// <summary>
        /// 打开添加默认用户的入口
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddDefaultUsers()
        {
            var uExist = await _userManager.FindByNameAsync("admin");
            if (uExist == null)
            {
                ViewBag.Message = "——添加默认用户——";
                ViewBag.PassError = true;
                return View();
            }
            ViewBag.Message = "——数据已经添加，您可以通过下方按钮选择一些操作——";
            ViewBag.PassError = false;
            return View();
        }

        /// <summary>
        /// 添加一些默认的用户 、当然在种子数据里面已经添加过了另外的用户
        /// ?在种子数据添加的用户不能够登录，原因可能在于哈希密码
        /// </summary>
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddDefaultUsers(string pass)
        {
            var uExist = await _userManager.FindByNameAsync("admin");
            if (uExist == null)
            {
                if (!addDefaultPass.Equals(pass))
                {
                    ViewBag.Message = "——密码错误，请重新输入——";
                    ViewBag.PassError = true;
                    return View();
                }
                ApplicationRole adminRole = null;
                ApplicationRole userRole = null;
                //?判断用户组是否存在
                var roleExists = await _roleManager.RoleExistsAsync("Admin");
                if (!roleExists)
                {
                    adminRole = new ApplicationRole() { Name = "Admin", DisplayName = "系统管理人员", Description = "适用于系统管理人员", ApplicationRoleType = ApplicationRoleTypeEnum.适用于系统管理人员, SortCode = "69a5f56g" };
                    userRole = new ApplicationRole() { Name = "AverageUser", DisplayName = "普通注册用户", Description = "适用于普通注册用户", ApplicationRoleType = ApplicationRoleTypeEnum.适用于普通注册用户, SortCode = "99avf56g" };
                    await _roleManager.CreateAsync(adminRole);
                    await _roleManager.CreateAsync(userRole);
                }

                //添加默认用户    
                var password = "123!qwe";
                for (int i = 0; i < 5 + 1; i++)
                {
                    var user = new ApplicationUser();

                    if (i == 0)
                    {
                        user = new ApplicationUser()
                        {
                            UserName = "admin",
                            FirstName = "易站",
                            LastName = "超级管理员",
                            ChineseFullName = user.FirstName + user.LastName,
                            Email = "admin@925i.cn"
                        };
                        var addAdmin = await _userManager.CreateAsync(user);
                        var addAdminPassword = await _userManager.AddPasswordAsync(user, password);
                        //查询用户是否已经添加了权限 若不在添加进用户组
                        if (!await _userManager.IsInRoleAsync(user, adminRole.Name))
                        {
                            var roleOK = await _userManager.AddToRoleAsync(user, adminRole.Name);
                        }
                    }
                    else
                    {
                        user = new ApplicationUser()
                        {
                            UserName = i == 1 ? "demo" : ("demo00" + (i - 1).ToString()),
                            FirstName = "易站演示",
                            LastName = "用户" + (i - 1),
                            ChineseFullName = "易站普通用户" + (i - 1),
                            Email = i == 1 ? "demo" : ("demo00" + (i - 1).ToString()) + "@925i.cn"
                        };
                        var addUser = await _userManager.CreateAsync(user);
                        var addUserPassword = await _userManager.AddPasswordAsync(user, password);
                        //查询用户是否已经添加了权限 若不在添加进用户组
                        if (!await _userManager.IsInRoleAsync(user, userRole.Name))
                        {
                            var roleOK = await _userManager.AddToRoleAsync(user, userRole.Name);
                        }
                    }
                    var avatar = new BusinessImage
                    {
                        Name = string.Empty,
                        DisplayName = string.Empty,
                        OriginalFileName = string.Empty,
                        Type = ImageType.Avatars,
                        RelevanceObjectId = Guid.Parse(user.Id),
                        UploaderId = Guid.Parse(user.Id),
                        Description = "这是用户【" + user.ChineseFullName + "】的头像",
                        FileSize = 0,
                        UploadPath = "../../images/Avatars/defaultAvatar.gif",
                        PhysicalPath = string.Empty
                    };
                    await _businessImage.AddOrEditAndSaveAsyn(avatar);
                }
                GetCurrUserName();
                ViewBag.Message = "——数据添加完成，您可以通过下方按钮选择一些操作——";
                ViewBag.PassError = false;
                return View();
            }
            else
            {
                ViewBag.Message = "——数据已经添加，您可以通过下方按钮选择一些操作——";
                ViewBag.PassError = false;
                return View();
            }
        }

        /// <summary>
        /// 处理用户登录
        /// </summary>
        /// <param name="jsonLoginInformation"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(string jsonLoginInformation)
        {
            if (!GetSiteConfiguration().CanLogin)
            {
                return Json(new { result = false, message = "管理员已经开启禁止登录，请关注公告！" });
            }
            if (GetCurrUserName() != null)
            {
                return RedirectToAction("UserCenter", "Account");
            }
            var loginVM = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginInformation>(jsonLoginInformation);
            if (_HasTheSameUser(loginVM.UserName))
            {
                var result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // 下面的登录成功后的导航应该具体依赖用户所在的角色组来进行处理的。     
                    var returnUrl = Url.Action("UserCenter", "Account");
                    var user = this._userManager.FindByNameAsync(loginVM.UserName).Result;
                    var notification = new Notification();
                    if (GetUserLoginLog(user) != null)
                    {
                        if (ClientIpAddress != GetUserLoginLog(user).Ip)
                        {
                            var message = "尊敬的用户，您的账号登录异常，登录IP:[" + ClientIpAddress + "] 若非本人操作，请及时修改密码。";
                            notification = new Notification
                            {
                                Receiver = user,
                                Name = "异常登录提醒",
                                Description = message,
                                Link = "javascript:",
                                IsAbnormal = true,
                                IsRead = false,
                                NotificationSource = NotificationSourceEnum.App
                            };
                        }
                        AppNotification.SendNotification(notification);
                        AddUserLoginLog(user);
                    }
                    return Json(new { result = true, message = "登录成功，正在跳转...", reUrl = returnUrl });
                }
                else
                {
                    return Json(new { result = false, message = "用户名或密码错误，请检查后再继续。" });
                }
            }
            else
            {
                return Json(new { result = false, message = "用户不存在，请注册后再进行登录操作！" });
            }
        }

        /// <summary>
        /// 普通用户资料注册管理
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register([Bind("Id,IsNew,UserName,EMail,Password,ConfirmPassword")]ApplicationUserVM boVM)
        {
            if (!GetSiteConfiguration().CanRegister)
            {
                return Json(new { result = false, message = "管理员已经禁用注册！请关注公告!" });
            }
            if (GetCurrUserName() != null)
            {
                return RedirectToAction("UserCenter", "Account");
            }
            if (boVM.UserName == null || boVM.EMail == null || boVM.Password == null || boVM.ConfirmPassword == null)
            {
                return Json(new { result = false, message = "表单中的所有必填项存在空值，请检查！" });
            }
            if (_HasTheSameUser(boVM.UserName))
            {
                return Json(new { result = false, message = "当前用户名已经被使用了！" });
            }
            var user = new ApplicationUser(EncodeFilterHelper.EncodeHtml(boVM.UserName))
            {
                Email = EncodeFilterHelper.EncodeHtml(boVM.EMail)
            };

            //普通用户
            const string averageUser = "AverageUser";
            //判断用户组是否存在
            var roleExists = await _roleManager.RoleExistsAsync(averageUser);
            if (!roleExists)
            {
                ApplicationRole userRole = new ApplicationRole() { Name = averageUser, DisplayName = "普通注册用户", Description = "适用于普通注册用户", ApplicationRoleType = ApplicationRoleTypeEnum.适用于普通注册用户, SortCode = "99avf56g" };
                await _roleManager.CreateAsync(userRole);
            }
            var a1 = await _userManager.CreateAsync(user);
            var a2 = await _userManager.AddPasswordAsync(user, boVM.Password);

            //注册完成添加默认头像
            var avatar = new BusinessImage
            {
                Name = string.Empty,
                DisplayName = string.Empty,
                OriginalFileName = string.Empty,
                Type = ImageType.Avatars,
                RelevanceObjectId = Guid.Parse(user.Id),
                UploaderId = Guid.Parse(user.Id),
                Description = "这是用户【" + user.UserName + "】的个人头像",
                FileSize = 0,
                UploadPath = "../../images/Avatars/defaultAvatar.gif",
                PhysicalPath = string.Empty
            };
            await _businessImage.AddOrEditAndSaveAsyn(avatar);
            //查询用户是否已经添加了权限 若不在添加进用户组
            if (!await _userManager.IsInRoleAsync(user, averageUser))
            {
                var roleOK = await _userManager.AddToRoleAsync(user, averageUser);
            }
            if (a1.Succeeded && a2.Succeeded)
            {
                var message = "恭喜您注册成功,注册时间[" + user.CreateTime + "],请牢记您的用户名[" + user.UserName + "]。";
                var notification = new Notification
                {
                    Receiver = user,
                    Name = "用户注册",
                    Description = message,
                    Link = "javascript:",
                    IsAbnormal = false,
                    IsRead = false,
                    NotificationSource = NotificationSourceEnum.App
                };
                AppNotification.SendNotification(notification);
                return Json(new { result = true, message = "注册成功，请牢记您的账号密码！", userName = user.UserName, password = boVM.Password });
            }
            else
            {
                return Json(new { result = false, message = "注册失败，在短时间内只能注册一次！" });
            }

        }

        /// <summary>
        /// 获取网站配置
        /// </summary>
        /// <returns></returns>
        private SiteConfiguration GetSiteConfiguration()
        {
            var siteConfiguration = _siteConfiguration.GetAll().FirstOrDefault();
            return siteConfiguration;
        }


        /// <summary>
        /// 根据学校学号密码登录（后期打算实现）
        /// </summary>
        /// <param name="jsonLogonInformation"></param>
        /// <returns></returns>
        public async Task<IActionResult> LoginByStudentId(string jsonLogonInformation)
        {
            return Json(new { result = "" });
        }

        /// <summary>
        /// 网站条款和声明
        /// 说明：用户注册注册的时候选项之一
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SiteTermsAndStatement()
        {
            //获取用户名
            GetCurrUserName();

            ViewData["Message"] = "在这里写网站的使用条款，以及其他的一些声明等。。。的综合页面";
            return PartialView();
        }

        /// <summary>
        /// 是否存在指定用户名的用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        private bool _HasTheSameUser(string userName) => _userManager.Users.Any(x => x.UserName == userName);

        /// <summary>
        /// 未登录提示页面
        /// </summary>
        /// <returns></returns>
        public IActionResult NoLogin()
        {
            ViewData["Message"] = " 还未登录，请先登录后再访问";
            return PartialView();
        }

        /// <summary>
        /// 处理用户注销操作
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            var url = Url.Action("Index", "Account");
            return Json(new { result = true, reUrl = url });
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        [Authorize]
        private ApplicationUser GetUser()
        {
            try
            {
                var user = this._userManager.FindByNameAsync(User.Identity.Name).Result;
                return user = user == null ? null : user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取用户的视图模型
        /// 说明：包括用户头像
        /// </summary>
        /// <returns></returns>
        [Authorize]
        private ApplicationUserVM GetUserVM()
        {
            //IsShowAddDefaultDataBtn();
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
        private void GetUserAvatar()
        {
            var avatarPath = this._businessImage.GetAll()
                .FirstOrDefault(i => i.RelevanceObjectId == Guid.Parse(GetUser().Id)).UploadPath;
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
        /// 登录之前获取用户的头像（如果存在）
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>用户的头像路径</returns>
        public async Task<IActionResult> GetUserAvatarForLogin(string userName)
        {
            try
            {
                var user = await this._userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return Json(new { isOk = false });
                }
                else
                {
                    var avatar = this._businessImage.GetAll().FirstOrDefault(i => i.RelevanceObjectId == Guid.Parse(user.Id));
                    if (avatar != null)
                    {
                        return Json(new { isOk = true, avatarPath = avatar.UploadPath });
                    }
                    else
                    {
                        return Json(new { isOk = false });
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { isOk = false });
            }
        }

        /// <summary>
        /// 判断是否显示添加默认数据按钮
        /// </summary>
        /// <returns></returns>
        public void IsShowAddDefaultDataBtn()
        {
            try
            {
                var userName = "Admin";
                var user = _userManager.FindByNameAsync(userName).Result;
                ViewBag.IsShowAddDefaultDataBtn = user == null ? true : false;
            }
            catch (Exception)
            {
                ViewBag.IsShowAddDefaultDataBtn = false;
            }
        }

        private UserLoginLog GetUserLoginLog(ApplicationUser user)
        {
            var loginLog = _userLoginLog.GetAll().OrderByDescending(x => x.loginTime).FirstOrDefault(x => x.User == user);
            return loginLog;
        }

        private void AddUserLoginLog(ApplicationUser user)
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

        /// <summary>
        /// 获取用户IP地址
        /// </summary>
        /// <returns></returns>
        private string GetClientIpAddress()
        {
            #region 获取客户端真实的IP地址
            GetClientIpAddress getClientIpAddress = new GetClientIpAddress(_httpContextAccessor);
            return getClientIpAddress.UserClientIpAddress;
            #endregion
        }

        #endregion
    }
}
