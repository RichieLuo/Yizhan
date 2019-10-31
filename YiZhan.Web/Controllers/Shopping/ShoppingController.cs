using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YiZhan.Common.JsonModels;
using YiZhan.DataAccess;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.BusinessManagement.Commodities;
using YiZhan.Entities.Notifications;
using YiZhan.ViewModels.BusinessManagement.CommodityVM;
using YiZhan.ViewModels.BusinessManagement.CommodityVM.Shopping;
using YiZhan.Web.App.CommonHelper;

namespace YiZhan.Web.Controllers.Shopping
{
    [Authorize]
    public class ShoppingController : Controller
    {
        private readonly IEntityRepository<YZ_Order> _Order;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IEntityRepository<YZ_Commodity> _YZ_Commodity;
        private readonly IEntityRepository<BusinessImage> _BusinessImage;
        public YZNotification AppNotification { get; set; }
        private readonly IEntityRepository<Notification> _notification;

        public ShoppingController(
            IEntityRepository<YZ_Commodity> commodity,
            IEntityRepository<BusinessImage> businessImage,
            UserManager<ApplicationUser> userManager,
            IEntityRepository<YZ_Order> order,
            IEntityRepository<Notification> notification)
        {
            _YZ_Commodity = commodity;
            _BusinessImage = businessImage;
            _UserManager = userManager;
            _Order = order;
            _notification = notification;
            AppNotification = new YZNotification(notification);
        }

        public async Task<IActionResult> Buy_now([Bind("Id")]Guid id)
        {

            var commodit = _YZ_Commodity.GetAllIncluding(x => x.Category, x => x.Comments, x => x.AscriptionUser, x => x.Images, x => x.LookCount).FirstOrDefault(x => x.Id == id);
            var cover = _BusinessImage.FindBy(m => m.RelevanceObjectId == commodit.Id).FirstOrDefault(m => m.Type == ImageType.CommodityCover);
            var buyModel = new YZ_BuyVM(commodit)
            {
                Cover = cover.UploadPath
            };
            return View(buyModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment([Bind("Id,OrderId")]Guid id, Guid orderId)
        {
            var currentUser = await _UserManager.GetUserAsync(User);
          
            var commodit = _YZ_Commodity.GetAllIncluding(x => x.AscriptionUser).FirstOrDefault(x => x.Id.Equals(id));//先找有没有这个商品
            if (currentUser.Equals(commodit.AscriptionUser))
            {
                return View("PaymentResult", new YZ_BuyStatusVM(false, Guid.Empty, "购买异常，您不能购买自己的商品！", commodit.State));
            }
            var hasOrder = _Order.GetAllIncluding(x => x.Buyers, x => x.Commodity).FirstOrDefault(x => x.Id.Equals(orderId));
            bool isCommodit = false;
            bool isUser = false;

            if (hasOrder != null)
            {
                isCommodit = hasOrder.Commodity.Equals(commodit);
                isUser = hasOrder.Buyers.Equals(currentUser);
            }
            bool isCurrentUser = isCommodit && isUser;
            if (commodit == null)
            {
                return View("PaymentResult", new YZ_BuyStatusVM(false, Guid.Empty, "支付异常", commodit.State));
            }
            if (!commodit.State.Equals(YZ_CommodityState.OnSale) && !isCurrentUser)
            {
                return View("PaymentResult", new YZ_BuyStatusVM(false, Guid.Empty, "购买异常", commodit.State));
            }
            if (isCurrentUser)
            {
                return View("PaymentResult", new YZ_BuyStatusVM(false, orderId, "已存在该笔订单，请勿重复支付！", new YZ_OrderVM(hasOrder)));
            }

            var orderBo = new YZ_Order();
            orderBo.Id = orderId;
            orderBo.Buyers = currentUser;
            orderBo.Seller = commodit.AscriptionUser;
            orderBo.CompletionTime = DateTime.UtcNow;
            orderBo.Commodity = commodit;
            orderBo.Price = commodit.Price;
            orderBo.State = YZ_OrderState.待发货;
            orderBo.Description = commodit.Description;
            var status = await _Order.AddOrEditAndSaveAsyn(orderBo);

            if (!status)
            {
                return View("PaymentResult", new YZ_BuyStatusVM(false, Guid.Empty, "订单异常,请立即联系官方人员！", new YZ_OrderVM(orderBo)));
            }
            commodit.State = YZ_CommodityState.HaveToSell;
            var commoditStatus = await _YZ_Commodity.AddOrEditAndSaveAsyn(commodit);

            //给卖家发送消息通知
            var message = "有用户在 [ " + DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss") + " ] 购买了您的商品 [ " + commodit.Name + " ] 请注意查看订单！";
            var notification = new Notification
            {
                Receiver = commodit.AscriptionUser,
                Name = "新订单",
                Description = message,
                Link = "javascript:",
                IsAbnormal = false,
                IsRead = false,
                NotificationSource = NotificationSourceEnum.App
            };
            AppNotification.SendNotification(notification);

            //给买家发送消息通知
            message = "您于 [ " + DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss") + " ] 购买的商品 [ " + commodit.Name + " ] 已经下单成功，正在等待卖家发货，请注意查看订单！";
            notification = new Notification
            {
                Receiver = currentUser,
                Name = "商品购买",
                Description = message,
                Link = "javascript:",
                IsAbnormal = false,
                IsRead = false,
                NotificationSource = NotificationSourceEnum.App
            };
            AppNotification.SendNotification(notification);

            return View("PaymentResult", new YZ_BuyStatusVM(true, orderBo.Id, "支付成功，正在等待卖家发货！", new YZ_OrderVM(orderBo)));
        }
    }
}