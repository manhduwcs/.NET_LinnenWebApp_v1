using System;

namespace LinnenWebApp_v1.Models
{
    public class Region
    {
        public int RegionID { get; set; }
        public string RegionDescription { get; set; }

        public Region()
        {

        }

        public Region(int regionID, string regionDescription)
        {
            RegionID = regionID;
            RegionDescription = regionDescription;
        }
    }
}