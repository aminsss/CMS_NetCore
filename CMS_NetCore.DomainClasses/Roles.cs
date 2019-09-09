﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CMS_NetCore.DomainClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Role
    {
        public Role()
        {
            this.User = new HashSet<User>();
        }
    
        public int RoleId { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "عنوان نقش")]
        [StringLength(50, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
        public string RoleTitle { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نقش سیستمی")]
        [StringLength(50, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
        public string RoleName { get; set; }
    
        public virtual ICollection<User> User { get; set; }
    }
}
