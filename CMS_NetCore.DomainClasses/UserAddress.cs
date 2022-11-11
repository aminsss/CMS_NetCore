namespace CMS_NetCore.DomainClasses
{
    public sealed class UserAddress
    {
        public int Id { get; set; }
        public string NameFamily { get; set; }
        public string MobileNo { get; set; }
        public string HomeNo { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostAddress { get; set; }
        public string PostalCode { get; set; }
        public int UserId { get; set; }
        public User Users { get; set; }
    }
}