using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using YiZhan.Common.JsonModels;
using YiZhan.DataAccess;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.Notifications;
using System.Linq;
using YiZhan.ViewModels.Notifications;

namespace YiZhan.Web.App.CommonHelper
{
    /// <summary>
    /// 消息通知
    /// </summary>
    public class YZNotification
    {
        private readonly IEntityRepository<Notification> _notification;
        public YZNotification(
            IEntityRepository<Notification> notification
            )
        {
            this._notification = notification;
        }

        /// <summary>
        /// 发送消息通知
        /// </summary>
        /// <param name="notification">消息实体</param>
        public void SendNotification(Notification notification)
        {
            if (notification != null)
            {
                _notification.AddAndSave(notification);
            }
        }

        /// <summary>
        /// 获取单条消息通知
        /// </summary>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public NotificationVM GetNotification(ApplicationUser receiver)
        {
            var notification = _notification
                .GetAllIncluding(x => x.Receiver)
                .FirstOrDefault(x => x.Receiver == receiver);
            var notificationVM = new NotificationVM(notification);
            return notificationVM;
        }

        /// <summary>
        /// 根据用户获取当前用户的所有消息通知
        /// </summary>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public List<NotificationVM> GetNotifications(ApplicationUser receiver)
        {
            var notificationsVM = new List<NotificationVM>();
            var notifications = _notification
                .GetAllIncluding(x => x.Receiver)
                .Where(x => x.Receiver == receiver)
                .OrderByDescending(x => x.AddTime).ToList();
            foreach (var notification in notifications)
            {
                var notificationVM = new NotificationVM(notification);
                notificationVM.OrderNumber = (notificationsVM.Count() + 1).ToString();
                notificationsVM.Add(notificationVM);
            }
            return notificationsVM;
        }

        /// <summary>
        /// 管理员获取所有的消息通知
        /// </summary>
        /// <returns></returns>
        public List<NotificationVM> AdminGetNotifications()
        {
            var notificationsVM = new List<NotificationVM>();
            var notifications = _notification
                .GetAllIncluding(x => x.Receiver)              
                .OrderByDescending(x => x.AddTime).ToList();
            foreach (var notification in notifications)
            {
                var notificationVM = new NotificationVM(notification);
                notificationVM.OrderNumber = (notificationsVM.Count() + 1).ToString();
                notificationsVM.Add(notificationVM);
            }
            return notificationsVM;
        }

        /// <summary>
        /// 获取未读消息数量
        /// </summary>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public int GetNotificationsNoRead(ApplicationUser receiver)
        {
            var notifications = _notification
                    .GetAllIncluding(x => x.Receiver)
                    .Where(x => x.Receiver == receiver && x.IsRead == false).ToList();
            var count = notifications.Count();
            return count;

        }


        public void DeleteNotification(Guid noticesId)
        {
            var notification = _notification.GetSingle(noticesId);
            _notification.DeleteAndSave(notification);
        }


        public void DeleteNotifications(Guid[] noticesId)
        {
            var notification = new Notification();
            foreach (var id in noticesId)
            {
                notification = _notification.GetSingle(id);
                _notification.DeleteAndSave(notification);
            }
        }

        public void SetNotificationIsRead(Guid noticesId)
        {
            var notification = _notification.GetSingle(noticesId);
            notification.IsRead = true;
            _notification.EditAndSave(notification);
        }

        public void SetNotificationsIsRead(Guid[] noticesId)
        {
            var notification = new Notification();
            foreach (var id in noticesId)
            {
                notification = _notification.GetSingle(id);
                notification.IsRead = true;
                _notification.EditAndSave(notification);
            }
        }
    }
}
