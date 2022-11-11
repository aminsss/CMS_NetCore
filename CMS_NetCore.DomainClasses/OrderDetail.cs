namespace CMS_NetCore.DomainClasses;

public sealed class OrderDetail
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int ProductCount { get; set; }
    public int ProductPrice { get; set; }
    public int Sum { get; set; }
    public Order Order { get; set; }
    public Product Product { get; set; }
}