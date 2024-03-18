using Microsoft.AspNetCore.Mvc;
using MvcProject99.Models;
using System.Data.SqlClient;

namespace MvcProject99.Controllers
{
    public class AdminController : Controller
    {
        public IConfiguration _configuration;

        public string connectionString = "";
        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("myConnection");
        }
        public IActionResult Admin()
        {
            AddingProducts product = new AddingProducts();
            return View("Admin", product);

            //return View();
        }
        public IActionResult AddingForm()
        {
            AddingProducts product= new AddingProducts();
            return View("AddingForm",product);
        }
        public IActionResult ProductForm(AddingProducts product)
        {
            
            
                //sql
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "insert into Products values (@value1,@value2,@value3,@value4)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@value1", product.PName);
                        command.Parameters.AddWithValue("@value2", product.Intense);
                        command.Parameters.AddWithValue("@value3", product.Company);
                        command.Parameters.AddWithValue("@value4", product.Price);

                        int howRowEffect = command.ExecuteNonQuery();
                        if (howRowEffect > 0)
                        {
                            return View("Admin", product);

                        }
                        else
                        {
                            return View("Admin",product);
                        }
                    }
                }
            }
          
        }
    }
    

