using System.Data.SqlTypes;
using System.Data;
using LinnenWebApp_v1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace LinnenWebApp_v1.Controllers
{
    public class OrderController : Controller
    {
        List<Order> orderList = new List<Order>();
        private string connectionString;
        public OrderController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        // GET: OrderController
        public ActionResult Index()
        {
            orderList = GetAllOrders();
            orderList.Reverse();
            return View(orderList);
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            Order order = GetOrderById(id);
            return View(order);
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            ViewBag.Customers = GetCustomerChoices();
            ViewBag.Employees = GetEmployeesChoices();
            ViewBag.Shippers = GetShipperChoices();
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order, IFormCollection form)
        {
            try
            {
                Order newOrd = new Order
                {
                    CustomerID = form["CustomerID"],
                    EmployeeID = int.TryParse(form["EmployeeID"], out int employeeId) ? employeeId : -1,
                    OrderDate = DateTime.TryParse(form["OrderDate"], out DateTime orderDate) ? orderDate : DateTime.MinValue,
                    RequiredDate = DateTime.TryParse(form["RequiredDate"], out DateTime requiredDate) ? requiredDate : DateTime.MinValue,
                    ShippedDate = DateTime.TryParse(form["ShippedDate"], out DateTime shippedDate) ? shippedDate : DateTime.MinValue,
                    ShipVia = int.TryParse(form["ShipVia"], out int shipVia) ? shipVia : -1,
                    Freight = decimal.TryParse(form["Freight"], out decimal freight) ? freight : 0,
                    ShipName = form["ShipName"],
                    ShipAddress = form["ShipAddress"],
                    ShipCity = form["ShipCity"],
                    ShipRegion = form["ShipRegion"],
                    ShipPostalCode = form["ShipPostalCode"],
                    ShipCountry = form["ShipCountry"]
                };

                if (CreateOrder(newOrd))
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            ViewBag.Customers = GetCustomerChoices();
            ViewBag.Employees = GetEmployeesChoices();
            ViewBag.Shippers = GetShipperChoices();
            return View(order);
        }

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            Order order = GetOrderById(id);
            ViewBag.Customers = GetCustomerChoices();
            ViewBag.Employees = GetEmployeesChoices();
            ViewBag.Shippers = GetShipperChoices();
            return View(order);
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order, IFormCollection form)
        {
            try
            {
                Order newOrd = new Order
                {
                    OrderID = int.TryParse(form["OrderID"], out int orderId) ? orderId : -1,
                    CustomerID = form["CustomerID"],
                    EmployeeID = int.TryParse(form["EmployeeID"], out int employeeId) ? employeeId : -1,
                    OrderDate = DateTime.TryParse(form["OrderDate"], out DateTime orderDate) ? orderDate : DateTime.MinValue,
                    RequiredDate = DateTime.TryParse(form["RequiredDate"], out DateTime requiredDate) ? requiredDate : DateTime.MinValue,
                    ShippedDate = DateTime.TryParse(form["ShippedDate"], out DateTime shippedDate) ? shippedDate : DateTime.MinValue,
                    ShipVia = int.TryParse(form["ShipVia"], out int shipVia) ? shipVia : -1,
                    Freight = decimal.TryParse(form["Freight"], out decimal freight) ? freight : 0,
                    ShipName = form["ShipName"],
                    ShipAddress = form["ShipAddress"],
                    ShipCity = form["ShipCity"],
                    ShipRegion = form["ShipRegion"],
                    ShipPostalCode = form["ShipPostalCode"],
                    ShipCountry = form["ShipCountry"]
                };

                if (newOrd.OrderID == -1)
                {
                    throw new Exception("Cannot resolve OrderID of this Order !");
                }

                if (EditOrder(newOrd))
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            ViewBag.Customers = GetCustomerChoices();
            ViewBag.Employees = GetEmployeesChoices();
            ViewBag.Shippers = GetShipperChoices();
            return View(order);
        }

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            Order order = GetOrderById(id);
            return View(order);
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Order order, IFormCollection collection)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "DELETE FROM Orders WHERE OrderID = @OrderID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", order.OrderID);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View(order);
            }
            return View(order);
        }

        public List<Customer> GetCustomerChoices()
        {
            List<Customer> customers = new List<Customer>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT CustomerID, ContactName FROM Customers";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Customer cus = new Customer
                            {
                                CustomerID = reader.GetString(reader.GetOrdinal("CustomerID")),
                                ContactName = reader.GetString(reader.GetOrdinal("ContactName")),
                            };
                            customers.Add(cus);
                        }
                    }
                }
            }
            return customers;
        }

        public List<Employee> GetEmployeesChoices()
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT EmployeeID, FirstName, LastName FROM Employees";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee emp = new Employee
                            {
                                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            };
                            emp.FullName = emp.FirstName + " " + emp.LastName;
                            employees.Add(emp);
                        }
                    }
                }
            }
            return employees;
        }


        public List<Shipper> GetShipperChoices()
        {
            List<Shipper> shippers = new List<Shipper>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT ShipperID, CompanyName FROM Shippers";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Shipper shipper = new Shipper
                            {
                                ShipperID = reader.GetInt32(reader.GetOrdinal("ShipperID")),
                                CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                            };
                            shippers.Add(shipper);
                        }
                    }
                }
            }
            return shippers;
        }

        public bool EditOrder(Order order)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = $@"UPDATE Orders SET
                        CustomerID = @CustomerID,
                        EmployeeID = @EmployeeID,
                        OrderDate = @OrderDate,
                        RequiredDate = @RequiredDate,
                        ShippedDate = @ShippedDate,
                        ShipVia = @ShipVia,
                        Freight = @Freight,
                        ShipName = @ShipName,
                        ShipAddress = @ShipAddress,
                        ShipCity = @ShipCity,
                        ShipRegion = @ShipRegion,
                        ShipPostalCode = @ShipPostalCode,
                        ShipCountry = @ShipCountry
                    WHERE OrderID = @OrderID;
                    ";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", order.OrderID);
                        cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                        cmd.Parameters.AddWithValue("@EmployeeID", order.EmployeeID);
                        cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                        cmd.Parameters.AddWithValue("@RequiredDate", order.RequiredDate);
                        cmd.Parameters.AddWithValue("@ShippedDate", order.ShippedDate);
                        cmd.Parameters.AddWithValue("@ShipVia", order.ShipVia);
                        cmd.Parameters.AddWithValue("@Freight", order.Freight);
                        cmd.Parameters.AddWithValue("@ShipName", order.ShipName);
                        cmd.Parameters.AddWithValue("@ShipAddress", order.ShipAddress);
                        cmd.Parameters.AddWithValue("@ShipCity", order.ShipCity);
                        cmd.Parameters.AddWithValue("@ShipRegion", order.ShipRegion);
                        cmd.Parameters.AddWithValue("@ShipPostalCode", order.ShipPostalCode);
                        cmd.Parameters.AddWithValue("@ShipCountry", order.ShipCountry);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        public bool CreateOrder(Order order)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = $@"INSERT INTO Orders (
                        CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate,
                        ShipVia, Freight, ShipName, ShipAddress, ShipCity,
                        ShipRegion, ShipPostalCode, ShipCountry
                    ) VALUES (
                        @CustomerID, @EmployeeID, @OrderDate, @RequiredDate, @ShippedDate,
                        @ShipVia, @Freight, @ShipName, @ShipAddress, @ShipCity,
                        @ShipRegion, @ShipPostalCode, @ShipCountry
                    );
                    ";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", order.OrderID);
                        cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                        cmd.Parameters.AddWithValue("@EmployeeID", order.EmployeeID);
                        cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                        cmd.Parameters.AddWithValue("@RequiredDate", order.RequiredDate);
                        cmd.Parameters.AddWithValue("@ShippedDate", order.ShippedDate);
                        cmd.Parameters.AddWithValue("@ShipVia", order.ShipVia);
                        cmd.Parameters.AddWithValue("@Freight", order.Freight);
                        cmd.Parameters.AddWithValue("@ShipName", order.ShipName);
                        cmd.Parameters.AddWithValue("@ShipAddress", order.ShipAddress);
                        cmd.Parameters.AddWithValue("@ShipCity", order.ShipCity);
                        cmd.Parameters.AddWithValue("@ShipRegion", order.ShipRegion);
                        cmd.Parameters.AddWithValue("@ShipPostalCode", order.ShipPostalCode);
                        cmd.Parameters.AddWithValue("@ShipCountry", order.ShipCountry);

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
                Console.WriteLine(e);
            }
            return false;
        }

        public Order GetOrderById(int id)
        {
            Order order = new();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = $@"SELECT dbo.Orders.*, 
                    dbo.Shippers.CompanyName as ShipperCompanyName, 
                    dbo.Employees.EmployeeID, 
                    dbo.Employees.LastName as EmployeeLastName, 
                    dbo.Employees.FirstName as EmployeeFirstName, 
                    dbo.Customers.CustomerID, 
                    dbo.Customers.CompanyName AS CustomerCompanyName 
                    FROM dbo.Customers
                    JOIN dbo.Orders ON dbo.Customers.CustomerID = dbo.Orders.CustomerID 
                    JOIN dbo.Employees ON dbo.Orders.EmployeeID = dbo.Employees.EmployeeID 
                    JOIN dbo.Shippers ON dbo.Orders.ShipVia = dbo.Shippers.ShipperID
                    WHERE OrderID = @OrderID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrderID", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            order = new Order
                            {
                                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                CustomerID = reader.GetString(reader.GetOrdinal("CustomerID")),
                                CustomerCompanyName = reader.IsDBNull(reader.GetOrdinal("CustomerCompanyName"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("CustomerCompanyName")),
                                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                EmployeeFirstName = reader.IsDBNull(reader.GetOrdinal("EmployeeFirstName"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("EmployeeFirstName")),
                                EmployeeLastName = reader.IsDBNull(reader.GetOrdinal("EmployeeLastName"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("EmployeeLastName")),
                                OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                RequiredDate = reader.IsDBNull(reader.GetOrdinal("RequiredDate"))
                                ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("RequiredDate")),
                                ShippedDate = reader.IsDBNull(reader.GetOrdinal("ShippedDate"))
                                ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("ShippedDate")),
                                ShipVia = reader.GetInt32(reader.GetOrdinal("ShipVia")),
                                ShipperCompanyName = reader.IsDBNull(reader.GetOrdinal("ShipperCompanyName"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ShipperCompanyName")),
                                Freight = reader.GetDecimal(reader.GetOrdinal("Freight")),
                                ShipName = reader.IsDBNull(reader.GetOrdinal("ShipName"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ShipName")),
                                ShipAddress = reader.IsDBNull(reader.GetOrdinal("ShipAddress"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ShipAddress")),
                                ShipCity = reader.IsDBNull(reader.GetOrdinal("ShipCity"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ShipCity")),
                                ShipRegion = reader.IsDBNull(reader.GetOrdinal("ShipRegion"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ShipRegion")),
                                ShipPostalCode = reader.IsDBNull(reader.GetOrdinal("ShipPostalCode"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ShipPostalCode")),
                                ShipCountry = reader.IsDBNull(reader.GetOrdinal("ShipCountry"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ShipCountry"))
                            };
                            order.EmployeeFullName = order.EmployeeFirstName + " " + order.EmployeeLastName;
                        }
                    }
                }
            }
            return order;
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = $@"SELECT dbo.Orders.*, 
                  dbo.Shippers.CompanyName as ShipperCompanyName, 
                  dbo.Employees.EmployeeID, 
                  dbo.Employees.LastName as EmployeeLastName, 
                  dbo.Employees.FirstName as EmployeeFirstName, 
                  dbo.Customers.CustomerID, 
                  dbo.Customers.CompanyName AS CustomerCompanyName 
                  FROM dbo.Customers 
                  INNER JOIN dbo.Orders ON dbo.Customers.CustomerID = dbo.Orders.CustomerID 
                  INNER JOIN dbo.Employees ON dbo.Orders.EmployeeID = dbo.Employees.EmployeeID 
                  INNER JOIN dbo.Shippers ON dbo.Orders.ShipVia = dbo.Shippers.ShipperID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order
                            {
                                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                CustomerID = reader.GetString(reader.GetOrdinal("CustomerID")),
                                CustomerCompanyName = reader.IsDBNull(reader.GetOrdinal("CustomerCompanyName"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("CustomerCompanyName")),
                                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                EmployeeFirstName = reader.IsDBNull(reader.GetOrdinal("EmployeeFirstName"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("EmployeeFirstName")),
                                EmployeeLastName = reader.IsDBNull(reader.GetOrdinal("EmployeeLastName"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("EmployeeLastName")),
                                OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                RequiredDate = reader.IsDBNull(reader.GetOrdinal("RequiredDate"))
                                ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("RequiredDate")),
                                ShippedDate = reader.IsDBNull(reader.GetOrdinal("ShippedDate"))
                                ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("ShippedDate")),
                                ShipVia = reader.GetInt32(reader.GetOrdinal("ShipVia")),
                                ShipperCompanyName = reader.IsDBNull(reader.GetOrdinal("ShipperCompanyName"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ShipperCompanyName")),
                                Freight = reader.GetDecimal(reader.GetOrdinal("Freight")),
                                ShipName = reader.IsDBNull(reader.GetOrdinal("ShipName"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ShipName")),
                                ShipAddress = reader.IsDBNull(reader.GetOrdinal("ShipAddress"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ShipAddress")),
                                ShipCity = reader.IsDBNull(reader.GetOrdinal("ShipCity"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ShipCity")),
                                ShipRegion = reader.IsDBNull(reader.GetOrdinal("ShipRegion"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ShipRegion")),
                                ShipPostalCode = reader.IsDBNull(reader.GetOrdinal("ShipPostalCode"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ShipPostalCode")),
                                ShipCountry = reader.IsDBNull(reader.GetOrdinal("ShipCountry"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ShipCountry"))
                            };
                            order.EmployeeFullName = order.EmployeeFirstName + " " + order.EmployeeLastName;
                            orders.Add(order);
                        }
                    }
                }
            }
            return orders;
        }
    }
}
