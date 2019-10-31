// 打开删除会话框
function openDeleteModal(id, tipString) {
    $('#deleteConfirmModal').modal({
        show: true,
        backdrop: 'static'
    });
    document.getElementById("deleteModalMessage").innerHTML = "<i class='fa fa-exclamation fa-1x'></i> " + tipString;
    $('#businessObjectId').val(id);
}
// 执行删除
function gotoDelete() {
    var _id = $('#businessObjectId').val();
    $.ajax({
        cache: false,
        type: 'post',
        async: false,
        url: '../../CommoditiesManager/DeleteCommodity/' + _id,
        beforeSend: function () {
        }
    }).done(function (data) {
        if (data.result) {
            $('#deleteConfirmModal').modal('hide')
            loadPage($("input[name=PageIndex]").val());
        } else {
            document.getElementById("deleteModalErrMessage").innerText = data.message;
        }
    }).fail(function () {
        alert("连接后台失败！");
    }).always(function () {
    });
}
function loadPage(index) {
    switch (index) {
        case 7: LoadMySecondhandView(); break;
        case 8: LoadAwaitExamineCommoditiesView(); break;
        case 9: LoadIsNotExamineCommoditiesView(); break;
    }
    LoadDefaultViewCss(index);
    toastr.success("删除成功");
}