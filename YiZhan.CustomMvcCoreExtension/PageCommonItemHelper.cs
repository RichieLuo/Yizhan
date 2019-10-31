using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using YiZhan.Common.JsonModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace YiZhan.CustomMvcCoreExtension
{
    public static class PageCommonItemHelper
    {
        public static HtmlString YiZhanSetListPageParameter(this IHtmlHelper helper, ListPageParameter pageParameter)
        {
            var htmlContent = new StringBuilder();

            htmlContent.Append(
                "<input type='hidden' name='对应的类型' id='yiZhanTypeId' value='" + pageParameter.ObjectTypeId + "' />");
            htmlContent.Append(
                "<input type='hidden' name='当前页码' id='yiZhanPageIndex' value='" + pageParameter.PageIndex + "' />");
            htmlContent.Append(
                "<input type='hidden' name='每页数据条数' id='yiZhanPageSize' value='" + pageParameter.PageSize +
                    "' /> ");
            htmlContent.Append(
                "<input type='hidden' name='分页数量' id='yiZhanPageAmount' value='" + pageParameter.PageAmount +
                    "' />");
            htmlContent.Append(
                "<input type='hidden' name='相关的对象的总数' id='yiZhanObjectAmount' value='" + pageParameter.ObjectAmount +
                    "' />");
            htmlContent.Append(
                "<input type='hidden' name='当前的检索关键词' id='yiZhanKeyword' value='" + pageParameter.Keyword + "' />");
            htmlContent.Append(
                "<input type='hidden' name='排序属性' id='yiZhanSortProperty' value='" + pageParameter.SortProperty +
                    "' /> ");
            htmlContent.Append(
                "<input type='hidden' name='排序方向' id='yiZhanSortDesc' value='" + pageParameter.SortDesc + "' />");
            htmlContent.Append(
                "<input type='hidden' name='当前焦点对象 Id' id='yiZhanSelectedObjectId' value='" +
                    pageParameter.SelectedObjectId + "' />");
            htmlContent.Append(
                "<input type='hidden' name='当前是否为检索' id='yiZhanIsSearch' value='" +
                    pageParameter.IsSearch + "' />");
            return new HtmlString(htmlContent.ToString());

        }
    }
}
