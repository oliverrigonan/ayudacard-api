using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ayudacard_api.Entities
{
    public class MstRegion
    {
        public Int32 Id { get; set; }
        public String Region { get; set; }
        public Int32 CountryId { get; set; }
        public String Country { get; set; }
    }
}