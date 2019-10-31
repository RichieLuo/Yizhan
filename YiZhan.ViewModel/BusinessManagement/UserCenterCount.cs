using System;
using System.Collections.Generic;
using System.Text;

namespace YiZhan.ViewModels.BusinessManagement
{
    /// <summary>
    /// 统计
    /// </summary>
    public class UserCenterCount
    {
        /// <summary>
        /// 我的商品统计
        /// </summary>
        public int CommoditiesCount { get; set; }

        /// <summary>
        /// 审核不通过的商品
        /// </summary>
        public int NotExamineCount { get; set; }

        /// <summary>
        /// 用户统计（管理员）
        /// </summary>
        public int UserCount { get; set; }
    
        /// <summary>
        /// 待审核的商品统计
        /// </summary>
        public int AwaitExamineCount { get; set; }
    }
}
