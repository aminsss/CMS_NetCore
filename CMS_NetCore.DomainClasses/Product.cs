using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class Product : BaseEntity
{
    public Product()
    {
        OrderDetails = new HashSet<OrderDetail>();
        ProductGalleries = new HashSet<ProductGallery>();
        ProductTags = new HashSet<ProductTag>();
        ProductAttributes = new HashSet<ProductAttribute>();
        ProductDetails = new HashSet<ProductDetail>();
        StoresProducts = new HashSet<StoreProduct>();
    }

    public int Id { get; set; }

    [Display(Name = "نام محصول (انگلیسی)")]
    [StringLength(
        100,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string Name { get; set; }

    [Display(Name = "گروه محصول")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public int ProductGroupId { get; set; }

    [Display(Name = "عنوان محصول")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [StringLength(
        100,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Title { get; set; }

    [Display(Name = "تصویر محصول")]
    [StringLength(
        100,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Image { get; set; }

    [Display(Name = "نام مستعار")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string AliasName { get; set; }

    public int? GroupModel { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; }
    public ICollection<ProductGallery> ProductGalleries { get; set; }
    public ICollection<ProductTag> ProductTags { get; set; }
    public ICollection<ProductAttribute> ProductAttributes { get; set; }
    public ICollection<ProductDetail> ProductDetails { get; set; }
    public ICollection<StoreProduct> StoresProducts { get; set; }
    public ProductGroup ProductGroup { get; set; }
}