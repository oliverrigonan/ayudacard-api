using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ayudacard_api.Entities
{
    public class MstCitizen
    {
        public Int32 Id { get; set; }
        public String Surname { get; set; }
        public String Firstname { get; set; }
        public String Middlename { get; set; }
        public String Extensionname { get; set; }
        public String DateOfBirth { get; set; }
        public String PlaceOfBirth { get; set; }
        public Int32 SexId { get; set; }
        public String Sex { get; set; }
        public Int32 CivilStatusId { get; set; }
        public String CivilStatus { get; set; }
        public Int32? ReligionId { get; set; }
        public String Religion { get; set; }
        public Decimal Height { get; set; }
        public Decimal Weight { get; set; }
        public Int32 BloodTypeId { get; set; }
        public String BloodType { get; set; }
        public String GSISNumber { get; set; }
        public String HDMFNumber { get; set; }
        public String PhilHealthNumber { get; set; }
        public String SSSNumber { get; set; }
        public String TINNumber { get; set; }
        public String AgencyEmployeeNumber { get; set; }
        public Int32 CitizenshipId { get; set; }
        public String Citizenship { get; set; }
        public Int32? TypeOfCitizenshipId { get; set; }
        public String TypeOfCitizenship { get; set; }
        public String DualCitizenshipCountry { get; set; }
        public String ResidentialNumber { get; set; }
        public String ResidentialStreet { get; set; }
        public String ResidentialVillage { get; set; }
        public Int32 ResidentialBarangayId { get; set; }
        public String ResidentialBarangay { get; set; }
        public Int32 ResidentialCityId { get; set; }
        public String ResidentialCity { get; set; }
        public Int32 ResidentialProvinceId { get; set; }
        public String ResidentialProvince { get; set; }
        public String ResidentialZipCode { get; set; }
        public String ResidentialPrecinctNumber { get; set; }
        public String PermanentNumber { get; set; }
        public String PermanentStreet { get; set; }
        public String PermanentVillage { get; set; }
        public Int32 PermanentBarangayId { get; set; }
        public String PermanentBarangay { get; set; }
        public Int32 PermanentCityId { get; set; }
        public String PermanentCity { get; set; }
        public Int32 PermanentProvinceId { get; set; }
        public String PermanentProvince { get; set; }
        public String PermanentZipCode { get; set; }
        public String PermanentPrecinctNumber { get; set; }
        public String TelephoneNumber { get; set; }
        public String MobileNumber { get; set; }
        public String EmailAddress { get; set; }
        public Int32 OccupationId { get; set; }
        public String Occupation { get; set; }
        public String EmployerName { get; set; }
        public String EmployerAddress { get; set; }
        public String EmployerTelephoneNumber { get; set; }
        public String SpouseSurname { get; set; }
        public String SpouseFirstname { get; set; }
        public String SpouseMiddlename { get; set; }
        public String SpouseExtensionname { get; set; }
        public Int32 SpouseOccupationId { get; set; }
        public String SpouseOccupation { get; set; }
        public String SpouseEmployerName { get; set; }
        public String SpouseEmployerAddress { get; set; }
        public String FatherSurname { get; set; }
        public String FatherFirstname { get; set; }
        public String FatherMiddlename { get; set; }
        public String FatherExtensionname { get; set; }
        public String MotherSurname { get; set; }
        public String MotherFirstname { get; set; }
        public String MotherMiddlename { get; set; }
        public String MotherExtensionname { get; set; }
        public String PictureURL { get; set; }
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