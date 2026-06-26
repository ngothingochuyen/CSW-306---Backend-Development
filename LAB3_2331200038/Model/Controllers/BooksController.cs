using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Models;

namespace Model.Controllers
{   

    // Exercise 2
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private static List<Book> _books = new List<Book>
        {
            new Book {
                Id = 1,
                Title = "One Liter of Tears",
                Author = "Kito Aya",
                Year = 1986,
                Genre = "Memoir"
            },
            new Book {
                Id = 2,
                Title = "Give Me a Ticket to Childhood",
                Author = "Nguyen Nhat Anh",
                Year = 2008,
                Genre = "Children's Literature"
            },
            new Book {
                Id = 3,
                Title = "Cay chuoi non di giay xanh",
                Author = "Nguyen Nhat Anh",
                Year = 2017,
                Genre = "Children's Literature"
            },
            new Book
            {
                Id = 4,
                Title = "Quan Go Di Len",
                Author = "Nguyen Nhat Anh",
                Year = 1999,
                Genre = "Children's Literature"
            }
        };



        //Exercise 3
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            return Ok(_books);
        }


        // Exercise 4
        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound(new { Message = $"Không tìm thấy cuốn sách nào có ID = {id}" });
            }

            return Ok(book);
        }


        // Exercise 5
        [HttpPost]
        public IActionResult AddBook([FromBody] Book newBook)
        {
            if (newBook == null)
            {
                return BadRequest("Invalid book data.");
            }

            int nextId = _books.Count > 0 ? _books.Max(b => b.Id) + 1 : 1;
            newBook.Id = nextId;

            _books.Add(newBook);

            return CreatedAtAction(nameof(GetBookById), new
            {
                id = newBook.Id
            }, newBook
            );
        }


        //Exercise 6
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book updatedBook)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == id);

            if (existingBook == null)
            {
                return NotFound(new { Message = $"Book with ID {id} not found." });
            }

            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.Year = updatedBook.Year;
            existingBook.Genre = updatedBook.Genre;

            return Ok(existingBook);
        }


        // Exercise 7
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound(new { Message = $"Book with ID {id} not found." });
            }

            _books.Remove(book);

            return Ok(new { Message = $"Book with ID {id} has been deleted successfully." });
        }
    }
}
