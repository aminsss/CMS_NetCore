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

    public partial class TicketGroup
    {
        public TicketGroup()
        {
            this.Ticket = new HashSet<Ticket>();
        }
    
        public int TicketGroupId { get; set; }

        [MaxLength(50)]
        public string Subject { get; set; }
        public int chartPostId { get; set; }
    
        public virtual chartPost chartPost { get; set; }
        public virtual ICollection<Ticket> Ticket { get; set; }
    }
}
