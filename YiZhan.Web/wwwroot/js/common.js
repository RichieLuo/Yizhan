$(".backTop").click(function () {
    $('body,html').animate({ scrollTop: 0 }, 500);
})

$(function () {
    //消息提醒插件初始化
    var init = function () {
        toastr.options.positionClass = 'toast-bottom-right';
    }
    init();
})


//注销登录
function LogOut() {
    if (confirm("Hi~亲，您是否要退出系统？")) {
        $.ajax({
            cache: false,
            type: 'POST',
            async: false,
            url: "../Account/LogOut",
            dataType: 'json',
            beforeSend: function () {
            }
        }).done(function (loginOutStatus) {
            if (loginOutStatus.result === true) {
                location.href = loginOutStatus.reUrl;
            } else {
                alert("请刷新浏览器，或清除缓存！");
            }
        }).fail(function () {
            alert("请刷新浏览器，或清除缓存！");
        }).always(function () {
        });
    }
}