namespace BlazorApp.Client.Services
{
    public interface IFileService
    {
        Task UploadFileAsync(Guid userId, byte[] fileData);
        Task<byte[]> GetFileAsync(Guid userId);
    }
}