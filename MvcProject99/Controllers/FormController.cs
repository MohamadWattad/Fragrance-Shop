using Microsoft.AspNetCore.Mvc;
using MvcProject99.Models;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Text.Json;
////using Newtonsoft.Json;

namespace MvcProject99.Controllers
{
    public class FormController : Controller
    {

        List<AddingProducts> purchased = new List<AddingProducts>();
        public IConfiguration _configuration;

        public string connectionString = "";
        public FormController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("myConnection");
        }
        public IActionResult Signup()
        {
            SignupModel model = new SignupModel();
            return View("Signup", model);
        }
        public IActionResult GetDeatils(SignupModel NewUser)
        {
            //create obj
            //HomePageModel NewUser = new HomePageModel();
            //NewUser.FirstName = "Mohamad";
            //NewUser.Email = "Abo@gmail.lcom";
            //NewUser.Password = 1598888;
            if (ModelState.IsValid)
            {
                //sql
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "insert into Users values (@value1,@value2,@value3,@value4)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@value1", NewUser.Email);
                        command.Parameters.AddWithValue("@value2", NewUser.FirstName);
                        command.Parameters.AddWithValue("@value3", NewUser.LastName);
                        command.Parameters.AddWithValue("@value4", NewUser.Password);
                        int howRowEffect = command.ExecuteNonQuery();
                        if (howRowEffect > 0)
                        {
                            TempData["userName"]=NewUser.FirstName+" "+NewUser.LastName;
                            return RedirectToAction("index","HomePage");

                        }
                        else
                        {
                            return View("SignUp", NewUser);
                        }
                    }
                }
            }
            else
            {
                return View("SignUp", NewUser);
            }
        }
        public IActionResult SearchPage(Warehouse Wproducts)
        {
            Wproducts.searchList = new List<AddingProducts>();
            Wproducts.realatedList = new List<AddingProducts>();
            string Comp = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // First query to fetch search results
                string selectQuery = "SELECT * FROM products WHERE PName = @PName";
                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@PName", Wproducts.FGname);

                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
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
                            Wproducts.searchList.Add(product);
                        }
                    }

                    if (Wproducts.searchList.Count > 0)
                    {
                        Comp = Wproducts.searchList[0].Company;
                    }
                }

                // Second query to fetch related products
                if (!string.IsNullOrEmpty(Comp))
                {
                    string relatedQuery = "SELECT * FROM products WHERE Company = @Company and PName!=@PName";
                    using (SqlCommand relatedCommand = new SqlCommand(relatedQuery, connection))
                    {
                        relatedCommand.Parameters.AddWithValue("@Company", Comp);
                        relatedCommand.Parameters.AddWithValue("@PName", Wproducts.FGname);

                        using (SqlDataReader reader = relatedCommand.ExecuteReader())
                        {
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
                                Wproducts.realatedList.Add(product);
                            }
                        }
                    }
                }
            }

            return View("SearchPage", Wproducts);
        }



        public IActionResult Login()
        {
            LoginModel user = new LoginModel();

            return View("Login", user);
        }
        public IActionResult GetLogIn(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                if (user.Email.Equals("Admin1@Admin.com", StringComparison.OrdinalIgnoreCase))
                {
                    // Redirect to admin page
                    if(user.Password.Equals("Md2002", StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("Admin", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login credentials.");
                        return View("Login", user);
                    }
                   
                }
                else if(user.Email.Equals("Admin2@Admin.com", StringComparison.OrdinalIgnoreCase))
                {
                    // Redirect to admin page
                    if (user.Password.Equals("MW2002", StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("Admin", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login credentials.");
                        return View("Login", user);
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE UserID = @UserID AND Password = @Password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", user.Email);
                        command.Parameters.AddWithValue("@Password", user.Password);

                        int userCount = Convert.ToInt32(command.ExecuteScalar());

                        if (userCount > 0)
                        {
                            string fullName="";
                            query = "SELECT FirstName, LastName FROM Users WHERE UserID = @UserID";
                            using (SqlCommand nameCommand = new SqlCommand(query, connection))
                            {
                                nameCommand.Parameters.AddWithValue("@UserID", user.Email);
                                using (SqlDataReader reader = nameCommand.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        string firstName = reader["FirstName"].ToString();
                                        string lastName = reader["LastName"].ToString();
                                        fullName = $"{firstName} {lastName}";
                                    }
                                }
                            }

                            TempData["userName"] = fullName;
                            return RedirectToAction("index", "HomePage");

                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login credentials.");
                            return View("Login", user);
                        }
                    }
                }
            }
            else
            {
                return View("Login", user);
            }
        }
        public IActionResult Cart()
        {

            //warehouse.product=new AddingProducts();

            //Wproduct.Bf ="";
            //Wproduct.amount= 0;
            ItemData item = new ItemData();
            return View("Cart", purchased);
        }
        public IActionResult BuyNow(Warehouse Wproducts)
        {
            // Get the stored Warehouse object from TempData or create a new one if it doesn't exist
            Warehouse storedWarehouse = TempData.ContainsKey("StoredWarehouse") ?
                JsonSerializer.Deserialize<Warehouse>(TempData["StoredWarehouse"].ToString()) :
                new Warehouse();

            // Add the purchased product to the stored Warehouse object
            if (storedWarehouse.purchased == null)
            {
                storedWarehouse.purchased = new List<AddingProducts>();
            }
            storedWarehouse.purchased.Add(Wproducts.purchasedProduct);

            // Serialize the updated Warehouse object
            string serializedWarehouse = JsonSerializer.Serialize(storedWarehouse);

            // Store the serialized Warehouse object in TempData
            TempData["StoredWarehouse"] = serializedWarehouse;

            return RedirectToAction("Index", "HomePage");
        }


        public IActionResult PaymentForm(string items)
        {
            // Deserialize the JSON string into a list of objects
            List<Item> itemsList = JsonSerializer.Deserialize<List<Item>>(items); // Using Newtonsoft.Json
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (Item fr in itemsList)
                {
                    string query = "SELECT Amount FROM Products WHERE PName = @PName";
                    string updateQuery = "UPDATE Products SET Amount = @Amount, Counter = Counter + (@PrevAmount - @Amount) WHERE PName = @PName";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PName", fr.PName);
                        int currentAmount = Convert.ToInt32(command.ExecuteScalar());

                        if (currentAmount >= fr.Quantity)
                        {
                            int newAmount = currentAmount - fr.Quantity;
                            // Update the amount in the database
                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@Amount", newAmount);
                                updateCommand.Parameters.AddWithValue("@PrevAmount", currentAmount);
                                updateCommand.Parameters.AddWithValue("@PName", fr.PName);

                                updateCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Insufficient amount.");
                            return RedirectToAction("Index", "HomePage");
                        }
                    }
                }
            }
            return RedirectToAction("Index", "HomePage");
        }

        public ActionResult Request(string message)
        {
            Messages storedMessages = TempData.ContainsKey("messages") ?
                JsonSerializer.Deserialize<Messages>(TempData["messages"].ToString()) :
                new Messages();

            if (storedMessages.msgList == null)
            {
                storedMessages.msgList = new List<string>();
            }

            storedMessages.msgList.Add(message);
            string serializedMessages = JsonSerializer.Serialize(storedMessages);
            TempData["messages"] = serializedMessages;

            return RedirectToAction("Index", "HomePage");
        }


        public class Item
        {
            public int Quantity { get; set; }
            public string PName { get; set; }
        }



    }
}


