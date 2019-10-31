using System;
using System.Collections.Generic;
using System.Text;

namespace YiZhan.Common.JsonModels
{
    /// <summary>
    /// 图片类型（用于区分图片用来干什么的）
    /// </summary>
    public enum ImageType
    {
        /// <summary>
        /// 用户头像
        /// </summary>
        Avatars,

        /// <summary>
        /// 商品封面
        /// </summary>
        CommodityCover,

        /// <summary>
        /// 商品图片
        /// </summary>
        CommodityImgs,

        /// <summary>
        /// 用户认证图片
        /// </summary>
        UserAuthentications,

        /// <summary>
        /// 广告图片
        /// </summary>
        Advertisements,

        /// <summary>
        /// 轮播图
        /// </summary>
        Banners,

        /// <summary>
        /// 网站Logo
        /// </summary>
        Logo,

        /// <summary>
        /// 公告图片
        /// </summary>
        Notices,

        /// <summary>
        /// 登录背景图
        /// </summary>
        LoginBg

    }
}
