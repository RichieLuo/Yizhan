using YiZhan.DataAccess.SqlServer;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.BusinessOrganization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiZhan.Entities.WebSettingManagement;
using YiZhan.Entities.BusinessManagement.Commodities;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.BusinessManagement.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace YiZhan.DataAccess.Seeds
{
    /// <summary>
    /// 构建一个初始化原始数据的组件，用于程序启动的时候执行一些数据初始化的操作
    /// </summary>
    public static class DbInitializer
    {
        static EntityDbContext _Context;

        public static void Initialize(EntityDbContext context)
        {
            _Context = context;
            context.Database.EnsureCreated(); //如果创建了，则不会重新创建

            //_AddApplicationRole();
            _SetWorkPlace();
            _PersonAndDepartment();
            _AddPerson();
            //_AddDefaultUsers();
            _AddStieDefaultData();

        }
        private static void _AddApplicationRole()
        {
            if (_Context.ApplicationRoles.Any())
                return;
            var roles = new List<ApplicationRole>()
            {
               new ApplicationRole(){Name="Admin",DisplayName="系统管理人员", Description="适用于系统管理人员",ApplicationRoleType=ApplicationRoleTypeEnum.适用于系统管理人员,SortCode="69a5f56g" },
               new ApplicationRole(){Name="Maintain",DisplayName="业务数据维护人员", Description="适用于业务数据维护人员",ApplicationRoleType=ApplicationRoleTypeEnum.适用于业务数据维护人员,SortCode="49aaf56g" },
               new ApplicationRole(){Name="AverageUser",DisplayName="普通注册用户", Description="适用于普通注册用户",ApplicationRoleType=ApplicationRoleTypeEnum.适用于普通注册用户 ,SortCode="99avf56g"}
            };

            foreach (var role in roles)
            {
                _Context.ApplicationRoles.Add(role);
            }
            _Context.SaveChanges();
        }

        private static void _SetWorkPlace()
        {
            if (_Context.SystemWorkPlaces.Any())
                return;
            if (_Context.SystemWorkTasks.Any())
                return;
            if (_Context.SystemWorkSections.Any())
                return;

            var wp01 = new SystemWorkPlace() { Name = "系统管理", Description = "", SortCode = "wp01", IconString = "mif-cog" };

            var ws01 = new SystemWorkSection() { Name = "角色用户", Description = "", SortCode = "wp01ws01" };
            var ws02 = new SystemWorkSection() { Name = "导航菜单", Description = "", SortCode = "wp01ws02" };

            var wt0101 = new SystemWorkTask() { Name = "系统角色管理", Description = "", SortCode = "wp01ws01wt001", IconName = "mif-tools", BusinessEntityName = "ApplicationRole", ControllerName = "ApplicationRole", ControllerMethod = "", ControllerMethodParameter = "" };
            var wt0102 = new SystemWorkTask() { Name = "系统用户管理", Description = "", SortCode = "wp01ws01wt002", IconName = "mif-user-3", BusinessEntityName = "ApplicationUser", ControllerName = "ApplicationUser", ControllerMethod = "", ControllerMethodParameter = "" };

            ws01.SystemWorkTasks = new List<SystemWorkTask>();
            ws01.SystemWorkTasks.Add(wt0101);
            ws01.SystemWorkTasks.Add(wt0102);

            var wt0201 = new SystemWorkTask() { Name = "通用菜单配置管理", Description = "", SortCode = "wp01ws01wt001", IconName = "mif-tools", BusinessEntityName = "SystemConfig", ControllerName = "SystemConfig", ControllerMethod = "", ControllerMethodParameter = "" };
            ws02.SystemWorkTasks = new List<SystemWorkTask>();
            ws02.SystemWorkTasks.Add(wt0201);

            wp01.SystemWorkSections = new List<SystemWorkSection>();
            wp01.SystemWorkSections.Add(ws01);
            wp01.SystemWorkSections.Add(ws02);
            _Context.SystemWorkPlaces.Add(wp01);

            _Context.SaveChanges();

        }

        #region  添加默认人员的种子数据

        private static void _PersonAndDepartment()
        {
            if (_Context.Departments.Any())
                return;
            var dept01 = new Department() { Name = "总经办", Description = "", SortCode = "01" };
            var dept02 = new Department() { Name = "综合管理办公室", Description = "", SortCode = "02" };
            var dept03 = new Department() { Name = "开发部", Description = "", SortCode = "03" };
            var dept04 = new Department() { Name = "营运部", Description = "", SortCode = "04" };
            var dept0401 = new Department() { Name = "客户响应服务组", Description = "", SortCode = "0401" };
            var dept0402 = new Department() { Name = "客户需求分析组", Description = "", SortCode = "0402" };
            var dept0403 = new Department() { Name = "应用设计开发组", Description = "", SortCode = "0403" };
            var dept05 = new Department() { Name = "市场部", Description = "", SortCode = "05" };
            var dept06 = new Department() { Name = "品管部", Description = "", SortCode = "06" };
            var dept0601 = new Department() { Name = "营运部驻场服务组", Description = "", SortCode = "0601" };
            var dept0602 = new Department() { Name = "开发部驻场服务组", Description = "", SortCode = "0602" };

            dept01.ParentDepartment = dept01;
            dept02.ParentDepartment = dept02;
            dept03.ParentDepartment = dept03;
            dept04.ParentDepartment = dept04;
            dept0401.ParentDepartment = dept04;
            dept0402.ParentDepartment = dept04;
            dept0403.ParentDepartment = dept04;
            dept05.ParentDepartment = dept05;
            dept06.ParentDepartment = dept06;
            dept0601.ParentDepartment = dept06;
            dept0602.ParentDepartment = dept06;

            var depts = new List<Department>() { dept01, dept02, dept03, dept04, dept0401, dept0402, dept0403, dept05, dept06, dept0601, dept0602 };
            foreach (var item in depts)
                _Context.Departments.Add(item);
            _Context.SaveChanges();

            if (_Context.Persons.Any())
            {
                return;
            }

            var persons = new List<Person>()
            {
                new Person() { Name="刘虎军", Email="Liuhj@qq.com", Mobile="15107728899", SortCode="01001", Description="请补充个人简介", Department=dept01 },
                new Person() { Name="魏小花", Email="weixh@163.com", Mobile="13678622345", SortCode="01002", Description="请补充个人简介",Department=dept02 },
                new Person() { Name="李文慧", Email="liwenhui@tom.com", Mobile="13690251923", SortCode="01003", Description="请补充个人简介",Department=dept02 },
                new Person() { Name="张江的", Email="zhangjd@msn.com", Mobile="13362819012", SortCode="01004", Description="请补充个人简介",Department=dept03 },
                new Person() { Name="萧可君", Email="xiaokj@qq.com", Mobile="13688981234", SortCode="01005", Description="请补充个人简介",Department=dept03 },
                new Person() { Name="魏铜生", Email="weitsh@qq.com", Mobile="18398086323", SortCode="01006", Description="请补充个人简介",Department=dept03 },
                new Person() { Name="刘德华", Email="liudh@icloud.com", Mobile="13866225636", SortCode="01007", Description="请补充个人简介",Department=dept03 },
                new Person() { Name="魏星亮", Email="weixl@liuzhou.com", Mobile="13872236091", SortCode="01008", Description="请补充个人简介",Department=dept04 },
                new Person() { Name="潘家富", Email="panjf@guangxi.com", Mobile="13052366213", SortCode="01009", Description="请补充个人简介",Department=dept0401 },
                new Person() { Name="黎温德", Email="liwende@qq.com", Mobile="13576345509", SortCode="01010", Description="请补充个人简介",Department=dept0401 },
                new Person() { Name="邓淇升", Email="dengqsh@qq.com", Mobile="13709823456", SortCode="01011", Description="请补充个人简介" ,Department=dept0402},
                new Person() { Name="谭冠希", Email="tangx@live.com", Mobile="18809888754", SortCode="01012", Description="请补充个人简介" ,Department=dept0403},
                new Person() { Name="陈慧琳", Email="chenhl@live.com", Mobile="13172038023", SortCode="01013", Description="请补充个人简介" ,Department=dept05},
                new Person() { Name="祁华钰", Email="qihy@qq.com", Mobile="15107726987", SortCode="01014", Description="请补充个人简介" ,Department=dept06},
                new Person() { Name="胡德财", Email="hudc@qq.com", Mobile="13900110988", SortCode="01015", Description="请补充个人简介" ,Department=dept0601},
                new Person() { Name="吴富贵", Email="wufugui@hotmail.com", Mobile="15087109921", SortCode="01016", Description="请补充个人简介" ,Department=dept0602}
            };

            foreach (var person in persons)
            {
                _Context.Persons.Add(person);
            }
            _Context.SaveChanges();
        }

        private static void _AddPerson()
        {
            var dept = _Context.Departments.FirstOrDefault(x => x.Name == "开发部");
            var persons = new List<Person>()
            {
                new Person() { Name="黄虎军", Email="Liuhj@qq.com", Mobile="15107728899", SortCode="01001", Description="请补充个人简介", Department=dept },
                new Person() { Name="河小花", Email="weixh@163.com", Mobile="13678622345", SortCode="01002", Description="请补充个人简介",Department=dept },
                new Person() { Name="陆文慧", Email="liwenhui@tom.com", Mobile="13690251923", SortCode="01003", Description="请补充个人简介",Department=dept },
                new Person() { Name="刘江的", Email="zhangjd@msn.com", Mobile="13362819012", SortCode="01004", Description="请补充个人简介",Department=dept },
                new Person() { Name="韦可君", Email="xiaokj@qq.com", Mobile="13688981234", SortCode="01005", Description="请补充个人简介",Department=dept },
                new Person() { Name="韦铜生", Email="weitsh@qq.com", Mobile="18398086323", SortCode="01006", Description="请补充个人简介",Department=dept },
                new Person() { Name="韦德华", Email="liudh@icloud.com", Mobile="13866225636", SortCode="01007", Description="请补充个人简介",Department=dept },
                new Person() { Name="蒋星亮", Email="weixl@liuzhou.com", Mobile="13872236091", SortCode="01008", Description="请补充个人简介",Department=dept },
                new Person() { Name="蒋家富", Email="panjf@guangxi.com", Mobile="13052366213", SortCode="01009", Description="请补充个人简介",Department=dept },
                new Person() { Name="张温德", Email="liwende@qq.com", Mobile="13576345509", SortCode="01010", Description="请补充个人简介",Department=dept },
                new Person() { Name="张淇升", Email="dengqsh@qq.com", Mobile="13709823456", SortCode="01011", Description="请补充个人简介" ,Department=dept},
                new Person() { Name="秦冠希", Email="tangx@live.com", Mobile="18809888754", SortCode="01012", Description="请补充个人简介" ,Department=dept},
                new Person() { Name="刘慧琳", Email="chenhl@live.com", Mobile="13172038023", SortCode="01013", Description="请补充个人简介" ,Department=dept},
                new Person() { Name="周华钰", Email="qihy@qq.com", Mobile="15107726987", SortCode="01014", Description="请补充个人简介" ,Department=dept},
                new Person() { Name="钱德财", Email="hudc@qq.com", Mobile="13900110988", SortCode="01015", Description="请补充个人简介" ,Department=dept},
                new Person() { Name="孙富贵", Email="wufugui@hotmail.com", Mobile="15087109921", SortCode="01016", Description="请补充个人简介" ,Department=dept},
                new Person() { Name="韦虎军", Email="Liuhj@qq.com", Mobile="15107728899", SortCode="01001", Description="请补充个人简介", Department=dept },
                new Person() { Name="韦小花", Email="weixh@163.com", Mobile="13678622345", SortCode="01002", Description="请补充个人简介",Department=dept },
                new Person() { Name="韦文慧", Email="liwenhui@tom.com", Mobile="13690251923", SortCode="01003", Description="请补充个人简介",Department=dept },
                new Person() { Name="韦江的", Email="zhangjd@msn.com", Mobile="13362819012", SortCode="01004", Description="请补充个人简介",Department=dept },
                new Person() { Name="温可君", Email="xiaokj@qq.com", Mobile="13688981234", SortCode="01005", Description="请补充个人简介",Department=dept },
                new Person() { Name="温铜生", Email="weitsh@qq.com", Mobile="18398086323", SortCode="01006", Description="请补充个人简介",Department=dept },
                new Person() { Name="温德华", Email="liudh@icloud.com", Mobile="13866225636", SortCode="01007", Description="请补充个人简介",Department=dept },
                new Person() { Name="温星亮", Email="weixl@liuzhou.com", Mobile="13872236091", SortCode="01008", Description="请补充个人简介",Department=dept },
                new Person() { Name="温家富", Email="panjf@guangxi.com", Mobile="13052366213", SortCode="01009", Description="请补充个人简介",Department=dept },
                new Person() { Name="覃温德", Email="liwende@qq.com", Mobile="13576345509", SortCode="01010", Description="请补充个人简介",Department=dept },
                new Person() { Name="覃淇升", Email="dengqsh@qq.com", Mobile="13709823456", SortCode="01011", Description="请补充个人简介" ,Department=dept},
                new Person() { Name="覃冠希", Email="tangx@live.com", Mobile="18809888754", SortCode="01012", Description="请补充个人简介" ,Department=dept},
                new Person() { Name="覃慧琳", Email="chenhl@live.com", Mobile="13172038023", SortCode="01013", Description="请补充个人简介" ,Department=dept},
                new Person() { Name="覃华钰", Email="qihy@qq.com", Mobile="15107726987", SortCode="01014", Description="请补充个人简介" ,Department=dept},
                new Person() { Name="覃德财", Email="hudc@qq.com", Mobile="13900110988", SortCode="01015", Description="请补充个人简介" ,Department=dept},
                new Person() { Name="覃富贵", Email="wufugui@hotmail.com", Mobile="15087109921", SortCode="01016", Description="请补充个人简介" ,Department=dept}
            };

            foreach (var person in persons)
            {
                _Context.Persons.Add(person);
            }
            _Context.SaveChanges();
        }
        #endregion

        /// <summary>
        /// 添加网站的基础种子数据
        /// </summary>
        private static void _AddStieDefaultData()
        {
            //默认网站配置数据   

            var siteSetting = new SiteSetting()
            {
                Name = "易站",
                Suffix = "校园闲置商品平台",
                DomainName = "2school.925i.cn",
                KeyWords = "易站，二手，校园闲置商品",
                Description = "校园闲置商品平台",
                Statistics = string.Empty,
                Logo = null,
                Copyright = "易站",
                ICP = "",
                SiteEmail = "YiZhan@925i.cn",
                CustomerService = "<script type='text/javascript'>"
                            + "(function(m, ei, q, i, a, j, s) {"
                            + "m[i] = m[i] || function() {"
                            + " (m[i].a = m[i].a || []).push(arguments)"
                            + " };"
                            + "j = ei.createElement(q),"
                            + "s = ei.getElementsByTagName(q)[0];"
                            + "j.async = true;"
                            + "j.charset = 'UTF-8';"
                            + "j.src = 'https://static.meiqia.com/dist/meiqia.js?_=t';"
                            + "s.parentNode.insertBefore(j, s);"
                     + " })(window, document, 'script', '_MEIQIA');"
                     + "_MEIQIA('entId', 104902); "
                     + " </script> "
            };
            _Context.SiteSettings.Add(siteSetting);

            //站点的默认友情链接
            if (_Context.FriendshipLinks.Any())
                return;
            var link1 = new FriendshipLink { IsBlank = true, Name = "桂林电子科技大学", Link = "https://www.gliet.edu.cn/", Description = "桂林电子科技大学" };
            //var link2 = new FriendshipLink { IsBlank = true, Name = "小莫云联盟", Link = "http://www.925i.cn", Description = "易站御用程序员：二颜的官方网站" };
            var link3 = new FriendshipLink { IsBlank = false, Name = "易站官网", Link = "/", Description = "易站官网" };
            var links = new List<FriendshipLink>
            {
                link1,link3
            };
            _Context.FriendshipLinks.AddRange(links);

            //默认网站广告数据   
            if (_Context.Advertisements.Any())
                return;
            var ad1 = new Advertisement { IsEnable = true, Position = AdvertisementPosition.IndexBanner, Name = "轮播图（600*270）", Link = "https://2school.925i.cn", Image = null, Description = "首页轮播图" };
            var ad2 = new Advertisement { IsEnable = true, Position = AdvertisementPosition.IndexBanner, Name = "轮播图（600*270）", Link = "https://2school.925i.cn", Image = null, Description = "首页轮播图" };
            var ad3 = new Advertisement { IsEnable = true, Position = AdvertisementPosition.IndexBanner, Name = "轮播图（600*270）", Link = "https://2school.925i.cn", Image = null, Description = "首页轮播图" };
            var ad4 = new Advertisement { IsEnable = true, Position = AdvertisementPosition.UserCenterIndex, Name = "广告图（960*140）", Link = "https://2school.925i.cn", Image = null, Description = "用户中心首页广告图" };
            var ad5 = new Advertisement { IsEnable = true, Position = AdvertisementPosition.Detail, Name = "广告图（320*320）", Link = "https://2school.925i.cn", Image = null, Description = "商品详细页广告图" };
            //var ad6 = new Advertisement { IsEnable = true, Position = AdvertisementPosition.Detail, Name = "广告图（320*320）", Link = "https://2school.925i.cn", Image = null, Description = "商品详细页广告图" };
            var ad7 = new Advertisement { IsEnable = true, Position = AdvertisementPosition.UserLoginPage, Name = "用户注册登录背景图（1920*550）", Link = string.Empty, Image = null, Description = "用户注册登录背景图" };
            var ad8 = new Advertisement { IsEnable = true, Position = AdvertisementPosition.AdminLoginPage, Name = "后台登录背景图（1920*927）", Link = string.Empty, Image = null, Description = "后台注册登录背景图" };
            var advertisements = new List<Advertisement> {
                ad1,ad2,ad3,ad4,ad5,ad7,ad8
            };
            _Context.Advertisements.AddRange(advertisements);

            //默认网站配置项
            if (_Context.SiteConfigurations.Any())
                return;
            var siteConfiguration = new SiteConfiguration
            {
                CanLogin = true,
                CanRegister = true,
                IsOpenAd = true,
                IsOpenCode = false,
                IsOpenCS = false
            };
            _Context.SiteConfigurations.Add(siteConfiguration);

            ////默认权限数据
            //if (_Context.ApplicationRoles.Any())
            //    return;
            //var role01 = new ApplicationRole() { Name = "Admin", DisplayName = "系统管理人员", Description = "适用于系统管理人员", ApplicationRoleType = ApplicationRoleTypeEnum.适用于系统管理人员, SortCode = "69a5f56g" };
            //var role02 = new ApplicationRole() { Name = "Maintain", DisplayName = "业务数据维护人员", Description = "适用于业务数据维护人员", ApplicationRoleType = ApplicationRoleTypeEnum.适用于业务数据维护人员, SortCode = "49aaf56g" };
            //var role03 = new ApplicationRole() { Name = "AverageUser", DisplayName = "普通注册用户", Description = "适用于普通注册用户", ApplicationRoleType = ApplicationRoleTypeEnum.适用于普通注册用户, SortCode = "99avf56g" };
            //var roles = new List<ApplicationRole> { role01, role02, role03 };
            //_Context.ApplicationRoles.AddRange(roles);

            ////默认用户数据
            if (_Context.ApplicationUsers.Any())
                return;
            var adminUser = new ApplicationUser() { LockoutEnabled = false, UserName = "admin001", ChineseFullName = "管理员", PhoneNumber = "15578806785", Email = "admin001@qq.com", NormalizedEmail = "admin001@qq.com" };
            var user01 = new ApplicationUser() { LockoutEnabled = false, UserName = "user001", ChineseFullName = "易站用户001", PhoneNumber = "15578806785", Email = "user001@qq.com", NormalizedEmail = "user001@qq.com" };
            var user02 = new ApplicationUser() { LockoutEnabled = false, UserName = "user002", ChineseFullName = "易站用户002", PhoneNumber = "15578806785", Email = "user002@qq.com", NormalizedEmail = "user002@qq.com" };
            var user03 = new ApplicationUser() { LockoutEnabled = false, UserName = "user003", ChineseFullName = "易站用户003", PhoneNumber = "15578806785", Email = "user003@qq.com", NormalizedEmail = "user003@qq.com" };
            var user04 = new ApplicationUser() { LockoutEnabled = false, UserName = "user004", ChineseFullName = "易站用户004", PhoneNumber = "15578806785", Email = "user004@qq.com", NormalizedEmail = "user004@qq.com" };
            var user05 = new ApplicationUser() { LockoutEnabled = false, UserName = "user005", ChineseFullName = "易站用户005", PhoneNumber = "15578806785", Email = "user户005@qq.com", NormalizedEmail = "user005@qq.com" };
            var user06 = new ApplicationUser() { LockoutEnabled = false, UserName = "user006", ChineseFullName = "易站用户006", PhoneNumber = "15578806785", Email = "user006@qq.com", NormalizedEmail = "user006@qq.com" };
            var password = "123@qwe";
            adminUser.PasswordHash = new PasswordHasher<ApplicationUser>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, password);
            user01.PasswordHash = new PasswordHasher<ApplicationUser>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user01, password);
            user02.PasswordHash = new PasswordHasher<ApplicationUser>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user02, password);
            user03.PasswordHash = new PasswordHasher<ApplicationUser>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user03, password);
            user04.PasswordHash = new PasswordHasher<ApplicationUser>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user04, password);
            user05.PasswordHash = new PasswordHasher<ApplicationUser>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user05, password);
            user06.PasswordHash = new PasswordHasher<ApplicationUser>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user06, password);
            var users = new List<ApplicationUser>
            {
               adminUser,user01,user02,user03,user04,user05,user06
            };
            _Context.ApplicationUsers.AddRange(users);


            //添加默认的公告

            if (_Context.SiteNotices.Any())
                return;
            var siteNotice = new SiteNotice { Publisher = adminUser, Name = "重要！重要！重要！易站临时管理注册！", Description = "易站关闭注册，哎妈呀咔咔的，敬请等待开放吧，哈哈哈哈哈哈！" };
            var siteNotices = new List<SiteNotice>
            {
                new SiteNotice{Publisher=adminUser, Name="亲爱的你毕业了吗，闲置的商品不知怎么办？",Description="亲爱的你毕业了吗，闲置的商品不知怎么处理？没关系，交给易站吧！"},
                new SiteNotice{Publisher=adminUser, Name="易站御用攻城狮终于熬出头了，开心开心~",Description="易站御用攻城狮终于熬出头了，开心开心~"},
                new SiteNotice{Publisher=adminUser, Name="嗯哼？你知道易站是干啥的吗，一个神秘的地带哦~",Description="嗯哼？你知道易站是干啥的吗，一个神秘的地带哦~"},
                new SiteNotice{Publisher=adminUser, Name="哈喽，亲爱的终于等到你，易站你来了",Description="哈喽，亲爱的终于等到你，易站你来了"}, 
                new SiteNotice{Publisher=adminUser, Name="重要：易站关闭注册通道通知！！！！！",Description="由于某些原因，易站暂时关闭用户注册通道，敬请等待开放通知！"},
                new SiteNotice{Publisher=adminUser, Name="哈喽，亲爱的终于等到你，易站你来了",Description="哈喽，亲爱的终于等到你，易站你来了"},
                siteNotice,
            };
            _Context.SiteNotices.AddRange(siteNotices);


            //默认商品分类数据
            if (_Context.YZ_CommodityCategories.Any())
                return;
            var category1 = new YZ_CommodityCategory { Name = "数码产品", Category = CommodityCategoryEnum.Electronics, Description = "" };
            var category2 = new YZ_CommodityCategory { Name = "生活用品", Category = CommodityCategoryEnum.Supplies, Description = "" };
            var category3 = new YZ_CommodityCategory { Name = "运动装备", Category = CommodityCategoryEnum.SportsEquipment, Description = "" };
            var category4 = new YZ_CommodityCategory { Name = "衣服鞋帽 ", Category = CommodityCategoryEnum.Clothing, Description = "" };
            var category5 = new YZ_CommodityCategory { Name = "首饰饰品 ", Category = CommodityCategoryEnum.Jewelry, Description = "" };
            var category6 = new YZ_CommodityCategory { Name = "化妆品 ", Category = CommodityCategoryEnum.Cosmetics, Description = "" };
            var commodityCategories = new List<YZ_CommodityCategory>
            {
                category1,category2,category3,category4,category5,category6
            };
            _Context.YZ_CommodityCategories.AddRange(commodityCategories);

            //默认为每一个新添加的商品一个浏览统计
            if (_Context.YZ_CommodityLookCounts.Any())
                return;
            var lookCount1 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount2 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount3 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount4 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount5 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount6 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount7 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount8 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount9 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount10 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount11 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount12 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount13 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount14 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount15 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount16 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount17 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount18 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount19 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount20 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount21 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount22 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount23 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount24 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount25 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount26 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount27 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount28 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount29 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount30 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount31 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount32 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount33 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount34 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount35 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount36 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount37 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount38 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount39 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount40 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount41 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount42 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount43 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount44 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount45 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount46 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount47 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount48 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount49 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount50 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount51 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount52 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount53 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount54 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount55 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount56 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount57 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount58 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount59 = new YZ_CommodityLookCount { LookCount = 0 };
            var lookCount60 = new YZ_CommodityLookCount { LookCount = 0 };

            var commodityLookCounts = new List<YZ_CommodityLookCount> {
                lookCount1,lookCount2,lookCount3,lookCount4,lookCount5,lookCount6 ,lookCount7,lookCount8,lookCount9,lookCount10,
                lookCount11,lookCount12,lookCount13,lookCount14,lookCount15,lookCount16,lookCount17,lookCount18,lookCount19,lookCount20,
                lookCount21,lookCount22,lookCount23,lookCount24,lookCount25,lookCount26,lookCount27,lookCount28,lookCount29,lookCount30,
                lookCount31,lookCount32,lookCount33,lookCount34,lookCount35,lookCount36,lookCount37,lookCount38,lookCount39,lookCount40,
                lookCount41,lookCount42,lookCount43,lookCount44,lookCount45,lookCount46,lookCount47,lookCount48,lookCount49,lookCount50,
                lookCount51,lookCount52,lookCount53,lookCount54,lookCount55,lookCount56,lookCount57,lookCount58,lookCount59,lookCount60
            };

            _Context.YZ_CommodityLookCounts.AddRange(commodityLookCounts);

            //默认商品数据
            if (_Context.YZ_Commodities.Any())
                return;
            var commodity1 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount1, EditTime = DateTime.Now, Name = "笔记本电脑", Description = "惠普笔记本 买了没多久，电池耐用，外观漂亮无任何磕碰和划痕九五新以上，配备三代I5处理器，英雄联盟穿越火线无压力，没有拆修和暗病，4G内存美滋滋，闲置出售", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category1, AscriptionUser = user01 };
            var commodity2 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount2, EditTime = DateTime.Now, Name = "有线耳机", Description = "讲真，正品保证，朋友英国留学回来带回闲置，随便卖卖不差钱，越城区面交", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category1, AscriptionUser = user01 };
            var commodity3 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount3, EditTime = DateTime.Now, Name = "iphone 5s", Description = "日版卡贴机，土豪金，iOS10.3，运行不卡顿，指纹解锁灵敏，支持移动联通2g，16g内存，换过屏幕和电池，操作灵敏，待机时间长，后壳较花，上下边角有磕碰，八成新。买赠送金属边外壳一个，配数据线一条。不包邮，售出不退。", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category1, AscriptionUser = user01 };
            var commodity4 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount4, EditTime = DateTime.Now, Name = "相机", Description = "尼康D300带70-300头，成色还不错如图，注意 无论任何理由引起的退换货，来回运费都由买家承担,拍下即接受!", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category1, AscriptionUser = user01 };
            var commodity5 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount5, EditTime = DateTime.Now, Name = "耳机Bamp", Description = "丹麦B&O H8耳机 买了用了不到1个半月 配件各个都齐全 不议价 一口价", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category1, AscriptionUser = user01 };
            var commodity6 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount6, EditTime = DateTime.Now, Name = "iPhone6原装耳机", Description = "绝对没拆封的原装耳机，不需要转手", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category1, AscriptionUser = user01 };
            var commodity7 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount7, EditTime = DateTime.Now, Name = "6s玫瑰金", Description = "原装正品国行，9成新，无拆无修，实体店出售，可以以旧换新，以旧换旧，欢迎本地朋友来购买，具体可以私聊", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category1, AscriptionUser = user01 };
            var commodity8 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount8, EditTime = DateTime.Now, Name = "数据线", Description = "全新转让！本品长期有效，如有需要请直接拍！", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category1, AscriptionUser = user01 };
            var commodity9 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount9, EditTime = DateTime.Now, Name = "i5s32g电信", Description = "iPhone5电信32g国行主板无修，备用或者老年人用，小孩做作，小巧携带方便。没毛病", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category1, AscriptionUser = user01 };
            var commodity10 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount10, EditTime = DateTime.Now, Name = "三星s8耳机", Description = "买S8配的这个耳机，因我本人从来不用耳机，留着也是放着，就便宜转了包邮，过了这个村，就没有这个耳机了赶紧下手吧", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category1, AscriptionUser = user01 };

            var commodity11 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount11, EditTime = DateTime.Now, Name = "指甲油", Description = "谜尚指甲油，颜色用量见图，专柜购入", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category6, AscriptionUser = user02 };
            var commodity12 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount12, EditTime = DateTime.Now, Name = "滚珠BB霜", Description = "韩国正品holika魔法猫猫水感滚珠滚轮BB霜裸妆遮瑕强保湿美白防晒 买来用了一次，我皮肤比较干不适合。", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category6, AscriptionUser = user02 };
            var commodity13 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount13, EditTime = DateTime.Now, Name = "浪凡香水", Description = "基本没喷，朋友送的", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category6, AscriptionUser = user02 };
            var commodity14 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount14, EditTime = DateTime.Now, Name = "口红", Description = "跟兰芝隔离一样是妈妈从台湾带回来的，全新未拆封，色号001，粉色，涂上去粉粉嫩嫩的，160转～绝对台湾专柜正品，自己也有一个已经用到一半了，不议价哦，福建省内可包邮", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category6, AscriptionUser = user02 };
            var commodity15 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount15, EditTime = DateTime.Now, Name = "360爽肤水", Description = "囤货太多，用不完，出一部分，保质期到19年4月22号", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category6, AscriptionUser = user02 };
            var commodity16 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount16, EditTime = DateTime.Now, Name = "大宝眼霜", Description = "仅使用过一次30", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category6, AscriptionUser = user02 };
            var commodity17 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount17, EditTime = DateTime.Now, Name = "伊思眼霜", Description = "伊思眼霜，两只装的，现在转一只全新，没用过，可议价", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category6, AscriptionUser = user02 };
            var commodity18 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount18, EditTime = DateTime.Now, Name = "阴影粉", Description = "can make阴影粉双睫老板眼影笔用量如图两个都要35包邮单个15不包邮", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category6, AscriptionUser = user02 };
            var commodity19 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount19, EditTime = DateTime.Now, Name = "发际线阴影粉", Description = "买来就只用过一次，基本都是全新，因为我的发迹线不高，也不明显，一下头脑发热买了，买了又用不上哦，有要的可以联系我，是韩国正品代购的", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category6, AscriptionUser = user02 };
            var commodity20 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount20, EditTime = DateTime.Now, Name = "多用途组合粉盒", Description = "白色的是高光，褐色的是阴影，后面两个是腮红。粉质细腻，使用方便，八成新，高光影音影用的多一点，腮红也就三四次。", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category6, AscriptionUser = user02 };

            var commodity21 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount21, EditTime = DateTime.Now, Name = "蓝月亮洗衣液", Description = "家里用的洗衣液，多两箱闲置，一箱4瓶50块钱，如果两箱都要90块钱拿走。不包邮不议价3千克一桶", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category2, AscriptionUser = user03 };
            var commodity22 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount22, EditTime = DateTime.Now, Name = "雨伞", Description = "乳白色伞，蕾丝花边，很漂亮，没用过几次，喜欢的留言吧", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category2, AscriptionUser = user03 };
            var commodity23 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount23, EditTime = DateTime.Now, Name = "台灯", Description = "可调节方向，送灯泡，九新以上 见图", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category2, AscriptionUser = user03 };
            var commodity24 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount24, EditTime = DateTime.Now, Name = "迷你usb电风扇", Description = "卡文，KW-777USB风扇，未使用，京东买东西送的。", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category2, AscriptionUser = user03 };
            var commodity25 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount25, EditTime = DateTime.Now, Name = "饮水机", Description = "机器和桶一起出售，九成新，地点常营，自取80元。很便宜。", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category2, AscriptionUser = user03 };
            var commodity26 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount26, EditTime = DateTime.Now, Name = "金属垃圾桶", Description = "转让9成新金属垃圾桶，是大桶，不包邮，有需要的朋友可以联系我。买其他金额较大的东西可以送垃圾桶", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category2, AscriptionUser = user03 };
            var commodity27 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount27, EditTime = DateTime.Now, Name = "海绵枕头", Description = "年前图新鲜买的记忆海绵枕，枕了不到两月不习惯，转。", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category2, AscriptionUser = user03 };
            var commodity28 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount28, EditTime = DateTime.Now, Name = "保温壶", Description = "800毫升。 1. 使用时长：全新2. 新旧程度描述：包装陈旧3. 存在的问题：无4. 到手时间/有效期/适用条件：无 低价全新转让，不交换，非诚勿扰！！！ ", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category2, AscriptionUser = user03 };
            var commodity29 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount29, EditTime = DateTime.Now, Name = "椅子", Description = "1、带滑轮的椅子，刚用了2个月，要搬家用不到了2、原价320，现在120，非诚勿扰3、另外还有不带轮滑的椅子，很舒服4、自提最好，就在南京西路旁站旁", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category2, AscriptionUser = user03 };
            var commodity30 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount30, EditTime = DateTime.Now, Name = "靠垫", Description = "靠垫 全新转了 地方太小了 不够用。喜欢就带走吧", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category2, AscriptionUser = user03 };

            var commodity31 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount31, EditTime = DateTime.Now, Name = "背心", Description = "前后V领背心，拼接雪纺显瘦，有点小淑女，均码，胸围95以下穿，有弹力", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category4, AscriptionUser = user04 };
            var commodity32 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount32, EditTime = DateTime.Now, Name = "休闲裤", Description = "180的 32码 全新 一次都没穿过 适合春夏秋穿", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category4, AscriptionUser = user04 };
            var commodity33 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount33, EditTime = DateTime.Now, Name = "牛仔短裤", Description = "适合平时26.27裤子，非全新", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category4, AscriptionUser = user04 };
            var commodity34 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount34, EditTime = DateTime.Now, Name = "棒球服", Description = "穿过一次！低价只为曝光，卖价70，跟全新差不多，质量很好，网购130多买回来的！大刀事妈请绕道，闲置品售出不退换", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category4, AscriptionUser = user04 };
            var commodity35 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount35, EditTime = DateTime.Now, Name = "灰色卫衣", Description = "厚实。洗过一次。", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category4, AscriptionUser = user04 };
            var commodity36 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount36, EditTime = DateTime.Now, Name = "女个性衬衫", Description = "穿过两次9.8成新。买来下过一次水", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category4, AscriptionUser = user04 };
            var commodity37 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount37, EditTime = DateTime.Now, Name = "白色五分袖条纹衬衫", Description = "穿过两次 有点短 均码 买上衣赠短裤 短裤全新 L码", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category4, AscriptionUser = user04 };
            var commodity38 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount38, EditTime = DateTime.Now, Name = "coach女挎包", Description = "专柜购买，背了几次，9.5新，五金件基本无磨损，懂的来。", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category4, AscriptionUser = user04 };
            var commodity39 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount39, EditTime = DateTime.Now, Name = "登山包", Description = "买来就用过两次 一直闲置 看上的拍走 有点积灰了自己去洗", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category4, AscriptionUser = user04 };
            var commodity40 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount40, EditTime = DateTime.Now, Name = "欧美复古英伦宽檐礼帽", Description = "就在寝室试过 闲置", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category4, AscriptionUser = user04 };

            var commodity41 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount41, EditTime = DateTime.Now, Name = "石英手表", Description = "只带过几次，几乎全新，有意者提前信息之后自取；或者顺丰到付。", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category5, AscriptionUser = user05 };
            var commodity42 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount42, EditTime = DateTime.Now, Name = "莫森太阳镜", Description = "16年在深圳实体店购买，年底结婚生宝宝，戴了不到5次，9成新，实话实说、实物实拍，没加任何美颜效果，喜欢的朋友可以联系我。", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category5, AscriptionUser = user05 };
            var commodity43 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount43, EditTime = DateTime.Now, Name = "iwatch3 Nike+ 黑色", Description = "去年底买的，95成新，没怎么带，经常放着，现在转掉。仅接受同城交易。", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category5, AscriptionUser = user05 };
            var commodity44 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount44, EditTime = DateTime.Now, Name = "dior女款太阳镜", Description = "没戴过几次", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category5, AscriptionUser = user04 };
            var commodity45 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount45, EditTime = DateTime.Now, Name = "DKNY 女表 ", Description = "个人闲置，15年购入 全新未用、膜都在，正品不退不换。私聊报价 合适就卖 爽快包邮 谢谢", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category5, AscriptionUser = user05 };
            var commodity46 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount46, EditTime = DateTime.Now, Name = "珠链", Description = "我自己玩了两年多 自我感觉玩的很有味道", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category5, AscriptionUser = user04 };
            var commodity47 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount47, EditTime = DateTime.Now, Name = "925银手链", Description = "生日礼物，全新！！银饰，镶嵌紫色钻，短手链。不议价不包邮！", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category5, AscriptionUser = user05 };
            var commodity48 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount48, EditTime = DateTime.Now, Name = "蜜蜡吊坠", Description = "今年买的，戴了一段时间，想再换其它样式了，便宜转出。当毛衣链也不错的！不是天然的，人工的。", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category5, AscriptionUser = user05 };
            var commodity49 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount49, EditTime = DateTime.Now, Name = "天梭男士全自动机械表", Description = "天梭男士全自动机械表 带了年把 成色九成新 一切正常 保卡有 专柜可验货", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category5, AscriptionUser = user05 };
            var commodity50 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount50, EditTime = DateTime.Now, Name = "香奈儿太阳镜", Description = "几乎全新，戴过一次，购于香港，全套包装带盒子，没有小票了，现在特价转，保证正品，支持任何形式的验货，闲置物品不退换，诚信交易，非诚勿扰，价格已经最低，不议价！", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category5, AscriptionUser = null };

            var commodity51 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount51, EditTime = DateTime.Now, Name = "乒乓球拍", Description = "胜捷正品乒乓球拍双拍直拍横拍初学者学生兵兵球训练比赛用 2只装", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category3, AscriptionUser = user06 };
            var commodity52 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount52, EditTime = DateTime.Now, Name = "篮球", Description = "九成左右新。正规商场购买，斯伯丁篮球。喜欢的联系我吧。", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category3, AscriptionUser = user06 };
            var commodity53 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount53, EditTime = DateTime.Now, Name = "羽毛球拍", Description = "正品无暇胜利victor超级波3250羽毛球拍低价转让换新拍了所以低价转让，已经是最低价格，不议价，不包邮，球拍快递使用专业羽毛球拍包装纸箱。", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category3, AscriptionUser = null };
            var commodity54 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount54, EditTime = DateTime.Now, Name = "足球", Description = "学生足球青少年足球冠合足球全新足球送打气筒气针球网袋同城自取不邮", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category3, AscriptionUser = user06 };
            var commodity55 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount55, EditTime = DateTime.Now, Name = "瑜伽垫", Description = "手瑜伽垫 之前买回来当垫子用后来就一直放着新旧程度：9成新原价是25一张，现在只要10元！秒杀价了，全部打包有优惠", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category3, AscriptionUser = null };
            var commodity56 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount56, EditTime = DateTime.Now, Name = "弹簧臂力器", Description = "臂力器拉力器，买回来新鲜了几天，不想玩了。本地自取。", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category3, AscriptionUser = user06 };
            var commodity57 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount57, EditTime = DateTime.Now, Name = "滑板", Description = "买回来新鲜一段时间就不玩了，各部件完好正常使用中。滑板长71cm，宽20cm，滑...", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category3, AscriptionUser = user05 };
            var commodity58 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount58, EditTime = DateTime.Now, Name = "轮滑鞋", Description = "乐秀rx4 轮滑鞋 穿过一次 还是新的 送桩跟鞋扣 鞋号是38的 原本思想玩轮滑的 后来发现驾驭不了 所以现在以低价格转卖 在京东买...", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category3, AscriptionUser = null };
            var commodity59 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount59, EditTime = DateTime.Now, Name = "网球拍", Description = "这是一款比较耐打的网球拍 配有全新名牌威尔胜高质网球一个 适合初学者及日常练习使用 球拍有八九成新 球拍的整体牢固度好 ", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category3, AscriptionUser = null };
            var commodity60 = new YZ_Commodity { Range = TransactionWayAndRange.校园内, Way = TransactionWayAndRange.线下交易, LookCount = lookCount60, EditTime = DateTime.Now, Name = "护腕", Description = "由于买了两个，空出一个现在便宜转出去，全新", Price = 99, Stock = 999, Unit = "件", State = YZ_CommodityState.OnSale, Category = category3, AscriptionUser = user06 };

            var commodities = new List<YZ_Commodity>
            {
                commodity1,commodity2,commodity3,commodity4,commodity5,commodity6,commodity7,commodity8,commodity9,commodity10,
                commodity11,commodity12,commodity13,commodity14,commodity15,commodity16,commodity17,commodity18,commodity19,commodity20,
                commodity21,commodity22,commodity23,commodity24,commodity25,commodity26,commodity27,commodity28,commodity29,commodity30,
                commodity31,commodity32,commodity33,commodity34,commodity35,commodity36,commodity37,commodity38,commodity39,commodity40,
                commodity41,commodity42,commodity43,commodity44,commodity45,commodity46,commodity47,commodity48,commodity49,commodity50,
                commodity51,commodity52,commodity53,commodity54,commodity55,commodity56,commodity57,commodity58,commodity59,commodity60
            };
            _Context.YZ_Commodities.AddRange(commodities);


            //默认商品图片数据
            if (_Context.BusinessImages.Any())
                return;
            //数码类
            var commodityImage1 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Electronics/commodity1.jpg", RelevanceObjectId = commodity1.Id, Type = ImageType.CommodityCover };
            var commodityImage2 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Electronics/commodity2.jpg", RelevanceObjectId = commodity2.Id, Type = ImageType.CommodityCover };
            var commodityImage3 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Electronics/commodity3.jpg", RelevanceObjectId = commodity3.Id, Type = ImageType.CommodityCover };
            var commodityImage4 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Electronics/commodity4.jpg", RelevanceObjectId = commodity4.Id, Type = ImageType.CommodityCover };
            var commodityImage5 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Electronics/commodity5.jpg", RelevanceObjectId = commodity5.Id, Type = ImageType.CommodityCover };
            var commodityImage6 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Electronics/commodity6.jpg", RelevanceObjectId = commodity6.Id, Type = ImageType.CommodityCover };
            var commodityImage7 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Electronics/commodity7.jpg", RelevanceObjectId = commodity7.Id, Type = ImageType.CommodityCover };
            var commodityImage701 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Electronics/commodity701.jpg", RelevanceObjectId = commodity7.Id, Type = ImageType.CommodityImgs };
            var commodityImage702 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Electronics/commodity702.jpg", RelevanceObjectId = commodity7.Id, Type = ImageType.CommodityImgs };
            var commodityImage703 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Electronics/commodity703.jpg", RelevanceObjectId = commodity7.Id, Type = ImageType.CommodityImgs };
            var commodityImage704 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Electronics/commodity704.jpg", RelevanceObjectId = commodity7.Id, Type = ImageType.CommodityImgs };
            var commodityImage8 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Electronics/commodity8.jpg", RelevanceObjectId = commodity8.Id, Type = ImageType.CommodityCover };
            var commodityImage9 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Electronics/commodity9.jpg", RelevanceObjectId = commodity9.Id, Type = ImageType.CommodityCover };
            var commodityImage10 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Electronics/commodity10.jpg", RelevanceObjectId = commodity10.Id, Type = ImageType.CommodityCover };
            //化妆品
            var commodityImage11 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Cosmetics/commodity11.jpg", RelevanceObjectId = commodity11.Id, Type = ImageType.CommodityCover };
            var commodityImage12 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Cosmetics/commodity12.jpg", RelevanceObjectId = commodity12.Id, Type = ImageType.CommodityCover };
            var commodityImage13 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Cosmetics/commodity13.jpg", RelevanceObjectId = commodity13.Id, Type = ImageType.CommodityCover };
            var commodityImage14 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Cosmetics/commodity14.jpg", RelevanceObjectId = commodity14.Id, Type = ImageType.CommodityCover };
            var commodityImage15 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Cosmetics/commodity15.jpg", RelevanceObjectId = commodity15.Id, Type = ImageType.CommodityCover };
            var commodityImage16 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Cosmetics/commodity16.jpg", RelevanceObjectId = commodity16.Id, Type = ImageType.CommodityCover };
            var commodityImage17 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Cosmetics/commodity17.jpg", RelevanceObjectId = commodity17.Id, Type = ImageType.CommodityCover };
            var commodityImage18 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Cosmetics/commodity18.jpg", RelevanceObjectId = commodity18.Id, Type = ImageType.CommodityCover };
            var commodityImage19 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Cosmetics/commodity19.jpg", RelevanceObjectId = commodity19.Id, Type = ImageType.CommodityCover };
            var commodityImage20 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Cosmetics/commodity20.jpg", RelevanceObjectId = commodity20.Id, Type = ImageType.CommodityCover };
            //生活用品
            var commodityImage21 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Supplies/commodity21.jpg", RelevanceObjectId = commodity21.Id, Type = ImageType.CommodityCover };
            var commodityImage22 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Supplies/commodity22.jpg", RelevanceObjectId = commodity22.Id, Type = ImageType.CommodityCover };
            var commodityImage23 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Supplies/commodity23.jpg", RelevanceObjectId = commodity23.Id, Type = ImageType.CommodityCover };
            var commodityImage24 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Supplies/commodity24.jpg", RelevanceObjectId = commodity24.Id, Type = ImageType.CommodityCover };
            var commodityImage25 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Supplies/commodity25.jpg", RelevanceObjectId = commodity25.Id, Type = ImageType.CommodityCover };
            var commodityImage26 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Supplies/commodity26.jpg", RelevanceObjectId = commodity26.Id, Type = ImageType.CommodityCover };
            var commodityImage27 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Supplies/commodity27.jpg", RelevanceObjectId = commodity27.Id, Type = ImageType.CommodityCover };
            var commodityImage28 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Supplies/commodity28.jpg", RelevanceObjectId = commodity28.Id, Type = ImageType.CommodityCover };
            var commodityImage29 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Supplies/commodity29.jpg", RelevanceObjectId = commodity29.Id, Type = ImageType.CommodityCover };
            var commodityImage30 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Supplies/commodity30.jpg", RelevanceObjectId = commodity30.Id, Type = ImageType.CommodityCover };
            //衣服鞋帽
            var commodityImage31 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Clothing/commodity31.jpg", RelevanceObjectId = commodity31.Id, Type = ImageType.CommodityCover };
            var commodityImage32 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Clothing/commodity32.jpg", RelevanceObjectId = commodity32.Id, Type = ImageType.CommodityCover };
            var commodityImage33 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Clothing/commodity33.jpg", RelevanceObjectId = commodity33.Id, Type = ImageType.CommodityCover };
            var commodityImage34 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Clothing/commodity34.jpg", RelevanceObjectId = commodity34.Id, Type = ImageType.CommodityCover };
            var commodityImage35 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Clothing/commodity35.jpg", RelevanceObjectId = commodity35.Id, Type = ImageType.CommodityCover };
            var commodityImage36 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Clothing/commodity36.jpg", RelevanceObjectId = commodity36.Id, Type = ImageType.CommodityCover };
            var commodityImage37 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Clothing/commodity37.jpg", RelevanceObjectId = commodity37.Id, Type = ImageType.CommodityCover };
            var commodityImage38 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Clothing/commodity38.jpg", RelevanceObjectId = commodity38.Id, Type = ImageType.CommodityCover };
            var commodityImage39 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Clothing/commodity39.jpg", RelevanceObjectId = commodity39.Id, Type = ImageType.CommodityCover };
            var commodityImage40 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Clothing/commodity40.jpg", RelevanceObjectId = commodity40.Id, Type = ImageType.CommodityCover };
            //首饰饰品
            var commodityImage41 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Jewelry/commodity41.jpg", RelevanceObjectId = commodity41.Id, Type = ImageType.CommodityCover };
            var commodityImage42 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Jewelry/commodity42.jpg", RelevanceObjectId = commodity42.Id, Type = ImageType.CommodityCover };
            var commodityImage43 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Jewelry/commodity43.jpg", RelevanceObjectId = commodity43.Id, Type = ImageType.CommodityCover };
            var commodityImage44 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Jewelry/commodity44.jpg", RelevanceObjectId = commodity44.Id, Type = ImageType.CommodityCover };
            var commodityImage45 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Jewelry/commodity45.jpg", RelevanceObjectId = commodity45.Id, Type = ImageType.CommodityCover };
            var commodityImage46 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Jewelry/commodity46.jpg", RelevanceObjectId = commodity46.Id, Type = ImageType.CommodityCover };
            var commodityImage47 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Jewelry/commodity47.jpg", RelevanceObjectId = commodity47.Id, Type = ImageType.CommodityCover };
            var commodityImage48 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Jewelry/commodity48.jpg", RelevanceObjectId = commodity48.Id, Type = ImageType.CommodityCover };
            var commodityImage49 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Jewelry/commodity49.jpg", RelevanceObjectId = commodity49.Id, Type = ImageType.CommodityCover };
            var commodityImage50 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/Jewelry/commodity50.jpg", RelevanceObjectId = commodity50.Id, Type = ImageType.CommodityCover };
            //运动装备
            var commodityImage51 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/SportsEquipment/commodity51.jpg", RelevanceObjectId = commodity51.Id, Type = ImageType.CommodityCover };
            var commodityImage52 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/SportsEquipment/commodity52.jpg", RelevanceObjectId = commodity52.Id, Type = ImageType.CommodityCover };
            var commodityImage53 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/SportsEquipment/commodity53.jpg", RelevanceObjectId = commodity53.Id, Type = ImageType.CommodityCover };
            var commodityImage54 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/SportsEquipment/commodity54.jpg", RelevanceObjectId = commodity54.Id, Type = ImageType.CommodityCover };
            var commodityImage55 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/SportsEquipment/commodity55.jpg", RelevanceObjectId = commodity55.Id, Type = ImageType.CommodityCover };
            var commodityImage56 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/SportsEquipment/commodity56.jpg", RelevanceObjectId = commodity56.Id, Type = ImageType.CommodityCover };
            var commodityImage57 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/SportsEquipment/commodity57.jpg", RelevanceObjectId = commodity57.Id, Type = ImageType.CommodityCover };
            var commodityImage58 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/SportsEquipment/commodity58.jpg", RelevanceObjectId = commodity58.Id, Type = ImageType.CommodityCover };
            var commodityImage59 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/SportsEquipment/commodity59.jpg", RelevanceObjectId = commodity59.Id, Type = ImageType.CommodityCover };
            var commodityImage60 = new BusinessImage { UploadPath = "../../images/DefaultImg/CommoditiesImg/SportsEquipment/commodity60.jpg", RelevanceObjectId = commodity60.Id, Type = ImageType.CommodityCover };
            var adImg1 = new BusinessImage { UploadPath = "../../images/Index/banner/banner1.jpg", RelevanceObjectId = ad1.Id, Type = ImageType.Banners };
            var adImg2 = new BusinessImage { UploadPath = "../../images/Index/banner/banner2.jpg", RelevanceObjectId = ad2.Id, Type = ImageType.Banners };
            var adImg3 = new BusinessImage { UploadPath = "../../images/Index/banner/banner3.jpg", RelevanceObjectId = ad3.Id, Type = ImageType.Banners };
            var adImg4 = new BusinessImage { UploadPath = "../../images/DefaultImg/userCenterIndexAd.jpg", RelevanceObjectId = ad4.Id, Type = ImageType.Advertisements };
            var adImg5 = new BusinessImage { UploadPath = "../../images/Index/banner/index_img_001.png", RelevanceObjectId = ad5.Id, Type = ImageType.Advertisements };
            //var adImg6 = new BusinessImage { UploadPath = "../../images/Index/banner/index_img_001.png", RelevanceObjectId = ad6.Id, Type = ImageType.Advertisements };
            var adImg7 = new BusinessImage { UploadPath = "../../images/Index/login/loginbg.jpg", RelevanceObjectId = ad7.Id, Type = ImageType.LoginBg };
            var adImg8 = new BusinessImage { UploadPath = "../../images/Admin/adbg_01.jpg", RelevanceObjectId = ad8.Id, Type = ImageType.LoginBg };

            var siteNoticeImg1 = new BusinessImage { UploadPath = "../../images/Index/banner/index_img_001.png", RelevanceObjectId = siteNotice.Id, Type = ImageType.Notices };
            var siteNoticeImg2 = new BusinessImage { UploadPath = "../../images/Index/banner/banner1.jpg", RelevanceObjectId = siteNotice.Id, Type = ImageType.Notices };

            var commoditiesImages = new List<BusinessImage>
            {
                commodityImage1,commodityImage2,commodityImage3,commodityImage4,commodityImage5,commodityImage6,commodityImage7,commodityImage701,commodityImage702,commodityImage703,commodityImage704,commodityImage8,commodityImage9,commodityImage10,
                commodityImage11,commodityImage12,commodityImage13,commodityImage14,commodityImage15,commodityImage16,commodityImage17,commodityImage18,commodityImage19,commodityImage20,
                commodityImage21,commodityImage22,commodityImage23,commodityImage24,commodityImage25,commodityImage26,commodityImage27,commodityImage28,commodityImage29,commodityImage30,
                commodityImage31,commodityImage32,commodityImage33,commodityImage34,commodityImage35,commodityImage36,commodityImage37,commodityImage38,commodityImage39,commodityImage40,
                commodityImage41,commodityImage42,commodityImage43,commodityImage44,commodityImage45,commodityImage46,commodityImage47,commodityImage48,commodityImage49,commodityImage50,
                commodityImage51,commodityImage52,commodityImage53,commodityImage54,commodityImage55,commodityImage56,commodityImage57,commodityImage58,commodityImage59,commodityImage60,
                adImg1,adImg2,adImg3,adImg4, adImg5, adImg7,adImg8,siteNoticeImg1,siteNoticeImg2
            };
            _Context.BusinessImages.AddRange(commoditiesImages);

            //默认用户浏览历史数据
            if (_Context.YZ_UserVisitorLogs.Any())
                return;
            var userVisitorLog01 = new YZ_UserVisitorLog { CommodityId = commodity1.Id, Category = commodity1.Category, UserIdOrIp = string.Empty };
            var userVisitorLog02 = new YZ_UserVisitorLog { CommodityId = commodity2.Id, Category = commodity2.Category, UserIdOrIp = string.Empty };
            var userVisitorLog03 = new YZ_UserVisitorLog { CommodityId = commodity3.Id, Category = commodity3.Category, UserIdOrIp = string.Empty };
            var userVisitorLog04 = new YZ_UserVisitorLog { CommodityId = commodity4.Id, Category = commodity4.Category, UserIdOrIp = string.Empty };
            var userVisitorLog05 = new YZ_UserVisitorLog { CommodityId = commodity5.Id, Category = commodity5.Category, UserIdOrIp = string.Empty };
            var userVisitorLog06 = new YZ_UserVisitorLog { CommodityId = commodity6.Id, Category = commodity6.Category, UserIdOrIp = string.Empty };
            var userVisitorLog07 = new YZ_UserVisitorLog { CommodityId = commodity7.Id, Category = commodity7.Category, UserIdOrIp = string.Empty };
            var userVisitorLogs = new List<YZ_UserVisitorLog>
            {
                userVisitorLog01,userVisitorLog02,userVisitorLog03,
                userVisitorLog04,userVisitorLog05,userVisitorLog06,userVisitorLog07
            };
            _Context.YZ_UserVisitorLogs.AddRange(userVisitorLogs);

            //统一保存
            _Context.SaveChanges();
        }
    }
}
