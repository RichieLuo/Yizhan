using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YiZhan.Common.JsonModels;
using YiZhan.Common.YZExtensions;
using YiZhan.DataAccess;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.BusinessManagement.Commodities;
using YiZhan.Entities.BusinessManagement.User;
using YiZhan.Entities.BusinessOrganization;
using YiZhan.Entities.Notifications;
using YiZhan.ViewModels.BusinessManagement;
using YiZhan.ViewModels.BusinessManagement.CommodityVM;
using YiZhan.ViewModels.BusinessManagement.User;
using YiZhan.Web.App.CommonHelper;

namespace YiZhan.Web.Controllers.Commodities
{
    /// <summary>
    /// 首页共用的商品管理控制器
    /// </summary>
    public class CommoditiesManagerController : Controller
    {
        public YZNotification AppNotification { get; set; }
        private readonly IEntityRepository<Notification> _notification;
        private IHostingEnvironment _hostingEnv;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEntityRepository<Person> _peson;
        private readonly IEntityRepository<BusinessImage> _businessImage;
        private readonly IEntityRepository<YZ_Commodity> _commodity;
        private readonly IEntityRepository<YZ_CommodityLookCount> _commodityLookCount;
        private readonly IEntityRepository<YZ_CommodityCategory> _commodityCategory;
        private readonly IEntityRepository<YZ_CommodityAndImage> _commodityAndImage;
        private readonly IEntityRepository<YZ_UserSearchLog> _userSearchLog;
        private readonly IEntityRepository<YZ_UserVisitorLog> _userVisitorLog;
        private readonly IEntityRepository<YZ_CommodityExamine> _commodityExamine;
        private readonly IEntityRepository<YZ_CommodityComment> _commodityComment;
        private readonly IEntityRepository<YZ_Order> _order;

