using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class TicketMessage : BaseEntity
{
    public int Id { get; set; }
    public int TicketId { get; set; }

    [StringLength(
        50,
        ErrorMessage = "تعداد کاراکترها را رعایت کنید"
    )]
    public string Message { get; set; }

    public int? UserId { get; set; }
    public Ticket Ticket { get; set; }
    public User User { get; set; }
}