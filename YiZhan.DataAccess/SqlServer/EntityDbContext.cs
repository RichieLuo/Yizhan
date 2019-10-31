using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.BusinessManagement.Audit;
using YiZhan.Entities.BusinessOrganization;
using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Entities.WebSettingManagement;
using YiZhan.Entities.BusinessManagement.Commodities;
using YiZhan.Entities.BusinessManagement.User;
using YiZhan.Entities.Notifications;

namespace YiZhan.DataAccess.SqlServer
{
    public class EntityDbContext : IdentityDbContext<ApplicationUser>
    {
        public EntityDbContext(DbContextOptions<EntityDbContext> options) : base(options)
        {
        }

        #region 用户与角色相关
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        #endregion

        #region 用户工作区与菜单相关
        public DbSet<SystemWorkPlace> SystemWorkPlaces { get; set; }
        public DbSet<SystemWorkSection> SystemWorkSections { get; set; }
        public DbSet<SystemWorkTask> SystemWorkTasks { get; set; }
        #endregion

        #region 业务组织相关
        public DbSet<Person> Persons { get; set; }
        public DbSet<Department> Departments { get; set; }


        #endregion

        #region 一些基础业务对象相关
        public DbSet<AuditRecord> AuditRecords { get; set; }
        public DbSet<BusinessFile> BusinessFiles { get; set; }
        public DbSet<BusinessImage> BusinessImages { get; set; }
        public DbSet<BusinessVideo> BusinessVideos { get; set; }
        #endregion

        #region 网站基础数据业务相关
        public DbSet<SiteSetting> SiteSettings { get; set; }
        public DbSet<UserFeedback> UserFeedbacks { get; set; }

        #endregion
        
        #region 网站业务相关

        public DbSet<YZ_Commodity> YZ_Commodities { get; set; }
        public DbSet<YZ_CommodityCategory> YZ_CommodityCategories { get; set; }
        public DbSet<YZ_CommodityLookCount> YZ_CommodityLookCounts { get; set; }
        public DbSet<YZ_CommodityAndImage> YZ_CommodityAndImages { get; set; }
        public DbSet<YZ_UserAddress> YZ_UserAddresses { get; set; }
        public DbSet<YZ_UserSearchLog> YZ_UserSearchLogs { get; set; }
        public DbSet<YZ_UserVisitorLog> YZ_UserVisitorLogs { get; set; }
        public DbSet<YZ_CommodityExamine> YZ_CommodityExamines { get; set; }
        public DbSet<YZ_CommodityComment> YZ_CommodityComments { get; set; }
        public DbSet<YZ_Order> YZ_Orders { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<FriendshipLink> FriendshipLinks { get; set; }
        public DbSet<OurTeam> OurTeams { get; set; }
        #endregion

        #region 应用系统相关
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserLoginLog> UserLoginLogs { get; set; }
        public DbSet<SiteConfiguration> SiteConfigurations { get; set; }
        public DbSet<SiteNotice> SiteNotices { get; set; }
        #endregion

        /// <summary>
        /// 如果不需要 DbSet<T> 所定义的属性名称作为数据库表的名称，可以在下面的位置自己重新定义
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Person>().ToTable("Person");
            base.OnModelCreating(modelBuilder);

        }
    }
}