        public CommoditiesManagerController(
            IEntityRepository<Notification> notification,
            IHostingEnvironment hostingEnv,
            UserManager<ApplicationUser> userManager,
            IEntityRepository<Person> person,
            IEntityRepository<BusinessImage> businessImage,
            IEntityRepository<YZ_Commodity> commodity,
            IEntityRepository<YZ_CommodityLookCount> commodityLookCount,
            IEntityRepository<YZ_CommodityCategory> commodityCategory,
            IEntityRepository<YZ_CommodityAndImage> commodityAndImage,
            IEntityRepository<YZ_UserSearchLog> userSearchLog,
            IEntityRepository<YZ_UserVisitorLog> userVisitorLog,
            IEntityRepository<YZ_CommodityExamine> commodityExamine,
            IEntityRepository<YZ_CommodityComment> commodityComment,
            IEntityRepository<YZ_Order> order
              )
        {
            _notification = notification;
            _hostingEnv = hostingEnv;
            _userManager = userManager;
            _peson = person;
            _businessImage = businessImage;
            _commodity = commodity;
            _commodityLookCount = commodityLookCount;
            _commodityCategory = commodityCategory;
            _commodityAndImage = commodityAndImage;
            _userSearchLog = userSearchLog;
            _userVisitorLog = userVisitorLog;
            _commodityExamine = commodityExamine;
            _commodityComment = commodityComment;
            _order = order;
            AppNotification = new YZNotification(notification);

        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 我发布的闲置商品
        /// </summary>
        /// <returns></returns>
        public IActionResult MySecondhand()
        {
            //获取当前用户名
            GetCurrUserName();
            //获取所有的商品
            var commodityListVM = new List<YZ_CommodityVM>();
            commodityListVM = GetYZCommodityVMs(x => x.AscriptionUser == GetUser() && x.State == YZ_CommodityState.OnSale);
            return PartialView("../../Views/Account/UserCenterPartialViews/_MySecondhand", commodityListVM);
        }

        /// <summary>
        /// 获取用户中心的统计
        /// </summary>
        /// <returns></returns>
        public UserCenterCount GetMyCount()
        {
            var userCenterCount = new UserCenterCount();
            var commoditiesVM = GetYZCommodityVMs(x => x.AscriptionUser == GetUser());
            if (commoditiesVM.Count > 0)
            {
                var notExamineCount = commoditiesVM.Where(x => x.State == YZ_CommodityState.IsReject);
                var awaitExamineCount = commoditiesVM.Where(x => x.State == YZ_CommodityState.IsExamine);
                var commoditiesCount = commoditiesVM.Where(x => x.State == YZ_CommodityState.OnSale);
                userCenterCount = new UserCenterCount
                {
                    AwaitExamineCount = awaitExamineCount.Count(),
                    NotExamineCount = notExamineCount.Count(),
                    CommoditiesCount = commoditiesCount.Count()
                };
            }
            return userCenterCount;
        }

        #region 浏览记录

        #endregion

        /// <summary>
        /// 根据条件查询一条商品数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public YZ_CommodityVM GetYZCommodityVM(Expression<Func<YZ_Commodity, bool>> predicate)
        {
            var commodityVM = new YZ_CommodityVM();
            var commodity = this._commodity
                .GetAllIncluding(x => x.Category, x => x.AscriptionUser, x => x.Images, x => x.LookCount)
                .FirstOrDefault(predicate);
            if (commodity != null)
            {
                var commodityImageList = _businessImage.FindBy(m => m.RelevanceObjectId == commodity.Id).ToList();
                var userAvatar = _businessImage.GetAll().FirstOrDefault(i => i.RelevanceObjectId == Guid.Parse(commodity.AscriptionUser.Id));
                if (userAvatar != null)
                {
                    commodity.AscriptionUser.Avatar = userAvatar;
                }
                commodityVM = new YZ_CommodityVM(commodity);
                commodityVM.Images = commodityImageList;
                commodityVM.IsNew = false;
            }
            return commodityVM;
        }


        /// <summary>
        /// 根据查询条件 获取商品的集合
        /// </summary>
        /// <param name="predicate">查询参数：使用lambda表达式</param>
        /// <returns></returns>
        public List<YZ_CommodityVM> GetYZCommodityVMs(Expression<Func<YZ_Commodity, bool>> predicate)
        {
            var commodityListVM = new List<YZ_CommodityVM>();
            var commodityList = this._commodity
                .GetAllIncluding(x => x.Category, x => x.AscriptionUser, x => x.Images, x => x.LookCount)
                .Where(predicate)
                .OrderByDescending(c => c.AddTime).ToList();
            if (commodityList.Count() > 0)
            {
                foreach (var commodity in commodityList)
                {
                    var commodityImageList = _businessImage.FindBy(m => m.RelevanceObjectId == commodity.Id).ToList();
                    var userAvatar = _businessImage.GetAll().FirstOrDefault(i => i.RelevanceObjectId == Guid.Parse(commodity.AscriptionUser.Id));
                    if (userAvatar != null)
                    {
                        commodity.AscriptionUser.Avatar = userAvatar;
                    }
                    var commodityVM = new YZ_CommodityVM(commodity);
                    commodityVM.Images = commodityImageList;
                    commodityVM.OrderNumber = (commodityListVM.Count() + 1).ToString();
                    commodityListVM.Add(commodityVM);
                }
                commodityListVM.OrderByDescending(m => m.AddTime);
            }
            return commodityListVM;
        }

        /// <summary>
        /// 获取添加或删除商品页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult AddOrEditCommodity(Guid? id, int? pageIndex)
        {
            var viewModel = new YZ_CommodityVM();
            if (id.HasValue)
            {
                viewModel = GetYZCommodityVM(x => x.Id == id);
            }
            ViewBag.PageIndex = pageIndex;
            ViewBag.Category = GetCategories();
            return PartialView("../../Views/Account/UserCenterPartialViews/_AddOrEditCommodity", viewModel);
        }

        /// <summary>
        /// 添加或编辑商品
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]

