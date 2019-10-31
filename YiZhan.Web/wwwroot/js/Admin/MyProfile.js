window.onload = function () {
    //修改用户信息
    $(document).on("click", "#ChangeProfileSaveBtn", function () {
        $("#ChangeProfileForm").ajaxSubmit(function (data) {
            if (data.result) {
                $("#adminNameArea").text($("input[name=Name]").val());
                toastr.success("信息修改成功！");
            } else {
                alert(data.message);
            }
        })
    })
    $("#submitUploadBtn").on("click", function () {
        $("form[name=avatarUploadForm]").ajaxSubmit(function (data) {
            console.info(data);
            if (data.isOK) {
                toastr.success(data.message);
            } else {
                toastr.error("更换失败！");
            }
        })
    })
}
var $changePasswordErrorMessage = $("#changePasswordErrorMessage");
var $password = $("input[name='Password']");
var $confirmPassword = $("input[name='ConfirmPassword']");
$($password, $confirmPassword).click(function () {
    $changePasswordErrorMessage.removeClass("yzError");
    $changePasswordErrorMessage.text("");
    $changePasswordErrorMessage.html("");
})
// 提交重置密码的数据
function resetUserPassword() {
    var changePasswordFormOptions = {
        dataType: 'json',
        type: "post",
        success: function (data) {
            if (data.result) {
                toastr.success(data.message);
                $changePasswordErrorMessage.removeClass("yzError").addClass("yzSuccess");
                $changePasswordErrorMessage.text(data.message);
            }
            else {
                $changePasswordErrorMessage.removeClass("yzSuccess").addClass("yzError");
                $changePasswordErrorMessage.text(data.message);
            }
        }
    };
    var _validPassRule = /^(?![a-zA-z]+$)(?!\d+$)(?![!@@#$%^&*]+$)(?![a-zA-z\d]+$)(?![a-zA-z!@@#$%^&*]+$)(?![\d!@@#$%^&*]+$)[a-zA-Z\d!@@#$%^&*]+$/;
    var _validPassMessage = "密码应包含数字特殊符号(如'@@')</br>大小写字母密码长度至少6位以上";
    if (!_validPassRule.test($password.val().trim())) {
        $changePasswordErrorMessage.removeClass("yzSuccess").addClass("yzError");
        $changePasswordErrorMessage.html(_validPassMessage);
    } else {
        $('#ChangePasswordForm').ajaxSubmit(changePasswordFormOptions);
    }
}
function imgPreview(fileDom) {
    //判断是否支持FileReader
    if (window.FileReader) {
        var reader = new FileReader();
    } else {
        alert("您的设备不支持图片预览功能，如需该功能请升级您的设备！");
    }
    //获取文件
    var file = fileDom.files[0];
    var imageType = /^image\//;
    //是否是图片
    if (!imageType.test(file.type)) {
        alert("请选择图片！");
        return;
    }
    //读取完成
    reader.onload = function (e) {
        //获取图片dom
        var img = document.getElementById("avatarPreview");
        //图片路径设置为读取的图片
        img.src = e.target.result;
    };
    reader.readAsDataURL(file);
}