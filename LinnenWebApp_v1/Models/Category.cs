namespace LinnenWebApp_v1.Models;

public class Category
{
    public int CategoryID { get; set; }
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