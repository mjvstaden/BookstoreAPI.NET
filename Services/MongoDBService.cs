using BookStoreAPI.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;

namespace BookStoreAPI.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Book> _books;
        public MongoDBService(IOptions<MongoDBSettings> settings)
        {
            MongoClient client = new MongoClient(settings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(settings.Value.DatabaseName);
            _books = database.GetCollection<Book>(settings.Value.CollectionName);
        }

        public async Task<List<Book>> GetAsync()
        {
            return await _books.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Book> GetBook(string id)
        {
            return await _books.Find<Book>(book => book.Id == id).FirstOrDefaultAsync();
        }

        public Book CreateBook(Book book)
        {
            _books.InsertOne(book);
            return book;
        }

        public void UpdateBook(string id, Book bookIn)
        {
            _books.ReplaceOne(book => book.Id == id, bookIn);
        }

        public void RemoveBook(Book bookIn)
        {
            _books.DeleteOne(book => book.Id == bookIn.Id);
        }

        public void RemoveBook(string id)
        {
            _books.DeleteOne(book => book.Id == id);
        }
    }
}