using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class ProductGallery
{
    public int Id { get; set; }
    public int ProductId { get; set; }

    [StringLength(100)]
    public string ImageName { get; set; }

    public Product Product { get; set; }
}