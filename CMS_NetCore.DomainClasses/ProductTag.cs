using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class ProductTag
{
    public int Id { get; set; }
    public int ProductId { get; init; }

    [StringLength(100)]
    public string TagTitle { get; init; }

    public Product Product { get; set; }
}