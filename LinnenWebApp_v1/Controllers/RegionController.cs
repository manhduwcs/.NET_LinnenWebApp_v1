using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LinnenWebApp_v1.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;
using System.Diagnostics;

namespace LinnenWebApp_v1.Controllers
{
    public class RegionController : Controller
    {
        // GET: RegionController
        public List<Region> regions = new List<Region>();
        private readonly string connectionString = "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";
        public ActionResult Index()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM Region";
                using (SqlCommand command = new SqlCommand(query,con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Region region = new Region
                            {
                                RegionID = reader.GetInt32(0),
                                RegionDescription = reader.GetString(1),
                            };
                            regions.Add(region);
                        }
                    }   
                }
            }
            return View(regions);
        }

        // GET: RegionController/Details/5
        public ActionResult Details(int id)
        {
            Region region = new();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM Region WHERE RegionID = @RegionID";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@RegionID", id);
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            region = new Region
                            {
                                RegionID = reader.GetInt32(0),
                                RegionDescription = reader.GetString(1),
                            };
                            regions.Add(region);
                        }
                    }
                }
            }
            return View(region);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: RegionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RegionController/Edit/5
        public ActionResult Edit(int id)
        {
            Region region = new();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                String query = "SELECT * FROM Region WHERE RegionID = @RegionID";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@RegionID", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            region = new Region
                            {
                                RegionID = reader.GetInt32(0),
                                RegionDescription = reader.GetString(1),
                            };
                        }
                    }
                }
            }
            return View(region);
        }

        // POST: RegionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IFormCollection collection)
        {
            Region region = new();
            try
            {
                string regionDescription = collection["RegionDescription"];
                regionDescription = regionDescription.Trim();
                Debug.WriteLine($"Region Desc : {regionDescription}");

                if (!int.TryParse(collection["RegionID"], out int id))
                {
                    return View(collection);
                }

                region = Find(id);
                if (region == null)
                {
                    return View(collection);
                }
                
                region.RegionDescription = regionDescription;

                string query = "UPDATE Region SET RegionDescription = @RegionDescription WHERE RegionID = @RegionID";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@RegionDescription", regionDescription);
                        cmd.Parameters.AddWithValue("@RegionID", id);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }

                return View(region);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while updating the region.");
                return View(region);
            }
        }

        public ActionResult ShowDelete(int id)
        {
            Region region = new();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM Region WHERE RegionID = @RegionID";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@RegionID", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            region = new Region
                            {
                                RegionID = reader.GetInt32(0),
                                RegionDescription = reader.GetString(1),
                            };
                            regions.Add(region);
                        }
                    }
                }
            }
            return View(region);
        }

        // GET: RegionController/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Region region = new();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                String query = "DELETE FROM Region WHERE RegionID = @RegionID";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@RegionID", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            region = new Region
                            {
                                RegionID = reader.GetInt32(0),
                                RegionDescription = reader.GetString(1),
                            };
                        }
                    }
                }
            }
            return View(region);
        }

        public Region Find(int id)
        {
            foreach (var region in regions)
            {
                if (region.RegionID == id)
                {
                    return region;
                }
            }

            return new Region(-1, "");
        }

        // POST: RegionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(IFormCollection collection)
        {
            
            try
            {
                int id = -1;
                if (!int.TryParse(collection["RegionID"], out id))
                {
                    return View();
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "DELETE FROM Region WHERE RegionID = @RegionID";
                    using(SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@RegionID", id);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if(rowsAffected > 0)
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
