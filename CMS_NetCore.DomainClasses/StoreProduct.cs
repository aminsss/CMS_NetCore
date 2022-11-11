using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class StoreProduct : BaseEntity
{
    public int Id { get; set; }

    public string StoreId { get; set; }

    public int ProductId { get; set; }

    [DisplayFormat(
        ApplyFormatInEditMode = true,
        DataFormatString = "{0:c}"
    )]
    public decimal? Price { get; set; }

    [StringLength(100)]
    public string OffPrice { get; set; }

    public bool? IsActive { get; set; }

    [StringLength(100)]
    public string Color { get; set; }

    [StringLength(100)]
    public string LinkSale { get; set; }

    [StringLength(100)]
    public string Detail { get; set; }

    public Product Product { get; set; }
    public Store Store { get; set; }
}