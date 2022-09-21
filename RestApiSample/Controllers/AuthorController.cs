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
    [Route("api/v1/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly BookLibraryContext _bookLibrary;

        public AuthorController(BookLibraryContext bookLibrary)
        {
            _bookLibrary = bookLibrary;
        }
        [HttpGet]
        public async Task<IEnumerable<Author>> Get() 
        {

            return await _bookLibrary.Authors.ToArrayAsync();
        }
        [HttpGet("{id}")]
        public ActionResult<Author> Get(Guid id)
        {
            Author author =_bookLibrary.Authors.FirstOrDefault(x => x.Id == id);
            if (author is null) return NotFound();
            return author;
        }
        [HttpPost]
        public ActionResult Post(AuthorCreateDto authorDto) 
        {
            Author author = new Author
            {
                FirstName=authorDto.FirstName,
                LastName=authorDto.LastName
            };
            _bookLibrary.Authors.Add(author);
            _bookLibrary.SaveChanges();
            return CreatedAtAction("Get", new { id = author.Id }, author);
        }
        [HttpPut("{id}")]
        public ActionResult Put(Guid id,AuthorUpdateDto authorDto) 
        {
            Author author = _bookLibrary.Authors.FirstOrDefault(x => x.Id == id);
            if (author is null)
            {
                author = new Author
                {
                    Id = id,
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName
                };
                _bookLibrary.Authors.Add(author);
                _bookLibrary.SaveChanges();
                return CreatedAtRoute("Get", new { id = author.Id }, author);
            }
            author.FirstName = authorDto.FirstName;
            author.LastName = authorDto.LastName;
            _bookLibrary.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid authorId, Guid id)
        {
            var author = _bookLibrary.Authors.FirstOrDefault(p => p.Id == authorId);
            if (author is null) return NotFound();

            var book = _bookLibrary.Books.FirstOrDefault(p => p.Id == id);
            if (book is null) return NotFound();

            _bookLibrary.Remove(book);
            _bookLibrary.SaveChanges();
            return NoContent();
        }
    }
}