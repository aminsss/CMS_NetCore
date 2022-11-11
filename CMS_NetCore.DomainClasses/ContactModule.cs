using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS_NetCore.DomainClasses;

public sealed class ContactModule
{
    public ContactModule()
    {
        ContactPersons = new HashSet<ContactPerson>();
    }

    public int Id { get; set; }
    public string Email { get; set; }
    public string PhoneNo { get; set; }
    public string MobileNo { get; set; }
    public string Address { get; set; }
    public string PostCode { get; set; }

    [DataType(DataType.MultilineText)] 
    public string Description { get; set; }
    
    public Module Module { get; set; }
    public ICollection<ContactPerson> ContactPersons { get; set; }
}