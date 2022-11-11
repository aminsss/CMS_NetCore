namespace CMS_NetCore.DomainClasses;

public sealed class MenuModule
{
    public int Id { get; set; }
    public int MenuGroupId { get; set; }
    public MenuGroup MenuGroup { get; set; }
    public Module Module { get; set; }
}