
$(function () {
    var $filesUploadForm = $("#filesUploadInput");
    //初始化FileInput插件
    $filesUploadForm.fileinput({
        language: 'zh', //设置语言
        theme: 'explorer-fa',
        uploadUrl: "../../FilesManager/FilesForAjaxUpload", //上传的地址
        //allowedFileExtensions: ['jpg', 'gif', 'png'],// 接收的文件后缀
        uploadAsync: true, //默认异步上传
        overwriteInitial: false,
        showUpload: true, //是否显示上传按钮
        showCaption: true, //是否显示标题
        showBrowse: false,
        showPreview: true, //是否显示预览
        browseOnZoneClick: true,     
        browseClass: "btn btn-primary", //按钮样式     			           
        dropZoneEnabled: true, //是否显示拖拽区域
        //minImageWidth: 50, //图片的最小宽度
        //minImageHeight: 50,//图片的最小高度
        //maxImageWidth: 1000,//图片的最大宽度
        //maxImageHeight: 1000,//图片的最大高度
        maxFileSize: 102400, //单位为kb，如果为0表示不限制文件大小
        //minFileCount: 0,
        maxFileCount: 10, //表示允许同时上传的最大文件个数
        enctype: 'multipart/form-data',
        validateInitialCount: true,
        previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",
        msgFilesTooMany: "选择上传的文件数量({n}) 超过允许的最大数值{m}！"
    });

    //隐藏上传按钮
    $(".fileinput-upload-button").css({ "display": "none" });
    

    ////异步上传返回错误结果处理
    //$filesUploadForm.on('fileerror', function (event, data, msg) {
    //    console.log(data.id);
    //    console.log(data.index);
    //    console.log(data.file);
    //    console.log(data.reader);
    //    console.log(data.files);
    //    // get message
    //    alert(msg);
    //});
    ////异步上传返回结果处理
    //$filesUploadForm.on("fileuploaded", function (data) {
    //    console.log(data.id);
    //    console.log(data.index);
    //    console.log(data.file);
    //    console.log(data.reader);
    //    console.log(data.files);
    //    var obj = data.response;
    //    alert(JSON.stringify(data.success));
    //});


})
