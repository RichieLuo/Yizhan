using YiZhan.Common.JsonModels;
using YiZhan.Common.ViewModelComponents;
using YiZhan.Entities.ApplicationOrganization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YiZhan.ViewModels.ApplicationOrganization
{
    public class ApplicationUserForEditVM : IEntityVM
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } // 列表时候需要的序号
        public bool IsNew { get; set; }
        public ListPageParameter ListPageParameter { get; set; }

        [Required(ErrorMessage = "显示名不能为空。")]
        [Display(Name = "名称")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public string Name { get; set; }

        [Display(Name = "简要说明")]
        [StringLength(1000, ErrorMessage = "你输入的数据超出限制1000个字符的长度。")]
        public string Description { get; set; }

        [Display(Name = "用户内部编码")]
        [StringLength(150, ErrorMessage = "你输入的数据超出限制150个字符的长度。")]
        public string SortCode { get; set; }

        [Display(Name = "归属用户组")]
        public List<string> RoleItemIdCollection { get; set; }
        [Display(Name = "归属用户组")]
        public string RoleItemNameString { get; set; }
        [PlainFacadeItemSpecification("RoleItemIdCollection")]
        public List<PlainFacadeItem> RoleItemColection { get; set; }

        [Required(ErrorMessage = "用户名不能为空值。")]
        [Display(Name = "用户名")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        public string UserName { get; set; }

        [Display(Name = "归属部门")]
        public string DepartmentName { get; set; }

        [Display(Name = "手机")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "电子邮件不能为空值。")]
        [Display(Name = "电子邮件")]
        [StringLength(100, ErrorMessage = "你输入的数据超出限制100个字符的长度。")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "非法的电子邮件格式。")]
        public string EMail { get; set; }

        [Display(Name = "关联人员")]
        public string PersonId { get; set; }
        [Display(Name = "关联人员")]
        public string PersonName { get; set; }
        [PlainFacadeItemSpecification("PersonId")]
        public List<PlainFacadeItem> PersonItemCollection { get; set; }

        public bool LockoutEnabled { get; set; }   // 用户被禁用状态
        public Guid RoleId { get; set; }           // 角色Id

        public ApplicationUserForEditVM()
        { }
        public ApplicationUserForEditVM(ApplicationUser bo,Guid id)
        {
            this.IsNew = false;
            this.Id = Guid.Parse(bo.Id);
            this.UserName = bo.UserName;
            this.MobileNumber = bo.MobileNumber;
            this.EMail = bo.Email;
            this.Name = bo.ChineseFullName;
            this.LockoutEnabled = bo.LockoutEnabled;
            this.RoleId = id;
            if (bo.Person != null)
            {
                this.PersonId = bo.Person.Id.ToString();
                this.PersonName = bo.Person.Name;

                if (bo.Person.Department != null)
                {
                    this.DepartmentName = bo.Person.Department.Name;
                }
            }

        }

    }
}
