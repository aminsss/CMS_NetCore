namespace CMS_NetCore.DomainClasses;

public sealed class StoreInfo
{
    public int Id { get; set; }
    public string StoreId { get; set; }
    public string Banner { get; set; }
    public string ZindexMap { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public Store Store { get; set; }
}