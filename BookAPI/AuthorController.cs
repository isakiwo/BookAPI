using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace BookApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : Controller
    {
        MySqlConnection connection =
            new MySqlConnection("server=localhost;uid=root;pwd=;database=books_app");

        [HttpGet]
        public ActionResult<List<Authors>> GetAuthors()
        {
            List<Authors> books = new List<Authors>();

            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM books";
                MySqlDataReader data = query.ExecuteReader();

                while (data.Read())
                {
                    Authors author = new Authors();
                    author.Id = data.GetInt32("id");
                    author.Author = data.GetString("author");
                    books.Add(author);

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
        public ActionResult PostBook(Authors author)
        {
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "INSERT INTO books(author) VALUES(@author)";
                query.Parameters.AddWithValue("@author", author.Author);
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

    }
}