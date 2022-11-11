using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class ProductDetail
{
    public int Id { get; set; }

    [StringLength(
        100,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Value { get; set; }

    public int? DetailItemId { get; set; }
    public int ProductId { get; set; }
    public DetailItem DetailItem { get; set; }
    public Product Product { get; set; }
}