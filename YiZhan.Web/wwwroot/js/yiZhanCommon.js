
/*
 *  根据指定的地址直接链接跳转回退
 * @param {} urlString 
 * @returns {} 
 */
function yiZhanGotoNewPage(urlString) {
    window.location.href = urlString;
}

/*
 *  根据指定的地址 urlString 访问控制器方法，然后根据返回的局部页刷新指定的 targetDiv 区域
 * @param {} urlString 
 * @param {} targetDivElelmentId 
 * @returns {} 
 */
function yiZhanGotoNewPartial(urlString, targetDivElelmentId) {
    yiZhanGotoNewPartialByJsonAndShowStatus(urlString, "", targetDivElelmentId, "", true);
}


/*
 * 根据指定的地址 urlString 访问控制器方法，
 * 执行访问时，在指定的位置呈现状态信息，
 * 然后根据返回的局部页刷新指定的 targetDiv 区域
 * @param {} urlString 
 * @param {} targetDivElelmentId 
 * @param {} statucMessage 
 * @returns {} 
 */
function yiZhanGotoNewPartialAndShowStatus(urlString, targetDivElelmentId, statucMessage) {
    yiZhanGotoNewPartialByJsonAndShowStatus(urlString, "", targetDivElelmentId, statucMessage, true);
}

/*
 * 根据指定的地址 urlString 和 jsonData 访问控制器方法，
 * 
 * @param {} urlString 
 * @param {} jsonData 
 * @param {} targetDivElelmentId 
 * @returns {} 
 */
function yiZhanGotoNewPartialByJson(urlString, jsonData, targetDivElelmentId) {
    yiZhanGotoNewPartialByJsonAndShowStatus(urlString, jsonData, targetDivElelmentId, "", true);
}

/*
 * 根据指定的地址 urlString 和 jsonData 访问控制器方法,
 * 执行访问时，在指定的位置呈现状态信息，
 * 然后根据返回的局部页刷新指定的 targetDivElelmentId 区域
 * 
 * @param {} urlString 
 * @param {} jsonData 
 * @param {} targetDivElelmentId 
 * @param {} statusMessage
 * * @param {} isAsync 
 * @returns {} 
 */
function yiZhanGotoNewPartialByJsonAndShowStatus(urlString, jsonData, targetDivElelmentId, statusMessage, isAsync) {
    $.ajax({
        cache: false,
        type: "POST",
        async: isAsync,
        url: urlString,
        data: jsonData,
        beforeSend: function () {
            if (statusMessage !== "") {
                $("#" + targetDivElelmentId).html(statusMessage);
            }
        }
    }).done(function (data) {
        var reg = /^<script>.*<\/script>$/;
        if (reg.test(data)) {
            // 这句是为了响应后台返回的js
            $('body').append("<span id='responseJs'>" + data + "</span>").remove("#responseJs");
            return;
        } else {
            if (targetDivElelmentId !== '') {
                $("#" + targetDivElelmentId).html(data);
            }
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.error("调试错误:" + errorThrown);
    }).always(function () {
    });
}

/*
 * 创建新的 ListParaJson 对象
 * @param {} typeId 
 * @returns {} 
 */
function yiZhanCreateListParaJson(typeId) {
    yiZhanIntializationListPageParameter(typeId);
    var listParaJson = yiZhanGetListParaJson();
    return listParaJson;
}

/*
 *  重新初始化页面规格参数
 * @param {} typeId 
 * @returns {} 
 */
function yiZhanIntializationListPageParameter(typeId) {
    $("#yiZhanTypeId").val(typeId);
    $("#yiZhanPageIndex").val("1");
    $("#yiZhanPageSize").val("18");
    $("#yiZhanPageAmount").val("0");
    $("#yiZhanObjectAmount").val("0");
    $("#yiZhanKeyword").val("");
    $("#yiZhanSortProperty").val("SortCode");
    $("#yiZhanSortDesc").val("default");
    $("#yiZhanSelectedObjectId").val("");
    $("#yiZhanIsSearch").val("False");
}

/*
 * 提取页面分页规格数据,构建 ListParaJson 对象
 * @returns {} 
 */
function yiZhanGetListParaJson() {
    // 提取缺省的页面规格参数
    var yiZhanPageTypeId = $("#yiZhanTypeId").val();
    var yiZhanPagePageIndex = $("#yiZhanPageIndex").val();
    var yiZhanPagePageSize = $("#yiZhanPageSize").val();
    var yiZhanPagePageAmount = $("#yiZhanPageAmount").val();
    var yiZhanPageObjectAmount = $("#yiZhanObjectAmount").val();
    var yiZhanPageKeyword = $("#yiZhanKeyword").val();
    var yiZhanPageSortProperty = $("#yiZhanSortProperty").val();
    var yiZhanPageSortDesc = $("#yiZhanSortDesc").val();
    var yiZhanPageSelectedObjectId = $("#yiZhanSelectedObjectId").val();
    var yiZhanPageIsSearch = $("#yiZhanIsSearch").val();
    // 创建前端 json 数据对象
    var listParaJson = "{" +
        "ObjectTypeId:\"" + yiZhanPageTypeId + "\", " +
        "PageIndex:\"" + yiZhanPagePageIndex + "\", " +
        "PageSize:\"" + yiZhanPagePageSize + "\", " +
        "PageAmount:\"" + yiZhanPagePageAmount + "\", " +
        "ObjectAmount:\"" + yiZhanPageObjectAmount + "\", " +
        "Keyword:\"" + yiZhanPageKeyword + "\", " +
        "SortProperty:\"" + yiZhanPageSortProperty + "\", " +
        "SortDesc:\"" + yiZhanPageSortDesc + "\", " +
        "IsSearch:\"" + yiZhanPageIsSearch + "\", " +
        "SelectedObjectId:\"" + yiZhanPageSelectedObjectId + "\"" +
        "}";

    return listParaJson;
}