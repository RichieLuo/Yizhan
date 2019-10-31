using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using YiZhan.Common.JsonModels;
using YiZhan.Entities.Attachments;

namespace YiZhan.ViewModels.Attachments
{
    /// <summary>
    /// 文件实体 视图模型
    /// </summary>
    public class BusinessFileVM : IEntityVM
    {
        [Key]
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        /// <summary>
        /// 附件的显示名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  附件说明 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 系统编码
        /// </summary>
        public string SortCode { get; set; }

        /// <summary>
        /// 附件上传时间 
        /// </summary>
        public DateTime AttachmentTimeUploaded { get; set; }

        /// <summary>
        /// 附件原始文件名称
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        /// 附件上传保存路径
        /// </summary>
        public string UploadPath { get; set; }

        /// <summary>
        /// 附件存放格式，如果使用二进制方式存在数据库中，则使用下一个属性进行处理
        /// </summary>
        public bool IsInDB { get; set; }

        /// <summary>
        /// 上传文件的后缀名
        /// </summary>
        public string UploadFileSuffix { get; set; }

        /// <summary>
        /// 附件存放的二进制内容
        /// </summary>
        public byte[] BinaryContent { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        ///  文件物理格式图标
        /// </summary>
        public string IconString { get; set; }

        /// <summary>
        /// 关联对象ID
        /// </summary>
        public Guid RelevanceObjectId { get; set; }

        /// <summary>
        /// 关联上传人ID
        /// </summary>
        public Guid UploaderId { get; set; }


        public BusinessFileVM() { }

        public BusinessFileVM(BusinessFile bo)
        {
            Id = bo.Id;
            Name = bo.Name;
            Description = bo.Description;
            SortCode = bo.SortCode;
            AttachmentTimeUploaded = bo.AttachmentTimeUploaded;
            OriginalFileName = bo.OriginalFileName;
            UploadPath = bo.UploadPath;
            IsInDB = bo.IsInDB;
            UploadFileSuffix = bo.UploadFileSuffix;
            BinaryContent = bo.BinaryContent;
            FileSize = bo.FileSize;
            IconString = bo.IconString;
            RelevanceObjectId = bo.RelevanceObjectId;
            UploaderId = bo.UploaderId;

        }
    }
}
