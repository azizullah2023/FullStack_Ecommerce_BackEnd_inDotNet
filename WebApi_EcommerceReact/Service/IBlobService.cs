namespace WebApi_EcommerceReact.Service
{
    public interface IBlobService
    {
       Task<string> GetBlob(string BlobName, string ContainerName);
        Task<bool> DeleteBlob(string BlobName, string ContainerName);
        Task<string> UploadBlob(string BlobName, string ContainerName,IFormFile file);
    }
}
