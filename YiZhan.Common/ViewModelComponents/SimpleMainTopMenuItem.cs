using System;
using System.Collections.Generic;
using System.Text;

namespace YiZhan.Common.ViewModelComponents
{
    /// <summary>
    /// 用于简化构建普通的横向顶层菜单数据的过渡性质的类
    /// </summary>
    public class SimpleMainTopMenuItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }     // 显示的名称
        public string URL { get; set; }      // 操作导航的
        public string SortCode { get; set; } // 编码
    }
}
