using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.WebSettingManagement;

namespace YiZhan.ViewModels.WebSettingManagement
{
    public class UserFeedbackVM : IEntityVM
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 程序错误页面链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 反馈描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 反馈时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactWay { get; set; }

        /// <summary>
        /// 反馈的用户
        /// </summary>
        public ApplicationUser FeedUser { get; set; }

        public UserFeedbackType Type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// 反馈者的IP地址
        /// </summary>
        public string FeedbackIPAddress { get; set; }

        public string SortCode { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }

        public ListPageParameter ListPageParameter { get; set; }

        public UserFeedbackVM() { }
        public UserFeedbackVM(UserFeedback bo)
        {
            Id = bo.Id;
            Name = bo.Name;
            Link = bo.Link;
            Type = bo.Type;
            CreateTime = bo.CreateTime;
            Description = bo.Description;
            ContactWay = bo.ContactWay;
            FeedUser = bo.FeedUser;
            State = bo.State;
            FeedbackIPAddress = bo.FeedbackIPAddress;
        }
    }
}
