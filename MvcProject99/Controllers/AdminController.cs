using Microsoft.AspNetCore.Mvc;
using MvcProject99.Models;
using System.Data;
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
            //AddingProducts product = new AddingProducts();
            Warehouse warehouse = new Warehouse();
            warehouse.product = new AddingProducts();
            warehouse.products = new List<AddingProducts>(); 
            return View("Admin", warehouse);

            //return View();
        }
        public IActionResult DiscountForm()
        {
            Warehouse warehouse = new Warehouse();
            warehouse.product = new AddingProducts();
            warehouse.products = new List<AddingProducts>();
            return View("DiscountForm", warehouse);
        }

        public IActionResult Discount(Warehouse Ndiscount)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT Discount FROM Products WHERE PName=@PName";
                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@PName", Ndiscount.product.PName);
                    object currentDiscount = selectCommand.ExecuteScalar();

                    // Check if the currentDiscount is not null before updating
                    if (currentDiscount != null)
                    {
                        // Update the discount in the database
                        string updateQuery = "UPDATE Products SET Discount=@Discount WHERE PName=@PName";
                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@PName", Ndiscount.product.PName);
                            updateCommand.Parameters.AddWithValue("@Discount", Ndiscount.product.Discount);
                            int rowsAffected = updateCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Return success view or redirect
                                return View("Admin", Ndiscount);
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "No rows updated. Product name not found.";
                                return View("Admin", Ndiscount);
                            }
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Product not found.";
                        return View("Admin", Ndiscount);
                    }
                }
            }
        }

        public IActionResult AddingForm()
        {
            //AddingProducts product= new AddingProducts();
            Warehouse warehouse = new Warehouse();
            warehouse.product = new AddingProducts();
            warehouse.products = new List<AddingProducts>();
            return View("AddingForm",warehouse.product);
        }
        public IActionResult DeletingForm()
        {
            //AddingProducts product = new AddingProducts();
            Warehouse warehouse = new Warehouse();
            warehouse.product = new AddingProducts();
            warehouse.products = new List<AddingProducts>();
            return View("DeletingForm", warehouse.product);
        }
        public IActionResult AfterDeleting(Warehouse Wproduct) 
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Products WHERE PName=@PName";
                using (SqlCommand deletecommand = new SqlCommand(query, connection))
                {
                    deletecommand.Parameters.AddWithValue("@PName", Wproduct.product.PName);
                    int howRowEffect = deletecommand.ExecuteNonQuery();
                    if (howRowEffect > 0)
                    {
                        return View("Admin", Wproduct);

                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No rows deleted. Fragrance name not found.";
                        return View("Admin", Wproduct);
                    }
                }
            }
        }

        public IActionResult ProductForm(Warehouse Wproduct)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Check if the combination of PName and Intense already exists
                    string selectQuery = "SELECT COUNT(*) FROM Products WHERE PName = @PName AND Intense = @Intense";
                    using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@PName", Wproduct.product.PName);
                        selectCommand.Parameters.AddWithValue("@Intense", Wproduct.product.Intense);
                        int existingCount = (int)selectCommand.ExecuteScalar();

                        if (existingCount > 0)
                        {
                            // If the combination already exists, increment the amount
                            string updateQuery = "UPDATE Products SET Amount = Amount + 1 WHERE PName = @PName AND Intense = @Intense";
                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@PName", Wproduct.product.PName);
                                updateCommand.Parameters.AddWithValue("@Intense", Wproduct.product.Intense);
                                int rowsAffected = updateCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Return success view or redirect
                                    return RedirectToAction("Admin");
                                }
                                else
                                {
                                    // Handle case where no rows were affected
                                    ViewBag.ErrorMessage = "No rows updated.";
                                    return View("Admin", Wproduct);
                                }
                            }
                        }
                        else
                        {
                            // If the combination doesn't exist, insert a new row
                            string insertQuery = "INSERT INTO Products VALUES (@PName, @Intense, @Company, @Price, @Discount, @Amount,@ImageURL)";
                            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@PName", Wproduct.product.PName);
                                insertCommand.Parameters.AddWithValue("@Intense", Wproduct.product.Intense);
                                insertCommand.Parameters.AddWithValue("@Company", Wproduct.product.Company);
                                insertCommand.Parameters.AddWithValue("@Price", Wproduct.product.Price);
                                insertCommand.Parameters.AddWithValue("@Discount", Wproduct.product.Discount);
                                insertCommand.Parameters.AddWithValue("@Amount", 1); // Set Amount to 1 for new entry
                                insertCommand.Parameters.AddWithValue("@ImageURL",Wproduct.product.ImageURL);
                                int rowsAffected = insertCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Return success view or redirect
                                    return RedirectToAction("Admin");
                                }
                                else
                                {
                                    // Handle case where no rows were affected
                                    ViewBag.ErrorMessage = "No rows inserted.";
                                    return View("Admin", Wproduct);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during database operation
                ViewBag.ErrorMessage = "Error occurred: " + ex.Message;
                return View("Admin", Wproduct);
            }
        }

        public IActionResult Warehouse() {
            Warehouse warehouse = new Warehouse();
            warehouse.product = new AddingProducts();
            warehouse.products = new List<AddingProducts>();
            return View("Warehouse",warehouse);
        }
        public IActionResult GetWarehouse()
        {
            Warehouse Wproducts = new Warehouse();
            Wproducts.product = new AddingProducts();

            // Initialize the products list to avoid null reference exception
            Wproducts.products = new List<AddingProducts>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "select * from products";
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
                        //product.NewPrice = reader.GetInt32(6);
                        Wproducts.products.Add(product);
                    }
                    reader.Close();
                }
                connection.Close();
            }

            return View("Admin", Wproducts);
        }

    }


}
    

