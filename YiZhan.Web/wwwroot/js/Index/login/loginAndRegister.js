
//获取登录输入框 以及错误提示区域
var userNameInput = $("#lUserName");
var passwordInput = $("#lPassword");
var loginErrMessage = $("#loginModalErrMessage");
var registerErrMessage = $("#registerModalErrMessage");
var $codeValidValue = $("#codeValidValue");
var _rUserName = $("#rUserName");
var _rPassword = $("#rPassword");
var _rConfirmPassword = $("#rConfirmPassword");
var _email = $("#rEmail");
var _mobileNumber = $("#rMobileNumber");
var _checkCheckVal = $("#iAgree");
var _loginAvatar = $("#login .login-head img");

//用于清空错误提示内容
$(".clearErrorMessage").click(function () {
    loginErrMessage.css("color", "#000");
    loginErrMessage.text("");
    registerErrMessage.css("color", "#000");
    registerErrMessage.text("");
})


//设置默认焦点
userNameInput.focus();
_rUserName.focus();
//监听回车事件 用于登录操作
passwordInput.keyup(function (event) {
    if (event.keyCode == 13) {
        if ($codeValidValue.val() != undefined) {
            Login();
        } else {
            GoToLogin();
        }
    }
});
//监听回车事件 用于注册操作
_rConfirmPassword.keyup(function (event) {
    if (event.keyCode == 13) {
        GoToRegister();
    }
});


//点击登录进行验证码校验
function Login() {
    loginVerify();
}

var code; //在全局定义验证码
var codeLength = 4; //验证码的长度  
window.onload = function () {
    createCode();
    $("#codeValidValue").attr("maxlength", codeLength);
}

//产生验证码
function createCode() {
    code = "";
    var checkCode = document.getElementById("code");
    if (checkCode != null) {
        var random = new Array(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R',
            'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'); //随机数  
        for (var i = 0; i < codeLength; i++) { //循环操作  
            var index = Math.floor(Math.random() * 36); //取得随机数的索引（0~35）  
            code += random[index]; //根据索引取得随机数加到code上  
        }
        checkCode.value = code; //把code值赋给验证码 
    }
}

//校验验证码 和 登录信息
function loginVerify() {
    if ($codeValidValue.val() != undefined) {
        var inputCode = document.getElementById("codeValidValue").value.toUpperCase(); //取得输入的验证码并转化为大写        
        if (inputCode.length <= 0) { //若输入的验证码长度为0
            loginErrMessage.css("color", "red");
            loginErrMessage.text("请输入验证码！");//则弹出请输入验证码     
            $codeValidValue.focus();
        } else if (inputCode != code) { //若输入的验证码与产生的验证码不一致时		
            loginErrMessage.css("color", "red");
            loginErrMessage.text("验证码输入错误，请重新输入！");//验证码输入错误     
            createCode(); //刷新验证码  
            document.getElementById("codeValidValue").value = ""; //清空文本框  
            $codeValidValue.focus();
        } else {
            GoToLogin(); //验证成功
        }
    }
}


// 处理登录操作
function GoToLogin() {

    var userName = userNameInput.val();
    var password = passwordInput.val();
    if (userName == "" || password == "") {
        loginErrMessage.css("color", "red");
        loginErrMessage.text("用户名或者密码不能为空。");
        return;

    } else {
        loginErrMessage.css("color", "#000");
        loginErrMessage.text("正在登录系统，请稍候......");
        // 创建登录数据模型
        var loginDataModel = "{" +
            "UserName:'" + userName + "'," +
            "Password:'" + password + "'" +
            "}";
        // 转换为 Json 模型
        var loginJsonModel = { 'jsonloginInformation': loginDataModel };
        // 执行提交
        $.ajax({
            cache: false,
            type: 'POST',
            async: false,
            url: "../Account/Login",
            data: loginJsonModel,
            dataType: 'json',
            beforeSend: function () {
            }
        }).done(function (loginStatus) {
            if (loginStatus.result == true) {
                loginErrMessage.css("color", "green");
                loginErrMessage.text(loginStatus.message);
                location.href = loginStatus.reUrl;
            } else {
                loginErrMessage.css("color", "red");
                loginErrMessage.text(loginStatus.message);
            }
        }).fail(function () {
            loginErrMessage.css("color", "red");
            loginErrMessage.text("连接后台失败。");
            alert("连接后台失败！");
        }).always(function () {
        });
    }
}

//处理登录之前的操作，获取头像
userNameInput.change(function () {
    if (userNameInput.val()) {
        $.get("../../Account/GetUserAvatarForLogin", { userName: userNameInput.val() }, function (data) {
            if (data.isOk) {
                _loginAvatar.attr("src", data.avatarPath);
            } else {
                _loginAvatar.attr("src", "/images/Avatars/defaultAvatar.jpg");
            }
        })
    } else {
        _loginAvatar.attr("src", "/images/Avatars/defaultAvatar.jpg");
    }
})



