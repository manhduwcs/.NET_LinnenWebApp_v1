using System.Reflection.Metadata;
using System.Data;
using System.Diagnostics;
using LinnenWebApp_v1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace LinnenWebApp_v1.Controllers
{
    public class TerritoryController : Controller
    {
        // GET: TerritoryController
        public List<Territory> territoryList;
        public static Dictionary<string,int> regionDict;
        private static string connectionString;
        public TerritoryController(IConfiguration configuration)
        {
            regionDict = new Dictionary<string, int>();
            territoryList = new List<Territory>();
            connectionString = configuration.GetConnectionString("DefaultConnection");

            // Call this method to add all region choices to regionDict
            // Consider require calling this method before Create / Edit because 
            // the region choices may be modified.

            // Each time a HTTP Request is sent, .NET Core create a new instance of this 
            // TerritoryController to handle that request. 
            // So that, we have multiple regionDict instance. We have to re-assign it for each request. This can be solved by making the regionDict "static" and assign it in the constructor ( this can be applied within the constructor only )
            GetRegionChoices();
        }
        public ActionResult Index()
        {
            territoryList = GetAllTerritories();
            // Call this to make sure we got the new Region values after Region modifying
            GetRegionChoices();
            territoryList.Reverse();
            return View(territoryList);
        }

        // GET: TerritoryController/Details/5
        public ActionResult Details(string id)
        {
            Territory ter = GetTerritoryByID(id);
            return View(ter);
        }

        // GET: TerritoryController/Create
        public ActionResult Create()
        {
            // Call it each time we Create / Edit
            TempData["RegionChoices"] = regionDict.Keys.ToList(); 
            return View();
        }

        // POST: TerritoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection form)
        {
            try
            {
                string territoryID = form["TerritoryID"];
                string territoryDescription = form["TerritoryDescription"];
                string regionDescription = form["RegionDescription"];

                Debug.WriteLine($"Territory ID: {territoryID}");
                Debug.WriteLine($"Territory Description: {territoryDescription}");
                Debug.WriteLine($"Region Description: {regionDescription}");

                if (string.IsNullOrEmpty(territoryID) || string.IsNullOrEmpty(territoryDescription) || string.IsNullOrEmpty(regionDescription))
                {
                    ModelState.AddModelError("TerritoryID", "Fields must not be empty !");
                    TempData["RegionChoices"] = regionDict.Keys.ToList();
                    return View();
                }

                // when creating this instance => RegionID still remains -1
                Territory ter = new Territory
                {
                    TerritoryID = territoryID,
                    TerritoryDescription = territoryDescription,
                    RegionDescription = regionDescription,
                };

                if (CheckDuplicateTerritoryID(territoryID))
                {
                    ModelState.AddModelError("TerritoryID", "This TerritoryID has already exist !");
                    TempData["RegionChoices"] = regionDict.Keys.ToList();
                    return View(ter);
                }

                int regionId = -1;
                if (!regionDict.TryGetValue(regionDescription, out regionId))
                {
                    ModelState.AddModelError("RegionDescription", "Cannot find this Region Description choice !");
                    return RedirectToAction("Create");
                }

                ter.RegionID = regionId;
                if (CreateTerritory(ter))
                {
                    return RedirectToAction("Index");
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }
            TempData["RegionChoices"] = regionDict.Keys.ToList();
            return View();
        }

        // GET: TerritoryController/Edit/5
        public ActionResult Edit(string id)
        {
            GetRegionChoices();
            Territory ter = GetTerritoryByID(id);
            TempData["RegionChoices"] = regionDict.Keys.ToList();
            return View(ter);
        }

        // POST: TerritoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IFormCollection form)
        {
            try
            {
                string territoryID = form["TerritoryID"];
                string territoryDescription = form["TerritoryDescription"];
                string regionDescription = form["RegionDescription"];

                Debug.WriteLine($"Territory ID: {territoryID}");
                Debug.WriteLine($"Territory Description: {territoryDescription}");
                Debug.WriteLine($"Region Description: {regionDescription}");

                if (string.IsNullOrEmpty(territoryID) || string.IsNullOrEmpty(territoryDescription) || string.IsNullOrEmpty(regionDescription))
                {
                    ModelState.AddModelError("TerritoryID", "Fields must not be empty !");
                    TempData["RegionChoices"] = regionDict.Keys.ToList();
                    return RedirectToAction("Edit");
                }

                // when creating this instance => RegionID still remains -1
                Territory ter = new Territory
                {
                    TerritoryID = territoryID,
                    TerritoryDescription = territoryDescription,
                    RegionDescription = regionDescription,
                };

                Territory currentTerritory = GetTerritoryByID(territoryID);

                if (CheckDuplicateTerritoryID(territoryID) && currentTerritory.TerritoryID != territoryID)
                {
                    ModelState.AddModelError("TerritoryID", "This TerritoryID has already exist !");
                    TempData["RegionChoices"] = regionDict.Keys.ToList();
                    return RedirectToAction("Edit");
                }

                int regionId = -1;
                if (!regionDict.TryGetValue(regionDescription, out regionId))
                {
                    ModelState.AddModelError("RegionDescription", "Cannot find this Region Description choice !");
                    return RedirectToAction("Edit");
                }

                ter.RegionID = regionId;
                if (EditTerritory(ter))
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            TempData["RegionChoices"] = regionDict.Keys.ToList();
            return RedirectToAction("Edit");
        }

        // GET: TerritoryController/Delete/5
        public ActionResult Delete(string id)
        {
            Territory ter = GetTerritoryByID(id);
            return View(ter);
        }

        // POST: TerritoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(IFormCollection form)
        {
            try
            {
                string terID = form["TerritoryID"];
                if (string.IsNullOrEmpty(terID))
                {
                    return RedirectToAction("Delete");
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "DELETE FROM Territories WHERE TerritoryID=@TerritoryID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@TerritoryID", terID);
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            return RedirectToAction(nameof(Index));

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return RedirectToAction("Delete");
        }

        public bool CheckDuplicateTerritoryID(string terID)
        {
            return GetAllTerritoryIDs().Contains(terID);
        }

        public List<string> GetAllTerritoryIDs()
        {
            List<string> territoryIDs = new List<string>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT TerritoryID FROM dbo.Territories";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            territoryIDs.Add(reader.GetString(reader.GetOrdinal("TerritoryID")));
                        }
                    }
                }
            }
            return territoryIDs;
        }

        public bool CreateTerritory(Territory territory)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "INSERT INTO Territories (TerritoryID, TerritoryDescription, RegionID) VALUES (@TerritoryID, @TerritoryDescription, @RegionID)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@TerritoryID", territory.TerritoryID);
                        cmd.Parameters.AddWithValue("@TerritoryDescription", territory.TerritoryDescription);
                        cmd.Parameters.AddWithValue("@RegionID", territory.RegionID);

                        Debug.WriteLine($"Create Territory Query : {query}");
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e);
            }
            return false;
        }

        public bool EditTerritory(Territory territory)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "UPDATE Territories SET TerritoryDescription=@TerritoryDescription, RegionID=@RegionID WHERE TerritoryID=@TerritoryID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@TerritoryID", territory.TerritoryID);
                        cmd.Parameters.AddWithValue("@TerritoryDescription", territory.TerritoryDescription);
                        cmd.Parameters.AddWithValue("@RegionID", territory.RegionID);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e);
            }
            return false;
        }

        public static void GetRegionChoices()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM dbo.Region;";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {        
                            string regionDescription = reader.GetString(1);
                            int regionID = reader.GetInt32(0);       
                            regionDict.TryAdd(regionDescription, regionID);
                        }
                    }
                }
            }
        }

        public Territory GetTerritoryByID(string terID)
        {
            Territory ter = new();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "select ter.TerritoryID, ter.TerritoryDescription, ter.RegionID, re.RegionDescription from dbo.Territories ter join dbo.Region re on ter.RegionID = re.RegionID WHERE TerritoryID=@TerritoryID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TerritoryID", terID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ter = new Territory
                            {
                                TerritoryID = reader.GetString(reader.GetOrdinal("TerritoryID")),
                                TerritoryDescription = reader.GetString(reader.GetOrdinal("TerritoryDescription")),
                                RegionID = reader.GetInt32(reader.GetOrdinal("RegionID")),
                                RegionDescription = reader.GetString(reader.GetOrdinal("RegionDescription")),
                            };
                        }
                    }
                }
            }
            return ter;
        }

        public List<Territory> GetAllTerritories()
        {
            List<Territory> territories = new List<Territory>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "select ter.TerritoryID, ter.TerritoryDescription, ter.RegionID, re.RegionDescription from dbo.Territories ter join dbo.Region re on ter.RegionID = re.RegionID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Territory ter = new Territory
                            {
                                TerritoryID = reader.GetString(reader.GetOrdinal("TerritoryID")),
                                TerritoryDescription = reader.GetString(reader.GetOrdinal("TerritoryDescription")),
                                RegionID = reader.GetInt32(reader.GetOrdinal("RegionID")),
                                RegionDescription = reader.GetString(reader.GetOrdinal("RegionDescription")),
                            };
                            territories.Add(ter);
                        }
                    }
                }
            }
            return territories;
        }
    }
}
