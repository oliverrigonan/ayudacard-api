using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ayudacard_api.Entities
{
    public class RepUtilizationReport
    {
        public String TypeOfAssistance { get; set; }
        public Decimal NumberOfBeneficiaries { get; set; }
        public Decimal Amount { get; set; }
    }
}