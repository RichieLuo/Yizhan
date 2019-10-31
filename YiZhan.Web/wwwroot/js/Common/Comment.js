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
            title: '���û�û��ʹ��QQ��ϵ',
            type: 'warning',
            confirmButtonText: 'ȷ��'
        })
    })

    //�ύ����
    $(document).on("click", ".commentBtn", function () {
        if ($commentInput.val() === "") {
            swal({
                title: '��û������������',
                Text: '�ύ֮ǰ����˵��ʲô��',
                type: 'warning',
                confirmButtonText: '�õ�'
            })
            $commentInput.focus();
            return;
        }
        submitComent();
    })

    $(".commentBtn").bind("keyup", function (e) {
        // ����FF��IE��Opera    
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
                        title: 'δ��¼',
                        text: data.message,
                        type: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: '������¼',
                        cancelButtonText: '�ݲ���¼',
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
                    toastr.success("���Գɹ���");
                    $(".commentTextarea").val("");
                    //ˢ��
                    getComments();
                    setTimeout(function () { getCommentCount(); }, 200);
                } else {
                    swal({
                        title: '�r(�s�n�t���q',
                        text: data.message,
                        confirmButtonText: 'ȷ��'
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
            title: 'ɾ��ȷ��',
            text: "��ȷ��Ҫɾ����������",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: 'ȡ��',
            confirmButtonText: 'ȷ��'
        }).then(function (isConfirm) {
            if (isConfirm) {
                $.post("../../CommoditiesManager/DeleteCommodityComment", { id: _thisComment }, function (data) {
                    if (data.result) {
                        toastr.success("ɾ���ɹ���");
                        getComments();
                        setTimeout(function () { getCommentCount(); }, 200);
                    } else {
                        swal(
                            '�r(�s�n�t���q',
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