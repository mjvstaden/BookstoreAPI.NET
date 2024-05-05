namespace BookStoreAPI.Models
{
    public class MongoDBSettings : IMongoDBSettings
    {
        public string ConnectionURI { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }

    public interface IMongoDBSettings
    {
        string ConnectionURI { get; set; }
        string DatabaseName { get; set; }
        string CollectionName { get; set; }
    }
}