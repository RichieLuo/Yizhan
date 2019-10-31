using System;
using System.Collections.Generic;
using System.Text;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.WebSettingManagement;

namespace YiZhan.ViewModels.WebSettingManagement
{
    public class AdvertisementVM : IEntityVM
    {
        public Guid Id { get; set; }
        public BusinessImage Image { get; set; }
        public DateTime CreateTime { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SortCode { get; set; }
        public string OrderNumber { get; set; }
        public AdvertisementPosition Position { get; set; }
        public bool IsNew { get; set; }
        public bool IsEnable { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        public AdvertisementVM() { }
        public AdvertisementVM(Advertisement bo)
        {
            this.Id = bo.Id;
            this.Link = bo.Link;
            this.Name = bo.Name;
            this.Image = bo.Image;
            this.Description = bo.Description;
            this.CreateTime = bo.CreateTime;
            this.IsEnable = bo.IsEnable;
            this.Position = bo.Position;
        }
    }
}
