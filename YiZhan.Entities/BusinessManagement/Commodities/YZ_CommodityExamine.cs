using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Entities.ApplicationOrganization;

namespace YiZhan.Entities.BusinessManagement.Commodities
{
    /// <summary>
    /// 审核
    /// </summary>
    public class YZ_CommodityExamine : IEntity
    {
        public Guid Id { get; set; }

        public YZ_Commodity Commodity { get; set; }

        public ApplicationUser ExamineUser { get; set; }

        public DateTime ExamineTime { get; set; }

        public YZ_CommodityState State { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string SortCode { get; set; }

    }
}
