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

    public partial class Address_User
    {
        public int Address_UserId { get; set; }

        public string NameFamily { get; set; }

        public string MobileNo { get; set; }

        public string HomeNo { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string PostAddress { get; set; }

        public string postalCode { get; set; }

        public int UserId { get; set; }
        public virtual User Users { get; set; }
    }
}
