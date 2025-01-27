namespace Million.Technical.Test.Application.Shared.Methods
{
    public interface IImageService
    {
        Task<byte[]> CompressImageAsync(byte[] imageData, string fileName);
    }
}