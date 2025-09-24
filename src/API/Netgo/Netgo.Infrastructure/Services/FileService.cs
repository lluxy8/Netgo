using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Netgo.Application.Contracts.Infrastructure;
using Netgo.Application.Models;

namespace Netgo.Infrastructure.Services
{
    public class S3FileService : IFileService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3FileService(IOptions<MinioSettings> options)
        {
            var settings = options.Value;

            _bucketName = settings.bucketName;

            var config = new AmazonS3Config
            {
                ServiceURL = settings.Endpoint,
                ForcePathStyle = true
            };

            _s3Client = new AmazonS3Client(settings.AccessKey, settings.SecretKey, config);
        }

        public async Task<string> SaveFileAsync(string folderName, IFormFile file)
        {
            var key = $"{folderName}/{file.FileName}";

            using var stream = file.OpenReadStream();

            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = stream,
                ContentType = file.ContentType
            };

            await _s3Client.PutObjectAsync(putRequest);

            return key;
        }

        public async Task<IEnumerable<string>> GetFilesAsync(string folderName)
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = $"{folderName}/"
            };

            var response = await _s3Client.ListObjectsV2Async(request);

            return response.S3Objects.Select(o => o.Key);
        }

        public async Task DeleteFileAsync(string folderName, string fileName)
        {
            var key = $"{folderName}/{fileName}";
            await _s3Client.DeleteObjectAsync(_bucketName, key);
        }

        public async Task DeleteFolderAsync(string folderName)
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = $"{folderName}/"
            };

            var response = await _s3Client.ListObjectsV2Async(request);

            foreach (var obj in response.S3Objects)
            {
                await _s3Client.DeleteObjectAsync(_bucketName, obj.Key);
            }
        }
    }
}
