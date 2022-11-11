using System.Collections.Generic;

namespace CMS_NetCore.DomainClasses;

public sealed class ChartPost
{
    public ChartPost()
    {
        TicketGroups = new HashSet<TicketGroup>();
        Users = new HashSet<User>();
    }

    public int Id { get; set; }
    public string PostDuty { get; set; }
    public ICollection<TicketGroup> TicketGroups { get; set; }
    public ICollection<User> Users { get; set; }
}