using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ayudacard_api.Entities
{
    public class MstServiceGroup
    {
        public Int32 Id { get; set; }
        public String ServiceGroup { get; set; }
        public String Description { get; set; }
        public Int32 ServiceDepartmentId { get; set; }
        public String ServiceDepartment { get; set; }
    }
}