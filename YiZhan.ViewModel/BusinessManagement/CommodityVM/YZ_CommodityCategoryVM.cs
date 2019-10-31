using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.BusinessManagement;
using YiZhan.Entities.BusinessManagement.Commodities;

namespace YiZhan.ViewModels.BusinessManagement
{
    /// <summary>
    /// 商品类别视图模型
    /// </summary>
    public class YZ_CommodityCategoryVM : IEntityVM
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类别描述
        /// </summary>
        public string Description { get; set; }
        public string SortCode { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        public YZ_CommodityCategoryVM() { }
        public YZ_CommodityCategoryVM(YZ_CommodityCategory bo)
        {
            Id = bo.Id;
            Name = bo.Name;
            Description = bo.Description;
            SortCode = bo.SortCode;
        }

        public void MapToBo(YZ_CommodityCategory bo)
        {
            bo.Id = Id;
            bo.Name = Name;
            bo.Description = Description;
            bo.SortCode = SortCode;
        }
    }
}
