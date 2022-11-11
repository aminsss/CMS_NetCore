namespace CMS_NetCore.DomainClasses;

public sealed class ModulePage
{
    public int Id { get; set; }
    public int MenuId { get; set; }
    public int ModuleId { get; set; }
    public Menu Menu { get; set; }
    public Module Module { get; set; }
}