using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.BusinessManagement;
using YiZhan.Entities.BusinessManagement.Commodities;
using YiZhan.ViewModels.BusinessManagement.CommodityVM;

namespace YiZhan.ViewModels.BusinessManagement
{
    /// <summary>
    /// 商品的视图模型
    /// </summary>
    public class YZ_CommodityVM : IEntityVM
    {
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
        ///修改时间
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
        /// 商品状态
        /// </summary>
        public YZ_CommodityState State { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public YZ_CommodityCategory Category { get; set; }

        /// <summary>
        /// 因为前端无法传YZ_CommodityCategory实体回来
        /// 实体设计缺陷：类别不应该作为实体，应作为枚举即可
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// 归属的用户
        /// </summary>
        public ApplicationUser AscriptionUser { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public ICollection<BusinessImage> Images { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public YZ_CommodityLookCount LookCount { get; set; }

        /// <summary>
        /// 审核原因描述（用于通过消息通知下发）
        /// </summary>
        public string ExamineDescription { get; set; }

        /// <summary>
        /// 交易方式
        /// </summary>
        public TransactionWayAndRange Way { get; set; }

        /// <summary>
        /// 交易范围
        /// </summary>
        public TransactionWayAndRange Range { get; set; }

        /// <summary>
        /// 商品评论集合
        /// </summary>
        public ICollection<YZ_CommodityCommentVM> Comments { get; set; }

        public bool IsBargain { get; set; }

        /// <summary>
        /// 排序编号
        /// </summary>
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        public YZ_CommodityVM()
        {

        }

        public YZ_CommodityVM(YZ_Commodity bo)
        {
            this.Id = bo.Id;
            this.Name = bo.Name;
            this.AddTime = bo.AddTime;
            this.EditTime = bo.EditTime;
            this.Description = bo.Description;
            this.SortCode = bo.SortCode;
            this.Price = bo.Price;
            this.Unit = bo.Unit;
            this.Stock = bo.Stock;
            this.State = bo.State;
            this.Category = bo.Category;
            this.AscriptionUser = bo.AscriptionUser;
            this.Images = bo.Images;
            this.LookCount = bo.LookCount;
            this.Way = bo.Way;
            this.Range = bo.Range;
            this.IsBargain = bo.IsBargain;           
        }

        public void MapToBo(YZ_Commodity bo)
        {
            bo.Id = this.Id;
            bo.Name = this.Name;
            bo.AddTime = this.AddTime;
            bo.EditTime = this.EditTime;
            bo.Description = this.Description;
            bo.Price = this.Price;
            bo.Unit = this.Unit;
            bo.Stock = this.Stock;
            bo.State = this.State;
            bo.Category = this.Category;
            bo.Way = Way;
            bo.Range = Range;
            bo.IsBargain = IsBargain;
        }
    }
}
