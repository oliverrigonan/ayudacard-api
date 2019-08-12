using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ayudacard_api.Entities
{
    public class MstProvince
    {
        public Int32 Id { get; set; }
        public String Province { get; set; }
        public Int32 RegionId { get; set; }
        public String Region { get; set; }
    }
}