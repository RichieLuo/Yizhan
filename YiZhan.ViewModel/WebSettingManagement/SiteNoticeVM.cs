using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.WebSettingManagement;

namespace YiZhan.ViewModels.WebSettingManagement
{
    /// <summary>
    /// 网站公告的视图模型
    /// </summary>
    public class SiteNoticeVM : IEntityVM
    {
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

        /// <summary>
        /// 附件
        /// </summary>
        public ICollection<BusinessImage> Attachments { get; set; }

        public string SortCode { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }
        public SiteNoticeVM() { }
        public SiteNoticeVM(SiteNotice bo)
        {
            Id = bo.Id;
            Name = bo.Name;
            Description = bo.Description;
            Publisher = bo.Publisher;
            CreateTime = bo.CreateTime;
        }
    }
}