        public async Task<JsonResult> AddOrEditCommodity([Bind("Id,IsNew,Name,Description,Price,Unit,Stock,State,Way,Range,IsBargain,CategoryId")] YZ_CommodityVM boVM)
        {
            var commodity = new YZ_Commodity();
            var category = _commodityCategory.GetAll().FirstOrDefault(x => x.Id == boVM.CategoryId);
            if (boVM.IsNew)
            {
                var lookCount = new YZ_CommodityLookCount { LookCount = 0 };
                _commodityLookCount.AddAndSave(lookCount);
                commodity = new YZ_Commodity
                {
                    Name = EncodeFilterHelper.EncodeHtml(boVM.Name),
                    Description = EncodeFilterHelper.EncodeHtml(boVM.Description),
                    Unit = EncodeFilterHelper.EncodeHtml(boVM.Unit),
                    State = YZ_CommodityState.IsExamine,
                    Category = category == null ? null : category,
                    Price = boVM.Price,
                    Stock = boVM.Stock,
                    LookCount = lookCount,
                    AscriptionUser = GetUser(),
                    EditTime = DateTime.Now,
                    IsBargain = boVM.IsBargain,
                    Way = boVM.Way,
                    Range = boVM.Range
                };
            }
            else
            {
                commodity = await _commodity.GetSingleAsyn(boVM.Id);
                if (commodity != null)
                {
                    commodity.Name = EncodeFilterHelper.EncodeHtml(boVM.Name);
                    commodity.Description = EncodeFilterHelper.EncodeHtml(boVM.Description);
                    commodity.Price = boVM.Price;
                    commodity.Unit = EncodeFilterHelper.EncodeHtml(boVM.Unit);
                    commodity.Stock = boVM.Stock;
                    commodity.AddTime = commodity.AddTime;
                    commodity.State = boVM.State; //调试时不修改商品状态
                    commodity.Category = category == null ? null : category;
                    commodity.EditTime = DateTime.Now;
                    commodity.IsBargain = boVM.IsBargain;
                    commodity.Way = boVM.Way;
                    commodity.Range = boVM.Range;
                }
            }
            //图片上传独立处理
            AddCommodityImg(commodity);
            var result = await _commodity.AddOrEditAndSaveAsyn(commodity);
            if (result)
            {
                if (boVM.IsNew)
                {
                    return Json(new { result = true, message = "添加成功！" });
                }
                else
                {
                    return Json(new { result = true, message = "修改成功！" });
                }
            }
            else
            {
                return Json(new { result = false, message = "操作失败！" });
            }
        }

        /// <summary>
        ///获取待审核的商品
        /// </summary>
        /// <returns></returns>    
        [Authorize]
        public IActionResult GetAwaitExamineCommodities()
        {
            var viewModel = GetYZCommodityVMs(x => x.AscriptionUser == GetUser() && x.State == YZ_CommodityState.IsExamine);
            var pageHelpers = new List<CommodityPageHelper>
            {
                //new CommodityPageHelper{btnTitle = "发布闲置",btnClass = "userCenterCreateBtn1",btnMethod = "LoadAddOrEditCommodityView()" }
            };
            LoadPageHelper("待审核的商品", 7, false, pageHelpers);
            return PartialView("../../Views/Account/UserCenterPartialViews/_GetCommodities", viewModel);
        }

        /// <summary>
        ///获取审核未通过的商品
        /// </summary>
        /// <returns></returns>    
        [Authorize]
        public IActionResult GetIsNotExamineCommodities()
        {
            var viewModel = GetYZCommodityVMs(x => x.AscriptionUser == GetUser() && x.State == YZ_CommodityState.IsReject);
            foreach (var item in viewModel)
            {
                var examineLog = _commodityExamine.GetAll().OrderByDescending(x => x.ExamineTime).FirstOrDefault(x => x.Commodity.Id == item.Id && x.State == YZ_CommodityState.IsReject);
                if (examineLog != null)
                {
                    item.ExamineDescription = examineLog.Description;
                }
            }
            var pageHelpers = new List<CommodityPageHelper>
            {
                //new CommodityPageHelper{btnTitle = "发布闲置",btnClass = "userCenterCreateBtn1",btnMethod = "LoadAddOrEditCommodityView()"},
                //new CommodityPageHelper{btnTitle = "批量删除",btnClass = "userCenterCreateBtn2",btnMethod = ""},
            };
            LoadPageHelper("审核未通过的商品", 8, true, pageHelpers);
            return PartialView("../../Views/Account/UserCenterPartialViews/_GetCommodities", viewModel);
        }

