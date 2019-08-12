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
    }
}