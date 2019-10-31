// 首页js书写 js文件


// 主要js内容开始

// 顶部导航js动画效果开始
//     $(".").css({});


// 顶部导航js动画效果结束 下拉菜单
$(".dw-MyYiZhan").hover(function () {
    $(".dw-MyYiZhan .dropdown-content").css({ "display": "block" });
    $(".dw-MyYiZhan>a").addClass("aAfter");

}, function () {
    $(".dw-MyYiZhan .dropdown-content").css({ "display": "none" });
    $(".dw-MyYiZhan>a").removeClass("aAfter");
});


$(".dw-SellerCenter").hover(function () {
    $(".dw-SellerCenter .dropdown-content").css({ "display": "block" });
    $(".dw-SellerCenter>a").addClass("aAfter");

}, function () {
    $(".dw-SellerCenter .dropdown-content").css({ "display": "none" });
    $(".dw-SellerCenter>a").removeClass("aAfter");
});

$(".dw-WebNavigation").hover(function () {
    $(".dw-WebNavigation .dropdown-content").css({ "display": "block" });
    $(".dw-WebNavigation>a").addClass("aAfter");

}, function () {
    $(".dw-WebNavigation .dropdown-content").css({ "display": "none" });
    $(".dw-WebNavigation>a").removeClass("aAfter");
});


//轮播图
$(function () {

    //系统安装年份和当前年份设置(安装时间后期从服务器获取)
    SystemInstallAndCurrentYear();

    var i = 0;
    var imgWidth = $("#show-area ul li").width();
    //得到所有li的个数
    var size = $("#show-area ul").children().length;

    var ulW = imgWidth * size + imgWidth;
    $("#show-area ul").css({ "width": ulW + "px" });

    var clone = $("#show-area ul li").first().clone();
    $("#show-area ul").append(clone);

    //复制第一张图片并且添加到最后达到无缝连接的效果
    /*第一步*/
    //一开始循环添加按钮
    for (var j = 0; j < size; j++) {
        $("#controler").append("<div></div>");
    }
    //为什么要size - 1？因为最后一张图片只是作一个过渡效果我们显示的始终还是4张图片
    //所以添加按钮的时候我们也应该添加4个按钮
    $("#controler div").eq(0).addClass("onclick");

    //左按钮
    $("#tab-left").click(function () {
        Toleft();
    })
    //右按钮
    $("#tab-right").click(function () {
        Toright();
    })
    //按钮移出移入事件
    $("#controler div").hover(function () {
        i = $(this).index();
        clearInterval(timer);
        $("#show-area ul").stop().animate({ left: -i * imgWidth });
        $(this).addClass("onclick").siblings().removeClass("onclick");
        //$("#index").html("index的值：" + index);

    }, function () {
        timer = setInterval(function () {
            Toright();
        }, 3000)
    })
    //ul鼠标移出移入事件
    $("#show-area ul").hover(function () {
        clearInterval(timer);
    }, function () {
        timer = setInterval(function () {
            Toright();
        }, 3000)
    })

    $("#indexBanner").hover(function () {
        $("#tab-lr").stop().fadeToggle();
    })
    //两个方向按钮鼠标移出移入事件
    $("#tab-left,#tab-right").mouseover(function () {
        clearInterval(timer);
    }).mouseout(function () {
        timer = setInterval(function () {
            Toright();
        }, 3000)
    })

    //定时器
    var timer = setInterval(function () {
        Toright();
    }, 3000)

    //左按钮实现的函数
    function Toleft() {
        i++;

        //当当前图片为最后一张图片的时候（我们一开始复制并且添加到ul最后面的图片）
        //并且再点击了一次左按钮，这时候我们就利用css的快速转换效果把ul移动第一张图片的位置
        //并且第二张图片滑入达到无缝效果（css的变换效果很快我们肉眼是很难看见的）
        if (i > size) {
            $("#show-area ul").css({ left: 0 });
            i = 1;
        }
        $("#show-area ul").stop().animate({ left: -i * imgWidth }, 1000);

        if (i == size) {
            $("#controler div").eq(0).addClass("onclick").siblings().removeClass("onclick");
        } else {
            $("#controler div").eq(i).addClass("onclick").siblings().removeClass("onclick");
        }
    }

    //右按钮实现的函数
    function Toright() {
        //同理，如果当前图片位置是第一张图片我再按一下右按钮那么我们也利用css的快速变换使它的位置来到最后一张图片的位置（size），并且让倒数第二张图片滑动进来
        i--;
        //console.log("size="+size+"， i="+i);
        if (i <= -1) {
            $("#show-area ul").css({ left: -size * imgWidth });
            i = size - 1;
        }
        $("#show-area ul").stop().animate({ left: -i * imgWidth }, 1000);
        $("#controler div").eq(i).addClass("onclick").siblings().removeClass("onclick");

    }
    //设置猜你喜欢的宽度	
    $(".tabContentItems").css({ "width": GetTabContentWidth() });
});


