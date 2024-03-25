﻿using Microsoft.AspNetCore.Mvc;
using MvcProject99.Models;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;

namespace MvcProject99.Controllers
{
    public class FormController : Controller
    {

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
                    string query = "insert into SignUp values (@value1,@value2,@value3)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@value1", NewUser.FirstName);
                        command.Parameters.AddWithValue("@value2", NewUser.Email);
                        command.Parameters.AddWithValue("@value3", NewUser.Password);
                        int howRowEffect = command.ExecuteNonQuery();
                        if (howRowEffect > 0)
                        {
                            return View("SignUp-Succ", NewUser);

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
                if (user.Email.Equals("Admin@gmail.com", StringComparison.OrdinalIgnoreCase))
                {
                    // Redirect to admin page
                    return RedirectToAction("Admin", "Admin");
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM SignUp WHERE Email = @Email AND Password = @Password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Password", user.Password);

                        int userCount = Convert.ToInt32(command.ExecuteScalar());

                        if (userCount > 0)
                        {
                            // Successful login
                            //return View("make1",user); // Redirect to dashboard or home page
                            return RedirectToAction("Index", "HomePage");


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
        
    }




}


