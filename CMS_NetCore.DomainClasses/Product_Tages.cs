//------------------------------------------------------------------------------
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

    public partial class ProductTag
    {
        public int ProductTagId { get; set; }

        public int ProductId { get; set; }

        [StringLength(100)]
        public string TagTitle { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
