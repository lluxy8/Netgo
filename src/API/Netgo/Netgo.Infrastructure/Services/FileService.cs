using Microsoft.AspNetCore.Http;
using Netgo.Application.Contracts.Infrastructure;

namespace Netgo.Infrastructure.Services
{
    //// FIX
    public class S3FileService : IFileService
    {
        /*
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3FileService(IAmazonS3 s3Client, string bucketName)
        {
            _s3Client = s3Client;
            _bucketName = bucketName;

            var bucketExists = _s3Client.ListBucketsAsync().Result.Buckets.Any(b => b.BucketName == _bucketName);
            if (!bucketExists)
            {
                _s3Client.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = _bucketName
                }).Wait();
            }
        }
        */

        public async Task<string> SaveFileAsync(string folderName, IFormFile file)
        {
            /*
            string key = $"{folderName}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using var stream = file.OpenReadStream();
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = stream
            };

            await _s3Client.PutObjectAsync(request);

            return key; 
            */

            await Task.Delay(1);
            return $"{folderName}/{Guid.NewGuid}.png";
        }

        public async Task<IEnumerable<string>> GetFilesAsync(string folderName)
        {
            /*
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = folderName + "/"
            };

            var response = await _s3Client.ListObjectsV2Async(request);
            return response.S3Objects.Select(o => o.Key);

            */

            await Task.Delay(1);
            return [$"{folderName}/orne1k.png", $"{folderName}/ornek2.png"];
        }

        public async Task DeleteFileAsync(string folderName, string fileName)
        {
            /*
            string key = $"{folderName}/{fileName}";
            await _s3Client.DeleteObjectAsync(_bucketName, key);
            */

            await Task.Delay(1);
        }

        public async Task DeleteFolderAsync(string folderName)
        {
            /*
            var files = await GetFilesAsync(folderName);

            foreach (var key in files)
            {
                await _s3Client.DeleteObjectAsync(_bucketName, key);
            }

            */

            await Task.Delay(1);
        }
    }
}
