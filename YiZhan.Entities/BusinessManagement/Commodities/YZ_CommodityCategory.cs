using System;
using System.ComponentModel.DataAnnotations;
using YiZhan.Common.JsonModels;

namespace YiZhan.Entities.BusinessManagement.Commodities
{
    /// <summary>
    /// 商品类别
    /// </summary>
    public class YZ_CommodityCategory : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品类型枚举
        /// </summary>
        public CommodityCategoryEnum Category { get; set; }

        public string Description { get; set; }
        public string SortCode { get; set; }

        public YZ_CommodityCategory()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
