namespace Million.Technical.Test.Domain.Entities
{
    public class Owner
    {
        public int IdOwner { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public DateTime Birthday { get; set; }
        public required byte[] Photo { get; set; }

        public virtual required ICollection<Property> Properties { get; set; }
    }
}