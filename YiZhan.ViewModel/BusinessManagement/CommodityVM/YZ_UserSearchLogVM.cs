using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Entities.BusinessManagement.User;

namespace YiZhan.ViewModels.BusinessManagement
{
    /// <summary>
    /// 搜索记录视图模型
    /// </summary>
    public class YZ_UserSearchLogVM
    {
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
        public YZ_UserSearchLogVM()
        {

        }

        public YZ_UserSearchLogVM(YZ_UserSearchLog bo)
        {
            this.Name = bo.Name;
            this.SearchTime = bo.SearchTime;
            this.UserIdOrIp = bo.UserIdOrIp;
            this.Description = bo.Description;         
        }
    }
}
