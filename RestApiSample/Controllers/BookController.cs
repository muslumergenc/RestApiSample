using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApiSample.Model;
using RestApiSample.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestApiSample.Model.Dtos;
namespace RestApiSample.Controllers
{
    [Route("api/v1/Books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookLibraryContext _bookLibrary;

        public BookController(BookLibraryContext bookLibrary)
        {
            _bookLibrary = bookLibrary;
        }
        [HttpGet]
        public async Task<IEnumerable<Book>> Get() 
        {

            return await _bookLibrary.Books.ToArrayAsync();
        }
        [HttpGet("{id}")]
        public ActionResult<Book> Get(Guid id)
        {
            Book Book =_bookLibrary.Books.FirstOrDefault(x => x.Id == id);
            if (Book is null) return NotFound();
            return Book;
        }
        [HttpPost]
        public ActionResult Post(BookCreateDto BookDto) 
        {
            Book Book = new Book
            {
                Name = BookDto.Name
            };
            _bookLibrary.Books.Add(Book);
            _bookLibrary.SaveChanges();
            return CreatedAtAction("Get", new { id = Book.Id }, Book);
        }
        [HttpPut("{id}")]
        public ActionResult Put(Guid id,BookUpdateDto BookDto) 
        {
            Book Book = _bookLibrary.Books.FirstOrDefault(x => x.Id == id);
            if (Book is null)
            {
                Book = new Book
                {
                    Id = id,
                   Name=BookDto.Name
                };
                _bookLibrary.Books.Add(Book);
                _bookLibrary.SaveChanges();
                return CreatedAtRoute("Get", new { id = Book.Id }, Book);
            }
            Book.Name= BookDto.Name;
            _bookLibrary.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid BookId, Guid id)
        {
            var Book = _bookLibrary.Books.FirstOrDefault(p => p.Id == BookId);
            if (Book is null) return NotFound();

            var book = _bookLibrary.Books.FirstOrDefault(p => p.Id == id);
            if (book is null) return NotFound();

            _bookLibrary.Remove(book);
            _bookLibrary.SaveChanges();
            return NoContent();
        }
    }
}