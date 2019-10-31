
//要加载的区域
var $adminCenterContentArea = $("#AdminCenterIndexConctent");

//加载管理中心的页面
$(function () {


})

function loadADCenterNotificationsView() {
    $.get("../../AdminCenter/GetNotifications", null, function (html) {
        $adminCenterContentArea.html(html);
        $("#deleteAll").css("top", "15px");
        $("#sendNewNotification").css("top", "15px");
    })
}

function loadADCenterUserFeedback() {
    $.get("../../SiteManager/UserFeedback", null, function (html) {
        $adminCenterContentArea.html(html);
    })
}