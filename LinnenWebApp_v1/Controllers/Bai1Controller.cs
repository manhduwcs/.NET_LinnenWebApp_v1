using Microsoft.AspNetCore.Mvc;

namespace LinnenWebApp_v1.Controllers
{
    public class Bai1Controller : Controller
    {
        // public IActionResult Index()
        // {
        //     return Content("This is an empty Index view");
        // }

        // public IActionResult ShowStudentName(string name)
        // {
        //     return Content(string.IsNullOrEmpty(name) ? "Input name is empty. Cannot show student name" : $"Student name is : {name}");
        // }

        // public IActionResult OddOrEven(string number)
        // {
        //     int num;

        //     return Content(int.TryParse(number, out num) == false ? "Invalid number option. This is not a number !" : $"Input number = {num}. This is a {(num % 2 == 0 ? "even" : "odd")} number.");
        // }

        // public IActionResult CalSum(string number)
        // {
        //     int num = 0;
        //     if (!int.TryParse(number, out num)) return Content($"Input {number} is not a valid number !");

        //     int res = 0;
        //     for (int i = 1; i < num; i++)
        //     {
        //         res += i;
        //     }

        //     double douRes = DisplayMimicSum(num);
        //     return Content($"Result of sum : {res} ; Result of mimic sum : {douRes}");
        // }

        // public static double DisplayMimicSum(int num)
        // {
        //     double result = 0;
        //     for (int i = 1; i <= num; i++)
        //     {
        //         result += 1 / i;
        //     }
        //     return result;
        // }

        // public IActionResult VowelCheck(string chracter)
        // {
        //     char chr = ' ';
        //     if (!char.TryParse(chracter, out chr))
        //     {
        //         return Content($"Input = {chracter} is not a valid character !");
        //     }

        //     char[] vowels = ['a', 'e', 'i', 'o', 'u'];
        //     bool isVowel = false;
        //     foreach (char ch in vowels)
        //     {
        //         if (chr.Equals(ch))
        //         {
        //             isVowel = true;
        //             break;
        //         }
        //     }

        //     return Content($"Char {chr} is {(isVowel ? "a vowel" : "not a vowel")}");
        // }

        // [Route("bai1/trian/{a}/{b}/{c}")]
        // public IActionResult Triangle(int a, int b, int c)
        // {
        //     string input = $"a = {a}, b = {b}, c = {c}\n";
        //     string content = "";
        //     bool isValidTriangle = true;
        //     if (a + b > c && a + c > b && b + c > a)
        //     {
        //         if (a == b && b == c)
        //             content = "Tam giac deu";
        //         else if (a == b || a == c || b == c)
        //             content = "Tam giac can";
        //         else
        //             content = "Tam giac khac";
        //     }
        //     else
        //     {
        //         isValidTriangle = false;
        //     }

        //     return Content(isValidTriangle ? $"{input}Tam giac nay la : {content}" : $"{input}Tam giac nay khong hop le");
        // }
    }
}
