using Microsoft.AspNetCore.Mvc;
using MvcProject99.Models;
using System.Data.SqlClient;

namespace MvcProject99.Controllers
{
    public class HomePageController : Controller
    {
        public IConfiguration _configuration;

        public string connectionString = "";

        public HomePageController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("myConnection");
        }

        [Route("")]
        public IActionResult Index(string CatgN)
        {
           
            Warehouse Wproducts = new Warehouse();
            Wproducts.product = new AddingProducts();
            Wproducts.products = new List<AddingProducts>();
            Wproducts.CatgN = CatgN;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM products";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        AddingProducts product = new AddingProducts();
                        product.PName = reader.GetString(0);
                        product.Intense = reader.GetString(1);
                        product.Company = reader.GetString(2);
                        product.Price = reader.GetInt32(3);
                        product.Discount = reader.GetInt32(4);
                        product.Amount = reader.GetInt32(5);
                        product.ImageURL = reader.GetString(6);
                        Wproducts.products.Add(product);
                    }
                    reader.Close();
                }
                connection.Close();
            }

            return View("Index", Wproducts);
        }

       


    }
}

