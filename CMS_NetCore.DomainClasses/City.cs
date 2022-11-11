using System.Collections.Generic;

namespace CMS_NetCore.DomainClasses;

public sealed class City
{
    public City()
    {
        Stores = new HashSet<Store>();
    }

    public int Id { get; set; }
    public string CityName { get; set; }
    public byte[] CityIcon { get; set; }
    public int StateId { get; set; }
    public State State { get; set; }
    public ICollection<Store> Stores { get; set; }
}