using System.Collections.Generic;

namespace CMS_NetCore.DomainClasses;

public sealed class AttributeGroup
{
    public AttributeGroup()
    {
        AttributeItems = new HashSet<AttributeItem>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string AttributeType { get; set; }
    public int ProductGroupId { get; set; }
    public ProductGroup ProductGroup { get; set; }
    public ICollection<AttributeItem> AttributeItems { get; set; }
}