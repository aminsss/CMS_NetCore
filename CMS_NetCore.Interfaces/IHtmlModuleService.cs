﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_NetCore.DomainClasses;

namespace CMS_NetCore.Interfaces
{
    public interface IHtmlModuleService
    {
        Task<HtmlModule> GetByModuleId(int? moduleId);
        Task Edit(HtmlModule htmlModule);
    }
}
