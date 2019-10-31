#region 相关命名空间引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YiZhan.DataAccess.SqlServer;
using YiZhan.DataAccess;
using YiZhan.DataAccess.SqlServerr;
using YiZhan.Entities.BusinessOrganization;
using YiZhan.DataAccess.Seeds;
using Microsoft.EntityFrameworkCore;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Web.Utilities;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.WebSettingManagement;
using YiZhan.Entities.BusinessManagement.User;
using YiZhan.Entities.BusinessManagement;
using YiZhan.Entities.BusinessManagement.Commodities;
using YiZhan.Common.YZExtensions;
using Microsoft.AspNetCore.Http;
using YiZhan.Entities.Notifications;
#endregion

namespace YiZhan.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加 EF Core 框架
            services.AddDbContext<EntityDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("YiZhan.Common")));
            //services.AddDbContext<EntityDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // 添加微软自己的用户登录令牌资料
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<EntityDbContext>()
                .AddDefaultTokenProviders();

            // Add framework services.
            services.AddMvc();

            // 配置 Identity
            services.Configure<IdentityOptions>(options =>
            {
                // 密码策略的常规设置
                options.Password.RequireDigit = true;            // 是否需要数字字符
                options.Password.RequiredLength = 6;             // 必须的长度
                options.Password.RequireNonAlphanumeric = true;  // 是否需要非拉丁字符，如%，@ 等
                options.Password.RequireUppercase = false;        // 是否需要大写字符
                options.Password.RequireLowercase = false;        // 是否需要小写字符

                // 登录尝试锁定策略
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // Cookie 设置
                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                options.Cookies.ApplicationCookie.LoginPath = "/Account/NoLogin";   // 缺省的登录路径
                options.Cookies.ApplicationCookie.LogoutPath = "/Home/Index";       // 注销以后的路径

                // 其它的一些设置
                options.User.RequireUniqueEmail = true;
            });

            #region 域控制器相关的依赖注入服务清单
            services.AddTransient<IEntityRepository<SystemWorkPlace>, EntityRepository<SystemWorkPlace>>();
            services.AddTransient<IEntityRepository<SystemWorkSection>, EntityRepository<SystemWorkSection>>();
            services.AddTransient<IEntityRepository<SystemWorkTask>, EntityRepository<SystemWorkTask>>();
            services.AddTransient<IEntityRepository<BusinessFile>, EntityRepository<BusinessFile>>();
            services.AddTransient<IEntityRepository<BusinessImage>, EntityRepository<BusinessImage>>();
            services.AddTransient<IEntityRepository<Person>, EntityRepository<Person>>();
            services.AddTransient<IEntityRepository<Department>, EntityRepository<Department>>();
            services.AddTransient<IEntityRepository<YZ_UserAddress>, EntityRepository<YZ_UserAddress>>();
            services.AddTransient<IEntityRepository<YZ_CommodityCategory>, EntityRepository<YZ_CommodityCategory>>();
            services.AddTransient<IEntityRepository<YZ_Commodity>, EntityRepository<YZ_Commodity>>();
            services.AddTransient<IEntityRepository<YZ_CommodityLookCount>, EntityRepository<YZ_CommodityLookCount>>();
            services.AddTransient<IEntityRepository<YZ_CommodityAndImage>, EntityRepository<YZ_CommodityAndImage>>();
            services.AddTransient<IEntityRepository<YZ_UserVisitorLog>, EntityRepository<YZ_UserVisitorLog>>();
            services.AddTransient<IEntityRepository<YZ_UserSearchLog>, EntityRepository<YZ_UserSearchLog>>();
            services.AddTransient<IEntityRepository<UserLoginLog>, EntityRepository<UserLoginLog>>();
            services.AddTransient<IEntityRepository<YZ_CommodityExamine>, EntityRepository<YZ_CommodityExamine>>();
            services.AddTransient<IEntityRepository<YZ_CommodityComment>, EntityRepository<YZ_CommodityComment>>();
            services.AddTransient<IEntityRepository<YZ_Order>, EntityRepository<YZ_Order>>();

            #endregion

            #region 网站配置管理注入
            services.AddTransient<IEntityRepository<Notification>, EntityRepository<Notification>>();
            services.AddTransient<IEntityRepository<Advertisement>, EntityRepository<Advertisement>>();
            services.AddTransient<IEntityRepository<FriendshipLink>, EntityRepository<FriendshipLink>>();
            services.AddTransient<IEntityRepository<SiteSetting>, EntityRepository<SiteSetting>>();
            services.AddTransient<IEntityRepository<UserFeedback>, EntityRepository<UserFeedback>>();
            services.AddTransient<IEntityRepository<SiteConfiguration>, EntityRepository<SiteConfiguration>>();
            services.AddTransient<IEntityRepository<SiteNotice>, EntityRepository<SiteNotice>>();
            services.AddTransient<IEntityRepository<OurTeam>, EntityRepository<OurTeam>>();
            #endregion

            #region  客户端请求上下文注入
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, EntityDbContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/Error");
                app.UseExceptionHandler("/Error/Error");
            }

            app.UseStaticFiles();
            app.UseIdentity();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            DbInitializer.Initialize(context);
            MenuItemCollection.Initializer(context);
        }
    }
}