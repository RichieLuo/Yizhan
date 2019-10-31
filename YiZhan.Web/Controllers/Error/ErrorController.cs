using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace YiZhan.Web.Controllers.Error
{
    /// <summary>
    /// 为网站提供一些错误页面和其他页面
    /// </summary>
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 当前用户对网站不是用户自己的数据进行操作的时候显示的页面
        /// </summary>
        /// <returns></returns>
        public IActionResult AWarmWarning()
        {
            ViewBag.WarningMessage = "恶意搞事情，可是会被关进小黑屋的哦！";
            return View();
        }

        public IActionResult Error()
        {    
            ViewData["Message"] = "Ծ‸Ծ页面未找到";
            return PartialView("../../Views/Error/404");
        }
    }
}
