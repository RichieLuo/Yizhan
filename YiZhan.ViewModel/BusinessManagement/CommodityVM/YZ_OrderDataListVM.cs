using System;
using System.Collections.Generic;
using System.Text;

namespace YiZhan.ViewModels.BusinessManagement.CommodityVM
{
    /// <summary>
    /// 订单数据（用户中心订单管理）
    /// </summary>
    public class YZ_OrderDataListVM
    {
        #region 买家订单数据
        /// <summary>
        /// 买家：所有订单
        /// </summary>
        public List<YZ_OrderVM> AllOrdersWithBuyer { get; set; }

        /// <summary>
        /// 买家：待付款的订单
        /// </summary>
        public List<YZ_OrderVM> WaitToPaymentOrdersWithBuyer { get; set; }
          
        /// <summary>
        /// 买家：待发货的订单
        /// </summary>
        public List<YZ_OrderVM> WaitToSendOutOrdersWithBuyer { get; set; }

        /// <summary>
        /// 买家：已完成的订单
        /// </summary>
        public List<YZ_OrderVM> SuccessOrdersWithBuyer { get; set; }

        #endregion

        #region 卖家订单数据   （全部订单、待发货、已完成）
        
        /// <summary>
        /// 卖家：所有订单
        /// </summary>
        public List<YZ_OrderVM> AllOrdersWithSeller { get; set; }
       
        /// <summary>
        /// 卖家：待发货的订单
        /// </summary>
        public List<YZ_OrderVM> WaitToSendOutOrdersWithSeller { get; set; }

        /// <summary>
        /// 卖家：已完成的订单
        /// </summary>
        public List<YZ_OrderVM> SuccessOrdersWithSeller { get; set; }

        #endregion

        public YZ_OrderDataListVM()
        {
            this.AllOrdersWithBuyer = new List<YZ_OrderVM>();
            this.WaitToPaymentOrdersWithBuyer = new List<YZ_OrderVM>();
            this.SuccessOrdersWithBuyer = new List<YZ_OrderVM>();
            this.WaitToSendOutOrdersWithBuyer = new List<YZ_OrderVM>();

            this.AllOrdersWithSeller = new List<YZ_OrderVM>();
            this.WaitToSendOutOrdersWithSeller = new List<YZ_OrderVM>();
            this.SuccessOrdersWithSeller = new List<YZ_OrderVM>();
        }
    }
}
