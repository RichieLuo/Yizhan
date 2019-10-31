using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using YiZhan.Entities.ApplicationOrganization;

namespace YiZhan.Entities.WebSettingManagement
{
    /// <summary>
    /// 用户反馈实体
    /// </summary>
    public class UserFeedback : IEntity
    {
        [Key]
        public Guid Id { get; set; }

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

        /// <summary>
        /// 反馈的类型
        /// </summary>
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
        public UserFeedback()
        {
            this.Id = Guid.NewGuid();
            this.CreateTime = DateTime.Now;
            this.State = false;
        }
    }
}
