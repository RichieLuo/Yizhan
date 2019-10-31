using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.ApplicationOrganization;
using YiZhan.Entities.BusinessManagement.User;

namespace YiZhan.ViewModels.BusinessManagement.User
{
    /// <summary>
    /// 用户地址视图模型
    /// </summary>
    public class YZ_UserAddressVM : IEntityVM
    {
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
        /// 地址添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 地址：使用Name属性就可以了
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 附加描述
        /// </summary>
        public string Description { get; set; }
        public string SortCode { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        public YZ_UserAddressVM() { }
        public YZ_UserAddressVM(YZ_UserAddress bo)
        {
            Id = bo.Id;
            IsDefault = bo.IsDefault;
            User = bo.User;
            AddTime = bo.AddTime;
            Name = bo.Name;
            Description = bo.Description;
            SortCode = bo.SortCode;

        }
        public void MapToBo(YZ_UserAddress bo)
        {
            bo.Id = Id;
            bo.IsDefault = IsDefault;
            bo.User = User;
            bo.AddTime = AddTime;
            bo.Name = Name;
            bo.Description = Description;
            bo.SortCode = SortCode;
        }
    }
}
