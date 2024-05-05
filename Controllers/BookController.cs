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

        [HttpGet("{id}", Name = "GetBook")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _mongoDBService.GetBook(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public ActionResult Create(Book book)
        {
            _mongoDBService.CreateBook(book);

            return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Book bookIn)
        {
            var book = _mongoDBService.GetBook(id);

            if (book == null)
            {
                return NotFound();
            }

            _mongoDBService.UpdateBook(id, bookIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            // var book = _mongoDBService.GetBook(id);

            // if (book == null)
            // {
            //     return NotFound();
            // }

            // _mongoDBService.RemoveBook(book.Id);

            return NoContent();
        }
    }
}