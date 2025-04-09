using System.Net;
using System.Data;
using System.Diagnostics;
using LinnenWebApp_v1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text;

namespace LinnenWebApp_v1.Controllers
{
    public class UserController : Controller
    {
        // GET: UserController
        // Connection String : appsetting.json
        List<User> userList = new List<User>();
        private readonly string connectionString;
        public static User loggedInUser = new();

        public UserController(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(IFormCollection form)
        {
            if (loggedInUser.UserID != -1)
            {
                ModelState.AddModelError("Username", "You must logout the current account before logging in another account !");
                return RedirectToAction("Login");
            }
            try
            {
                string username = form["Username"];
                string password = form["Password"];

                Debug.WriteLine($"-- Login. Username = {username} ; Password = {password}");

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("Username", "Username & Password must not be empty !");
                    return View();
                }

                User loginUser = GetUserByUsername(username);

                if (loginUser.UserID == -1)
                {
                    ModelState.AddModelError("Username", "We cannot find any User with this Username. Try again !");
                    return View();
                }

                if (password == loginUser.Password)
                {
                    loggedInUser = loginUser;
                    Debug.WriteLine($"Logged In User : {loggedInUser.UserID} - {loggedInUser.Username}");
                    TempData["loginMessage"] = $"User {username} logged in successfully !";

                    string encodedUsernameValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(username));
                    HttpContext.Response.Cookies.Append("Username", encodedUsernameValue);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Password", "Incorrect username or password. Try again !");
                    return View();
                }
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e);
            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(IFormCollection form)
        {
            if (loggedInUser.UserID != -1)
            {
                ModelState.AddModelError("Username", "You must logout the current account before registering new user !");
                return RedirectToAction("Register");
            }
            try
            {
                string username = form["Username"];
                string description = form["Description"];
                string password = form["Password"];
                string rePassword = form["RePassword"];

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("Username", "Fields cannot be empty !");
                    return View();
                }

                if (!password.Equals(rePassword))
                {
                    ModelState.AddModelError("Password", "Passwords not match ! Try again.");
                    return View();
                }

                User user = new User
                {
                    Username = username,
                    Description = description,
                    Password = password,
                };

                bool success = CreateUser(user);
                if (success)
                {
                    loggedInUser = GetUserByUsername(user.Username);
                    TempData["loginMessage"] = $"User {username} logged in successfully !";

                    string encodedUsername = Convert.ToBase64String(Encoding.UTF8.GetBytes(username));
                    HttpContext.Response.Cookies.Append("Username", encodedUsername);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e);
            }
            return View();
        }

        public IActionResult Logout()
        {
            if (loggedInUser.UserID != -1)
            {
                HttpContext.Response.Cookies.Delete("Username");
                loggedInUser = new();
                return RedirectToAction("Login");
            }
            return RedirectToAction("Login");
        }
        public ActionResult Index()
        {
            userList = GetAllUsers();
            userList.Reverse();
            return View(userList);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            User user = new();
            user = GetUserById(id);
            return View(user);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                User user = new User
                {
                    Username = collection["Username"],
                    Password = collection["Password"],
                    Description = collection["Description"],
                };

                bool success = CreateUser(user);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return View();
            }
            return View();
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            User user = new();
            user = GetUserById(id);
            return View(user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IFormCollection collection)
        {
            User user = new();
            try
            {
                int id = 0;

                if (!int.TryParse(collection["UserID"], out id))
                {
                    ModelState.AddModelError("", "An unexpected error happened. Try again later!");
                    return RedirectToAction("Edit");
                }

                user = new User
                {
                    UserID = id,
                    Username = collection["Username"],
                    Password = collection["Password"],
                    Description = collection["Description"],
                    // Skip Employee ID
                };

                bool success = EditUser(user);

                if (success)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                // If an error occurs, redirect to the Edit action with the UserID
                Debug.WriteLine(e);
            }

            ModelState.AddModelError("Username", "An unexpected error happened. Try again later!");
            return RedirectToAction("Edit");
        }
        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            User user = new();
            user = GetUserById(id);
            return View(user);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(IFormCollection collection)
        {
            try
            {
                int id = 0;
                if (!int.TryParse(collection["UserID"], out id))
                {
                    ModelState.AddModelError("", "An unexpected error happen. Try again later !");
                    return View();
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "DELETE FROM Users WHERE UserID=@UserID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserID", id);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }
            catch
            {
                return View();
            }
            return View();
        }

        public bool EditUser(User user)
        {
            try
            {
                string query = "UPDATE Users SET Username=@Username,Password=@Password,Description=@Description WHERE UserID=@UserID";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    // string query2 = $"UPDATE dbo.Users SET Username='{user.Username}', Password='{user.Password}', Description='{user.Description}' WHERE UserID={user.UserID}";
                    Debug.WriteLine($"Edit Query = {query}");
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Username", user.Username);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.Parameters.AddWithValue("@Description", user.Description);
                        cmd.Parameters.AddWithValue("@UserID", user.UserID);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            ModelState.AddModelError("", "An unexpected error happen. Try again later !");
            return false;
        }

        public bool CreateUser(User user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "INSERT INTO dbo.Users(Username,Password,Description) VALUES(@Username,@Password,@Description)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Username", user.Username);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.Parameters.AddWithValue("@Description", user.Description);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else return false;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return false;
        }

        public User GetUserById(int userID)
        {
            User user = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "SELECT * FROM dbo.Users WHERE UserID=@UserID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user = new User
                                {
                                    UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                    Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? "" : reader.GetString(reader.GetOrdinal("Username")),
                                    Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? "" : reader.GetString(reader.GetOrdinal("Password")),
                                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString(reader.GetOrdinal("Description")),
                                    EmployeeID = reader.IsDBNull(reader.GetOrdinal("EmployeeID")) ? -1 : reader.GetInt32(reader.GetOrdinal("EmployeeID"))
                                };
                                return user;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return user;
        }

        public User GetUserByUsername(string username)
        {
            User user = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "SELECT * FROM dbo.Users WHERE Username=@Username";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user = new User
                                {
                                    UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                    Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? "" : reader.GetString(reader.GetOrdinal("Username")),
                                    Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? "" : reader.GetString(reader.GetOrdinal("Password")),
                                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString(reader.GetOrdinal("Description")),
                                    EmployeeID = reader.IsDBNull(reader.GetOrdinal("EmployeeID")) ? -1 : reader.GetInt32(reader.GetOrdinal("EmployeeID"))
                                };

                                Debug.WriteLine($"GetUserByUsername - UserID {user.UserID} - Username - {user.Username}");
                                return user;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return user;
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "SELECT * FROM dbo.Users";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                    Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? "" : reader.GetString(reader.GetOrdinal("Username")),
                                    Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? "" : reader.GetString(reader.GetOrdinal("Password")),
                                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString(reader.GetOrdinal("Description")),
                                    EmployeeID = reader.IsDBNull(reader.GetOrdinal("EmployeeID")) ? -1 : reader.GetInt32(reader.GetOrdinal("EmployeeID"))
                                };
                                users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return users;
        }
    }
}
