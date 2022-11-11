namespace CMS_NetCore.DomainClasses;

public sealed class HtmlModule
{
    public int Id { get; set; }
    public int ModuleId { get; set; }
    public string HtmlText { get; set; }
    public Module Module { get; set; }
}