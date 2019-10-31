using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YiZhan.DataAccess;
using YiZhan.Entities;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.BusinessOrganization;
using YiZhan.Entities.BusinessManagement.User;
using YiZhan.ViewModels.BusinessManagement;
using YiZhan.Entities.BusinessManagement.Commodities;
using YiZhan.Common.JsonModels;
using YiZhan.Common.YZExtensions;
using System.Linq.Expressions;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using YiZhan.Entities.Notifications;
using YiZhan.Web.App.CommonHelper;
using YiZhan.Entities.WebSettingManagement;
using YiZhan.ViewModels.WebSettingManagement;
using YiZhan.ViewModels.BusinessManagement.CommodityVM;

namespace YiZhan.Web.Controllers
{
    public class HomeController : Controller
    {
        private YZNotification AppNotification { get; set; }
        private string ClientIpAddress => GetClientIpAddress();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEntityRepository<Notification> _notification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEntityRepository<Person> _peson;
        private readonly IEntityRepository<BusinessImage> _businessImage;
        private readonly IEntityRepository<YZ_Commodity> _commodity;
        private readonly IEntityRepository<YZ_CommodityLookCount> _commodityLookCount;
        private readonly IEntityRepository<YZ_CommodityComment> _commodityComment;
        private readonly IEntityRepository<YZ_CommodityCategory> _commodityCategory;
        private readonly IEntityRepository<YZ_CommodityAndImage> _commodityAndImage;
        private readonly IEntityRepository<YZ_UserSearchLog> _userSearchLog;
        private readonly IEntityRepository<YZ_UserVisitorLog> _userVisitorLog;
        private readonly IEntityRepository<Advertisement> _advertisement;
        private readonly IEntityRepository<SiteNotice> _siteNotice;


        public HomeController(
            IHttpContextAccessor httpContextAccessor,
            IEntityRepository<Notification> notification,
            UserManager<ApplicationUser> userManager,
            IEntityRepository<Person> person,
            IEntityRepository<BusinessImage> businessImage,
            IEntityRepository<YZ_Commodity> commodity,
            IEntityRepository<YZ_CommodityLookCount> commodityLookCount,
            IEntityRepository<YZ_CommodityComment> commodityComment,
            IEntityRepository<YZ_CommodityCategory> commodityCategory,
            IEntityRepository<YZ_CommodityAndImage> commodityAndImage,
            IEntityRepository<YZ_UserSearchLog> userSearchLog,
            IEntityRepository<YZ_UserVisitorLog> userVisitorLog,
            IEntityRepository<Advertisement> advertisement,
            IEntityRepository<SiteNotice> siteNotice
              )
        {
            _httpContextAccessor = httpContextAccessor;
            _notification = notification;
            _userManager = userManager;
            _peson = person;
            _businessImage = businessImage;
            _commodity = commodity;
            _commodityLookCount = commodityLookCount;
            _commodityComment = commodityComment;
            _commodityCategory = commodityCategory;
            _commodityAndImage = commodityAndImage;
            _userSearchLog = userSearchLog;
            _userVisitorLog = userVisitorLog;
            _advertisement = advertisement;
            _siteNotice = siteNotice;
            AppNotification = new YZNotification(notification);
        }

        /// <summary>
        /// App下载引导页面
        /// </summary>
        /// <returns></returns>
        public IActionResult DownloadApp()
        {
            //获取用户名
            GetCurrUserName();
            ViewData["Message"] = "下载属于你的手机易站、易站手机APP更方便！";
            return View();
        }

        /// <summary>
        /// 网站首页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            //获取广告和轮播
            GetAdvertisements();

            //获取所有的商品
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
            //最新发布的商品
            ViewBag.LatestRelease = commodityListForIndexVM;
            GetWhatYouLike();   //获取猜你喜欢的内容
            return View(commodityListForIndexVM);
        }

        /// <summary>
        /// 获取广告和轮播
        /// </summary>
        public void GetAdvertisements()
        {
            var advertisementsVM = new List<AdvertisementVM>();
            var advertisements = _advertisement.GetAll();
            foreach (var advertisement in advertisements)
            {
                var advertisementVM = new AdvertisementVM(advertisement);
                var image = _businessImage.GetSingleBy(x => x.RelevanceObjectId == advertisement.Id);
                advertisementVM.Image = image;
                advertisementsVM.Add(advertisementVM);
            }
            ViewBag.Advertisements = advertisementsVM;
        }