////左侧列表的显示
//var _iListLis = $(".indexCategoryList ul").children();
//var _iListShow = $(".cListShow").children();
//var _cShowIndx;
//_iListLis.hover(function () {
//    var currIndex = $(this).index();
//    _iListShow.each(function (index, element) {
//        if (index == currIndex) {
//            _iListShow.eq(currIndex).addClass("cShow").siblings().removeClass("cShow");
//            _cShowIndx = currIndex;
//        }
//    })

//}, function () {
//    _iListShow.eq(_cShowIndx).removeClass("cShow");
//});


//猜你喜欢Tab栏
var _iTabMenu = $(".i-tabHead-tabMenu").children();
var _iTabContent = $(".i-Ylike-tabContent").children();
_iTabMenu.hover(function () {
    var _tabMenuIndex = $(this).index();
    if (_tabMenuIndex == 0) { $(".likeChange").fadeIn(); $(".moreSecond-hand").fadeOut(); }
    else { $(".likeChange").fadeOut(); $(".moreSecond-hand").fadeIn(); }
    _iTabContent.each(function (index, element) {
        if (index == _tabMenuIndex) {
            _iTabMenu.eq(_tabMenuIndex).addClass("i-tabMenu-active").siblings().removeClass("i-tabMenu-active");
            _iTabContent.eq(_tabMenuIndex).addClass("i-tabContent-active").siblings().removeClass("i-tabContent-active");
            $(".i-tabContent-active").css({ "width": GetTabContentWidth() });
        }
    })
})


//获取猜你喜欢的内容宽度
function GetTabContentWidth() {
    var _tabCItemCount = $(".i-tabContent-active").children().length;
    var _itemWidth = $(".tab-c-item").width() + 20;
    var _tabCItemWidth = _tabCItemCount * _itemWidth;
    return _tabCItemWidth;
}

//换一换功能
var _iTabContentWidth = $(".i-Ylike-tabContent").width();
var _tabCItemWidthForLikeChange = $(".tabContentItems");
var _rangCount = Math.ceil(GetTabContentWidth() / _iTabContentWidth);
var _clickCount = 1;
$(".likeChange").click(function () {
    if (GetTabContentWidth() > _iTabContentWidth) {
        $(this).find("span").css({ "transform": " rotateZ(" + (360 * _clickCount) + "deg)", "transition": "transform 1s" });
        if (_clickCount < _rangCount) {
            _iTabContent.eq(0).css({ "left": -_iTabContentWidth * _clickCount, "transition": "left 1s" });
            _clickCount++;
        }
        else if (_clickCount == _rangCount) {
            _clickCount = 1;
            _iTabContent.eq(0).css({ "left": 0 });
        }
    }
})

//站点公告的脚本
//$(document).on("hover", ".bulletinLi", function () {
//    $(this).css({ "background": "#f2f2f2" });
//}, function () {
//    $(this).css({ "background": "#fff" });
//    })
$(document).on("mouseenter", ".bulletinLi", function () {
    $(this).css({ "background": "#f2f2f2" });
})
$(document).on("mouseleave", ".bulletinLi", function () {
    $(this).css({ "background": "#fff" });
})

//系统安装年份和当前年份设置(安装时间后期从服务器获取)

function SystemInstallAndCurrentYear() {
    var time = new Date();
    //var installYear = time.getFullYear() - 1;
    var currentYear = time.getFullYear();
    //$("#cStartTime").text(installYear);
    $("#cEndTime").text(currentYear);
}
