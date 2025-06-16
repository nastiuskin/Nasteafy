namespace Nasteafy.Infrastructure.Options
{
    public class MinioOptions
    {
        public const string SectionName = "MinioOptions";
        public string Endpoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public BucketNames Buckets { get; set; }

        public class BucketNames
        {
            public string Audio { get; set; }
            public string Image { get; set; }
        }
    }
}
