using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class State
{
    public State()
    {
        Cities = new HashSet<City>();
    }

    public int Id { get; set; }

    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Name { get; set; }

    [StringLength(
        100,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string StateIcon { get; set; }

    public ICollection<City> Cities { get; set; }
}