        /// <summary>
        /// 商品详细页面
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetCommodityDetail(Guid id)
        {
            GetAdvertisements();
            GetCurrUserName();
            await AddUserVisitorLog(id);
            var images = new List<BusinessImage>();
            var viewModel = new YZ_CommodityVM();
            var model = this._commodity.GetAllIncluding(x => x.Category, x => x.Comments, x => x.AscriptionUser, x => x.Images, x => x.LookCount).FirstOrDefault(x => x.Id == id);
            if (model != null)
            {
                if (model.AscriptionUser != null)
                {
                    var userAvatar = _businessImage.GetAll().FirstOrDefault(i => i.RelevanceObjectId == Guid.Parse(model.AscriptionUser.Id));
                    if (userAvatar != null)
                    {
                        model.AscriptionUser.Avatar = userAvatar;
                    }
                }
                images = this._businessImage.GetAll().Where(x => x.RelevanceObjectId == model.Id).ToList();
                viewModel = new YZ_CommodityVM(model);
                viewModel.Images = images;
                viewModel.Comments = await GetCommodityCommentsVM(model.Id);
                var lookCount = model.LookCount;
                lookCount.LookCount += 1;
                _commodityLookCount.EditAndSave(lookCount);
                return PartialView("../../Views/CommodityManagement/CommodityDetail", viewModel);
            }
            return RedirectToAction("Error", "Home");
        }

        /// <summary>
        /// 根据商品的Id获取该商品下的留言（返回实体）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<List<YZ_CommodityComment>> GetCommodityComments(Guid id)
        {
            var commodityComments = new List<YZ_CommodityComment>();
            var query = await _commodityComment.GetAllIncludingAsyn(x => x.Commodity, x => x.CommentUser);
            commodityComments = query.Where(x => x.Commodity.Id == id).ToList();
            var userAvatarQuery = await _businessImage.GetAllAsyn();
            if (commodityComments.Count() > 0)
            {
                foreach (var commodityComment in commodityComments)
                {
                    var userAvatar = userAvatarQuery.FirstOrDefault(x => x.RelevanceObjectId == Guid.Parse(commodityComment.CommentUser.Id));
                    commodityComment.CommentUser.Avatar = userAvatar;
                }
            }
            return commodityComments;
        }

        /// <summary>
        /// 根据商品的Id获取该商品下的留言（返回视图模型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<List<YZ_CommodityCommentVM>> GetCommodityCommentsVM(Guid id)
        {
            var commodityCommentsVM = new List<YZ_CommodityCommentVM>();
            var queryCommodities = await _commodity.GetAllIncludingAsyn(x => x.AscriptionUser);
            var query = await _commodityComment.GetAllIncludingAsyn(x => x.Commodity, x => x.CommentUser);
            if (query.Count() > 0)
            {
                var user = GetUser();
                var commodityComments = query.Where(x => x.Commodity.Id == id).ToList();
                if (commodityComments.Count() > 0)
                {
                    foreach (var commodityComment in commodityComments.OrderByDescending(x => x.CreateTime))
                    {
                        var commodityCommentVM = new YZ_CommodityCommentVM(commodityComment);
                        var userAvatarQuery = await _businessImage.GetAllAsyn();
                        var userAvatar = userAvatarQuery.FirstOrDefault(x => x.RelevanceObjectId == Guid.Parse(commodityComment.CommentUser.Id));
                        commodityCommentVM.CommentUser.Avatar = userAvatar;
                        var commodity = queryCommodities.FirstOrDefault(x => x.Id == commodityComment.Commodity.Id);
                        if (commodity != null)
                        {
                            if (user != null)
                            {
                                if (commodity.AscriptionUser == user || commodityComment.CommentUser == user)
                                {
                                    commodityCommentVM.IsShowDeleteBtn = true;
                                }
                                if (commodityComment.CommentUser == user)
                                {
                                    commodityCommentVM.CommentUser.Name = "٩(๑❛ᴗ❛๑)۶我";
                                }
                            }
                            else
                            {
                                commodityCommentVM.IsShowDeleteBtn = false;
                            }
                        }
                        commodityCommentsVM.Add(commodityCommentVM);
                    }
                }
            }
            return commodityCommentsVM;
        }


