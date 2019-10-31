//商品详细页面轮播

//默认的索引
var index = 0;
//获取当前节点下的子元素数量
var bannerNum = $("#bannerBox ul").children().length - 1;

//轮播图点击左右侧上一张下一张 显示效果
$(".commodityImages").hover(function () {
    $(".tab-lr").stop().fadeToggle();
})
$(".tab-left").click(function () {
    var leftIndex = index - 1;
    leftIndex = (leftIndex < 0) ? bannerNum : leftIndex;
    index = leftIndex;
    $("#bannerBox ul li").hide().eq(leftIndex).show();
    $('#bannerBox ol li').eq(leftIndex).addClass('current').siblings().removeClass('current');
})
$(".tab-right").click(function () {
    var rightIndex = index + 1;
    rightIndex = (rightIndex > bannerNum) ? 0 : rightIndex;
    index = rightIndex;
    $("#bannerBox ul li").hide().eq(rightIndex).show();
    $('#bannerBox ol li').eq(rightIndex).addClass('current').siblings().removeClass('current');
})
//轮播图的底部缩略图 鼠标经过事件对应的图片
$("#bannerBox ol li").hover(function () {
    var index = $(this).index();
    $("#bannerBox ul li").eq(index).show().siblings().hide();
    $(this).addClass('current').siblings().removeClass('current');
}, function () {

})

$(function () {
    var $ol = $('#bannerBox ol');
    var _olLiW = $('#bannerBox ol li').width() + 4;
    var _olLiLen = $('#bannerBox ol li').length;
    var _olLiOtherW = 15 * (_olLiLen - 1);

    var _olLiNewW = (_olLiW * _olLiLen) + _olLiOtherW;
    $ol.css("width", _olLiNewW);
})
