window.onload = function () {

    var $commodityId = $("input[name=commodityId]").val();
    var $commentInput = $(".commentTextarea");

    $(".commentTextarea").focus(function () {
        $(this).addClass("commentFocus");
        $(this).next().find("button").addClass("commentFocus");
    })
    $(".commentTextarea").blur(function () {
        if (!$(this).val()) {
            $(this).removeClass("commentFocus");
            $(this).next().find("button").removeClass("commentFocus");
        }
    })
    $(".commentTextarea").click(function () {
        $(this).addClass("commentFocus");
        $(this).next().find("button").addClass("commentFocus");
    })
    $(".commentTextarea").mouseout(function () {
        if (!$(this).val()) {
            $(this).blur();
            $(this).removeClass("commentFocus");
            $(this).next().find("button").removeClass("commentFocus");
        }
    })

    $(".sendQQMessgae").on("click", function () {
        swal({
            title: '该用户没有使用QQ联系',
            type: 'warning',
            confirmButtonText: '确定'
        })
    })

    //提交评论
    $(document).on("click", ".commentBtn", function () {
        if ($commentInput.val() === "") {
            swal({
                title: '还没有输入内容呢',
                Text: '提交之前，请说点什么吧',
                type: 'warning',
                confirmButtonText: '好的'
            })
            $commentInput.focus();
            return;
        }
        submitComent();
    })

    $(".commentBtn").bind("keyup", function (e) {
        // 兼容FF和IE和Opera    
        var theEvent = e || window.event;
        var code = theEvent.keyCode || theEvent.which || theEvent.charCode;
        if (code === 13) {
            submitComent();
        }
    })

    function submitComent() {
        var options = {
            dataType: 'json',
            success: function (data) {
                if (!data.result && data.noLogin) {
                    swal({
                        title: '未登录',
                        text: data.message,
                        type: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: '立即登录',
                        cancelButtonText: '暂不登录',
                        confirmButtonClass: 'btn rightSaveBtn',
                        cancelButtonClass: 'btn btn-danger',
                        buttonsStyling: false
                    }).then(function (isConfirm) {
                        if (isConfirm === true) {
                            window.open("../../Account/Index");
                        }
                    })
                }
                else if (data.result) {
                    toastr.success("留言成功！");
                    $(".commentTextarea").val("");
                    //刷新
                    getComments();
                    setTimeout(function () { getCommentCount(); }, 200);
                } else {
                    swal({
                        title: '╮(╯﹏╰）╭',
                        text: data.message,
                        confirmButtonText: '确定'
                    });
                }
            }
        };
        $('form[name=commentForm]').ajaxSubmit(options);
    }

    function getComments() {
        $.get("../../CommoditiesManager/GetCommodityCommentsView", { id: $commodityId }, function (html) {
            $("#commentListItemsFullArea").html(html);
        })
    }

    $(document).on("click", ".commentDelete", function () {
        var _thisComment = $(this).val();
        swal({
            title: '删除确认',
            text: "您确定要删除此留言吗？",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: '取消',
            confirmButtonText: '确定'
        }).then(function (isConfirm) {
            if (isConfirm) {
                $.post("../../CommoditiesManager/DeleteCommodityComment", { id: _thisComment }, function (data) {
                    if (data.result) {
                        toastr.success("删除成功！");
                        getComments();
                        setTimeout(function () { getCommentCount(); }, 200);
                    } else {
                        swal(
                            '╮(╯﹏╰）╭',
                            data.message
                        );
                    }
                })
            }
        })
    })

    $(document).on("click", "#sendLeavingAMessage", function () {
        var $userCommentLi = $("#userCommentLi");
        var $userComment = $("#userComment");
        $userCommentLi.addClass("active").siblings().removeClass("active");
        $userComment.addClass("active in").siblings().removeClass("active in");
    })

    var getCommentCount = function () {
        $.get("../../CommoditiesManager/GetCommentCount", { id: $commodityId }, function (data) {
            $(".commentCount").text(data);
        })
    }
    getCommentCount();
}