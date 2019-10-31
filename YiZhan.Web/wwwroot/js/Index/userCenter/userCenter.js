//这里书写用户中心的脚本代码
(function () {
    var $userNavList = $(".user-nav ul").children();
    $userNavList.each(function (e, index) {
        $(this).click(function () {
            $(this).addClass("liScale").siblings().removeClass("liScale");
            $(this).find("a").addClass("YiZhanThemeColor").parent().siblings().find("a").removeClass("YiZhanThemeColor");
        })
    });

})(jQuery)
