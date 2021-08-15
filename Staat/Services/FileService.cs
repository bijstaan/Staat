using Microsoft.Extensions.Configuration;
using Storage.Net;
using Storage.Net.Blobs;

namespace Staat.Services
{
    public interface IFileService
    {
        IBlobStorage BlobStorage();
    }
    
    public class FileService : IFileService
    {
        public readonly IConfiguration _configuration;
        
        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IBlobStorage BlobStorage()
        {
            StorageFactory.Modules.UseAwsStorage();
            StorageFactory.Modules.UseAzureStorage();
            StorageFactory.Modules.UseGoogleCloudStorage();
            return StorageFactory.Blobs.FromConnectionString(_configuration.GetSection("App")["StorageString"]);
        }
    }
}