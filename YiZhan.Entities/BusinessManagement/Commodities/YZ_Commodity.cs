using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.Ultilities;

namespace YiZhan.Entities.BusinessManagement.Commodities
{
    /// <summary>
    /// 商品实体
    /// </summary>
    public class YZ_Commodity : IEntity
    {
        [Key]    
        public Guid Id { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime EditTime { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 系统内部编码
        /// </summary>
        public string SortCode { get; set; } 

        /// <summary>
        /// 商品售价  
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 商品单位
        /// </summary>
        public string Unit { get; set; } 

        /// <summary>
        /// 商品库存
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// 商品的状态
        /// </summary>
        public YZ_CommodityState State { get; set; }

        /// <summary>
        /// 归属的类型
        /// </summary>
        public YZ_CommodityCategory Category { get; set; } 

        /// <summary>
        /// 归属的用户
        /// </summary>
        public ApplicationUser AscriptionUser { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public ICollection<BusinessImage> Images { get; set; }

        /// <summary>
        /// 查看统计
        /// </summary>
        public YZ_CommodityLookCount LookCount { get; set; }

        /// <summary>
        /// 交易方式
        /// </summary>
        public TransactionWayAndRange Way { get; set; }

        /// <summary>
        /// 交易范围
        /// </summary>
        public TransactionWayAndRange Range { get; set; }

        /// <summary>
        /// 该商品包含的留言
        /// </summary>
        public ICollection<YZ_CommodityComment> Comments { get; set; } 

        public bool IsBargain { get; set; }

        public YZ_Commodity()
        {
            this.SortCode = BusinessEntityComponentsFactory.SortCodeByDefaultDateTime<YZ_Commodity>();
            this.Id = Guid.NewGuid();
            this.AddTime = DateTime.Now;
        }


    }
}
