using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ayudacard_api.Entities
{
    public class MstServiceDepartment
    {
        public Int32 Id { get; set; }
        public String ServiceDepartment { get; set; }
        public String OfficerInCharge { get; set; }
        public String ContactNumber { get; set; }
        public String EmailAddress { get; set; }
    }
}