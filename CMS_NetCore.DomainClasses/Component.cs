using System.Collections.Generic;

namespace CMS_NetCore.DomainClasses;

public sealed class Component
{
    public Component()
    {
        Modules = new HashSet<Module>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string ModuleName { get; set; }
    public string ModuleType { get; set; }
    public string Description { get; set; }
    public string AdminAction { get; set; }
    public string AdminController { get; set; }
    public ICollection<Module> Modules { get; set; }
}