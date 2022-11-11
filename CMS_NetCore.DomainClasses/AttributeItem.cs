using System.Collections.Generic;

namespace CMS_NetCore.DomainClasses
{
    public sealed class AttributeItem
    {
        public AttributeItem()
        {
            ProductAttributes = new HashSet<ProductAttribute>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string FilterId { get; set; }
        public int AttributeGroupId { get; set; }
        public AttributeGroup AttributeGroup { get; set; }
        public ICollection<ProductAttribute> ProductAttributes { get; set; }
    }
}