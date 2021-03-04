using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [RoutePrefix("api/rep/citizens/report")]
    public class ApiRepCitizensReportController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list/{barangayId}")]
        public List<Entities.MstCitizen> CitizensReportList(Int32 barangayId)
        {
            if (barangayId == 0)
            {
                var citizens = from d in db.MstCitizens
                               where d.IsLocked == true
                               select new Entities.MstCitizen
                               {
                                   Id = d.Id,
                                   Surname = d.Surname,
                                   Firstname = d.Firstname,
                                   Middlename = d.Middlename,
                                   Extensionname = d.Extensionname,
                                   DateOfBirth = d.DateOfBirth.ToShortDateString(),
                                   PlaceOfBirth = d.PlaceOfBirth,
                                   SexId = d.SexId,
                                   Sex = d.MstSex.Sex,
                                   CivilStatusId = d.CivilStatusId,
                                   CivilStatus = d.MstCivilStatus.CivilStatus,
                                   ReligionId = d.ReligionId,
                                   Religion = d.MstReligion.Religion,
                                   Height = d.Height,
                                   Weight = d.Weight,
                                   BloodTypeId = d.BloodTypeId,
                                   BloodType = d.MstBloodType.BloodType,
                                   GSISNumber = d.GSISNumber,
                                   HDMFNumber = d.HDMFNumber,
                                   PhilHealthNumber = d.PhilHealthNumber,
                                   SSSNumber = d.SSSNumber,
                                   TINNumber = d.TINNumber,
                                   AgencyEmployeeNumber = d.AgencyEmployeeNumber,
                                   CitizenshipId = d.CitizenshipId,
                                   Citizenship = d.MstCitizenship.Citizenship,
                                   TypeOfCitizenshipId = d.TypeOfCitizenshipId,
                                   TypeOfCitizenship = d.TypeOfCitizenshipId != null ? d.MstTypeOfCitizenship.TypeOfCitizenship : "",
                                   DualCitizenshipCountry = d.DualCitizenshipCountry,
                                   ResidentialNumber = d.ResidentialNumber,
                                   ResidentialStreet = d.ResidentialStreet,
                                   ResidentialVillage = d.ResidentialVillage,
                                   ResidentialBarangayId = d.ResidentialBarangayId,
                                   ResidentialBarangay = d.MstBarangay.Barangay,
                                   ResidentialCityId = d.ResidentialCityId,
                                   ResidentialCity = d.MstCity.City,
                                   ResidentialProvinceId = d.ResidentialProvinceId,
                                   ResidentialProvince = d.MstProvince.Province,
                                   ResidentialZipCode = d.ResidentialZipCode,
                                   ResidentialPrecinctNumber = d.ResidentialPrecinctNumber,
                                   PermanentNumber = d.PermanentNumber,
                                   PermanentStreet = d.PermanentStreet,
                                   PermanentVillage = d.PermanentVillage,
                                   PermanentBarangayId = d.PermanentBarangayId,
                                   PermanentBarangay = d.MstBarangay1.Barangay,
                                   PermanentCityId = d.PermanentCityId,
                                   PermanentCity = d.MstCity1.City,
                                   PermanentProvinceId = d.PermanentProvinceId,
                                   PermanentProvince = d.MstProvince1.Province,
                                   PermanentZipCode = d.PermanentZipCode,
                                   PermanentPrecinctNumber = d.PermanentPrecinctNumber,
                                   TelephoneNumber = d.TelephoneNumber,
                                   MobileNumber = d.MobileNumber,
                                   EmailAddress = d.EmailAddress,
                                   OccupationId = d.OccupationId,
                                   Occupation = d.MstOccupation.Occupation,
                                   EmployerName = d.EmployerName,
                                   EmployerAddress = d.EmployerAddress,
                                   EmployerTelephoneNumber = d.EmployerTelephoneNumber,
                                   SpouseSurname = d.SpouseSurname,
                                   SpouseFirstname = d.SpouseFirstname,
                                   SpouseMiddlename = d.SpouseMiddlename,
                                   SpouseExtensionname = d.SpouseExtensionname,
                                   SpouseOccupationId = d.SpouseOccupationId,
                                   SpouseOccupation = d.MstOccupation1.Occupation,
                                   SpouseEmployerName = d.SpouseEmployerName,
                                   SpouseEmployerAddress = d.SpouseEmployerAddress,
                                   FatherSurname = d.FatherSurname,
                                   FatherFirstname = d.FatherFirstname,
                                   FatherMiddlename = d.FatherMiddlename,
                                   FatherExtensionname = d.FatherExtensionname,
                                   MotherSurname = d.MotherSurname,
                                   MotherFirstname = d.MotherFirstname,
                                   MotherMiddlename = d.MotherMiddlename,
                                   MotherExtensionname = d.MotherExtensionname,
                                   PictureURL = d.PictureURL,
                                   StatusId = d.StatusId,
                                   Status = d.MstStatus.Status,
                                   IsLocked = d.IsLocked,
                                   CreatedByUserId = d.CreatedByUserId,
                                   CreatedByUser = d.MstUser.Fullname,
                                   CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                   UpdatedByUserId = d.UpdatedByUserId,
                                   UpdatedByUser = d.MstUser1.Fullname,
                                   UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                               };

                return citizens.OrderByDescending(d => d.Id).ToList();
            }
            else
            {
                var citizens = from d in db.MstCitizens
                               where d.PermanentBarangayId == barangayId
                               && d.IsLocked == true
                               select new Entities.MstCitizen
                               {
                                   Id = d.Id,
                                   Surname = d.Surname,
                                   Firstname = d.Firstname,
                                   Middlename = d.Middlename,
                                   Extensionname = d.Extensionname,
                                   DateOfBirth = d.DateOfBirth.ToShortDateString(),
                                   PlaceOfBirth = d.PlaceOfBirth,
                                   SexId = d.SexId,
                                   Sex = d.MstSex.Sex,
                                   CivilStatusId = d.CivilStatusId,
                                   CivilStatus = d.MstCivilStatus.CivilStatus,
                                   ReligionId = d.ReligionId,
                                   Religion = d.MstReligion.Religion,
                                   Height = d.Height,
                                   Weight = d.Weight,
                                   BloodTypeId = d.BloodTypeId,
                                   BloodType = d.MstBloodType.BloodType,
                                   GSISNumber = d.GSISNumber,
                                   HDMFNumber = d.HDMFNumber,
                                   PhilHealthNumber = d.PhilHealthNumber,
                                   SSSNumber = d.SSSNumber,
                                   TINNumber = d.TINNumber,
                                   AgencyEmployeeNumber = d.AgencyEmployeeNumber,
                                   CitizenshipId = d.CitizenshipId,
                                   Citizenship = d.MstCitizenship.Citizenship,
                                   TypeOfCitizenshipId = d.TypeOfCitizenshipId,
                                   TypeOfCitizenship = d.TypeOfCitizenshipId != null ? d.MstTypeOfCitizenship.TypeOfCitizenship : "",
                                   DualCitizenshipCountry = d.DualCitizenshipCountry,
                                   ResidentialNumber = d.ResidentialNumber,
                                   ResidentialStreet = d.ResidentialStreet,
                                   ResidentialVillage = d.ResidentialVillage,
                                   ResidentialBarangayId = d.ResidentialBarangayId,
                                   ResidentialBarangay = d.MstBarangay.Barangay,
                                   ResidentialCityId = d.ResidentialCityId,
                                   ResidentialCity = d.MstCity.City,
                                   ResidentialProvinceId = d.ResidentialProvinceId,
                                   ResidentialProvince = d.MstProvince.Province,
                                   ResidentialZipCode = d.ResidentialZipCode,
                                   ResidentialPrecinctNumber = d.ResidentialPrecinctNumber,
                                   PermanentNumber = d.PermanentNumber,
                                   PermanentStreet = d.PermanentStreet,
                                   PermanentVillage = d.PermanentVillage,
                                   PermanentBarangayId = d.PermanentBarangayId,
                                   PermanentBarangay = d.MstBarangay1.Barangay,
                                   PermanentCityId = d.PermanentCityId,
                                   PermanentCity = d.MstCity1.City,
                                   PermanentProvinceId = d.PermanentProvinceId,
                                   PermanentProvince = d.MstProvince1.Province,
                                   PermanentZipCode = d.PermanentZipCode,
                                   PermanentPrecinctNumber = d.PermanentPrecinctNumber,
                                   TelephoneNumber = d.TelephoneNumber,
                                   MobileNumber = d.MobileNumber,
                                   EmailAddress = d.EmailAddress,
                                   OccupationId = d.OccupationId,
                                   Occupation = d.MstOccupation.Occupation,
                                   EmployerName = d.EmployerName,
                                   EmployerAddress = d.EmployerAddress,
                                   EmployerTelephoneNumber = d.EmployerTelephoneNumber,
                                   SpouseSurname = d.SpouseSurname,
                                   SpouseFirstname = d.SpouseFirstname,
                                   SpouseMiddlename = d.SpouseMiddlename,
                                   SpouseExtensionname = d.SpouseExtensionname,
                                   SpouseOccupationId = d.SpouseOccupationId,
                                   SpouseOccupation = d.MstOccupation1.Occupation,
                                   SpouseEmployerName = d.SpouseEmployerName,
                                   SpouseEmployerAddress = d.SpouseEmployerAddress,
                                   FatherSurname = d.FatherSurname,
                                   FatherFirstname = d.FatherFirstname,
                                   FatherMiddlename = d.FatherMiddlename,
                                   FatherExtensionname = d.FatherExtensionname,
                                   MotherSurname = d.MotherSurname,
                                   MotherFirstname = d.MotherFirstname,
                                   MotherMiddlename = d.MotherMiddlename,
                                   MotherExtensionname = d.MotherExtensionname,
                                   PictureURL = d.PictureURL,
                                   StatusId = d.StatusId,
                                   Status = d.MstStatus.Status,
                                   IsLocked = d.IsLocked,
                                   CreatedByUserId = d.CreatedByUserId,
                                   CreatedByUser = d.MstUser.Fullname,
                                   CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                   UpdatedByUserId = d.UpdatedByUserId,
                                   UpdatedByUser = d.MstUser1.Fullname,
                                   UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                               };

                return citizens.OrderByDescending(d => d.Id).ToList();
            }
        }
    }
}
