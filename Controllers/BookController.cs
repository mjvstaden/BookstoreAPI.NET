using System;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Models;
using BookStoreAPI.Services;

namespace BookStoreAPI.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly MongoDBService _mongoDBService;

        public BooksController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet("healthcheck")]
        public string HealthCheck()
        {
            return "BookStoreAPI is running!";
        }

        [HttpGet("all")]
        public async Task<List<Book>> Get()
        {
            return await _mongoDBService.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _mongoDBService.GetBook(id);

            if (book == null)
            {
                return NotFound("Book not found!");
            }

            return Ok(book);
        }

        [HttpPost]
        public ActionResult Create(BookRequest book)
        {
            try 
            {
                if (book.title == null || book.author == null || book.genre == null || book.price == 0)
                {
                    return BadRequest("Missing required fields!");
                }
                _mongoDBService.CreateBook(book);
            } 
            catch (Exception e) 
            {
                return BadRequest(e.Message);
            }
            return Ok("Book created!");
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, BookRequest bookIn)
        {
            if (bookIn.title == null && bookIn.author == null && bookIn.genre == null && bookIn.price == 0)
            {
                return BadRequest("Please enter a field to update");
            }

            var book = await _mongoDBService.GetBook(id);

            if (book == null)
            {
                return NotFound("Book not found!");
            }

            var book_updated = new Book
            {
                Id = id,
                title = string.IsNullOrEmpty(bookIn.title) ? book.title : bookIn.title,
                author = string.IsNullOrEmpty(bookIn.author) ? book.author : bookIn.author,
                genre = string.IsNullOrEmpty(bookIn.genre) ? book.genre : bookIn.genre,
                price = bookIn.price == 0 ? book.price : bookIn.price
            };

            _mongoDBService.UpdateBook(id, book_updated);

            return Ok("Book updated!");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var book = _mongoDBService.GetBook(id);

            if (book == null)
            {
                return NotFound();
            }

            _mongoDBService.RemoveBook(id);

            return Ok("Book deleted!");
        }

    }
}