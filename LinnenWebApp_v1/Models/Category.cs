using System.ComponentModel.DataAnnotations;

namespace LinnenWebApp_v1.Models;

public class Category
{
    public int CategoryID { get; set; }
    [Required(ErrorMessage = "Category Name is required.")]
    [StringLength(15, ErrorMessage = "Category Name cannot be longer than 15 characters.")]
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public string Picture { get; set; }

    public Category()
    {
        CategoryID = -1;
        CategoryName = "";
        Description = "";
        Picture = "";
    }
    
}