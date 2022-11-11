using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class TicketGroup
{
    public TicketGroup()
    {
        Tickets = new HashSet<Ticket>();
    }

    public int Id { get; set; }

    [MaxLength(50)]
    public string Subject { get; set; }

    public int ChartPostId { get; set; }
    public ChartPost ChartPost { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}