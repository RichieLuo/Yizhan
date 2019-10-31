using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using YiZhan.Entities.ApplicationOrganization;

namespace YiZhan.Entities.BusinessManagement.Commodities
{
    /// <summary>
    /// 商品留言
    /// </summary>
    public class YZ_CommodityComment : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 留言的人
        /// </summary>
        public ApplicationUser CommentUser { get; set; }

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

        public YZ_CommodityComment()
        {
            this.CreateTime = DateTime.Now;
        }
    }
}
