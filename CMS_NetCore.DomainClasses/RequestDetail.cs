using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class RequestDetail
{
    public int RequestDetailId { get; set; }

    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Value { get; set; }

    public int DetailItemId { get; set; }

    public int? ProductRequestId { get; set; }

    public ProductRequest ProductRequest { get; set; }
    public DetailItem DetailItem { get; set; }
}