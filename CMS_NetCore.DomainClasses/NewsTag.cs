using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class NewsTag
{
    public int Id { get; set; }
    public int NewsId { get; set; }

    [StringLength(
        150,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string TagsTitle { get; set; }

    public News News { get; set; }
}