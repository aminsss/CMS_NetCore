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

    public partial class UserStore : BaseEntity
    {
        public int UserStoreId { get; set; }

        public int UserId { get; set; }

        public string StoreId { get; set; }
    
        public virtual Store Store { get; set; }
        public virtual User User { get; set; }
    }
}
