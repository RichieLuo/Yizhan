$(function () {

    //$.get("../../FilesManager/GetFiles", function (data) {
    //    console.log(data);
    //});

    //var $table = $('#filesDataTable');
    //var dataTable = null;
    //var load = function () {
    //    this.dataTable.ajax.reload();
    //};
    //var init = function () {
    //    this.dataTable = $table.DataTable({
    //        paging: true,
    //        serverSide: true,
    //        processing: true,
    //        //deferLoading: 0, //prevents table for ajax request on initialize
    //        listAction: {                
    //        ajaxFunction:function () {
    //            $.get("../../FilesManager/GetFiles", function (data, callback) {
    //                console.log(data);
    //                callback(data);
    //            });
    //        },
    //        inputFilter: function () {
    //          return {
    //        folderId: _folderId
    //            }
    //          }
    //        },
    //        columnDefs: [{
    //            targets: 0,
    //            data: "orderNumber"
    //        },
    //        {
    //            targets: 1,
    //            data: "name",
    //            render: function (data, type, row) {
    //                if (data !== null) {
    //                    return '<i class="' + data + '"></i>';
    //                } else {
    //                    return '';
    //                }
    //            }
    //        },
    //        {
    //            targets: 2,
    //            data: "iconString"
    //        },
    //        {
    //            targets: 3,
    //            data: "attachmentTimeUploaded"
    //        },
    //        {
    //            targets: 4,
    //            data: "name"

    //        },
    //        {
    //            targets: 5,
    //            data: "description"
    //        },
    //        {
    //            targets: 6,
    //            data: "FileSize"

    //        },
    //        {
    //            targets: 7,
    //            data: "UploaderID"
    //        },
    //        {
    //            //操作列
    //            targets: 8,
    //            data: null,
    //            orderable: false,
    //            autoWidth: false,
    //            defaultContent: '',
    //            rowAction: {
    //                cssClass: 'btn btn-xs btn-primary blue',
    //                text: '<i class="fa fa-cog"></i>操作<span class="caret"></span>',
    //                items: [{
    //                    text: "下载",
    //                    visible: function () {
    //                        return;
    //                    },
    //                    action: function (data) {
    //                        //TODO:方法+参数                                    
    //                    }
    //                },
    //                {
    //                    text: "编辑",
    //                    visible: function () {
    //                        return;
    //                    },
    //                    action: function (data) {
    //                        //_createOrEditFileModal.open({ id: data.record.id });
    //                    }
    //                },
    //                {
    //                    text: "删除",
    //                    visible: function () {
    //                        return;
    //                    },
    //                    action: function (data) {
    //                        return;
    //                    }
    //                }
    //                ]
    //            }
    //        },
    //        ]
    //        //, ajax: function (data, callback, settings) {
    //        //    //封装请求参数
    //        //    var param = {};
    //        //    param.limit = data.length;//页面显示记录条数，在页面显示每页显示多少项的时候
    //        //    param.start = data.start;//开始的记录序号
    //        //    param.page = (data.start / data.length) + 1;//当前页码
    //        //    //console.log(param);
    //        //    //ajax请求数据
    //        //    $.ajax({
    //        //        type: "GET",
    //        //        url: "../../FilesManager/GetFiles",
    //        //        cache: false, //禁用缓存
    //        //        //data: param, //传入组装的参数
    //        //        dataType: "json",
    //        //        success: function (result) {
    //        //            console.log(result);
    //        //            //setTimeout仅为测试延迟效果
    //        //            setTimeout(function () {
    //        //                //封装返回数据
    //        //                var returnData = {};
    //        //                returnData.draw = data.draw;//这里直接自行返回了draw计数器,应该由后台返回
    //        //                returnData.recordsTotal = result.total;//返回数据全部记录
    //        //                returnData.recordsFiltered = result.total;//后台不实现过滤功能，每次查询均视作全部结果
    //        //                returnData.data = result.data;//返回的数据列表
    //        //                //console.log(returnData);
    //        //                //调用DataTables提供的callback方法，代表数据已封装完成并传回DataTables进行渲染
    //        //                //此时的数据需确保正确无误，异常判断应在执行此回调前自行处理完毕
    //        //                callback(returnData);
    //        //            }, 200);
    //        //        }
    //        //    });
    //        //}
    //    });
    //}
    //init();
})