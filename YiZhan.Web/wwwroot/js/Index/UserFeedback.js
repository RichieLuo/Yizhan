$(function () {
    $('.feedbackCategory').on('change', function () {
        var _selectVal = $(".feedbackCategory option:selected").val();
        console.info(_selectVal);
        if (_selectVal == "系统错误") {
            $(".pageLink").find("input").removeAttr("disabled");
        }      
        else {
            $(".pageLink").find("input").attr("disabled", "disabled");
            $(".pageLink").find("input").val("");
        }
    })
})