using System;
using System.Collections.Generic;
using System.Text;

namespace YiZhan.Entities.ApplicationOrganization
{
    /// <summary>
    /// 用户登录日志
    /// </summary>
    public class UserLoginLog : IEntity
    {
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public string Ip { get; set; }
        public string BrowserInfo { get; set; }
        public string Address { get; set; }
        public DateTime loginTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SortCode { get; set; }

        public UserLoginLog()
        {
            this.loginTime = DateTime.Now;
        }
    }
}
