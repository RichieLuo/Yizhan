$(function () {
    function switchInit() {
        //初始化组件
        $('[name="CanRegister"]').bootstrapSwitch({
            onText: "开启",
            offText: "关闭",
            onColor: "success",
            offColor: "info",
            size: "small",
            onSwitchChange: function (event, state) {
                if (state == true) {
                    $(this).val(true);
                } else {
                    $(this).val(false);
                }
            }
        })
        $('[name="CanLogin"]').bootstrapSwitch({
            onText: "开启",
            offText: "关闭",
            onColor: "success",
            offColor: "info",
            size: "small",
            onSwitchChange: function (event, state) {
                if (state == true) {
                    $(this).val(true);
                } else {
                    $(this).val(false);
                }
            }
        })
        //$('[name="IsOpenAd"]').bootstrapSwitch({
        //    onText: "开启",
        //    offText: "关闭",
        //    onColor: "success",
        //    offColor: "info",
        //    size: "small",
        //    onSwitchChange: function (event, state) {
        //        if (state == true) {
        //            $(this).val(true);
        //        } else {
        //            $(this).val(false);
        //        }
        //    }
        //})
        $('[name="IsOpenCode"]').bootstrapSwitch({
            onText: "开启",
            offText: "关闭",
            onColor: "success",
            offColor: "info",
            size: "small",
            onSwitchChange: function (event, state) {
                if (state == true) {
                    $(this).val(true);
                } else {
                    $(this).val(false);
                }
            }
        })
        $('[name="IsOpenCS"]').bootstrapSwitch({
            onText: "开启",
            offText: "关闭",
            onColor: "success",
            offColor: "info",
            size: "small",
            onSwitchChange: function (event, state) {
                if (state == true) {
                    $(this).val(true);
                } else {
                    $(this).val(false);
                }
            }
        })
    }
    switchInit();

    $(".bootstrap-switch-container span").on("click", function () {
        var _formData = $("#SetSiteConfigurationForm").serializeArray();
        $.ajax({
            url: "../../SiteManager/SetSiteConfiguration",
            data: _formData,
            dataType: "json",
            type: "post"
        }).done(function (data) {
            if (data.result) {
                toastr.success("设置成功！");
            }
            else {
                var $checkBoxIsOpenCS = $('[name="IsOpenCS"]');
                setTimeout(function () {
                    $checkBoxIsOpenCS.bootstrapSwitch('state', false);
                    $checkBoxIsOpenCS.val(false);
                }, 100);
                toastr.error(data.message);
            }
        })
    });

    ////保存修改
    //$(".siteConfigurationSave").on("click", function () {
    //    var _formData = $("#SetSiteConfigurationForm").serializeArray();
    //    $.ajax({
    //        url: "../../SiteManager/SetSiteConfiguration",
    //        data: _formData,
    //        dataType: "json",
    //        type: "post"
    //    }).done(function (data) {
    //        if (data.result) {
    //            toastr.success("设置成功！");
    //        }
    //        else {
    //            alert(data.message);
    //        }
    //    })
    //})
})

//加载组件数据
function switchChecked() {
    $.ajax({
        url: '../../AdminCenter/GetSiteConfiguration',
        type: 'get',
        async: false,
        dataType: 'json'
    }).done(function (data) {
        var $checkBoxCanRegister = $('[name="CanRegister"]');
        var $checkBoxCanLogin = $('[name="CanLogin"]');
        var $checkBoxIsOpenCode = $('[name="IsOpenCode"]');
        var $checkBoxIsOpenCS = $('[name="IsOpenCS"]');
        //var $checkBoxIsOpenAd = $('[name="IsOpenAd"]');

        //注册开关状态
        if (data.canRegister) {
            setTimeout(function () {
                $checkBoxCanRegister.bootstrapSwitch('state', true);
                $checkBoxCanRegister.val(true);
            }, 200);
        } else {
            setTimeout(function () {
                $checkBoxCanRegister.bootstrapSwitch('state', false);
                $checkBoxCanRegister.val(false);
            }, 200);
        }
        //登录开关状态
        if (data.canLogin) {
            setTimeout(function () {
                $checkBoxCanLogin.bootstrapSwitch('state', true);
                $checkBoxCanLogin.val(true);
            }, 200);

        } else {
            setTimeout(function () {
                $checkBoxCanLogin.bootstrapSwitch('state', false);
                $checkBoxCanLogin.val(false);
            }, 200);
        }
        //登录验证码开关状态
        if (data.isOpenCode) {
            setTimeout(function () {
                $checkBoxIsOpenCode.bootstrapSwitch('state', true);
                $checkBoxIsOpenCode.val(true);
            }, 200);

        } else {
            setTimeout(function () {
                $checkBoxIsOpenCode.bootstrapSwitch('state', false);
                $checkBoxIsOpenCode.val(false);
            }, 200);
        }
        //站内客服开关状态
        if (data.isOpenCS) {
            setTimeout(function () {
                $checkBoxIsOpenCS.bootstrapSwitch('state', true);
                $checkBoxIsOpenCS.val(true);
            }, 200);

        } else {
            setTimeout(function () {
                $checkBoxIsOpenCS.bootstrapSwitch('state', false);
                $checkBoxIsOpenCS.val(false);
            }, 200);
        }

        //广告开启开关状态
        //if (data.isOpenAd) {
        //    setTimeout(function () {
        //        $checkBoxIsOpenAd.bootstrapSwitch('state', true);
        //        $checkBoxIsOpenAd.val(true);
        //    }, 200);

        //} else {
        //    setTimeout(function () {
        //        $checkBoxIsOpenAd.bootstrapSwitch('state', false);
        //        $checkBoxIsOpenAd.val(false);
        //    }, 200);
        //}
    })
}
