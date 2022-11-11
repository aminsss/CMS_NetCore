using System.Collections.Generic;

namespace CMS_NetCore.DomainClasses;

public sealed class Order : BaseEntity
{
    public Order()
    {
        OrderDetail = new HashSet<OrderDetail>();
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    public bool? IsFinally { get; set; }
    public ICollection<OrderDetail> OrderDetail { get; set; }
    public User User { get; set; }
}