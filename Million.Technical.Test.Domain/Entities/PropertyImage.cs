namespace Million.Technical.Test.Domain.Entities
{
    public class PropertyImage
    {
        public Guid IdPropertyImage { get; set; }
        public byte[]? ImageData { get; set; }
        public bool Enabled { get; set; }

        public Guid IdProperty { get; set; }
        public virtual Property? Property { get; set; }
    }
}