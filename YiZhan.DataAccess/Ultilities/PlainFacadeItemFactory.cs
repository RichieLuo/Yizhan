﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YiZhan.Common.ViewModelComponents;
using YiZhan.DataAccess.SqlServerr;
using YiZhan.DataAccess.SqlServer;
using YiZhan.Entities;

namespace YiZhan.DataAccess.Ultilities
{
    /// <summary>
    /// 用于将一些需要的对象集合转换为简单的 PlainFacdeItem 集合或者个体的方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class PlainFacadeItemFactory<T> where T : class, IEntity, new()
    {
        /// <summary>
        /// 根据泛型类型指定的对象来提取数据库中对应的全部对象，并转换为 PlainFacdeItem 集合
        /// </summary>
        /// <returns></returns>
        public static List<PlainFacadeItem> Get()
        {
            //var service = new EntityRepository<T>(new EntityDbContext());
            //var sourceItems = service.GetAll().OrderBy(s => s.SortCode);

            var items = new List<PlainFacadeItem>();
            //foreach (var pItem in sourceItems)
            //{
            //    var item = new PlainFacadeItem() { ID = pItem.ID.ToString(), Name = pItem.Name,DisplayName=pItem.Name, Description = pItem.Description, SortCode = pItem.SortCode };
            //    items.Add(item);
            //}
            return items;

        }

        /// <summary>
        /// 将已经提取的对应的泛型集合对象，直接转换为 PlainFacdeItem 集合
        /// </summary>
        /// <param name="sourceItems"></param>
        /// <returns></returns>
        public static List<PlainFacadeItem> Get(List<T> sourceItems)
        {
            var items = new List<PlainFacadeItem>();
            foreach (var pItem in sourceItems)
            {
                var item = new PlainFacadeItem()
                {
                    Id = pItem.Id.ToString(),
                    Name = pItem.Name,
                    DisplayName=pItem.Name,
                    Description = pItem.Description,
                    SortCode = pItem.SortCode
                };
                items.Add(item);
            }
            return items;

        }

        /// <summary>
        /// 直接将泛型类型中指定的枚举类型转换为 PlainFacdeItem 集合
        /// </summary>
        /// <returns></returns>
        public static List<PlainFacadeItem> GetByEnum()
        {
            var items = new List<PlainFacadeItem>();
            foreach (var eItem in Enum.GetValues(typeof(T)))
            {
                var item = new PlainFacadeItem()
                {
                    Id = Enum.GetName(typeof(T), eItem),
                    Name = eItem.ToString(),
                    DisplayName = eItem.ToString(),
                    Description = "",
                    SortCode = Enum.GetName(typeof(T), eItem)
                };
            }
            return items;
        }

        /// <summary>
        /// 将布尔类型，转换为 PlainFacadeItem 集合，并提供缺省值设置
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static List<PlainFacadeItem> GetByBool(bool defaultValue)
        {
            var t = new PlainFacadeItem() { Id = "true", Name = "是", DisplayName = "是" };
            var f = new PlainFacadeItem() { Id = "false", Name = "否", DisplayName = "否" };

            if (defaultValue)
                t.OperateName = "checked";
            else
                f.OperateName = "checked";

            var results = new List<PlainFacadeItem>() { t, f };
            return results;

        }

        public static List<PlainFacadeItem> GetByBool()
        {
            var t = new PlainFacadeItem() { Id = "true", Name = "是", DisplayName = "是", OperateName = "" };
            var f = new PlainFacadeItem() { Id = "false", Name = "否", DisplayName = "否", OperateName = "" };
            var results = new List<PlainFacadeItem>() { t, f };
            return results;

        }

        /// <summary>
        /// 处理男女，将布尔类型，转换为 PlainFacadeItem 集合，并提供缺省值设置
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static List<PlainFacadeItem> GetBySex(bool defaultValue)
        {
            var t = new PlainFacadeItem() { Id = "true", Name = "男", DisplayName="男" };
            var f = new PlainFacadeItem() { Id = "false", Name = "女",DisplayName="女" };

            if (defaultValue)
                t.OperateName = "checked";
            else
                f.OperateName = "checked";

            var results = new List<PlainFacadeItem>() { t, f };
            return results;

        }

        public static List<PlainFacadeItem> GetBySex()
        {
            var t = new PlainFacadeItem() { Id = "true", Name = "男", DisplayName = "男", OperateName = "checked" };
            var f = new PlainFacadeItem() { Id = "false", Name = "女",DisplayName="女", OperateName = "" };
            var results = new List<PlainFacadeItem>() { t, f };
            return results;

        }
    }
}
