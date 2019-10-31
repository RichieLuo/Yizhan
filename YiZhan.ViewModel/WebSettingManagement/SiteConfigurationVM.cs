using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.WebSettingManagement;

namespace YiZhan.ViewModels.WebSettingManagement
{
    /// <summary>
    /// 网站的一些基础配置视图模型
    /// </summary>
    public class SiteConfigurationVM
    {
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


        public SiteConfigurationVM() { }

        public SiteConfigurationVM(SiteConfiguration bo)
        {
            this.CanRegister = bo.CanRegister;
            this.CanLogin = bo.CanLogin;
            this.IsOpenAd = bo.IsOpenAd;
            this.IsOpenCode = bo.IsOpenCode;
            this.IsOpenCS = bo.IsOpenCS;
        }
    }
}
