using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class ProductRequest
{
    public ProductRequest()
    {
        RequestDetails = new HashSet<RequestDetail>();
    }

    public int ProductRequestId { get; set; }
    public string NAme { get; set; }
    public int ProductGroupId { get; set; }
    public int UserId { get; set; }

    [StringLength(
        150,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Status { get; set; }

    public string Description { get; set; }

    [StringLength(
        150,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Response { get; set; }

    [StringLength(
        150,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Detail { get; set; }

    public int? ProductId { get; set; }

    public User Users { get; set; }
    public ICollection<RequestDetail> RequestDetails { get; set; }
    public ProductGroup ProductGroup { get; set; }
}