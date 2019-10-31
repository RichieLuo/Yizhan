window.onload = function () {
    $("#UserName").focus();
}
var $adLoginErrorMessage = $("#adLoginErrorMessage");
var $userName = $("#UserName");
var $password = $("#Password");
var $loginAvatar = $("#adLogin .adAvatar img");
//用于清空错误提示内容
$(".clearErrorMessage").change(function () {
    $adLoginErrorMessage.removeClass("adLoginSuccess adLoginError");
    $adLoginErrorMessage.html("");
})

// 处理登录系统操作
function GoToLogin() {
    if ($userName.val() === "" || $password.val() === "") {
        $adLoginErrorMessage.addClass("adLoginError").removeClass("adLoginSuccess");
        $adLoginErrorMessage.html("用户名或者密码不能为空。");
    } else {
        $adLoginErrorMessage.addClass("adLoginSuccess").removeClass("adLoginError");
        $adLoginErrorMessage.html("正在登录系统，请稍候......");
        // 创建登录数据模型
        var loginDataModel = "{" +
            "UserName:'" + $userName.val() + "'," +
            "Password:'" + $password.val() + "'" +
            "}";
        // 转换为 Json 模型
        var loginJsonModel = { 'jsonLoginInformation': loginDataModel };
        // 执行提交
        $.ajax({
            cache: false,
            type: 'POST',
            async: false,
            url: "../Admin/Login",
            data: loginJsonModel,
            dataType: 'json',
            beforeSend: function () {
            }
        }).done(function (loginStatus) {
            if (!loginStatus.result && !loginStatus.isAdminRole && !loginStatus.canLogin) {
                $adLoginErrorMessage.addClass("adLoginError").removeClass("adLoginSuccess");
                $adLoginErrorMessage.html(loginStatus.message);
            }
            else if (loginStatus.result === true) {
                if (loginStatus.isAdminRole === true) {
                    location.href = loginStatus.message;
                    $adLoginErrorMessage.addClass("adLoginSuccess").removeClass("adLoginError");
                    $adLoginErrorMessage.html("正在登录系统，请稍候......");
                } else {
                    $adLoginErrorMessage.addClass("adLoginError").removeClass("adLoginSuccess");
                    $adLoginErrorMessage.html(loginStatus.message);
                }
            } else {
                $adLoginErrorMessage.addClass("adLoginError").removeClass("adLoginSuccess");
                $adLoginErrorMessage.html(loginStatus.message);
            }
        }).fail(function () {
            $adLoginErrorMessage.addClass("adLoginError").removeClass("adLoginSuccess");
            $adLoginErrorMessage.html("系统异常");
        }).always(function () {
        });
    }
}

$("#Password").keyup(function (event) {
    if (event.keyCode == 13) {
        GoToLogin();
    }
});

//处理登录之前的操作，获取头像
$userName.change(function () {
    if ($userName.val()) {
        $.get("../../Account/GetUserAvatarForLogin", { userName: $userName.val() }, function (data) {
            if (data.isOk) {
                $loginAvatar.attr("src", data.avatarPath);
            } else {
                $loginAvatar.attr("src", "/images/Avatars/defaultAvatar.gif");
            }
        })
    } else {
        $loginAvatar.attr("src", "/images/Avatars/defaultAvatar.gif");
    }
})