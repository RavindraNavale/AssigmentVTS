using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTS_ASSIGNMENT.Models
{
    public class VehicleSearchModel
    {
        public int UserID { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleType { get; set; }
        public string ChassisNumber { get; set; }
        public string EngineNumber { get; set; }
        public string ManufacturingYear { get; set; }
    }
}