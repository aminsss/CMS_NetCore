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

    public partial class AttributGrp
    {
        public AttributGrp()
        {
            this.AttributItem = new HashSet<AttributItem>();
        }
    
        public int AttributGrpId { get; set; }

        public string Name { get; set; }

        public string Attr_type { get; set; }

        public int ProductGroupId { get; set; }

        public virtual ProductGroup ProductGroup { get; set; }
        public virtual ICollection<AttributItem> AttributItem { get; set; }
    }
}