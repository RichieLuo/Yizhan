using YiZhan.Common.JsonModels;
using YiZhan.Common.ViewModelComponents;
using YiZhan.Entities.Attachments;
using YiZhan.Entities.Ultilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YiZhan.ViewModels.Attachments
{
    /// <summary>
    /// 图片的视图模型
    /// </summary>
    public class BusinessImageVM : IEntityVM
    {
        [Key]
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        /// <summary>
        /// 图片显示名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图片说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 内部业务编码
        /// </summary>

        public string SortCode { get; set; }

        /// <summary>
        /// 图片显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///  图片原始文件
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        /// 图片上传时间
        /// </summary>
        public DateTime UploadedTime { get; set; }

        /// <summary>
        /// 图片上传保存路径
        /// </summary>

        public string UploadPath { get; set; }

        /// <summary>
        /// 上传文件的后缀名
        /// </summary>
        public string UploadFileSuffix { get; set; }

        /// <summary>
        /// 图片大小
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 文件物理格式图标
        /// </summary>

        public string IconString { get; set; }

        /// <summary>
        /// 图片类型
        /// </summary>
        public ImageType Type { get; set; }

        /// <summary>
        /// 封面（用于商品）
        /// </summary>
        public bool IsCover { get; set; }

        /// <summary>
        /// 使用该图片的业务对象的 id
        /// </summary>
        public Guid RelevanceObjectId { get; set; }
        
        /// <summary>
        /// 关联上传人ID
        /// </summary>
        public Guid UploaderId { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public BusinessImageVM()
        { }
        public BusinessImageVM(BusinessImage bo)
        {
            Id = bo.Id;
            Name = bo.Name;
            Description = bo.Description;
            SortCode = bo.SortCode;
            DisplayName = bo.DisplayName;
            OriginalFileName = bo.OriginalFileName;
            UploadedTime = bo.UploadedTime;
            UploadPath = bo.UploadPath;
            UploadFileSuffix = bo.UploadFileSuffix;
            FileSize = bo.FileSize;
            IconString = bo.IconString;
            X = bo.X;
            Y = bo.Y;
            Width = bo.Width;
            Type = bo.Type;
            IsCover = bo.IsCover;
            RelevanceObjectId = bo.RelevanceObjectId;
            UploaderId = bo.UploaderId;
        }

        //public void MapToBo(BusinessImage bo)
        //{
        //     bo.ID =ID;
        //     bo.Name =Name;
        //     bo.Description =Description;
        //    bo.SortCode = SortCode;
        //    bo.DisplayName = DisplayName;
        //    bo.OriginalFileName = OriginalFileName;

        //    bo.UploadedTime = DateTime.Now;
        //    bo.UploadPath = bo.UploadPath;
        //    UploadFileSuffix = UploadFileSuffix;
        //    bo.FileSize = FileSize;
        //    bo.IconString = IconString;
        //    bo.IsForTitle = IsForTitle;
        //    bo.X = X;
        //    Y = bo.Y;
        //    bo.Width = Width;
        //    bo.RelevanceObjectID = RelevanceObjectID;
        //    bo.UploaderID = UploaderID;
        //}
    }
}
