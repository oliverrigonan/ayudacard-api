using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ayudacard_api.Entities
{
    public class MstBarangay
    {
        public Int32 Id { get; set; }
        public String Barangay { get; set; }
        public String BarangayChairman { get; set; }
        public Int32 ProvinceId { get; set; }
        public String Province { get; set; }
        public Int32 RegionId { get; set; }
        public String Region { get; set; }
        public Int32 CountryId { get; set; }
        public String Country { get; set; }
        public Int32 CityId { get; set; }
        public String City { get; set; }
    }
}