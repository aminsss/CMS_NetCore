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

    public partial class NewsTag
    {
        public int NewsTagId { get; set; }

        public int NewsId { get; set; }

        [StringLength(150, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
        public string TagsTitle { get; set; }
    
        public virtual News News { get; set; }
    }
}
