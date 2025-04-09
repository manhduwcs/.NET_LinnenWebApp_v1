using LinnenWebApp_v1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace LinnenWebApp_v1.Controllers
{
    public class ShipperController : Controller
    {
        List<Shipper> shippers = new List<Shipper>();
        private readonly string connectionString;

        public ShipperController(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        // GET: ShipperControllers
        public ActionResult Index()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM Shippers";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Shipper shipper = new Shipper
                            {
                                ShipperID = reader.GetInt32(0),
                                CompanyName = reader.GetString(1),
                                Phone = reader.GetString(2),
                            };
                            shippers.Add(shipper);
                        }
                    }
                }
            }
            return View(shippers);
        }

        // GET: ShipperControllers/Details/5
        public ActionResult Details(int id)
        {
            Shipper shipper = new();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM Shippers WHERE ShipperID = @ShipperID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ShipperID", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            shipper = new Shipper
                            {
                                ShipperID = reader.GetInt32(0),
                                CompanyName = reader.GetString(1),
                                Phone = reader.GetString(2),
                            };
                        }
                    }
                }
            }
            return View(shipper);
        }

        // GET: ShipperControllers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShipperControllers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                string companyName = collection["CompanyName"];
                string phone = collection["Phone"];

                if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(companyName))
                {
                    ModelState.AddModelError("", "Company Name and Phone are required.");
                    return View();
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "INSERT INTO Shippers(CompanyName, Phone) VALUES(@CompanyName, @Phone)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CompanyName", companyName);
                        cmd.Parameters.AddWithValue("@Phone", phone);

                        int affectedRows = cmd.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }

                ModelState.AddModelError("", "Failed to create shipper.");
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) for debugging purposes
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
                return View();
            }
        }

        // GET: ShipperControllers/Edit/5
        public ActionResult Edit(int id)
        {
            Shipper shipper = new();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM Shippers WHERE ShipperID = @ShipperID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ShipperID", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            shipper = new Shipper
                            {
                                ShipperID = reader.GetInt32(0),
                                CompanyName = reader.GetString(1),
                                Phone = reader.GetString(2),
                            };
                        }
                    }
                }
            }
            return View(shipper);
        }

        // POST: ShipperControllers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IFormCollection collection)
        {
            try
            {
                int shipperId = -1;
                string companyName = collection["CompanyName"];
                string phone = collection["Phone"];

                if (!int.TryParse(collection["ShipperID"], out shipperId))
                {
                    ModelState.AddModelError("", "There's an error in ShipperID. Try again later.");
                    return View();
                }
                if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(companyName))
                {
                    ModelState.AddModelError("", "Company Name and Phone are required.");
                    return View();
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "UPDATE Shippers SET CompanyName=@CompanyName, Phone=@Phone WHERE ShipperID=@ShipperID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ShipperID", shipperId);
                        cmd.Parameters.AddWithValue("@CompanyName", companyName);
                        cmd.Parameters.AddWithValue("@Phone", phone);

                        int affectedRows = cmd.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }

                ModelState.AddModelError("", "Failed to update shipper.");
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: ShipperControllers/Delete/5
        public ActionResult Delete(int id)
        {
            Shipper shipper = new();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM Shippers WHERE ShipperID = @ShipperID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ShipperID", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            shipper = new Shipper
                            {
                                ShipperID = reader.GetInt32(0),
                                CompanyName = reader.GetString(1),
                                Phone = reader.GetString(2),
                            };
                        }
                    }
                }
            }
            return View(shipper);
        }

        // POST: ShipperControllers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "DELETE Shippers WHERE ShipperID=@ShipperID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ShipperID", id);

                        int affectedRows = cmd.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
