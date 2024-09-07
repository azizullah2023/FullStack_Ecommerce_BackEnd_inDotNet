
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace WebApi_EcommerceReact.Service
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public BlobService(BlobServiceClient blobServiceClient)
        {
                _blobServiceClient=blobServiceClient;
    }
        public async Task<bool> DeleteBlob(string BlobName, string ContainerName)
        {
            BlobContainerClient Container = _blobServiceClient.GetBlobContainerClient(ContainerName);
            BlobClient blobClient = Container.GetBlobClient(BlobName);
            return await blobClient.DeleteIfExistsAsync();
        }

        public async Task<string> GetBlob(string BlobName, string ContainerName)
        {
            BlobContainerClient Container = _blobServiceClient.GetBlobContainerClient(ContainerName);
            BlobClient blobClient = Container.GetBlobClient(BlobName);
            return  blobClient.Uri.AbsoluteUri;
        }

        public async Task<string> UploadBlob(string BlobName, string ContainerName, IFormFile file)
        {
            BlobContainerClient Container = _blobServiceClient.GetBlobContainerClient(ContainerName);
            BlobClient blobClient = Container.GetBlobClient(BlobName);
            var hhtpHeader = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };
            var result = await blobClient.UploadAsync(file.OpenReadStream(), hhtpHeader);
            if (result!=null) {
                return await GetBlob(BlobName, ContainerName);
            }
            return "something is wrong";
        }
    }
}
