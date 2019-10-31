using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YiZhan.Common.JsonModels;
using YiZhan.DataAccess;
using YiZhan.DataAccess.Common;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.BusinessManagement.Commodities;
using YiZhan.Entities.Notifications;
using YiZhan.ViewModels.BusinessManagement;
using YiZhan.Web.App.CommonHelper;

namespace YiZhan.Web.Controllers.Admin
{
    /// <summary>
    /// 后台商品管理
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdCommoditiesController : Controller
    {
        public YZNotification AppNotification { get; set; }
        private readonly IEntityRepository<Notification> _notification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEntityRepository<BusinessImage> _businessImage;
        private readonly IEntityRepository<YZ_Commodity> _commodity;
        private readonly IEntityRepository<YZ_CommodityCategory> _commodityCategory;
        private readonly IEntityRepository<YZ_CommodityExamine> _commodityExamine;
        public AdCommoditiesController(
            IEntityRepository<Notification> notification,
            UserManager<ApplicationUser> userManager,
            IEntityRepository<BusinessImage> businessImage,
            IEntityRepository<YZ_Commodity> commodity,
            IEntityRepository<YZ_CommodityCategory> commodityCategory,
            IEntityRepository<YZ_CommodityExamine> commodityExamine
            )
        {
            _notification = notification;
            _userManager = userManager;
            _businessImage = businessImage;
            _commodity = commodity;
            _commodityCategory = commodityCategory;
            _commodityExamine = commodityExamine;
            AppNotification = new YZNotification(notification);
        }

        public IActionResult Index()
        {
            //获取当前用户名
            GetCurrUserName();
            var pageSize = 8;
            var pageIndex = 1;
            var counter = 0;

            //获取所有的商品
            var commodityListVM = new List<YZ_CommodityVM>();
            var commodities = this._commodity.GetAllIncluding(x => x.Category, x => x.AscriptionUser);
            var commodityCollectionPageList = commodities.ToPaginatedList(pageIndex, pageSize);
            if (commodities.Count() > 0)
            {
                foreach (var commodity in commodityCollectionPageList)
                {
                    var commodityImage = _businessImage.FindBy(m => m.RelevanceObjectId == commodity.Id).FirstOrDefault(m => m.Type == ImageType.CommodityCover);
                    var commodityImages = new List<BusinessImage>();
                    commodityImages.Add(commodityImage);
                    var commodityVM = new YZ_CommodityVM(commodity);
                    commodityVM.Images = commodityImages;
                    commodityVM.OrderNumber = (++counter).ToString();
                    commodityListVM.Add(commodityVM);
                }
                ViewBag.CommodityVMCollection = commodityListVM;
                // 提取当前页面关联的分页器实例
                var pageGroup = PagenateGroupRepository.GetItem<YZ_Commodity>(commodityCollectionPageList, 10, pageIndex);
                ViewBag.PageGroup = pageGroup;

                var listPageParameter = new ListPageParameter()
                {
                    PageIndex = commodityCollectionPageList.PageIndex,
                    Keyword = "",
                    PageSize = commodityCollectionPageList.PageSize,
                    ObjectTypeId = commodityListVM.FirstOrDefault().Id.ToString(),
                    ObjectAmount = commodityCollectionPageList.TotalCount,
                    SortDesc = "Default",
                    SortProperty = "Name",
                    PageAmount = 0,
                    SelectedObjectId = ""
                };
                ViewBag.PageParameter = listPageParameter;
            }
            else
            {
                commodityListVM = null;
            }
            return View("../../Views/AdminCenter/CommodityManagement/Index", commodityListVM);
        }

        /// <summary>
        /// 列表数据
        /// </summary>
        /// <param name="listPageParaJson">
        /// 用于简单定义从前端页面返回的数据列表相关的 Json 变量，变量的定义依赖 ShiKe.Common.JsonModels.ListPageParameter,
        /// 前端 json 数据构建相关的代码，参见：wwwroot/js/yiZhanCommon.js 其中的方法：function yiZhanGetListParaJson()
        /// </param>
        /// <returns></returns>
        public IActionResult List(string listPageParaJson)
        {
            var listPagePara = Newtonsoft.Json.JsonConvert.DeserializeObject<ListPageParameter>(listPageParaJson);

            var typeId = "";
            var keyword = "";
            if (!String.IsNullOrEmpty(listPagePara.ObjectTypeId))
            { typeId = listPagePara.ObjectTypeId; }
            if (!String.IsNullOrEmpty(listPagePara.Keyword))
            { keyword = listPagePara.Keyword; }

            #region 1.构建与 keyword 相关的查询 lambda 表达式，用于对查询结果的过滤（给 Where 使用）
            Expression<Func<YZ_Commodity, bool>> predicate = x =>
                x.Name.Contains(keyword) ||
                x.Description.Contains(keyword) ||
                x.Category.Name.Contains(keyword);
            #endregion

            var commodities = this._commodity.GetAllIncluding(x => x.Category, x => x.AscriptionUser);

            // 2.处理条件过滤
            var filterCommoditiesCollection = commodities.Where(predicate);

            #region 3.根据属性名称确定排序的属性的 lambda 表达式
            var sortPropertyName = listPagePara.SortProperty;
            var type = typeof(YZ_Commodity);
            var target = Expression.Parameter(typeof(object));
            var castTarget = Expression.Convert(target, type);
            var getPropertyValue = Expression.Property(castTarget, sortPropertyName);
            var sortExpession = Expression.Lambda<Func<YZ_Commodity, object>>(getPropertyValue, target);
            #endregion

            #region 4.对过滤的数据进行排序
            // 处理排序
            IQueryable<YZ_Commodity> sortedUserCollection;
            if (listPagePara.SortDesc == "")
            { sortedUserCollection = filterCommoditiesCollection.OrderByDescending(sortExpession); }
            else
            { sortedUserCollection = filterCommoditiesCollection.OrderBy(sortExpession); }
            // 处理分页
            var commodityCollectionPageList = sortedUserCollection.ToPaginatedList(listPagePara.PageIndex, listPagePara.PageSize);
            #endregion


            #region 5.构建相关的视图模型和所需要向前端提交一些与约束相关的参数
            var commodityListVM = new List<YZ_CommodityVM>();
            var counter = 0;
            if (commodities.Count() > 0)
            {
                foreach (var commodity in commodityCollectionPageList)
                {
                    var commodityImage = _businessImage.FindBy(m => m.RelevanceObjectId == commodity.Id).FirstOrDefault(m => m.Type == ImageType.CommodityCover);
                    var commodityImages = new List<BusinessImage>();
                    commodityImages.Add(commodityImage);
                    var commodityVM = new YZ_CommodityVM(commodity);
                    commodityVM.Images = commodityImages;
                    commodityVM.OrderNumber = (++counter + ((listPagePara.PageIndex - 1) * listPagePara.PageSize)).ToString();
                    commodityListVM.Add(commodityVM);
                }

            }
            // 提取当前页面关联的分页器实例
            var pageGroup = PagenateGroupRepository.GetItem<YZ_Commodity>(commodityCollectionPageList, 10, listPagePara.PageIndex);
            ViewBag.PageGroup = pageGroup;
            ViewBag.PageParameter = listPagePara;
            #endregion
            return PartialView("../../Views/AdminCenter/CommodityManagement/_List", commodityListVM);

        }

        /// <summary>
        ///获取待审核的商品
        /// </summary>
        /// <returns></returns>
        public IActionResult GetAwaitExamineCommodities()
        {
            var viewModel = GetYZCommodityVMs(x => x.State == YZ_CommodityState.IsExamine);
            return PartialView("../../Views/AdminCenter/CommodityManagement/AwaitExamine/_GetCommodities", viewModel);
        }

        /// <summary>
        /// 打开审核商品的模态框
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult EditCommodity(Guid id)
        {
            var boVM = GetYZCommodityVM(x => x.Id == id);
            return PartialView("../../Views/AdminCenter/CommodityManagement/AwaitExamine/_EditCommodityModal", boVM);
        }

        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <param name="boVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveEditCommodity([Bind("Id,State,ExamineDescription")] YZ_CommodityVM boVM)
        {
            var commodity = _commodity.GetAllIncluding(x => x.AscriptionUser).FirstOrDefault(x => x.Id == boVM.Id);
            if (commodity != null)
            {
                commodity.State = boVM.State;
            }
            var examine = new YZ_CommodityExamine
            {
                Description = boVM.ExamineDescription,
                Commodity = commodity,
                ExamineTime = DateTime.Now,
                ExamineUser = GetUser(),
                State = boVM.State
            };
            _commodityExamine.AddAndSave(examine);
            var result = await _commodity.AddOrEditAndSaveAsyn(commodity);
            if (result)
            {     
                var message = "";
                if (boVM.State == YZ_CommodityState.OnSale)
                {
                    message = "尊敬的用户您好，您的商品 [ " + commodity.Name + " ] 已经审核并通过，已经自动上架。";
                }
                else
                {
                    message = "尊敬的用户您好，您的商品 [ " + commodity.Name + " ] 已经审核，但未通过已被驳回，原因是[" + boVM.ExamineDescription + "]，请您修改后再次提交审核。";
                }
                var notification = new Notification
                {
                    Receiver = commodity.AscriptionUser,
                    Name = "商品审核",
                    Description = message,
                    Link = "javascript:",
                    IsAbnormal = false,
                    IsRead = false,
                    NotificationSource = NotificationSourceEnum.App
                };
                AppNotification.SendNotification(notification);
                return Json(new { result = true, message = "操作成功，请关闭当前页面！" });
            }
            else
            {
                return Json(new { result = false, message = "操作失败，请关闭当前页面！" });
            }
        }

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
        /// 根据查询条件 获取商品的集合（不分页）
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
        /// 获取商品
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<YZ_Commodity> GetYZCommodities(Expression<Func<YZ_Commodity, bool>> predicate)
        {
            var commodityList = this._commodity
           .GetAllIncluding(x => x.Category, x => x.AscriptionUser, x => x.Images, x => x.LookCount)
           .Where(predicate)
           .OrderByDescending(c => c.AddTime).ToList();
            return commodityList;
        }

        #region 获取用户信息代码
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

    }
}
