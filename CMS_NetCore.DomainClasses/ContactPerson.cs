namespace CMS_NetCore.DomainClasses;

public sealed class ContactPerson
{
    public int Id { get; set; }
    public int ContactModuleId { get; set; }
    public int UserId { get; set; }
    public ContactModule ContactModule { get; set; }
    public User User { get; set; }
}