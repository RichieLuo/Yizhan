using System;
using System.Collections.Generic;
using System.Text;

namespace YiZhan.Entities.BusinessManagement.Commodities
{
    /// <summary>
    /// 商品浏览统计
    /// </summary>
    public class YZ_CommodityLookCount:IEntityBase
    {
        public virtual Guid Id { get; set; }
        public virtual int LookCount { get; set; }

        public YZ_CommodityLookCount()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
