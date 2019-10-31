using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YiZhan.Entities.Attachments;

namespace YiZhan.Entities.BusinessManagement.Commodities
{
    /// <summary>
    /// 商品图片中间表：应用于一对多
    /// </summary>
    public class YZ_CommodityAndImage:IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 关联的商品Id
        /// </summary>
        [Required]
        public Guid CommodityId { get; set; }

        /// <summary>
        /// 关联的商品
        /// </summary>
        [ForeignKey("CommodityId")]
        public YZ_Commodity Commodity { get; set; }

        /// <summary>
        /// 关联的图片Id
        /// </summary>
        [Required]
        public Guid BusinessImageId { get; set; }

        /// <summary>
        ///关联的图片
        /// </summary>
        [ForeignKey("BusinessImageId")]
        public BusinessImage BusinessImage { get; set; }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SortCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public YZ_CommodityAndImage()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
