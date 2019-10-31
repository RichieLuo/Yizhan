using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.WebSettingManagement;

namespace YiZhan.ViewModels.WebSettingManagement
{
    /// <summary>
    /// 网站基础信息
    /// </summary>
    public class SiteSettingVM
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 网站标题
        /// </summary>
        [StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 网站标题后缀
        /// </summary>
        [StringLength(20)]
        public string Suffix { get; set; }

        /// <summary>
        ///网站域名
        /// </summary>
        [StringLength(20)]
        public string DomainName { get; set; }

        /// <summary>
        /// 网站关键字
        /// </summary>
        public string KeyWords { get; set; }

        /// <summary>
        /// 网站描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 网站统计代码
        /// </summary>
        public string Statistics { get; set; }

        /// <summary>
        /// 网站客服代码
        /// </summary>
        public string CustomerService { get; set; }

        public string SortCode { get; set; }

        /// <summary>
        /// 网站LOGO
        /// </summary>
        public BusinessImage Logo { get; set; }

        /// <summary>
        /// 网站LOGO路径
        /// </summary>
        public string LogoPath { get; set; }

        /// <summary>
        /// 网站版权
        /// </summary>
        [StringLength(30)]
        public string Copyright { get; set; }

        /// <summary>
        /// 网站备案号
        /// </summary>
        [StringLength(30)]
        public string ICP { get; set; }

        /// <summary>
        /// 网站邮箱
        /// </summary>
        public string SiteEmail { get; set; }

        /// <summary>
        /// 系统安装时间
        /// </summary>
        public DateTime InstallTime { get; set; }

        public SiteSettingVM() { }
        public SiteSettingVM(SiteSetting bo)
        {
            Id = bo.Id;
            Name = bo.Name;
            Suffix = bo.Suffix;
            DomainName = bo.DomainName;
            KeyWords = bo.KeyWords;
            Description = bo.Description;
            Copyright = bo.Copyright;
            ICP = bo.ICP;
            SiteEmail = bo.SiteEmail;
            Statistics = bo.Statistics;
            CustomerService = bo.CustomerService;
            InstallTime = bo.InstallTime;
        }

        public void MapTo(SiteSetting bo)
        {
            bo.Id = Id;
            bo.Name = Name;
            bo.Suffix = Suffix;
            bo.DomainName = DomainName;
            bo.KeyWords = KeyWords;
            bo.Description = Description;
            bo.Copyright = Copyright;
            bo.ICP = ICP;
            bo.Statistics = Statistics;
        }
    }
}
