using System.Collections.Generic;

namespace CMS_NetCore.DomainClasses;

public sealed class DetailItem
{
    public DetailItem()
    {
        ProductDetails = new HashSet<ProductDetail>();
        RequestDetails = new HashSet<RequestDetail>();
    }

    public int Id { get; set; }
    public string DetailTitle { get; set; }
    public string DetailType { get; set; }
    public int DetailGroupId { get; set; }
    public DetailGroup DetailGroup { get; set; }
    public ICollection<ProductDetail> ProductDetails { get; set; }
    public ICollection<RequestDetail> RequestDetails { get; set; }
}