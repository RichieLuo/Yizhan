using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.BusinessManagement.Commodities;

namespace YiZhan.ViewModels.BusinessManagement.CommodityVM
{
    public class YZ_OrderVM : IEntityVM
    {
        public Guid Id { get; set; }
        public string Name { get ; set ; }
        public string Description { get ; set ; }
        public string SortCode { get ; set ; }
        public string OrderNumber { get ; set ; }
        public bool IsNew { get ; set ; }
        public ApplicationUser Buyers { get; set; }
        public ApplicationUser Seller { get; set; }
        public YZ_Commodity Commodity { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime CompletionTime { get; set; }
        public YZ_OrderState State { get; set; }
        public decimal Price { get; set; }
        public ListPageParameter ListPageParameter { get ; set ; }

        public YZ_OrderVM() { }

        public YZ_OrderVM(YZ_Order bo)
        {
            Id = bo.Id;
            Name = bo.Name;
            Description = bo.Description;
            Buyers = bo.Buyers;
            Seller = bo.Seller;
            Commodity = bo.Commodity;
            CreateTime = bo.CreateTime;
            CompletionTime = bo.CompletionTime;
            State = bo.State;
            Price = bo.Price;
        }
    }
}
