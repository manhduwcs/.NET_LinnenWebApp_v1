namespace LinnenWebApp_v1.Models;

public class Territory
{
    public string TerritoryID { get; set; }
    public string TerritoryDescription { get; set; }
    public int RegionID { get; set; }
    public string RegionDescription { get; set; }

    public Territory()
    {
        TerritoryID = "";
        TerritoryDescription = "";
        RegionID = -1;
        RegionDescription = "";
    }
}