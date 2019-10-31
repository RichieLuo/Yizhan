using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YiZhan.Entities.WebSettingManagement
{

    /// <summary>
    /// 网站的一些基础配置（一些限制、权限之类的）实体
    /// </summary>
    public class SiteConfiguration:IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 是否允许注册
        /// </summary>
        public bool CanRegister { get; set; }

        /// <summary>
        /// 是否允许登录
        /// </summary>
        public bool CanLogin { get; set; }

        /// <summary>
        /// 开启广告位
        /// </summary>
        public bool IsOpenAd { get; set; }

        /// <summary>
        /// 开启登录验证码
        /// </summary>
        public bool IsOpenCode { get; set; }

        /// <summary>
        /// 开启网站客服（需配置客服）
        /// </summary>
        public bool IsOpenCS { get; set; }

        public string Name {get;set;}
        public string Description {get;set;}
        public string SortCode {get;set;}

        public SiteConfiguration()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
