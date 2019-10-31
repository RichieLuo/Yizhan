using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.WebSettingManagement;

namespace YiZhan.ViewModels.WebSettingManagement
{
    public class FriendshipLinkVM : IEntityVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public string Link { get; set; }
        public bool IsBlank { get; set; }
        public string Description { get; set; }
        public string SortCode { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        public FriendshipLinkVM(FriendshipLink bo)
        {
            Id = bo.Id;
            Name = bo.Name;
            Description = bo.Description;
            CreateTime = bo.CreateTime;
            Link = bo.Link;
            IsBlank = bo.IsBlank;
        }
    }

    public class FriendshipLinkInput
    {
        public Guid Id { get; set; }
        public bool IsNew { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }
        public bool IsBlank { get; set; }
        public string Description { get; set; }

        public FriendshipLinkInput() { }
    }
}
