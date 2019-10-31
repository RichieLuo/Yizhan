//头像上传带预览js脚本
(function () {
    var $errorMessageArea = $("#changeAvatarErrorMessage");
    function getObjectURL(file) {
        if (file == null) {
            return "../../images/Avatars/notChooseAvatar.jpg";
        }
        var url = null;
        if (window.createObjectURL != undefined) { // basic
            url = window.createObjectURL(file);
        } else if (window.URL != undefined) { // mozilla(firefox)
            url = window.URL.createObjectURL(file);
        } else if (window.webkitURL != undefined) { // webkit or chrome
            url = window.webkitURL.createObjectURL(file);
        }
        return url;
    }

    //图片上传预览，因为是动态加载的局部页面 需要进行事件的绑定
    $(document).on("change", "#avatarFile", function () {
        var eImg = $(".avatar");
        eImg.attr('src', getObjectURL($(this)[0].files[0])); // 或 this.files[0] this->input
    });

    //要提交的ajaxForm的配置  
    var myData = {}; //自定义Json数据
    var $url = "../../Account/SaveChangeAvatar";
    var ajaxFormOption = {
        type: "post", //提交方式 
        dataType: "json", //数据类型 
        //data: myData,//自定义数据参数，视情况添加
        url: $url, //请求url 
        beforeSubmit: function (formData, jqForm, options) {
            var $fileInput = $('#avatarFile');
            var files = $fileInput.get()[0].files;
            if (!files.length) {
                return false;
            }
            var file = files[0];
            var type = '|' + file.type.slice(file.type.lastIndexOf('/') + 1) + '|';
            if ('|jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF|bmp'.indexOf(type) === -1) {
                $errorMessageArea.addClass("yiError");
                $errorMessageArea.text("请选择图片类型的文件！");
                $(".avatar").attr("src", "../../images/Avatars/notChooseAvatar.jpg");
                return false;
            }
            if (file.size > 2097152) //2MB
            {
                $errorMessageArea.addClass("yiError");
                $errorMessageArea.text("请上传小于2M的图片！");
                return false;
            }
            return true;
        },
        success: function (data) { //提交成功的回调函数 
            if (data.isOK) {
                toastr.success(data.message);
                $errorMessageArea.removeClass("yzError");
                $errorMessageArea.addClass("yzSuccess");
                $errorMessageArea.text(data.message);
                var _imgHtml = '<img src="' + data.url + '" alt="修改头像" onclick="LoadBigAvatarPreviewModal()"/>';
                $(".user-avatar").html(_imgHtml);
            }
            else {
                $errorMessageArea.addClass("yiError");
                $errorMessageArea.text(data.message);
            }
        }
    };
    //对表单进行ajax提交，因为是动态加载的局部页面 需要进行事件的绑定
    $(document).on("click", ".submitUploadBtn", function () {
        $("#avatarUploadForm").ajaxForm(ajaxFormOption);
    })
    $(document).on("click", "input[name='file']", function () {
        $errorMessageArea.removeClass("yiError");
        $errorMessageArea.text("");
    })

})(jQuery)