using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Linq.Expressions;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : Controller
    {
        MySqlConnection connection =
            new MySqlConnection("server=localhost;uid=root;pwd=;database=books_app");

        [HttpGet]
        public ActionResult<List<Book>> GetBooks()
        {
            List<Book> books = new List<Book>();

            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM books";
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    Book book = new Book();
                    book.Id = data.GetInt32("id");
                    book.Title = data.GetString("title");
                    book.Author = data.GetString("author");
                    books.Add(book);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, "Something went wrong");
            }
            return Ok(books);
        }

        [HttpPost]
        public ActionResult PostBook(Book book)
        {
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "INSERT INTO books(title, author) VALUES(@title, @author)";
                query.Parameters.AddWithValue("@title", book.Title);
                query.Parameters.AddWithValue("@author", book.Author);
                int rows = query.ExecuteNonQuery();

                if (rows < 0)
                {
                    return StatusCode(500, "Could not post book");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
            return StatusCode(201);
        }


        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(int id)
        {
            Book book = new Book();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM book where id = @id";
                query.Parameters.AddWithValue("@id", id);
                MySqlDataReader data = query.ExecuteReader();

                if ((data.Read()))
                {
                    book.Id = id;
                    book.Author = data.GetString("author");
                    book.Title = data.GetString("title");
                }
                else
                {
                    return NotFound("The book does not exist");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, "Something went wrong");
            }
            return Ok(book);
        }

        [HttpGet("author/{author}")]
        public ActionResult<List<Book>> GetBook(string author)
        {
            List<Book> books = new List<Book>();
            Book book = new Book();
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM book WHERE Author = @author";
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    book.Author = data.GetString("author");
                    book.Title = data.GetString("title");
                    books.Add(book);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, "Something went wrong");
            }
            return Ok(book);
        }
    }
}