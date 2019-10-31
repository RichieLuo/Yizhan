using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.BusinessManagement.Commodities;

namespace YiZhan.ViewModels.BusinessManagement.CommodityVM.Shopping
{
   public class YZ_BuyVM : IEntityVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SortCode { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }
       // public ApplicationUser Buyers { get; set; }
        //public ApplicationUser Seller { get; set; }
        public YZ_Commodity Commodity { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime CompletionTime { get; set; }
        public double Price { get; set; }

        public string Cover { get; set; }

        public YZ_BuyVM()
        {
         
        }
        public YZ_BuyVM(YZ_Commodity bo)
        {
            Commodity = bo;
        }
    }
}
