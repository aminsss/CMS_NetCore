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

    public partial class Component
    {
        public Component()
        {
            this.Module = new HashSet<Module>();
        }
    
        public int ComponentId { get; set; }

        public string ComponentName { get; set; }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }

        public string Descroption { get; set; }

        public string AdminAction { get; set; }

        public string AdminController { get; set; }

        public virtual ICollection<Module> Module { get; set; }
    }
}
