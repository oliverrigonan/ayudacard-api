using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ayudacard_api.Entities
{
    public class MstCitizensCard
    {
        public Int32 Id { get; set; }
        public Int32 CitizenId { get; set; }
        public String Citizen { get; set; }
        public String CardNumber { get; set; }
        public Decimal TotalBalance { get; set; }
        public Int32 StatusId { get; set; }
        public String Status { get; set; }
    }
}