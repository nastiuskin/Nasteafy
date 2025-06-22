namespace Nasteafy.Infrastructure.Options
{
    public class MinioOptions
    {
        public const string SectionName = "MinioOptions";
        public required string Endpoint { get; set; }
        public required string AccessKey { get; set; }
        public required string SecretKey { get; set; }
        public required BucketSection Buckets { get; set; }

        public class BucketSection
        {
            public required BucketSettings Image { get; set; }
            public required BucketSettings Audio { get; set; }
        }

        public class BucketSettings
        {
            public required string Name { get; set; }
            public string[] AllowedExtensions { get; set; } = [];
        }
    }
}
