using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.Notifications;

namespace YiZhan.ViewModels.Notifications
{
    public class NotificationVM : IEntityVM
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 消息通知名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 消息通知内容
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 消息通知链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 消息接收者
        /// </summary>
        public ApplicationUser Receiver { get; set; }

        /// <summary>
        /// 消息通知来源
        /// </summary>
        public NotificationSourceEnum NotificationSource { get; set; }

        /// <summary>
        /// 通知创建时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// 是否异常
        /// </summary>
        public bool IsAbnormal { get; set; }

        public string SortCode { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }
        public NotificationVM() { }
        public NotificationVM(Notification bo)
        {
            Id = bo.Id;
            Name = bo.Name;
            Description = bo.Description;
            Link = bo.Link;
            Receiver = bo.Receiver;
            NotificationSource = bo.NotificationSource;
            IsRead = bo.IsRead;
            AddTime = bo.AddTime;
            IsAbnormal=bo.IsAbnormal;
        }
    }
}
