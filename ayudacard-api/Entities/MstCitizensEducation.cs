using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ayudacard_api.Entities
{
    public class MstCitizensEducation
    {
        public Int32 Id { get; set; }
        public Int32 CitizenId { get; set; }
        public Int32 EducationLevelId { get; set; }
        public String EducationLevel { get; set; }
        public String NameOfSchool { get; set; }
        public String Degree { get; set; }
        public String PeriodFrom { get; set; }
        public String PeriodTo { get; set; }
        public Decimal? UnitsEarned { get; set; }
        public Int32? YearGraduated { get; set; }
    }
}