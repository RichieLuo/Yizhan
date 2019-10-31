using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Entities.BusinessManagement.Commodities;

namespace YiZhan.ViewModels.BusinessManagement.CommodityVM.Shopping
{
    public class YZ_BuyStatusVM
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        public YZ_OrderVM OrderInfo { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsOK { get; set; }

        /// <summary>
        /// 订单的Id
        /// </summary>
        public Guid? OrderId { get; set; }

        /// <summary>
        /// 返回的信息
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// 商品的状态
        /// </summary>
        public YZ_CommodityState? CommodityState { get; set; }
        public YZ_BuyStatusVM(bool isOK, Guid? orderId, string info, YZ_OrderVM order)
        {
            this.IsOK = isOK;
            OrderId = orderId;
            Info = info;
            OrderInfo = order;
        }
        public YZ_BuyStatusVM(bool isOK, Guid? orderId, string info, YZ_CommodityState state)
        {
            this.IsOK = isOK;
            OrderId = orderId;
            Info = info;
            CommodityState = state;

        }

    }
}
