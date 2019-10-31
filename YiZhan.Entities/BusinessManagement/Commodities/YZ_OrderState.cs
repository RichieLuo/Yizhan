using System;
using System.Collections.Generic;
using System.Text;

namespace YiZhan.Entities.BusinessManagement.Commodities
{
    public enum YZ_OrderState
    {        
        /// <summary>
        /// 等待付款的订单
        /// </summary>
        待付款,

        /// <summary>
        /// 等待发货当中的订单
        /// </summary>
        待发货,

        /// <summary>
        /// 交易成功的订单状态
        /// </summary>
        已完成,

        /// <summary>
        /// 订单成功提交但未付款用户可取消
        /// </summary>
        已取消
    }
}
