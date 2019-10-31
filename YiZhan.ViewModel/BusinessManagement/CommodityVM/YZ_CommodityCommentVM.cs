using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.BusinessManagement.Commodities;
using YiZhan.ViewModels.ApplicationOrganization;

namespace YiZhan.ViewModels.BusinessManagement.CommodityVM
{
    /// <summary>
    /// 商品留言的视图
    /// </summary>
    public class YZ_CommodityCommentVM : IEntityVM
    {
        /// <summary>
        /// 提交的时候用做商品的Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 留言的人
        /// </summary>
        public ApplicationUserVM CommentUser { get; set; }

        /// <summary>
        /// 留言对应的商品
        /// </summary>
        public YZ_Commodity Commodity { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 留言内容
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 留言时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public string SortCode { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }

        /// <summary>
        /// 是否显示删除按钮
        /// </summary>
        public bool IsShowDeleteBtn { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        public YZ_CommodityCommentVM() { }

        public YZ_CommodityCommentVM(YZ_CommodityComment bo)
        {
            Id = bo.Id;
            Commodity = bo.Commodity;
            Name = bo.Name;
            Description = bo.Description;
            CreateTime = bo.CreateTime;
            CommentUser = new ApplicationUserVM(bo.CommentUser);
        }
    }
}
