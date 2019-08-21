using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ayudacard_api.Entities
{
    public class MstCitizensChildren
    {
        public Int32 Id { get; set; }
        public Int32 CitizenId { get; set; }
        public String Fullname { get; set; }
        public String DateOfBirth { get; set; }
    }
}