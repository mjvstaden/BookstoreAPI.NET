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
            foreach (var book in await GetAsync())
            {
                if (book.Id == id)
                {
                    return book;
                }
            }
            return null;
        }

        public void CreateBook(BookRequest book)
        {
            var new_book = new Book
            {
                title = book.title,
                author = book.author,
                genre = book.genre,
                price = book.price
            };

            _books.InsertOne(new_book);
        }

        public void UpdateBook(string id, Book bookIn)
        {
            _books.UpdateOne(book => book.Id == id, new BsonDocument("$set", bookIn.ToBsonDocument()));
        }

        public void RemoveBook(string id)
        {
            _books.DeleteOne(book => book.Id == id);
        }

        public async Task<Book> getBookByTitle(string title) 
        {
            foreach (var book in await GetAsync())
            {
                if (book.title.Equals(title, StringComparison.CurrentCultureIgnoreCase))
                {
                    return book;
                }
            }
            return null;
        }
        public async Task<List<Book>> getBookByAuthor(string author) 
        {
            List<Book> books = new List<Book>();
            foreach (var book in await GetAsync())
            {
                if (book.author.Equals(author, StringComparison.CurrentCultureIgnoreCase))
                {
                    books.Add(book);
                }
            }
            return books;
        }
        public async Task<List<Book>> getBookByGenre(string genre) 
        {
            List<Book> books = new();
            foreach (var book in await GetAsync())
            {
                if (book.genre.Equals(genre, StringComparison.CurrentCultureIgnoreCase))
                {
                    books.Add(book);
                }
            }
            return books;
        }
    }
}