        /// <summary>
        /// 网站搜索
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<IActionResult> Search(string keyword)
        {
            //获取用户名
            GetCurrUserName();
            var commoditiesForSearch = this._commodity.
                GetAllIncluding(x => x.Category, x => x.AscriptionUser, x => x.Images)
                .Where(x => x.State == YZ_CommodityState.OnSale);
            var commodityImage = new BusinessImage();
            var commodityImages = new List<BusinessImage>();
            var commodityForSearchVM = new YZ_CommodityVM();
            var commodityListForSearchVM = new List<YZ_CommodityVM>();
            if (!String.IsNullOrEmpty(keyword))
            {
                Expression<Func<YZ_Commodity, bool>> condition = x =>
                x.Name.Contains(keyword) ||
                x.Description.Contains(keyword) ||
                x.Category.Name.Contains(keyword) ||
                x.AscriptionUser.ChineseFullName.Contains(keyword);

                if (commoditiesForSearch.Count() > 0)
                {
                    foreach (var commodity in commoditiesForSearch.Where(condition))
                    {
                        commodityImage = _businessImage.FindBy(m => m.RelevanceObjectId == commodity.Id).FirstOrDefault(m => m.Type == ImageType.CommodityCover);
                        commodityImages = new List<BusinessImage>();
                        commodityImages.Add(commodityImage);
                        commodityForSearchVM = new YZ_CommodityVM(commodity);
                        commodityForSearchVM.Images = commodityImages;
                        commodityListForSearchVM.Add(commodityForSearchVM);
                    }
                    commodityListForSearchVM.OrderByDescending(m => m.AddTime);
                    //添加搜索记录
                    if (commodityListForSearchVM.Count > 0)
                    {
                        AddSearchLog(keyword);
                    }
                }
                else
                {
                    commodityListForSearchVM = null;
                }
                ViewBag.Keyword = keyword;
            }
            else
            {
                if (commoditiesForSearch.Count() > 0)
                {
                    foreach (var commodity in commoditiesForSearch.ToList())
                    {
                        if (commodityListForSearchVM.Count() >= 30)
                        {
                            break;
                        }
                        commodityImage = _businessImage.FindBy(m => m.RelevanceObjectId == commodity.Id).FirstOrDefault(m => m.Type == ImageType.CommodityCover);
                        commodityImages = new List<BusinessImage>();
                        commodityImages.Add(commodityImage);
                        commodityForSearchVM = new YZ_CommodityVM(commodity);
                        commodityForSearchVM.Images = commodityImages;
                        commodityListForSearchVM.Add(commodityForSearchVM);
                    }
                    commodityListForSearchVM.OrderByDescending(m => m.AddTime);
                }
                else
                {
                    commodityListForSearchVM = null;
                }
                ViewBag.Keyword = null;
            }

            GuessYouSearch();                //获取猜你想搜
            await GetHotSearch();            //热门搜索  
            await GetCommodityCategorList(); //获取所有分类

            return View(commodityListForSearchVM);
        }

        /// <summary>
        /// 添加搜索记录
        /// </summary>
        /// <returns></returns>
        public void AddSearchLog(string keyword)
        {
            var userIp = ClientIpAddress;
            var searchLog = new YZ_UserSearchLog();
            if (GetUser() != null)
            {
                searchLog = _userSearchLog.GetSingleBy(x => x.Name == keyword && x.UserIdOrIp == GetUser().Id);
            }
            else
            {
                searchLog = _userSearchLog.GetSingleBy(x => x.Name == keyword && x.UserIdOrIp == userIp);
            }
            if (searchLog == null)
            {
                searchLog = new YZ_UserSearchLog
                {
                    Name = keyword,
                    Description = string.Empty,
                    UserIdOrIp = GetUser() == null ? userIp : GetUser().Id
                };
                _userSearchLog.AddAndSave(searchLog);
            }
        }

        /// <summary>
        /// 获取所有的分类
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetCommodityCategorList()
        {
            var categories = await _commodityCategory.GetAllAsyn();
            var commodityCategoryListVM = new List<YZ_CommodityCategoryVM>();
            //获取所有分类
            foreach (var item in categories)
            {
                var commodityCategoryVM = new YZ_CommodityCategoryVM(item);
                commodityCategoryListVM.Add(commodityCategoryVM);
            }
            ViewBag.CommodityCategory = commodityCategoryListVM;
            return commodityCategoryListVM.Count();
        }

