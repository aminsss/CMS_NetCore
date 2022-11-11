using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class NewsGallery
{
    public int Id { get; set; }
    public int NewsId { get; set; }

    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string ImageName { get; set; }

    public News News { get; set; }
}