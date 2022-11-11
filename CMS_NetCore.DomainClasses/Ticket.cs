using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class Ticket : BaseEntity
{
    public Ticket()
    {
        TicketMessages = new HashSet<TicketMessage>();
    }

    public int TicketId { get; set; }
    public int TicketGroupId { get; set; }

    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Title { get; set; }

    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Status { get; set; }

    public int UserId { get; set; }
    public ICollection<TicketMessage> TicketMessages { get; set; }
    public TicketGroup TicketGroup { get; set; }
    public User User { get; set; }
}