        /// <summary>
        /// 获取热搜
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetHotSearch()
        {
            var searchLogLisVM = new List<YZ_UserSearchLogVM>();
            var searchLogvM = new YZ_UserSearchLogVM();
            var list = await _userSearchLog.GetAllAsyn();
            list = list.OrderByDescending(x => x.SearchTime);
            foreach (var item in list.YZDistinct(x => x.Name))
            {
                if (searchLogLisVM.Count() >= 6)
                {
                    break;
                }
                if (GetUser() != null)
                {
                    if (item.UserIdOrIp != GetUser().Id)
                    {
                        searchLogvM = new YZ_UserSearchLogVM(item);
                        searchLogLisVM.Add(searchLogvM);
                    }
                }
                else
                {
                    if (item.UserIdOrIp != ClientIpAddress)
                    {
                        searchLogvM = new YZ_UserSearchLogVM(item);
                        searchLogLisVM.Add(searchLogvM);
                    }
                }
            }
            ViewBag.HotSearch = searchLogLisVM;
            return searchLogLisVM.Count();

        }

        /// <summary>
        /// 猜你想搜
        /// </summary>
        /// <returns></returns>
        public void GuessYouSearch()
        {
            var searchLogListVM = new List<YZ_UserSearchLogVM>();
            var searchLogs = _userSearchLog.GetAll();
            var searchLogList = new List<YZ_UserSearchLog>();
            if (GetUser() != null)
            {
                searchLogList = searchLogs.Where(x => x.UserIdOrIp == GetUser().Id).OrderByDescending(x => x.SearchTime).ToList();
            }
            else
            {
                var userIp = ClientIpAddress;
                searchLogList = searchLogs.Where(x => x.UserIdOrIp == userIp).OrderByDescending(x => x.SearchTime).ToList();
            }
            foreach (var item in searchLogList)
            {
                if (searchLogListVM.Count() > 8)
                {
                    break;
                }
                var searchLogVM = new YZ_UserSearchLogVM(item);
                searchLogListVM.Add(searchLogVM);
            }
            ViewBag.GuessYouSearch = searchLogListVM;
        }

        /// <summary>
        /// 获取猜你喜欢的数据（首页）
        /// </summary>
        /// <returns></returns>
        public void GetWhatYouLike()
        {
            //根据用户最近浏览的商品类别找到类似的商品     
            var commodityLisForLiketVM = new List<YZ_CommodityVM>();
            var visitorLogs = new List<YZ_UserVisitorLog>();
            if (GetCurrUserName() != null)
            {
                visitorLogs = _userVisitorLog.GetAllIncluding(x => x.Category)
                    .Where(x => x.UserIdOrIp == GetUser().Id)
                    .ToList();
                commodityLisForLiketVM = GetCommodityVMForGuessYouLike(visitorLogs);
            }
            else
            {
                var userIp = ClientIpAddress;
                visitorLogs = _userVisitorLog.GetAllIncluding(x => x.Category)
                    .Where(x => x.UserIdOrIp == userIp)
                    .ToList();
                commodityLisForLiketVM = GetCommodityVMForGuessYouLike(visitorLogs);
            }
            //总共是18条数据
            ViewBag.GuessWhatYouLike = commodityLisForLiketVM.Count() == 0 ? null : commodityLisForLiketVM;
        }

        /// <summary>
        /// 获取商品视图
        /// </summary>
        /// <returns></returns>
        public List<YZ_CommodityVM> GetCommodityVMForGuessYouLike(List<YZ_UserVisitorLog> visitorLogList)
        {
            var commodityLisForLiketVM = new List<YZ_CommodityVM>();
            var visitorLogs = visitorLogList;
            visitorLogs = visitorLogs.OrderByDescending(v => v.LookTime).ToList();
            if (visitorLogs.Count() > 0)
            {
                //去除重复的数据对象
                visitorLogs = visitorLogs.YZDistinct(x => x.Category).OrderByDescending(x => x.LookTime).ToList();
                foreach (var item in visitorLogs)
                {
                    //一共取15条数据
                    if (commodityLisForLiketVM.Count() > 15)
                    {
                        break;
                    }
                    var listCount = 0;
                    var commodityForLikeList = _commodity.GetAllIncluding(x => x.Category).Where(x => x.Category == item.Category).ToList();
                    if (commodityForLikeList.Count() > 0)
                    {
                        foreach (var commodityForLikeItem in commodityForLikeList)
                        {
                            if (listCount >= 5) { break; }
                            var commodityImageForLike = _businessImage.FindBy(m => m.RelevanceObjectId == commodityForLikeItem.Id).FirstOrDefault(m => m.Type == ImageType.CommodityCover);
                            var commodityImagesForLike = new List<BusinessImage>();
                            commodityImagesForLike.Add(commodityImageForLike);
                            var commodityForLikeVM = new YZ_CommodityVM(commodityForLikeItem);
                            commodityForLikeVM.Images = commodityImagesForLike;
                            commodityLisForLiketVM.Add(commodityForLikeVM);
                            listCount++;
                        }
                    }
                }
            }
            return commodityLisForLiketVM;
        }


