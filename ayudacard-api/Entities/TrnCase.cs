﻿using System;
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
        public String CitizenDateOfBirth { get; set; }
        public String CitizenBirthPlace { get; set; }
        public String CitizenGender { get; set; }
        public Int32 CitizenAge { get; set; }
        public String CitizenCivilStatus { get; set; }
        public String CitizenEducationalAttainment { get; set; }
        public String CitizenOccupation { get; set; }
        public String CitizenReligion { get; set; }
        public String CitizenAddress { get; set; }
        public String CitizenMobileNumber { get; set; }
        public Int32 CitizenCardId { get; set; }
        public String CitizenCardNumber { get; set; }
        public String CitizenPermanentBarangay { get; set; }
        public Int32 ServiceId { get; set; }
        public String Service { get; set; }
        public String ServiceGroup { get; set; }
        public Decimal Amount { get; set; }
        public String Problem { get; set; }
        public String Background { get; set; }
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
        public String PersonApplied { get; set; }
        public String PersonOfInterest { get; set; }
        public String EndorsementPharmacyCode { get; set; }
        public String EndorsementPharmacyName { get; set; }
    }
}