using LinnenWebApp_v1.Models;
using Microsoft.AspNetCore.Mvc;

namespace LinnenWebApp_v1.Controllers
{
    public class EmployeeController : Controller
    {
        private List<Employee> employees = new List<Employee>();
        public IActionResult Index()
        {
            employees.Add(new Employee { EmployeeID = 1, FirstName = "John", LastName = "Doe", Title = "Software Engineer", TitleOfCourtesy = "Mr.", BirthDate = new DateTime(1990, 1, 1), HireDate = new DateTime(2015, 6, 15), Address = "123 Main St", City = "New York", Region = "NY", PostalCode = "10001", Country = "USA", HomePhone = "555-1234", Extension = "101", Notes = "Excellent performance.", PhotoPath = "/images/johndoe.jpg", ReportsTo = null });
            employees.Add(new Employee { EmployeeID = 2, FirstName = "Jane", LastName = "Smith", Title = "Project Manager", TitleOfCourtesy = "Ms.", BirthDate = new DateTime(1985, 5, 20), HireDate = new DateTime(2018, 3, 10), Address = "456 Elm St", City = "Los Angeles", Region = "CA", PostalCode = "90001", Country = "USA", HomePhone = "555-5678", Extension = "102", Notes = "Strong leadership skills.", PhotoPath = "/images/janesmith.jpg", ReportsTo = null });
            employees.Add(new Employee { EmployeeID = 3, FirstName = "Michael", LastName = "Johnson", Title = "Data Analyst", TitleOfCourtesy = "Mr.", BirthDate = new DateTime(1992, 3, 15), HireDate = new DateTime(2020, 1, 1), Address = "789 Oak St", City = "Chicago", Region = "IL", PostalCode = "60601", Country = "USA", HomePhone = "555-8765", Extension = "103", Notes = "Detail-oriented.", PhotoPath = "/images/michaeljohnson.jpg", ReportsTo = 1 });
            employees.Add(new Employee { EmployeeID = 4, FirstName = "Emily", LastName = "Davis", Title = "Graphic Designer", TitleOfCourtesy = "Ms.", BirthDate = new DateTime(1988, 7, 22), HireDate = new DateTime(2019, 2, 14), Address = "321 Pine St", City = "Houston", Region = "TX", PostalCode = "77001", Country = "USA", HomePhone = "555-4321", Extension = "104", Notes = "Creative thinker.", PhotoPath = "/images/emilydavis.jpg", ReportsTo = 2 });
            employees.Add(new Employee { EmployeeID = 5, FirstName = "David", LastName = "Brown", Title = "Web Developer", TitleOfCourtesy = "Mr.", BirthDate = new DateTime(1995, 11, 30), HireDate = new DateTime(2021, 5, 20), Address = "654 Maple St", City = "Phoenix", Region = "AZ", PostalCode = "85001", Country = "USA", HomePhone = "555-3456", Extension = "105", Notes = "Passionate coder.", PhotoPath = "/images/davidbrown.jpg", ReportsTo = 1 });
            employees.Add(new Employee { EmployeeID = 6, FirstName = "Sarah", LastName = "Wilson", Title = "HR Specialist", TitleOfCourtesy = "Ms.", BirthDate = new DateTime(1990, 4, 10), HireDate = new DateTime(2017, 8, 15), Address = "987 Cedar St", City = "San Francisco", Region = "CA", PostalCode = "94101", Country = "USA", HomePhone = "555-6543", Extension = "106", Notes = "Experienced in employee relations.", PhotoPath = "/images/sarahwilson.jpg", ReportsTo = 2 });
            employees.Add(new Employee { EmployeeID = 7, FirstName = "Lisa", LastName = "Taylor", Title = "Marketing Coordinator", TitleOfCourtesy = "Ms.", BirthDate = new DateTime(1993, 9, 25), HireDate = new DateTime(2022, 4, 10), Address = "234 Birch St", City = "Seattle", Region = "WA", PostalCode = "98101", Country = "USA", HomePhone = "555-7890", Extension = "107", Notes = "Creative and organized.", PhotoPath = "/images/lisataylor.jpg", ReportsTo = 2 });
            return View(employees);
        }

        public IActionResult EditEmployee(int employeeID)
        {
            Employee employee = new();
            foreach(Employee em in employees)
            {
                if(em.EmployeeID == employeeID)
                {
                    employee = em;
                }
            }
            return View("EditEmployee", employee);
        }

        public IActionResult DeleteEmployee(int employeeID)
        {
            return View();
        }
    }
}
