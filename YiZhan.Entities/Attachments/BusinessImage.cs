using YiZhan.Entities.Ultilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiZhan.Common.JsonModels;

namespace YiZhan.Entities.Attachments
{
    public class BusinessImage : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        ///  图片显示名称
        /// </summary>
        [StringLength(1000)]
        public string Name { get; set; }

        /// <summary>
        /// 图片显示名称
        /// </summary>
        [StringLength(100)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 图片说明
        /// </summary>
        [StringLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// 内部业务编码
        /// </summary>
        [StringLength(150)]
        public string SortCode { get; set; }

        /// <summary>
        /// 图片原始文件
        /// </summary>
        [StringLength(256)]
        public string OriginalFileName { get; set; }

        /// <summary>
        /// 图片上传时间
        /// </summary>
        public DateTime UploadedTime { get; set; } 

        /// <summary>
        /// 相对路径 图片上传保存路径
        /// </summary>
        [StringLength(256)]
        public string UploadPath { get; set; }

        /// <summary>
        /// 图片物理路径 用于删除
        /// </summary>
        public string PhysicalPath { get; set; }

        /// <summary>
        /// 上传文件的后缀名
        /// </summary>
        [StringLength(256)]
        public string UploadFileSuffix { get; set; } 

        /// <summary>
        /// 图片大小
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 文件物理格式图标
        /// </summary>
        [StringLength(120)]
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

        public BusinessImage()
        {
            this.Id = Guid.NewGuid();
            this.SortCode = BusinessEntityComponentsFactory.SortCodeByDefaultDateTime<BusinessImage>();
            this.UploadedTime = DateTime.Now;
        }
    }
}
