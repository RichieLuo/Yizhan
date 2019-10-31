//用户中心:根据左侧导航菜单加载相应内容

//要要进行填充的区域
var $userCenterMain = $("#userCenterMain");

$(function () {
    //默认加载我的个人中心首页界面
    LoadUserCenterIndexView();
    LoadDefaultViewCss(1);
})

//根据右侧默认加载界面给左侧nav加载选中状态
function LoadDefaultViewCss(num) {
    var $liNav = $(".user-nav ul li:nth-child(" + num + ")");
    $liNav.addClass("liScale").siblings().removeClass("liScale");
    $liNav.find("a").addClass("YiZhanThemeColor").parent().siblings().find("a").removeClass("YiZhanThemeColor");
}


//加载个人中心首页的界面
function LoadUserCenterIndexView() {
    $.get("../../Account/UserCenterIndex", null, function (html) {
        $userCenterMain.html(html);
    })
}

//加载个人信息的界面
function LoadMyProfileView() {
    $.get("../../Account/MyProfile", null, function (html) {
        $userCenterMain.html(html);
    })
}

//加载消息中心界面
function LoadMyNotificationsView() {
    $.get("../../Account/MyNotifications", null, function (html) {
        $userCenterMain.html(html);
    })
}

//加载我发布的闲置界面
function LoadMySecondhandView() {
    $.get("../../CommoditiesManager/MySecondhand", null, function (html) {
        $userCenterMain.html(html);
    })
}

//加载添加或修改闲置商品界面
function LoadAddOrEditCommodityView(id, index) {
    $.get("../../CommoditiesManager/AddOrEditCommodity", { id: id, pageIndex: index }, function (html) {
        $userCenterMain.html(html);
    })
}

//加载等待审核的闲置商品界面
function LoadAwaitExamineCommoditiesView() {
    $.get("../../CommoditiesManager/GetAwaitExamineCommodities", null, function (html) {
        $userCenterMain.html(html);
    })
}

//加载审核未通过的闲置商品界面
function LoadIsNotExamineCommoditiesView() {
    $.get("../../CommoditiesManager/GetIsNotExamineCommodities", null, function (html) {
        $userCenterMain.html(html);
    })
}

//加载修改审核未通过的闲置商品界面
function LoadEditIsNotExaminedCommodityView(id) {
    $.get("../../CommoditiesManager/EditIsNotExaminedCommodity", { id: id }, function (html) {
        $userCenterMain.html(html);
    })
}

//加载修改个人信息模态框
function LoadChangeMyProfileModal() {
    $('#changeMyProfileModal').modal({
        show: true,
        backdrop: 'static'
    });
}

//用户头像大图预览(现改为修改头像界面)
function LoadBigAvatarPreviewModal() {
    LoadChangeAvatarView();
    LoadDefaultViewCss(4);
    //$.get("../../Account/UserBigAvatarPreviewModal", null, function (html) {
    //    $('#userBigAvatarPreviewModal').modal({
    //        show: true,
    //        backdrop: 'static'
    //    });
    //})
}

//加载修改密码的界面
function LoadChangePasswordView() {
    $.get("../../Account/ChangePassword", null, function (html) {
        $userCenterMain.html(html);
    })
}

//加载修改头像的界面
function LoadChangeAvatarView() {
    $.get("../../Account/ChangeAvatar", null, function (html) {
        $userCenterMain.html(html);
    })
}

//加载订单管理的界面
function LoadMyOrdersView() {
    $.get("../../CommoditiesManager/GetMyOrders", null, function (html) {
        $userCenterMain.html(html);
    })
}

//加载我的足迹的界面
function LoadMyVistorLogsView() {
    $.get("../../CommoditiesManager/GetMyVistorLogs", null, function (html) {
        $userCenterMain.html(html);
    })
}

//加载我的反馈页面
function LoadMyMyFeedbacksView() {
    $.get("../../SiteManager/GetMyFeedbacks", null, function (html) {
        $userCenterMain.html(html);
    })
}