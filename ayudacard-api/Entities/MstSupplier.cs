using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ayudacard_api.Entities
{
    public class MstSupplier
    {
        public Int32 Id { get; set; }
        public String Supplier { get; set; }
        public String Address { get; set; }
        public Boolean IsVAT { get; set; }
        public Decimal VATRate { get; set; }
        public Boolean IsWithheld { get; set; }
        public Decimal WithholdingRate { get; set; }
        public Boolean IsCityTax { get; set; }
        public Decimal CityTaxRate { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public String CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public String UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}