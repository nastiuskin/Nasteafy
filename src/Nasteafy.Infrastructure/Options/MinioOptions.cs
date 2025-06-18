namespace Nasteafy.Infrastructure.Options
{
    public class MinioOptions
    {
        public const string SectionName = "MinioOptions";
        public  required string Endpoint { get; set; }
        public required string AccessKey { get; set; }
        public required string SecretKey { get; set; }
        public required BucketSettings Image { get; set; }
        public required BucketSettings Audio { get; set; } 

        public class BucketSettings
        {
            public string Name { get; set; } = default!;
            public string[] AllowedExtensions { get; set; } = [];
        }
    }
}
