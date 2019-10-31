using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using YiZhan.Entities.Attachments;

namespace YiZhan.Entities.WebSettingManagement
{
    public class Advertisement : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public BusinessImage Image { get; set; }
        public DateTime CreateTime { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SortCode { get; set; }
        public bool IsEnable { get; set; }
        public AdvertisementPosition Position { get; set; }
        public Advertisement()
        {
            this.Id = Guid.NewGuid();
            this.CreateTime = DateTime.Now;
        }

    }
}
