using System;
using System.Collections.Generic;
using System.Text;

namespace YiZhan.Common.JsonModels
{   
    public class CommodityPageHelper
    {
        /// <summary>
        /// 按钮样式样式
        /// PS:样式有三种 位置不同：userCenterCreateBtn1，userCenterCreateBtn2，userCenterCreateBtn3
        /// 说明：数字越小越靠近右边
        /// </summary>
        public string btnClass { get; set; }

        /// <summary>
        /// 按钮对应的方法
        /// PS:写在loadUserCenterMainByNavMenu.js
        /// </summary>
        public string btnMethod { get; set; }

        /// <summary>
        /// 按钮的标题
        /// </summary>
        public string btnTitle { get; set; }
        
    }
}
