using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class Position
{
    public Position()
    {
        Module = new HashSet<Module>();
    }

    public int Id { get; set; }

    [MaxLength(50)]
    public string Title { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }

    public ICollection<Module> Module { get; set; }
}