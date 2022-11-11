using System.Collections.Generic;

namespace CMS_NetCore.DomainClasses;

public sealed class DetailGroup
{
    public DetailGroup()
    {
        DetailItems = new HashSet<DetailItem>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public int ProductGroupId { get; set; }
    public ProductGroup ProductGroup { get; set; }
    public ICollection<DetailItem> DetailItems { get; set; }
}