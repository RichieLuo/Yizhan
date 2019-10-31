using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Entities.ApplicationOrganization;

namespace YiZhan.Entities.BusinessManagement.Commodities
{
    /// <summary>
    /// 订单
    /// </summary>
    public class YZ_Order : IEntity
    {
        public Guid Id { get; set; }
        public ApplicationUser Buyers { get; set; }
        public ApplicationUser Seller { get; set; }
        public YZ_Commodity Commodity { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime CompletionTime { get; set; }
        public YZ_OrderState State { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SortCode { get; set; }

        public YZ_Order()
        {
            this.Id = Guid.NewGuid();
            CreateTime = DateTime.Now;
        }
    }
}
