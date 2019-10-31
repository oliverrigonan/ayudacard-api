using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ayudacard_api.Entities
{
    public class MstServiceGroupTemplate
    {
        public Int32 Id { get; set; }
        public Int32 ServiceGroupId { get; set; }
        public String ServiceGroup { get; set; }
        public String TemplateName { get; set; }
        public String Problem { get; set; }
        public String Background { get; set; }
        public String Recommendation { get; set; }
    }
}