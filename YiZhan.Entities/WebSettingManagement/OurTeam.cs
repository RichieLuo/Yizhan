using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YiZhan.Entities.WebSettingManagement
{
    /// <summary>
    /// 团队
    /// </summary>
    public class OurTeam : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 成员名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工作职位
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// 成员描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// QQ号码
        /// </summary>
        public string QQnumber { get; set; }

        /// <summary>
        /// 微博链接
        /// </summary>
        public string WeiboLink { get; set; }

        /// <summary>
        /// 个人网站链接
        /// </summary>
        public string PersonalSiteLink { get; set; }

        /// <summary>
        /// 加入时间
        /// </summary>
        public DateTime CreateTime { get; set; }


        public string SortCode { get; set; }

        public OurTeam()
        {
            this.Id = Guid.NewGuid();
            this.CreateTime = DateTime.Now;
        }
    }
}
