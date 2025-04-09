using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LinnenWebApp_v1.Models;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace LinnenWebApp_v1.Controllers;

public class StudentController : Controller
{
    public static List<Student> studentList = new List<Student>();
    public IActionResult Index()
    { 
        return View(studentList);
    }

    public IActionResult ShowEditStudentPage(int studentID)
    {
        if (ModelState.IsValid)
        {
            Student std = new();
            foreach (Student s in studentList)
            {
                if (s.StudentID == studentID)
                {
                    std = s;
                    break;
                }
            }
            return RedirectToAction("EditStudent", studentList);
        }
        else return BadRequest();
    }

    [HttpPost]
    public IActionResult SubmitStudent(Student student)
    {
        if (ModelState.IsValid)
        {
            studentList.Add(student);
        }
        return RedirectToAction("Index");
    }

}
