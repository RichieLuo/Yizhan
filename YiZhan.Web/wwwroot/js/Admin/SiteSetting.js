$(function () {
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
            var img = document.getElementById("logoPreview");
            //图片路径设置为读取的图片
            img.src = e.target.result;
        };
        reader.readAsDataURL(file);
    }

    $("#siteSettingSave").on("click", function () {
        var _formData = $("form[name=siteSettingForm]").serializeArray();
        $.ajax({
            url: "../../SiteManager/SetSiteSetting",
            type: "post",
            data: _formData,
            dataType: 'json',
        }).done(function (data) {
            if (data.result) {
                toastr.success("网站信息保存成功！");
            } else {
                alert(data.message);
            }
        })
    })

    //网站LOGO上传
    $("#siteSettingSubmitSave").on("click", function () {
        $("form[name=siteSettingForm]").ajaxSubmit(function (data) {
            if (data.result) {
                toastr.success("LOGO保存成功！");
            } else {
                toastr.error("LOGO保存出错！");
            }
        })
    })
   //$("#cnzzTestArea").html($("textarea[name=Statistics]").val());
})