        /// <summary>
        /// 加载商品公共页面的一些信息
        /// </summary>
        /// <param name="pageTitle">页面的标题</param>
        /// <param name="helper">一个CommodityPageHelper集合</param>
        public void LoadPageHelper(string pageTitle, int pageIndex, bool isExaminePage, List<CommodityPageHelper> helper)
        {
            ViewBag.CurrPageTitle = pageTitle;
            ViewBag.BtnHelper = helper.Count() == 0 ? null : helper;
            ViewBag.IsExaminePage = isExaminePage;
            ViewBag.PageIndex = pageIndex;
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>  
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteCommodity(Guid id)
        {
            #region 旧版商品删除
            //try
            //{
            //    if (id == Guid.Empty)
            //    {
            //        return Json(new { result = false, message = "删除失败！" });
            //    }
            //    var query = await _commodity.GetAllIncludingAsyn(x => x.AscriptionUser, x => x.LookCount);
            //    if (query.Count() == 0)
            //    {
            //        return Json(new { result = false, message = "商品不存在！" });
            //    }
            //    var user = GetUser();
            //    var entities = query.Where(x => x.AscriptionUser == user);
            //    if (entities.Count() == 0)
            //    {
            //        return Json(new { result = false, message = "商品不存在！" });
            //    }
            //    var commodity = entities.FirstOrDefault(x => x.Id == id);
            //    if (commodity == null)
            //    {
            //        return Json(new { result = false, message = "商品不存在！" });
            //    }
            //    var examineEntities = await _commodityExamine.FindByAsyn(x => x.Commodity == commodity);
            //    if (examineEntities.Count() > 0)
            //    {
            //        foreach (var item in examineEntities)
            //        {
            //            _commodityExamine.DeleteAndSave(item);
            //        }
            //    }
            //    await _commodityLookCount.DeleteAndSaveAsyn(commodity.LookCount.Id);
            //    _commodity.DeleteAndSave(commodity);
            //    return Json(new { result = true });
            //}
            //catch (Exception)
            //{
            //    return Json(new { result = false, message = "删除失败！" });
            //}
            #endregion


            #region 商品不再进行删除,使用商品下架方式处理


            if (id == Guid.Empty)
            {
                return Json(new { result = false, message = "下架失败！" });
            }
            var commodity = _commodity.GetAllIncludingAsyn(x => x.AscriptionUser).Result.FirstOrDefault(x => x.Id.Equals(id));
            if (commodity == null)
            {
                return Json(new { result = false, message = "商品不存在！" });
            }
            var user = GetUser();
            if (commodity.AscriptionUser != user)
            {
                return Json(new { result = false, message = "下架失败！" });
            }
            commodity.State = YZ_CommodityState.CancelASale;
            var r = await _commodity.AddOrEditAndSaveAsyn(commodity);
            if (!r)
            {
                return Json(new { result = false, message = "下架失败！" });
            }
            return Json(new { result = true, message = "下架成功！" });

            #endregion
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteCommodities(Guid[] ids)
        {
            if (ids.Count() <= 0)
            {
                return Json(new { result = false, message = "删除失败！" });
            }
            var query = await _commodity.GetAllIncludingAsyn(x => x.AscriptionUser, x => x.LookCount);
            if (query.Count() == 0)
            {
                return Json(new { result = false, message = "商品不存在！" });
            }
            var user = GetUser();
            var entities = query.Where(x => x.AscriptionUser == user);
            if (entities.Count() == 0)
            {
                return Json(new { result = false, message = "商品不存在！" });
            }
            foreach (var id in ids)
            {
                var commodity = entities.FirstOrDefault(x => x.Id == id);
                if (commodity == null)
                {
                    continue;
                }
                var examineEntities = await _commodityExamine.FindByAsyn(x => x.Commodity == commodity);
                if (examineEntities.Count() > 0)
                {
                    foreach (var item in examineEntities)
                    {
                        _commodityExamine.DeleteAndSave(item);
                    }
                }
                await _commodityLookCount.DeleteAndSaveAsyn(commodity.LookCount.Id);
                _commodity.DeleteAndSave(commodity);
            }
            return Json(new { result = true });
        }

        /// <summary>
        /// 统一处理图片上传（添加）
        /// </summary>
        /// <param name="commodity"></param>
        private void AddCommodityImg(YZ_Commodity commodity)
        {
            try
            {
                var images = Request.Form.Files;
                var isCover = true;
                if (images != null)
                {
                    foreach (var image in images)
                    {
                        var currImageName = image.FileName;
                        var timeForFile = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-").Trim();
                        string extensionName = currImageName.Substring(currImageName.LastIndexOf("."));
                        var imageName = ContentDispositionHeaderValue
                                        .Parse(image.ContentDisposition)
                                        .FileName
                                        .Trim('"')
                                        .Substring(image.FileName.LastIndexOf("\\") + 1);
                        var newImageName = timeForFile + GetUser().Id + extensionName;
                        var boPath = "../../images/UploadImages/" + ImageType.CommodityImgs.ToString() + "/" + newImageName;
                        var imagePath = _hostingEnv.WebRootPath + $@"\images\UploadImages\{ImageType.CommodityImgs.ToString()}";
                        imageName = _hostingEnv.WebRootPath + $@"\images\UploadImages\{ImageType.CommodityImgs.ToString()}\{newImageName}";

                        Directory.CreateDirectory(imagePath); //创建目录
                        using (FileStream fs = System.IO.File.Create(imageName))
                        {
                            image.CopyTo(fs);
                            fs.Flush();
                        }
                        var commodityImg = new BusinessImage
                        {
                            Name = string.Empty,
                            DisplayName = string.Empty,
                            OriginalFileName = string.Empty,
                            Description = "这是闲置商品【" + commodity.Name + "】的图片",
                            RelevanceObjectId = commodity.Id,
                            UploaderId = Guid.Parse(GetUser().Id),
                            UploadPath = boPath,
                            PhysicalPath = imageName,
                            FileSize = image.Length,
                            Type = isCover ? ImageType.CommodityCover : ImageType.CommodityImgs
                        };
                        _businessImage.AddAndSave(commodityImg);
                        isCover = false;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 删除商品图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> DeleteCommodityImg(Guid id)
        {
            try
            {
                var commdoityImg = _businessImage.GetAll().FirstOrDefault(x => x.Id == id);
                if (commdoityImg == null)
                {
                    return Json(new { result = false, message = "删除失败！" });
                }
                if (commdoityImg.PhysicalPath != string.Empty)
                {
                    System.IO.File.Delete(commdoityImg.PhysicalPath);
                }
                var result = await _businessImage.DeleteAndSaveAsyn(id);
                if (result.DeleteSatus)
                {
                    return Json(new { result = true, message = "删除成功！" });
                }
                return Json(new { result = false, message = "删除失败！" });
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "删除失败！" });
            }
        }

        /// <summary>
        /// 获取所有的商品类别
        /// </summary>
        /// <returns></returns>
        public List<YZ_CommodityCategory> GetCategories()
        {
            var categories = _commodityCategory.GetAll().ToList();
            return categories;
        }

        /// <summary>
        /// 根据id获取单条商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<YZ_Commodity> GetCommodity(Guid id)
        {
            var query = await _commodity.GetAllIncludingAsyn(x => x.AscriptionUser);
            if (query.Count() > 0)
            {
                var entity = query.FirstOrDefault(x => x.Id == id);
                return entity;
            }
            return null;
        }

        #region 用户中心的订单管理

        /// <summary>
        /// 默认获取全部订单
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> GetMyOrders()
        {
            var result = new YZ_OrderDataListVM();

            //TODO:获取订单
            var currentUser = await _userManager.GetUserAsync(User);
            //所有订单(包括买家和卖家的订单)
            var allOrders = _order.GetAllIncludingAsyn(x => x.Commodity, x => x.Buyers, x => x.Seller).Result.ToList();
            allOrders.ForEach(x =>
              x.Commodity.Images = _businessImage.FindBy(b => b.Type.Equals(ImageType.CommodityCover) && b.RelevanceObjectId.Equals(x.Commodity.Id)).ToList()
            );

            #region 买家订单 查询

            //买家的所有订单
            var allOrdersWithBuyer = allOrders.Where(x => x.Buyers.Equals(currentUser)).ToList();

            //待付款 
            var waitToPaymentOrders = allOrdersWithBuyer.Where(x => x.State.Equals(YZ_OrderState.待付款));

            //待发货
            var waitToSendOutOrdersWithBuyer = allOrdersWithBuyer.Where(x => x.State.Equals(YZ_OrderState.待发货));

            //已完成（已收货）
            var successOrdersWithBuyer = allOrdersWithBuyer.Where(x => x.State.Equals(YZ_OrderState.已完成));

            var counter = 0;
            foreach (var order in allOrdersWithBuyer)
            {
                result.AllOrdersWithBuyer.Add(new YZ_OrderVM(order) { OrderNumber = (++counter).ToString() });
            }
            counter = 0;
            foreach (var order in waitToPaymentOrders)
            {
                result.WaitToPaymentOrdersWithBuyer.Add(new YZ_OrderVM(order) { OrderNumber = (++counter).ToString() });
            }
            counter = 0;
            foreach (var order in waitToSendOutOrdersWithBuyer)
            {
                result.WaitToSendOutOrdersWithBuyer.Add(new YZ_OrderVM(order) { OrderNumber = (++counter).ToString() });
            }
            counter = 0;
            foreach (var order in successOrdersWithBuyer)
            {
                result.SuccessOrdersWithBuyer.Add(new YZ_OrderVM(order) { OrderNumber = (++counter).ToString() });
            }
            #endregion

            #region 卖家订单查询


            //卖家全部订单
            var allOrdersWithSeller = allOrders.Where(x => x.Seller.Equals(currentUser)).ToList();
            //待发货订单
            var waitToSendOutOrdersWithSeller = allOrdersWithSeller.Where(x => x.State.Equals(YZ_OrderState.待发货)).ToList();
            //已完成订单
            var successOrdersWidthSeller = allOrdersWithSeller.Where(x => x.State.Equals(YZ_OrderState.已完成)).ToList();

            counter = 0;
            foreach (var order in allOrdersWithSeller)
            {
                result.AllOrdersWithSeller.Add(new YZ_OrderVM(order) { OrderNumber = (++counter).ToString() });
            }
            counter = 0;
            foreach (var order in waitToSendOutOrdersWithSeller)
            {
                result.WaitToSendOutOrdersWithSeller.Add(new YZ_OrderVM(order) { OrderNumber = (++counter).ToString() });
            }
            counter = 0;
            foreach (var order in successOrdersWidthSeller)
            {
                result.SuccessOrdersWithSeller.Add(new YZ_OrderVM(order) { OrderNumber = (++counter).ToString() });
            }

            #endregion


            return PartialView("../../Views/Account/UserCenterPartialViews/_MyOrders", result);
        }


        /// <summary>
        /// 处理发货
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<JsonResult> SendOut(Guid id)
        {
            //查询到订单
            //var waitToSendOutOrder = await _order.GetSingleAsyn(id);
            var waitToSendOutOrder = _order.GetAllIncludingAsyn(x => x.Buyers, x => x.Seller, x => x.Commodity).Result.Where(x => x.Id.Equals(id)).FirstOrDefault();
            var currentUser = await _userManager.GetUserAsync(User);
            if (waitToSendOutOrder == null)
            {
                return Json(new { isOk = false, message = "订单不存在！" });
            }
            if (!waitToSendOutOrder.Seller.Equals(currentUser))
            {
                return Json(new { isOk = false, message = "订单处理失败！" });
            }

            waitToSendOutOrder.State = YZ_OrderState.已完成;
            var r = await _order.AddOrEditAndSaveAsyn(waitToSendOutOrder);
            if (!r)
            {
                return Json(new { isOk = true, message = "订单处理失败！" });
            }

            //发送消息通知
            var message = "您购买的商品 [ " + waitToSendOutOrder.Commodity.Name + " ] 卖家于 [ " + DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss") + " ] 已经发货，请注意查收！";
            var notification = new Notification
            {
                Receiver = waitToSendOutOrder.Buyers,
                Name = "订单发货",
                Description = message,
                Link = "javascript:",
                IsAbnormal = false,
                IsRead = false,
                NotificationSource = NotificationSourceEnum.App
            };
            AppNotification.SendNotification(notification);
            return Json(new { isOk = true, message = "订单处理成功，已发货！" });
        }

        #endregion

        #region 商品的留言

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
        /// 获取该商品下的评论统计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> GetCommentCount(Guid id)
        {
            var commodityComments = await GetCommodityComments(id);
            var count = commodityComments.Count();
            return count;
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
        /// 获取单条留言（实体）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<YZ_CommodityComment> GetCommodityComment(Guid id)
        {
            var query = await _commodityComment.GetAllIncludingAsyn(x => x.CommentUser, x => x.Commodity);
            if (query.Count() > 0)
            {
                var entity = query.FirstOrDefault(x => x.Id == id);
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 根据表达式获取单条评论
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private async Task<YZ_CommodityComment> GetCommodityComment(Expression<Func<YZ_CommodityComment, bool>> predicate)
        {
            var query = await _commodityComment.GetAllIncludingAsyn(x => x.CommentUser, x => x.Commodity);
            if (query.Count() > 0)
            {
                var entity = query.Where(predicate).OrderByDescending(x => x.CreateTime);
                var commodityComment = entity.FirstOrDefault();
                return commodityComment;
            }
            return null;
        }

        /// <summary>
        /// 根据留言Id删除留言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteCommodityComment(Guid id)
        {
            //!删除条件：
            //1.自己的留言
            //2.该商品所属的用户的留言
            if (id == Guid.Empty)
            {
                return Json(new { result = false, message = "删除失败！" });
            }
            var query = await _commodityComment.GetAllIncludingAsyn(x => x.CommentUser, x => x.Commodity);
            if (query.Count() != 0)
            {
                var comment = query.FirstOrDefault(x => x.Id == id);
                if (comment != null)
                {
                    var commodity = await GetCommodity(comment.Commodity.Id);
                    if (commodity != null)
                    {
                        var user = GetUser();
                        if (commodity.AscriptionUser == user || comment.CommentUser == user)
                        {
                            var r = await _commodityComment.DeleteAndSaveAsyn(comment.Id);
                            if (r.DeleteSatus)
                            {
                                return Json(new { result = true });
                            }
                            else
                            {
                                return Json(new { result = false, message = "删除失败！" });
                            }
                        }
                    }
                }
                return RedirectToAction("AWarmWarning", "Error");
            }
            return Json(new { result = false, message = "删除失败！" });
        }

        /// <summary>
        /// 根据商品Id获取商品的留言（返回视图）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetCommodityCommentsView(Guid id)
        {
            var viewModel = await GetCommodityCommentsVM(id);
            return PartialView("../../Views/Home/_CommodityComments", viewModel);
        }

        /// <summary>
        /// 留言提交
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SubmitComment(YZ_CommodityCommentVM input)
        {
            if (input.Id == Guid.Empty)
            {
                return Json(new { result = false, message = "留言失败！" });
            }
            if (string.IsNullOrEmpty(input.Description))
            {
                return Json(new { result = false, message = "请填写留言内容！" });
            }
            var user = GetUser();
            var userComment = await GetCommodityComment(x => x.CommentUser == user && x.Commodity.Id == input.Id);
            if (userComment != null)
            {
                TimeSpan ts = (DateTime.Now - userComment.CreateTime);
                if (ts.Minutes < 3)
                {
                    return Json(new { result = false, message = "操作过于频繁，距离上一次留言时间过短\n请几分钟后再来留言吧!" });
                }
            }
            if (user == null)
            {
                return Json(new { result = false, noLogin = true, message = "您未登录无法进行留言，是否现在登录？" });
            }
            var commodity = await GetCommodity(input.Id);
            if (commodity != null)
            {
                var newComment = new YZ_CommodityComment
                {
                    CommentUser = user,
                    Commodity = commodity,
                    Description = EncodeFilterHelper.EncodeHtmlToNull(input.Description)
                };
                var r = await _commodityComment.AddOrEditAndSaveAsyn(newComment);
                if (r)
                {
                    if (commodity.AscriptionUser != user)
                    {
                        //发送消息通知
                        var message = "用户[ " + user.ChineseFullName + " ] 在 [ " + DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss") + " ] 给您的商品 [ " + commodity.Name + " ] 留了言，可能是有意向购买,您可以点击链接查看。";
                        var notification = new Notification
                        {
                            Receiver = commodity.AscriptionUser,
                            Name = "商品留言",
                            Description = message,
                            Link = "../../Home/GetCommodityDetail?id=" + commodity.Id,
                            IsAbnormal = false,
                            IsRead = false,
                            NotificationSource = NotificationSourceEnum.App
                        };
                        AppNotification.SendNotification(notification);
                    }
                    return Json(new { result = true });
                }
                return Json(new { result = false, message = "留言失败！" });
            }
            return Json(new { result = false, message = "留言失败！" });
        }

        #endregion 


        #region 用户浏览记录

        /// <summary>
        /// 获取浏览记录
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> GetMyVistorLogs()
        {
            var viewModel = new List<YZ_UserVisitorLogsVM>();
            var query = await _userVisitorLog.GetAllIncludingAsyn(x => x.Category);
            if (query.Count() > 0)
            {
                var visitorLogs = query.Where(x => x.UserIdOrIp == GetUser().Id);
                if (visitorLogs.Count() > 0)
                {
                    foreach (var item in visitorLogs)
                    {
                        var commodity = GetYZCommodityVM(x => x.Id == item.CommodityId);
                        if (commodity != null)
                        {
                            var visitorLogVM = new YZ_UserVisitorLogsVM(item);
                            visitorLogVM.CommodityName = commodity.Name;
                            visitorLogVM.Description = commodity.Description;
                            visitorLogVM.CommodityCover = commodity.Images.FirstOrDefault().UploadPath;
                            visitorLogVM.OrderNumber = (viewModel.Count() + 1).ToString();
                            viewModel.Add(visitorLogVM);
                        }
                    }
                }
            }
            return PartialView("../../Views/Account/UserCenterPartialViews/_MyVistorLog", viewModel);
        }

        /// <summary>
        /// 删除一条浏览历史
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> DeleteVistorLog(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Json(new { result = false, message = "删除失败！" });
            }
            var entity = await _userVisitorLog.GetSingleAsyn(id);
            if (entity != null)
            {
                var r = await _userVisitorLog.DeleteAndSaveAsyn(entity.Id);
                if (r.DeleteSatus)
                {
                    return Json(new { result = true });
                }
            }
            return Json(new { result = false, message = "删除失败！" });
        }

        #endregion


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

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public ApplicationUser GetUser()
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
                var userName = User.Identity.Name;
                if (userName != null)
                {
                    GetUserAvatar();
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

    }
}
