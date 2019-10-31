using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using YiZhan.Common.ViewModelComponents.ForMvcHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace YiZhan.CustomMvcCoreExtension
{
    public static class FormItemHelper
    {
        /// <summary>
        /// 使用 BootStrap 框架简单的 Helper
        /// </summary>
        /// <param name="helper">扩展的实例</param>
        /// <param name="inputItemSpec">显示的名称</param>
        /// <returns>用于前端的 HtmlString </returns>
        public static HtmlString YiZhanBoorStrapInputSimple(this IHtmlHelper helper, HtmlInputItemSpecification inputItemSpec)
        {
            var itemDisplayName = inputItemSpec.ItemDisplayName;
            var itemId = inputItemSpec.ItemId;
            var itemVale = inputItemSpec.ItemValue;
            var placeholderString = inputItemSpec.PlaceholderString;

            var htmlContentBuilder = new StringBuilder();

            htmlContentBuilder.Append("<div class='form-group' id='"+ itemId + "_DIV'>");
            htmlContentBuilder.Append("<div class='col-xs-3'>");
            htmlContentBuilder.Append("<label for='" + itemId + "' class='pull-right control-label'>" + itemDisplayName+"：</label>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("<div class='col-xs-9'>");
            htmlContentBuilder.Append("<input type='text' class='form-control' " +
                "name='" + itemId + "' " +
                "id='" + itemId + "' " +
                "placeholder='" + placeholderString + "' " +
                "value='" + itemVale + "' " +
                "onfocus='' " +
                "onBlur='' " +
                "autofocus=''>");
            htmlContentBuilder.Append("<div id='" + itemId + "_Help'></div>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("</div>");
            return new HtmlString(htmlContentBuilder.ToString());
        }

        /// <summary>
        /// 使用 BootStrap 框架简单的 Helper
        /// </summary>
        /// <param name="helper">扩展的实例</param>
        /// <param name="inputItemSpec">显示的名称</param>
        /// <returns>用于前端的 HtmlString </returns>
        public static HtmlString YiZhanBoorStrapInputValidate(this IHtmlHelper helper, HtmlInputItemSpecification inputItemSpec)
        {
            var itemDisplayName   = inputItemSpec.ItemDisplayName;
            var itemId            = inputItemSpec.ItemId;
            var itemVale          = inputItemSpec.ItemValue;
            var placeholderString = inputItemSpec.PlaceholderString;
            var onfocusFuntion    = inputItemSpec.OnfocusFuntion;
            var onBlurFunction    = inputItemSpec.OnBlurFunction;
            var autofocusName     = inputItemSpec.AutofocusName;

            var htmlContentBuilder = new StringBuilder();

            htmlContentBuilder.Append("<div class='form-group' id='" + itemId + "_DIV'>");
            htmlContentBuilder.Append("<div class='col-xs-3'>");
            htmlContentBuilder.Append("<label for='" + itemId + "' class='pull-right  control-label'>" + itemDisplayName + "：</label>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("<div class='col-xs-9'>");

            htmlContentBuilder.Append("<input type='text' class='form-control' " +
                "name='" + itemId + "' " +
                "id='" + itemId + "' " +
                "placeholder='" + placeholderString + "' " +
                "value='" + itemVale + "' " +
                "onfocus='" + onfocusFuntion + "' " +
                "onBlur='" + onBlurFunction + "' " +
                "autofocus='" + autofocusName + "'>");

            htmlContentBuilder.Append("<div id='" + itemId + "_Help'></div>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("</div>");
            return new HtmlString(htmlContentBuilder.ToString());
        }

        /// <summary>
        /// 使用 BootStrap 框架简单的 Helper
        /// </summary>
        /// <param name="helper">扩展的实例</param>
        /// <param name="inputItemSpec">显示的名称</param>
        /// <returns>用于前端的 HtmlString </returns>
        public static HtmlString YiZhanBoorStrapInputPassword(this IHtmlHelper helper, HtmlInputItemSpecification inputItemSpec)
        {
            var itemDisplayName = inputItemSpec.ItemDisplayName;
            var itemId = inputItemSpec.ItemId;
            var itemVale = inputItemSpec.ItemValue;
            var placeholderString = inputItemSpec.PlaceholderString;
            var onfocusFuntion = inputItemSpec.OnfocusFuntion;
            var onBlurFunction = inputItemSpec.OnBlurFunction;
            var autofocusName = inputItemSpec.AutofocusName;

            var htmlContentBuilder = new StringBuilder();
            
            htmlContentBuilder.Append("<div class='form-group' id='" + itemId + "_DIV'>");
            htmlContentBuilder.Append("<div class='col-xs-3'>");
            htmlContentBuilder.Append("<label for='" + itemId + "' class='pull-right control-label'>" + itemDisplayName + "：</label>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("<div class='col-xs-9'>");

            htmlContentBuilder.Append("<input type='password' class='form-control' " +
                "name='" + itemId + "' " +
                "id='" + itemId + "' " +
                "placeholder='" + placeholderString + "' " +
                "value='" + itemVale + "' " +
                "onfocus='" + onfocusFuntion + "' " +
                "onBlur='" + onBlurFunction + "' " +
                "autofocus='" + autofocusName + "'>");

            htmlContentBuilder.Append("<div id='" + itemId + "_Help'></div>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("</div>");
            return new HtmlString(htmlContentBuilder.ToString());
        }

        /// <summary>
        /// 使用 BootStrap 框架简单的 Helper
        /// </summary>
        /// <param name="helper">扩展的实例</param>
        /// <param name="inputItemSpec">显示的名称</param>
        /// <returns>用于前端的 HtmlString </returns>
        public static HtmlString YiZhanBoorStrapInputForGoods(this IHtmlHelper helper, HtmlInputItemSpecification inputItemSpec)
        {
            var itemDisplayName = inputItemSpec.ItemDisplayName;
            var itemId = inputItemSpec.ItemId;
            var itemVale = inputItemSpec.ItemValue;
            var placeholderString = inputItemSpec.PlaceholderString;
            var onfocusFuntion = inputItemSpec.OnfocusFuntion;
            var onBlurFunction = inputItemSpec.OnBlurFunction;
            var autofocusName = inputItemSpec.AutofocusName;

            var htmlContentBuilder = new StringBuilder();

            
            htmlContentBuilder.Append("<div class='form-group' id='" + itemId + "_DIV'>"); 
            htmlContentBuilder.Append("<div class='col-xs-2'>");
            htmlContentBuilder.Append("<label for='" + itemId + "' class='pull-right control-label'>" + itemDisplayName + "：</label>"); 
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("<div class='col-xs-10'>");
            htmlContentBuilder.Append("<input type='text' class='form-control' " +
                "name='" + itemId + "' " +
                "id='" + itemId + "' " +
                "placeholder='" + placeholderString + "' " +
                "value='" + itemVale + "' " +
                "onfocus='" + onfocusFuntion + "' " +
                "onBlur='" + onBlurFunction + "' " +
                "autofocus='" + autofocusName + "'>");
            htmlContentBuilder.Append("<div id='" + itemId + "_Help'></div>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("</div>");
            return new HtmlString(htmlContentBuilder.ToString());
        }

        /// <summary>
        /// 使用 BootStrap 框架简单的 Helper
        /// </summary>
        /// <param name="helper">扩展的实例</param>
        /// <param name="inputItemSpec">显示的名称</param>
        /// <returns>用于前端的 HtmlString </returns>
        public static HtmlString YiZhanBoorStrapInputForArea(this IHtmlHelper helper, HtmlInputItemSpecification inputItemSpec)
        {
            var itemDisplayName = inputItemSpec.ItemDisplayName;
            var itemId = inputItemSpec.ItemId;
            var itemVale = inputItemSpec.ItemValue;
            var placeholderString = inputItemSpec.PlaceholderString;
            var onfocusFuntion = inputItemSpec.OnfocusFuntion;
            var onBlurFunction = inputItemSpec.OnBlurFunction;
            var autofocusName = inputItemSpec.AutofocusName;

            var htmlContentBuilder = new StringBuilder();


            htmlContentBuilder.Append("<div class='form-group' id='" + itemId + "_DIV'>");
            htmlContentBuilder.Append("<div class='col-xs-2'>");
            htmlContentBuilder.Append("<label for='" + itemId + "' class='pull-right control-label'>" + itemDisplayName + "：</label>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("<div class='col-xs-10'>");
            htmlContentBuilder.Append("<textarea  class='form-control' " +"rows = '4'"+ "style='resize: none;'"+
                "name='" + itemId + "' " +
                "id='" + itemId + "' " +
                "placeholder='" + placeholderString + "' " +
                 "onfocus='" + onfocusFuntion + "' " +
                "onBlur='" + onBlurFunction + "' " +
                "autofocus='" + autofocusName + "'>" +
                itemVale);
            htmlContentBuilder.Append("</textarea>");
            htmlContentBuilder.Append("<div id='" + itemId + "_Help'></div>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("</div>");
            return new HtmlString(htmlContentBuilder.ToString());
        }

        public static HtmlString YiZhanBoorStrapInputForSettled(this IHtmlHelper helper, HtmlInputItemSpecification inputItemSpec)
        {
            var itemDisplayName = inputItemSpec.ItemDisplayName;
            var itemId = inputItemSpec.ItemId;
            var itemVale = inputItemSpec.ItemValue;
            var placeholderString = inputItemSpec.PlaceholderString;
            var onfocusFuntion = inputItemSpec.OnfocusFuntion;
            var onBlurFunction = inputItemSpec.OnBlurFunction;
            var autofocusName = inputItemSpec.AutofocusName;

            var htmlContentBuilder = new StringBuilder();
           
            htmlContentBuilder.Append("<div class='form-group settledgroup' id='" + itemId + "_DIV'>");
            htmlContentBuilder.Append("<div class='col-xs-3'>");
            htmlContentBuilder.Append("<label for='" + itemId + "' class='pull-right control-label'>" + itemDisplayName + "：</label>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("<div class='col-xs-5'>");
            htmlContentBuilder.Append("<input type='text' class='form-control' " +
                "name='" + itemId + "' " +
                "id='" + itemId + "' " +
                "placeholder='" + placeholderString + "' " +
                "value='" + itemVale + "' " +
                "onfocus='" + onfocusFuntion + "' " +
                "onBlur='" + onBlurFunction + "' " +
                "autofocus='" + autofocusName + "'>");
            htmlContentBuilder.Append("</div>");
            htmlContentBuilder.Append("<div class='col-xs-4' id='" + itemId + "_Help'></div>");
            htmlContentBuilder.Append("</div>");
            return new HtmlString(htmlContentBuilder.ToString());
        }

    }
}
