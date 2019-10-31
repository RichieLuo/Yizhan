using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.BusinessManagement.User;

namespace YiZhan.ViewModels.BusinessManagement.User
{
    /// <summary>
    /// 网站搜索历史 视图模型
    /// </summary>
    public class YZ_UserSearchLogsVM : IEntityVM
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 如果是未登录的话就存IP
        /// </summary>
        public string UserIdOrIp { get; set; }

        /// <summary>
        /// 搜索时间
        /// </summary>
        public DateTime LookTime { get; set; }

        /// <summary>
        /// 用来存搜索关键字
        /// </summary>
        public string Name { get; set; }
        public string Description { get; set; }
        public string SortCode { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        public YZ_UserSearchLogsVM() { }
        public YZ_UserSearchLogsVM(YZ_UserVisitorLog bo)
        {
            Id = bo.Id;
            UserIdOrIp = bo.UserIdOrIp;
            LookTime = bo.LookTime;
            Name = bo.Name;
            Description = bo.Description;
            SortCode = bo.SortCode;
        }

        public void MapToBo(YZ_UserVisitorLog bo)
        {

            bo.Id = Id;
            bo.UserIdOrIp = UserIdOrIp;
            bo.LookTime = LookTime;
            bo.Name = Name;
            bo.Description = Description;
            bo.SortCode = SortCode;
        }

    }
}
