using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.BusinessOrganization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace YiZhan.Entities.ApplicationOrganization
{
    /// <summary>
    /// 系统用户定义，这是直接继承 IdentityUser 实现的
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string LastName { get; set; }
        [StringLength(100)]
        public string ChineseFullName { get; set; }
        [StringLength(50)]
        public string MobileNumber { get; set; }           // 移动电话
        public string QQNumber { get; set; }
        public virtual Person Person { get; set; }         // 关联的实际个人
        public virtual BusinessImage Avatar { get; set; }  // 用户头像

        public string Description { get; set; }

        public DateTime CreateTime { get; set; } //注册时间

        public ApplicationUser() : base()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreateTime = DateTime.Now;
        }
        public ApplicationUser(string userName) : base(userName)
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserName = userName;
            this.CreateTime = DateTime.Now;
        }
    }
}
