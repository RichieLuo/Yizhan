//这里是存放后台管理的共用的脚本

//超级管理员注销
function LogOut() {
    if (confirm("Hi~亲，您是否要退出系统？")) {
        $.ajax({
            cache: false,
            type: 'POST',
            async: false,
            url: "../AdminCenter/LogOut",
            dataType: 'json',
            beforeSend: function () {
            }
        }).done(function (logOutStatus) {          
            if (logOutStatus.status) {
                window.location.href = logOutStatus.url;
            } else {
                alert(logOutStatus.message);
            }
        }).fail(function () {
            alert("请刷新浏览器，或清除缓存！");
        }).always(function () {
        });
    }
}

