namespace Million.Technical.Test.Domain.Entities
{
    public class Property
    {
        public Guid IdProperty { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public decimal? Price { get; set; }
        public string? CodeInternal { get; set; }
        public int Year { get; set; }

        public Guid IdOwner { get; set; }
        public virtual Owner? Owner { get; set; }
        public virtual ICollection<PropertyImage>? PropertyImages { get; set; }
        public virtual ICollection<PropertyTrace>? PropertyTraces { get; set; }
    }
}