        /// 获取所有的公告
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SiteNotices()
        {
            var siteNoticesVM = new List<SiteNoticeVM>();
            var query = await _siteNotice.GetAllListAsyn();
            var siteNotices = query.OrderByDescending(x => x.CreateTime);
            foreach (var siteNotice in siteNotices)
            {
                var siteNoticeVM = new SiteNoticeVM(siteNotice);
                siteNoticeVM.OrderNumber = (siteNoticesVM.Count() + 1).ToString();
                siteNoticesVM.Add(siteNoticeVM);
            }
            return View(siteNoticesVM);
        }

        public IActionResult About()
        {
            //获取用户名
            GetCurrUserName();

            ViewData["Message"] = "这里是关于易站介绍的页面，代码也将会在此处开始写！";

            return View();
        }

        public IActionResult Contact()
        {
            //获取用户名
            GetCurrUserName();

            ViewData["Message"] = "联系我们，联系我们页面的代码将在这里开始写！";

            return View();
        }

        public IActionResult CopyRight()
        {
            //获取用户名
            GetCurrUserName();

            ViewData["Message"] = "版权声明：本站源码以及开发文档在未经【2018毕业设计团队】的书面许可不允许公开！";
            return View();

        }


        /// <summary>
        /// 文件上传Demo
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult FilesUpload()
        {
            //获取用户名
            GetCurrUserName();

            ViewData["Message"] = "文件上传Demo";
            return PartialView();
        }



        /// <summary>
        /// 用户登录后点击商品添加用户查看历史
        /// </summary>
        /// <param name="id"></param>
        public async Task<bool> AddUserVisitorLog(Guid id)
        {
            try
            {
                var visitorLog = new YZ_UserVisitorLog();
                var commodity = this._commodity.GetAllIncluding(x => x.Category).FirstOrDefault(x => x.Id == id);

                if (commodity != null)
                {
                    var userIp = ClientIpAddress;
                    if (GetUser() != null)
                    {
                        visitorLog = _userVisitorLog.GetSingleBy(x => x.CommodityId == commodity.Id && x.UserIdOrIp == GetUser().Id);
                    }
                    else
                    {
                        visitorLog = _userVisitorLog.GetSingleBy(x => x.CommodityId == commodity.Id && x.UserIdOrIp == userIp);
                    }
                    if (visitorLog == null)
                    {
                        visitorLog = new YZ_UserVisitorLog
                        {
                            CommodityId = commodity.Id,
                            Category = commodity.Category,
                            UserIdOrIp = GetUser() == null ? userIp : GetUser().Id
                        };
                    }
                    else
                    {
                        visitorLog.LookTime = DateTime.Now;
                    }
                    await this._userVisitorLog.AddOrEditAndSaveAsyn(visitorLog);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取用户IP地址
        /// </summary>
        /// <returns></returns>
        public string GetClientIpAddress()
        {
            GetClientIpAddress getClientIpAddress = new GetClientIpAddress(_httpContextAccessor);
            return getClientIpAddress.UserClientIpAddress;

        }




        #region 获取用户信息


        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public ApplicationUser GetUser()
        {
            try
            {
                var user = this._userManager.FindByNameAsync(User.Identity.Name).Result;
                return user == null ? null : user;
            }
            catch (Exception)
            {
                return null;
            }

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
        public string GetCurrUserName()
        {
            try
            {
                //IsShowAddDefaultDataBtn();
                var userName = User.Identity.Name;
                if (userName != null)
                {
                    //var user = _userManager.FindByNameAsync(userName).Result;                 
                    //var userFullName = user.ChineseFullName;
                    GetUserAvatar();//获取用户头像
                    var user = GetUser();
                    ViewBag.CurrUserName = user.ChineseFullName == null ? user.UserName : user.ChineseFullName;
                    return user.ChineseFullName == null ? user.UserName : user.ChineseFullName;
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

        #endregion

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

        public IActionResult UserFeedback()
        {
            return View();
        }
    }
}
