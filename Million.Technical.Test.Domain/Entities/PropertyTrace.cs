namespace Million.Technical.Test.Domain.Entities
{
    public class PropertyTrace
    {
        public int IdPropertyTrace { get; set; }
        public DateTime DateSale { get; set; }
        public required string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
        public int IdProperty { get; set; }

        public virtual required Property Property { get; set; }
    }
}