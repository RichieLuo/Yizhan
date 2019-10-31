using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using YiZhan.Entities.BusinessManagement.Commodities;

namespace YiZhan.Entities.BusinessManagement.User
{
    /// <summary>
    /// 浏览历史
    /// </summary>
    public class YZ_UserVisitorLog : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 浏览的商品
        /// </summary>
        public Guid CommodityId { get; set; }

        /// <summary>
        /// 商品的分类
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

        public YZ_UserVisitorLog()
        {
            this.Id = Guid.NewGuid();
            this.LookTime = DateTime.Now;
        }

    }
}
