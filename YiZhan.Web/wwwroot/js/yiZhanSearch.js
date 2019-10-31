
//按钮搜索
$("#YiZhanSearch").click(function () {
    yizhanSearch();
})

//回车搜索
$("#YiZhanSearchKeyword").keydown(function (e) {
    if (e.keyCode == 13) {
        yizhanSearch();
    }
})

//搜索页面下的关键字搜索（点击方式）
$(".searchKeyword").click(function () {
    var keyword = $(this).find("a").text().trim() || $(this).text().trim();
    location.href = '../../Home/Search?keyword=' + keyword;
})

//搜索页面下的关键字搜索（点击方式）
$(document).on("click", ".moreKeyword", function () {
    var keyword = $(this).find("input").val();
    console.info(keyword);
    location.href = '../../Home/Search?keyword=' + keyword;
})
//易站搜索跳转
function yizhanSearch() {
    var keyword = $("#YiZhanSearchKeyword").val();
    location.href = '../../Home/Search?keyword=' + keyword;
}