//处理注册操作
function GoToRegister() {

    //数据校验开始
    if (!_checkCheckVal.is(":checked")) {
        registerErrMessage.css("color", "red");
        registerErrMessage.text("请阅读并同意本站使用条款后继续！");
        return;
    }
    _checkCheckVal.click(function () {
        registerErrMessage.css("color", "#000");
        registerErrMessage.text("");
    })

    if (_rUserName.val() == "" || _rPassword.val() == "" || _email.val() == "") {
        registerErrMessage.css("color", "red");
        registerErrMessage.text("注册信息中存在空的值，请检查！");
        return;
    } else {
        registerErrMessage.css("color", "#000");
        registerErrMessage.text("");
    }

    var _validEmailRule = /[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}/;
    var _validEmailMessage = "请输入正确的电子邮件格式";
    if (!IsValid(_email, _validEmailRule)) {
        registerErrMessage.css("color", "red");
        registerErrMessage.text(_validEmailMessage);
        _email.focus();
        return;
    }

    //var _validMobileRule = /^1[3|4|5|7|8][0-9]\d{4,8}$/;
    //var _validMobileMessage = "请输入正确的手机号码";
    //if (!IsValid(_mobileNumber, _validMobileRule)) {
    //    registerErrMessage.css("color", "red");
    //    registerErrMessage.text(_validMobileMessage);
    //    _mobileNumber.focus();
    //    return;
    //}

    var _validPassRule = /^(?![a-zA-z]+$)(?!\d+$)(?![!@#$%^&*]+$)(?![a-zA-z\d]+$)(?![a-zA-z!@#$%^&*]+$)(?![\d!@#$%^&*]+$)[a-zA-Z\d!@#$%^&*]+$/;
    var _validPassMessage = "密码应包含数字、特殊符号(如'@')</br>大小写字母密码长度至少6位以上";
    if (!IsValid(_rPassword, _validPassRule)) {
        registerErrMessage.css("color", "red");
        registerErrMessage.html(_validPassMessage);
        _rPassword.focus();
        return;
    }

    if (_rConfirmPassword.val() != _rPassword.val()) {
        registerErrMessage.css("color", "red");
        registerErrMessage.text("两次密码输入不正确，请确认！");
        return;
    }

    //数据校验结束

    _rPassword.onclick = function () {
        registerErrMessage.text("");
    }
    _rConfirmPassword.onclick = function () {
        registerErrMessage.text("");
    }

    $("#RegisterForm").ajaxSubmit(function (data) {
        if (data.result === true) {
            registerErrMessage.css("color", "green");
            registerErrMessage.text(data.message);
            userNameInput.val(data.userName);
            passwordInput.val(data.password);
            BackLogin();
            setTimeout(function () {
                GoToLogin();
            }, 2000);
        } else {
            registerErrMessage.css("color", "red");
            registerErrMessage.text(data.message);
        }
    });
}

//表单数据校验
function IsValid(validValue, validRule) {
    var regular = validRule;
    if (regular.test(validValue.val())) {
        return true;
    }
    else {
        return false;
    }
}


//注册和登录界面的切换效果 开始
var _login = $("#login");
var _register = $("#register");
function TabRegister() {
    //、旋转式
    //	_login.css({
    //		"transform":"rotateY(180deg)",
    //		"opacity":"0",
    //		"transition":"all 1s"
    //	});
    //	setTimeout(function(){
    //		_login.fadeOut();
    //		_register.css({
    //			"opacity":"1",
    //			"transform":"rotateY(0deg)"
    //		});
    //		_register.fadeIn();
    //	},500);	

    //、缓缓载入式
    _register.css({
        "right": "50px",
        "opacity": "1",
        "z-index": "999",
        "transition": "all 1s"
    });
    setTimeout(function () {
        _login.css({
            "right": "0px",
            "opacity": "0",
            "z-index": "-1"
        });
    }, 100);
    $("#rUserName").focus();
};
function BackLogin() {
    //、旋转式
    //	_register.css({
    //		"transform":"rotateY(180deg)",
    //		"opacity":"0",
    //		"transition":"all 1s"
    //	});
    //	setTimeout(function(){
    //		_register.fadeOut();
    //		_login.css({
    //			"opacity":"1",
    //			"transform":"rotateY(0deg)"
    //		});
    //		_login.fadeIn();
    //	},500);	

    //、缓缓载入式		
    _login.css({
        "right": "50px",
        "opacity": "1",
        "z-index": "999",
        "transition": "all 1s"
    });
    setTimeout(function () {
        _register.css({
            "right": "0px",
            "opacity": "0",
            "z-index": "-1"
        });
    }, 100);
    $("#lUserName").focus();
};
//注册和登录界面的切换效果 结束

//抓取从注册链接传过来的值 如果为true 则是显示注册界面
$(function () {
    var $state = GetQueryString("state");
    if ($state != null && $state.toString().length > 1 && $state == "register") {
        TabRegister();
        $("#rUserName").focus();
    } else {
        $("#lUserName").focus();
        return;
    }
})
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return decodeURI(r[2]); return null;
}