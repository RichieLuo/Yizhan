using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YiZhan.Entities.BusinessManagement.User
{
    /// <summary>
    /// 搜索历史
    /// </summary>
    public class YZ_UserSearchLog : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 如果是未登录的话就存IP
        /// </summary>
        public string UserIdOrIp { get; set; }

        /// <summary>
        /// 搜索时间
        /// </summary>
        public DateTime SearchTime { get; set; }

        /// <summary>
        /// 用来存搜索关键字
        /// </summary>
        public string Name { get; set; }
        public string Description { get; set; }
        public string SortCode { get; set; }

        public YZ_UserSearchLog()
        {
            this.SearchTime = DateTime.Now;
        }

    }
}
