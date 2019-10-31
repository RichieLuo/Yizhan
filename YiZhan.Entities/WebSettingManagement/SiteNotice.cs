using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using YiZhan.Entities.ApplicationOrganization;

namespace YiZhan.Entities.WebSettingManagement
{
    /// <summary>
    /// 网站公告实体
    /// </summary>
    public class SiteNotice : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 公告的标题
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 公告的内容
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 公告发布的时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>
        public ApplicationUser Publisher { get; set; }


        public string SortCode { get; set; }

        public SiteNotice()
        {
            this.Id = Guid.NewGuid();
            this.CreateTime = DateTime.Now;
        }
    }
}
