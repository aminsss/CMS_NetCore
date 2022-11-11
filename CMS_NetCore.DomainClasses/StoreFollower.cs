namespace CMS_NetCore.DomainClasses;

public sealed class StoreFollower
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string StoreId { get; set; }
    public Store Store { get; set; }
    public User User { get; set; }
}