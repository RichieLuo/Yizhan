using System;
using System.Collections.Generic;
using System.Text;

namespace YiZhan.Entities.BusinessManagement.Commodities
{
    /// <summary>
    /// 商品状态枚举
    /// 状态说明：用户上架（添加）商品，进入已经待审核状态，管理员审核完成 状态改为审核通过
    /// </summary>
    public enum YZ_CommodityState
    {     
        /// <summary>
        /// 正在审核当中（未进行出售）
        /// </summary>
        IsExamine,

        /// <summary>
        /// 审核不通过，被驳回（用户可重新修改商品信息再提交）
        /// </summary>
        IsReject,

        /// <summary>
        /// 审核通过（显示在用户的已审核商品列表，用户可以出售）
        /// </summary>
        IsExamineOk,

        /// <summary>
        /// 正在出售（已经显示在页面上）
        /// </summary>
        OnSale,

        /// <summary>
        /// 已经出售
        /// </summary>
        HaveToSell,

        /// <summary>
        /// 取消出售（下架，页面上将不再显示）
        /// </summary>
        CancelASale

    }
}
