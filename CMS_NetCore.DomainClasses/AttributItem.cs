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

    
    public partial class AttributItem
    {
        public AttributItem()
        {
            this.Product_Attribut = new HashSet<Product_Attribut>();
        }
    
        public int AttributItemId { get; set; }

        public string Name { get; set; }

        public string value { get; set; }

        public string idfilter { get; set; }

        public int AttributGrpId { get; set; }
        public virtual AttributGrp AttributGrp { get; set; }
        public virtual ICollection<Product_Attribut> Product_Attribut { get; set; }
    }
}