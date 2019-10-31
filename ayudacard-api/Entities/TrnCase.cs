using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ayudacard_api.Entities
{
    public class TrnCase
    {
        public Int32 Id { get; set; }
        public String CaseNumber { get; set; }
        public String CaseDate { get; set; }
        public Int32 CitizenId { get; set; }
        public String Citizen { get; set; }
        public Int32 CitizenCardId { get; set; }
        public String CitizenCardNumber { get; set; }
        public Int32 ServiceId { get; set; }
        public String Service { get; set; }
        public String ServiceGroup { get; set; }
        public String Problem { get; set; }
        public String Backgroud { get; set; }
        public String Recommendation { get; set; }
        public Int32 PreparedById { get; set; }
        public String PreparedBy { get; set; }
        public Int32 CheckedById { get; set; }
        public String CheckedBy { get; set; }
        public Int32 StatusId { get; set; }
        public String Status { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public String CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public String UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}