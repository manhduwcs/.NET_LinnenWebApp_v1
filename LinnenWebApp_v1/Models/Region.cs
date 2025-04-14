using System;
using System.ComponentModel.DataAnnotations;

namespace LinnenWebApp_v1.Models
{
    public class Region
    {
        public int RegionID { get; set; }
        [Required(ErrorMessage = "Region Name is required.")]
        [StringLength(50, ErrorMessage = "Region Description cannot be longer than 50 characters.")]
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