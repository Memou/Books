using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly DataContext _context;
        public BooksController(DataContext context)
        {
            _context = context;
        }

        // GET api/v1/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> Get()
        {
            List<Book> books = await _context.Books.ToListAsync();

            //No offset&limit|page,TODO:for the future.
            return Ok(books.OrderBy(i => i.Id));
        }

        // GET api/v1/books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            Book book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }


        // POST api/V1/books
        [HttpPost]
        public async Task<ActionResult<Book>> Post([FromBody] Book book)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }


                if (await _context.Books.Where(o => (o.Title == book.Title) && (o.Author == book.Author)).AnyAsync())
                {
                    return BadRequest("Book and Author already exists in the system.");
                }



                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return Created($"Api/v1/Books/{book.Id}", book);


            }
            catch (Exception)
            {
                return BadRequest();
            }

        }



        // PUT api/V1/books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int Id, [FromBody] Book book)
        {
            try
            {

                var tempbook = await _context.Books.FindAsync(Id);

                if (tempbook == null)
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                tempbook.Author = book.Author;
                tempbook.Price = book.Price;
                tempbook.Description = book.Description;
                tempbook.Title = book.Title;
                tempbook.CoverImage = book.CoverImage;


                await _context.SaveChangesAsync();

                //Could have returned ok with the object and its affected relationships in a real scenario,but for simplicity returning 204.
                //return Ok(_context.Books.FindAsync(Id).Result);

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        // DELETE api/v1/books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> Delete(int id)
        {
            Book book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return Ok(book);

        }
    }
}
