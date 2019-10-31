using System;
using System.Collections.Generic;
using System.Text;

namespace YiZhan.Common.JsonModels
{
    /// <summary>
    /// 消息通知来源
    /// </summary>
    public enum NotificationSourceEnum
    {
        /// <summary>
        /// 系统
        /// </summary>
        App,

        /// <summary>
        /// 管理员用户
        /// </summary>
        AppUser,
        //可能还有更多例如第三方通知待补充
    }
}
