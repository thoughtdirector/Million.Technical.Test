namespace Million.Technical.Test.Domain.Entities
{
    public class PropertyImage
    {
        public int IdPropertyImage { get; set; }
        public int IdProperty { get; set; }
        public required byte[] ImageData { get; set; }
        public bool Enabled { get; set; }
        public bool IsPrimary { get; set; }

        public virtual required Property Property { get; set; }
    }
}