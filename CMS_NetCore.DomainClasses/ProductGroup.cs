using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class ProductGroup : BaseEntity
{
    public ProductGroup()
    {
        Products = new HashSet<Product>();
        AttributeGroups = new HashSet<AttributeGroup>();
        ProductRequests = new HashSet<ProductRequest>();
        DetailGroups = new HashSet<DetailGroup>();
    }

    public int Id { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [Display(Name = "عنوان گروه")]
    public string GroupTitle { get; set; }

    public int? Depth { get; set; }
    public string Path { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [Display(Name = "وضعیت گروه")]
    public bool? IsActive { get; set; }

    public int? DisplayOrder { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [Display(Name = "گروه والد")]
    public int? ParentId { get; set; }

    [Display(Name = "نام مستعار")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string AliasName { get; set; }

    public string Type { get; set; }
    public ICollection<Product> Products { get; set; }
    public ICollection<AttributeGroup> AttributeGroups { get; set; }
    public ICollection<ProductRequest> ProductRequests { get; set; }
    public ICollection<DetailGroup> DetailGroups { get; set; }
}