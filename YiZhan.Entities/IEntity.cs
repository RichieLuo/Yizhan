using System;
using System.Collections.Generic;
using System.Text;

namespace YiZhan.Entities
{
    public interface IEntity:IEntityBase
    {
        string Name { get; set; }
        string Description { get; set; }
        string SortCode { get; set; }
    }
}
