using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class ProductAttribute
{
    public int Id { get; set; }
    public bool? IsChecked { get; set; }

    [StringLength(100)]
    public string Value { get; set; }

    public int? AttributeItemId { get; set; }
    public int ProductId { get; set; }
    public AttributeItem AttributeItem { get; set; }
    public Product Product { get; set; }
}