using System.ComponentModel.DataAnnotations;

namespace LinnenWebApp_v1.Models;

public class Territory
{
    public string TerritoryID { get; set; }
    [Required(ErrorMessage = "Territory Description is required.")]
    [StringLength(50, ErrorMessage = "Territory Description cannot be longer than 50 characters.")]
    public string TerritoryDescription { get; set; }
    [Required]
    public int RegionID { get; set; }
    [Required]
    public string RegionDescription { get; set; }

    public Territory()
    {
        TerritoryID = "";
        TerritoryDescription = "";
        RegionID = -1;
        RegionDescription = "";
    }
}