using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using YiZhan.Entities.ApplicationOrganization;

namespace YiZhan.Entities.BusinessManagement.User
{
    /// <summary>
    /// 用户地址表
    /// </summary>
    public class YZ_UserAddress : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 是否为默认地址
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 关联的用户
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 地址直接使用Name属性
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 可能会有的描述
        /// </summary>
        public string Description { get; set; }
        public string SortCode { get; set; }
     

        public YZ_UserAddress()
        {
            Id = Guid.NewGuid();
        }

    }
}
