using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YiZhan.Entities.WebSettingManagement
{
    public enum AdvertisementPosition
    {
        [Description("首页轮播")]
        IndexBanner,
        [Description("用户中心首页广告")]
        UserCenterIndex,
        [Description("详细页广告")]
        Detail,
        [Description("用户注册登录页面背景图")]
        UserLoginPage,
        [Description("管理员注册登录页面背景图")]
        AdminLoginPage
    }
}
