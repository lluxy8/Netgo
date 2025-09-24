namespace Netgo.Application.Models
{
    public class MinioSettings
    {
        public string Endpoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string bucketName { get; set; }
    }
}
