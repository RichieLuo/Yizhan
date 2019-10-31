using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.BusinessManagement.Commodities;
using YiZhan.Entities.BusinessManagement.User;

namespace YiZhan.ViewModels.BusinessManagement.User
{
    /// <summary>
    /// 用户查看商品历史记录视图模型
    /// </summary>
    public class YZ_UserVisitorLogsVM : IEntityVM
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 商品的Id
        /// </summary>
        public Guid CommodityId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string CommodityName { get; set; }

        /// <summary>
        /// 商品状态 
        /// </summary>
        public string CommodityState { get; set; }

        /// <summary>
        /// 商品封面
        /// </summary>
        public string CommodityCover{ get; set; }

        /// <summary>
        /// 浏览的商品类别
        /// </summary>
        public YZ_CommodityCategory Category { get; set; }

        /// <summary>
        /// 如果是未登录的话就存IP
        /// </summary>
        public string UserIdOrIp { get; set; }

        /// <summary>
        /// 浏览时间
        /// </summary>
        public DateTime LookTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SortCode { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        public YZ_UserVisitorLogsVM() { }
        public YZ_UserVisitorLogsVM(YZ_UserVisitorLog bo)
        {
            Id = bo.Id;
            CommodityId = bo.CommodityId;
            Category = bo.Category;
            UserIdOrIp = bo.UserIdOrIp;
            LookTime = bo.LookTime;
            Name = bo.Name;
            Description = bo.Description;
            SortCode = bo.SortCode;
        }
        public void MapToBo(YZ_UserVisitorLog bo)
        {
            bo.Id = Id;
            bo.CommodityId = CommodityId;
            bo.Category = Category;
            bo.UserIdOrIp = UserIdOrIp;
            bo.LookTime = LookTime;
            bo.Name = Name;
            bo.Description = Description;
            bo.SortCode = SortCode;
        }
    }
}
