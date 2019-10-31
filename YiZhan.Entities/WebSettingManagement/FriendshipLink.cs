using System;
using System.Collections.Generic;
using System.Text;

namespace YiZhan.Entities.WebSettingManagement
{
    public class FriendshipLink : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public bool IsBlank { get; set; }
        public string SortCode { get; set; }

        public FriendshipLink()
        {
            this.Id = Guid.NewGuid();
            this.CreateTime = DateTime.Now;
        }
    }
}
