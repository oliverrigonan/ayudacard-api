using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ayudacard_api.Azure.AzureStorage;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ayudacard_api.ApiControllers
{
    //[Authorize, RoutePrefix("api/mst/citizen")]
    [RoutePrefix("api/mst/citizen")]
    public class ApiMstCitizenController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstCitizen> CitizenList()
        {
            var citizens = from d in db.MstCitizens
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

        [HttpGet, Route("detail/{id}")]
        public Entities.MstCitizen CitizenDetail(String id)
        {
            var citizen = from d in db.MstCitizens
                          where d.Id == Convert.ToInt32(id)
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

            return citizen.FirstOrDefault();
        }

        [HttpGet, Route("sex/dropdown/list")]
        public List<Entities.MstSex> SexDropdownList()
        {
            var sexes = from d in db.MstSexes
                        select new Entities.MstSex
                        {
                            Id = d.Id,
                            Sex = d.Sex
                        };

            return sexes.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("civilStatus/dropdown/list")]
        public List<Entities.MstCivilStatus> CivilStatusDropdownList()
        {
            var civilStatus = from d in db.MstCivilStatus
                              select new Entities.MstCivilStatus
                              {
                                  Id = d.Id,
                                  CivilStatus = d.CivilStatus
                              };

            return civilStatus.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("bloodType/dropdown/list")]
        public List<Entities.MstBloodType> BloodTypeDropdownList()
        {
            var bloodTypes = from d in db.MstBloodTypes
                             select new Entities.MstBloodType
                             {
                                 Id = d.Id,
                                 BloodType = d.BloodType
                             };

            return bloodTypes.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("citizenship/dropdown/list")]
        public List<Entities.MstCitizenship> CitizenshipDropdownList()
        {
            var citizenships = from d in db.MstCitizenships
                               select new Entities.MstCitizenship
                               {
                                   Id = d.Id,
                                   Citizenship = d.Citizenship
                               };

            return citizenships.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("typeOfCitizenship/dropdown/list")]
        public List<Entities.MstTypeOfCitizenship> TypeOfCitizenshipDropdownList()
        {
            var typeOfCitizenships = from d in db.MstTypeOfCitizenships
                                     select new Entities.MstTypeOfCitizenship
                                     {
                                         Id = d.Id,
                                         TypeOfCitizenship = d.TypeOfCitizenship
                                     };

            return typeOfCitizenships.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("barangay/dropdown/list")]
        public List<Entities.MstBarangay> BarangayDropdownList()
        {
            var barangays = from d in db.MstBarangays
                            select new Entities.MstBarangay
                            {
                                Id = d.Id,
                                Barangay = d.Barangay
                            };

            return barangays.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("city/dropdown/list")]
        public List<Entities.MstCity> CityDropdownList()
        {
            var cities = from d in db.MstCities
                         select new Entities.MstCity
                         {
                             Id = d.Id,
                             City = d.City
                         };

            return cities.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("province/dropdown/list")]
        public List<Entities.MstProvince> ProvinceDropdownList()
        {
            var provinces = from d in db.MstProvinces
                            select new Entities.MstProvince
                            {
                                Id = d.Id,
                                Province = d.Province
                            };

            return provinces.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("region/dropdown/list")]
        public List<Entities.MstRegion> RegionDropdownList()
        {
            var regions = from d in db.MstRegions
                          select new Entities.MstRegion
                          {
                              Id = d.Id,
                              Region = d.Region
                          };

            return regions.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("occupation/dropdown/list")]
        public List<Entities.MstOccupation> OccupationDropdownList()
        {
            var occupations = from d in db.MstOccupations
                              select new Entities.MstOccupation
                              {
                                  Id = d.Id,
                                  Occupation = d.Occupation
                              };

            return occupations.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("status/dropdown/list")]
        public List<Entities.MstStatus> StatusDropdownList()
        {
            var statuses = from d in db.MstStatus
                           where d.Category.Equals("Citizen")
                           select new Entities.MstStatus
                           {
                               Id = d.Id,
                               Status = d.Status
                           };

            return statuses.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddCitizen()
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                var sex = from d in db.MstSexes select d;
                if (sex.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Sex not found!");
                }

                var civilStatus = from d in db.MstCivilStatus select d;
                if (civilStatus.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Civil status not found!");
                }

                var bloodType = from d in db.MstBloodTypes select d;
                if (bloodType.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Blood type not found!");
                }

                var citizenship = from d in db.MstCitizenships select d;
                if (citizenship.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizenship not found!");
                }

                var barangay = from d in db.MstBarangays select d;
                if (barangay.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Barangay not found!");
                }

                var city = from d in db.MstCities select d;
                if (city.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "City not found!");
                }

                var province = from d in db.MstProvinces select d;
                if (province.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Province not found!");
                }

                var occupation = from d in db.MstOccupations select d;
                if (occupation.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Occupation not found!");
                }

                var status = from d in db.MstStatus
                             where d.Category.Equals("Citizen")
                             select d;

                if (status.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Status not found!");
                }

                Data.MstCitizen newCitizen = new Data.MstCitizen()
                {
                    Surname = "",
                    Firstname = "",
                    Middlename = "",
                    Extensionname = "",
                    DateOfBirth = DateTime.Today,
                    PlaceOfBirth = "",
                    SexId = sex.FirstOrDefault().Id,
                    CivilStatusId = civilStatus.FirstOrDefault().Id,
                    Height = 0,
                    Weight = 0,
                    BloodTypeId = bloodType.FirstOrDefault().Id,
                    GSISNumber = "",
                    HDMFNumber = "",
                    PhilHealthNumber = "",
                    SSSNumber = "",
                    TINNumber = "",
                    AgencyEmployeeNumber = "",
                    CitizenshipId = citizenship.FirstOrDefault().Id,
                    TypeOfCitizenshipId = null,
                    DualCitizenshipCountry = "",
                    ResidentialNumber = "",
                    ResidentialStreet = "",
                    ResidentialVillage = "",
                    ResidentialBarangayId = barangay.FirstOrDefault().Id,
                    ResidentialCityId = city.FirstOrDefault().Id,
                    ResidentialProvinceId = province.FirstOrDefault().Id,
                    ResidentialZipCode = "",
                    PermanentNumber = "",
                    PermanentStreet = "",
                    PermanentVillage = "",
                    PermanentBarangayId = barangay.FirstOrDefault().Id,
                    PermanentCityId = city.FirstOrDefault().Id,
                    PermanentProvinceId = province.FirstOrDefault().Id,
                    PermanentZipCode = "",
                    TelephoneNumber = "",
                    MobileNumber = "",
                    EmailAddress = "",
                    OccupationId = occupation.FirstOrDefault().Id,
                    EmployerName = "",
                    EmployerAddress = "",
                    EmployerTelephoneNumber = "",
                    SpouseSurname = "",
                    SpouseFirstname = "",
                    SpouseMiddlename = "",
                    SpouseExtensionname = "",
                    SpouseOccupationId = occupation.FirstOrDefault().Id,
                    SpouseEmployerName = "",
                    SpouseEmployerAddress = "",
                    FatherSurname = "",
                    FatherFirstname = "",
                    FatherMiddlename = "",
                    FatherExtensionname = "",
                    MotherSurname = "",
                    MotherFirstname = "",
                    MotherMiddlename = "",
                    MotherExtensionname = "",
                    PictureURL = "",
                    StatusId = status.FirstOrDefault().Id,
                    IsLocked = false,
                    CreatedByUserId = currentUser.FirstOrDefault().Id,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = currentUser.FirstOrDefault().Id,
                    UpdatedDateTime = DateTime.Now
                };

                db.MstCitizens.InsertOnSubmit(newCitizen);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, newCitizen.Id);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("save/{id}")]
        public HttpResponseMessage SaveCitizen(String id, Entities.MstCitizen objCitizen)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                var sex = from d in db.MstSexes where d.Id == objCitizen.SexId select d;
                if (sex.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Sex not found!");
                }

                var civilStatus = from d in db.MstCivilStatus where d.Id == objCitizen.StatusId select d;
                if (civilStatus.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Civil status not found!");
                }

                var bloodType = from d in db.MstBloodTypes where d.Id == objCitizen.BloodTypeId select d;
                if (bloodType.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Blood type not found!");
                }

                var citizenship = from d in db.MstCitizenships where d.Id == objCitizen.CitizenshipId select d;
                if (citizenship.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizenship not found!");
                }

                if (objCitizen.TypeOfCitizenshipId != null)
                {
                    var typeOfCitizenship = from d in db.MstTypeOfCitizenships where d.Id == objCitizen.TypeOfCitizenshipId select d;
                    if (typeOfCitizenship.Any() == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Type of citizenship not found!");
                    }
                }

                var residentialBarangay = from d in db.MstBarangays where d.Id == objCitizen.ResidentialBarangayId select d;
                if (residentialBarangay.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Residential barangay not found!");
                }

                var permanentBarangay = from d in db.MstBarangays where d.Id == objCitizen.PermanentBarangayId select d;
                if (permanentBarangay.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Permanent barangay not found!");
                }

                var residentialCity = from d in db.MstCities where d.Id == objCitizen.ResidentialCityId select d;
                if (residentialCity.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Residential city not found!");
                }

                var permanentCity = from d in db.MstCities where d.Id == objCitizen.PermanentCityId select d;
                if (permanentCity.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Permanent city not found!");
                }

                var residentialProvince = from d in db.MstProvinces where d.Id == objCitizen.ResidentialProvinceId select d;
                if (residentialProvince.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Residential province not found!");
                }

                var permanentProvince = from d in db.MstProvinces where d.Id == objCitizen.PermanentProvinceId select d;
                if (permanentProvince.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Permanent province not found!");
                }

                var occupation = from d in db.MstOccupations where d.Id == objCitizen.OccupationId select d;
                if (occupation.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Occupation not found!");
                }

                var spouseoccupation = from d in db.MstOccupations where d.Id == objCitizen.SpouseOccupationId select d;
                if (spouseoccupation.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Spouse's occupation not found!");
                }

                var status = from d in db.MstStatus
                             where d.Id == objCitizen.StatusId
                             && d.Category.Equals("Citizen")
                             select d;

                if (status.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Status not found!");
                }

                var citizen = from d in db.MstCitizens
                              where d.Id == Convert.ToInt32(id)
                              select d;

                if (citizen.Any())
                {
                    var saveCitizen = citizen.FirstOrDefault();
                    saveCitizen.Surname = objCitizen.Surname;
                    saveCitizen.Firstname = objCitizen.Firstname;
                    saveCitizen.Middlename = objCitizen.Middlename;
                    saveCitizen.Extensionname = objCitizen.Extensionname;
                    saveCitizen.DateOfBirth = Convert.ToDateTime(objCitizen.DateOfBirth);
                    saveCitizen.PlaceOfBirth = objCitizen.PlaceOfBirth;
                    saveCitizen.SexId = objCitizen.SexId;
                    saveCitizen.CivilStatusId = objCitizen.CivilStatusId;
                    saveCitizen.Height = objCitizen.Height;
                    saveCitizen.Weight = objCitizen.Weight;
                    saveCitizen.BloodTypeId = objCitizen.BloodTypeId;
                    saveCitizen.GSISNumber = objCitizen.GSISNumber;
                    saveCitizen.HDMFNumber = objCitizen.HDMFNumber;
                    saveCitizen.PhilHealthNumber = objCitizen.PhilHealthNumber;
                    saveCitizen.SSSNumber = objCitizen.SSSNumber;
                    saveCitizen.TINNumber = objCitizen.TINNumber;
                    saveCitizen.AgencyEmployeeNumber = objCitizen.AgencyEmployeeNumber;
                    saveCitizen.CitizenshipId = objCitizen.CitizenshipId;
                    saveCitizen.TypeOfCitizenshipId = objCitizen.TypeOfCitizenshipId;
                    saveCitizen.DualCitizenshipCountry = objCitizen.DualCitizenshipCountry;
                    saveCitizen.ResidentialNumber = objCitizen.ResidentialNumber;
                    saveCitizen.ResidentialStreet = objCitizen.ResidentialStreet;
                    saveCitizen.ResidentialVillage = objCitizen.ResidentialVillage;
                    saveCitizen.ResidentialBarangayId = objCitizen.ResidentialBarangayId;
                    saveCitizen.ResidentialCityId = objCitizen.ResidentialCityId;
                    saveCitizen.ResidentialProvinceId = objCitizen.ResidentialProvinceId;
                    saveCitizen.ResidentialZipCode = objCitizen.ResidentialZipCode;
                    saveCitizen.PermanentNumber = objCitizen.PermanentNumber;
                    saveCitizen.PermanentStreet = objCitizen.PermanentStreet;
                    saveCitizen.PermanentVillage = objCitizen.PermanentVillage;
                    saveCitizen.PermanentBarangayId = objCitizen.PermanentBarangayId;
                    saveCitizen.PermanentCityId = objCitizen.PermanentCityId;
                    saveCitizen.PermanentProvinceId = objCitizen.PermanentProvinceId;
                    saveCitizen.PermanentZipCode = objCitizen.PermanentZipCode;
                    saveCitizen.TelephoneNumber = objCitizen.TelephoneNumber;
                    saveCitizen.MobileNumber = objCitizen.MobileNumber;
                    saveCitizen.EmailAddress = objCitizen.EmailAddress;
                    saveCitizen.OccupationId = objCitizen.OccupationId;
                    saveCitizen.EmployerName = objCitizen.EmployerName;
                    saveCitizen.EmployerAddress = objCitizen.EmployerAddress;
                    saveCitizen.EmployerTelephoneNumber = objCitizen.EmployerTelephoneNumber;
                    saveCitizen.SpouseSurname = objCitizen.SpouseSurname;
                    saveCitizen.SpouseFirstname = objCitizen.SpouseFirstname;
                    saveCitizen.SpouseMiddlename = objCitizen.SpouseMiddlename;
                    saveCitizen.SpouseExtensionname = objCitizen.SpouseExtensionname;
                    saveCitizen.SpouseOccupationId = objCitizen.SpouseOccupationId;
                    saveCitizen.SpouseEmployerName = objCitizen.SpouseEmployerName;
                    saveCitizen.SpouseEmployerAddress = objCitizen.SpouseEmployerAddress;
                    saveCitizen.FatherSurname = objCitizen.FatherSurname;
                    saveCitizen.FatherFirstname = objCitizen.FatherFirstname;
                    saveCitizen.FatherMiddlename = objCitizen.FatherMiddlename;
                    saveCitizen.FatherExtensionname = objCitizen.FatherExtensionname;
                    saveCitizen.MotherSurname = objCitizen.MotherSurname;
                    saveCitizen.MotherFirstname = objCitizen.MotherFirstname;
                    saveCitizen.MotherMiddlename = objCitizen.MotherMiddlename;
                    saveCitizen.MotherExtensionname = objCitizen.MotherExtensionname;
                    //saveCitizen.PictureURL = objCitizen.PictureURL;
                    saveCitizen.StatusId = objCitizen.StatusId;
                    saveCitizen.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    saveCitizen.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("lock/{id}")]
        public HttpResponseMessage LockCitizen(String id, Entities.MstCitizen objCitizen)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                var sex = from d in db.MstSexes where d.Id == objCitizen.SexId select d;
                if (sex.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Sex not found!");
                }

                var civilStatus = from d in db.MstCivilStatus where d.Id == objCitizen.StatusId select d;
                if (civilStatus.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Civil status not found!");
                }

                var bloodType = from d in db.MstBloodTypes where d.Id == objCitizen.BloodTypeId select d;
                if (bloodType.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Blood type not found!");
                }

                var citizenship = from d in db.MstCitizenships where d.Id == objCitizen.CitizenshipId select d;
                if (citizenship.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizenship not found!");
                }

                if (objCitizen.TypeOfCitizenshipId != null)
                {
                    var typeOfCitizenship = from d in db.MstTypeOfCitizenships where d.Id == objCitizen.TypeOfCitizenshipId select d;
                    if (typeOfCitizenship.Any() == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Type of citizenship not found!");
                    }
                }

                var residentialBarangay = from d in db.MstBarangays where d.Id == objCitizen.ResidentialBarangayId select d;
                if (residentialBarangay.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Residential barangay not found!");
                }

                var permanentBarangay = from d in db.MstBarangays where d.Id == objCitizen.PermanentBarangayId select d;
                if (permanentBarangay.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Permanent barangay not found!");
                }

                var residentialCity = from d in db.MstCities where d.Id == objCitizen.ResidentialCityId select d;
                if (residentialCity.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Residential city not found!");
                }

                var permanentCity = from d in db.MstCities where d.Id == objCitizen.PermanentCityId select d;
                if (permanentCity.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Permanent city not found!");
                }

                var residentialProvince = from d in db.MstProvinces where d.Id == objCitizen.ResidentialProvinceId select d;
                if (residentialProvince.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Residential province not found!");
                }

                var permanentProvince = from d in db.MstProvinces where d.Id == objCitizen.PermanentProvinceId select d;
                if (permanentProvince.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Permanent province not found!");
                }

                var occupation = from d in db.MstOccupations where d.Id == objCitizen.OccupationId select d;
                if (occupation.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Occupation not found!");
                }

                var spouseoccupation = from d in db.MstOccupations where d.Id == objCitizen.SpouseOccupationId select d;
                if (spouseoccupation.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Spouse's occupation not found!");
                }

                var status = from d in db.MstStatus
                             where d.Id == objCitizen.StatusId
                             && d.Category.Equals("Citizen")
                             select d;

                if (status.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Status not found!");
                }

                var citizen = from d in db.MstCitizens
                              where d.Id == Convert.ToInt32(id)
                              select d;

                if (citizen.Any())
                {
                    if (citizen.FirstOrDefault().IsLocked == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Already locked!");
                    }

                    var lockCitizen = citizen.FirstOrDefault();
                    lockCitizen.Surname = objCitizen.Surname;
                    lockCitizen.Firstname = objCitizen.Firstname;
                    lockCitizen.Middlename = objCitizen.Middlename;
                    lockCitizen.Extensionname = objCitizen.Extensionname;
                    lockCitizen.DateOfBirth = Convert.ToDateTime(objCitizen.DateOfBirth);
                    lockCitizen.PlaceOfBirth = objCitizen.PlaceOfBirth;
                    lockCitizen.SexId = objCitizen.SexId;
                    lockCitizen.CivilStatusId = objCitizen.CivilStatusId;
                    lockCitizen.Height = objCitizen.Height;
                    lockCitizen.Weight = objCitizen.Weight;
                    lockCitizen.BloodTypeId = objCitizen.BloodTypeId;
                    lockCitizen.GSISNumber = objCitizen.GSISNumber;
                    lockCitizen.HDMFNumber = objCitizen.HDMFNumber;
                    lockCitizen.PhilHealthNumber = objCitizen.PhilHealthNumber;
                    lockCitizen.SSSNumber = objCitizen.SSSNumber;
                    lockCitizen.TINNumber = objCitizen.TINNumber;
                    lockCitizen.AgencyEmployeeNumber = objCitizen.AgencyEmployeeNumber;
                    lockCitizen.CitizenshipId = objCitizen.CitizenshipId;
                    lockCitizen.TypeOfCitizenshipId = objCitizen.TypeOfCitizenshipId;
                    lockCitizen.DualCitizenshipCountry = objCitizen.DualCitizenshipCountry;
                    lockCitizen.ResidentialNumber = objCitizen.ResidentialNumber;
                    lockCitizen.ResidentialStreet = objCitizen.ResidentialStreet;
                    lockCitizen.ResidentialVillage = objCitizen.ResidentialVillage;
                    lockCitizen.ResidentialBarangayId = objCitizen.ResidentialBarangayId;
                    lockCitizen.ResidentialCityId = objCitizen.ResidentialCityId;
                    lockCitizen.ResidentialProvinceId = objCitizen.ResidentialProvinceId;
                    lockCitizen.ResidentialZipCode = objCitizen.ResidentialZipCode;
                    lockCitizen.PermanentNumber = objCitizen.PermanentNumber;
                    lockCitizen.PermanentStreet = objCitizen.PermanentStreet;
                    lockCitizen.PermanentVillage = objCitizen.PermanentVillage;
                    lockCitizen.PermanentBarangayId = objCitizen.PermanentBarangayId;
                    lockCitizen.PermanentCityId = objCitizen.PermanentCityId;
                    lockCitizen.PermanentProvinceId = objCitizen.PermanentProvinceId;
                    lockCitizen.PermanentZipCode = objCitizen.PermanentZipCode;
                    lockCitizen.TelephoneNumber = objCitizen.TelephoneNumber;
                    lockCitizen.MobileNumber = objCitizen.MobileNumber;
                    lockCitizen.EmailAddress = objCitizen.EmailAddress;
                    lockCitizen.OccupationId = objCitizen.OccupationId;
                    lockCitizen.EmployerName = objCitizen.EmployerName;
                    lockCitizen.EmployerAddress = objCitizen.EmployerAddress;
                    lockCitizen.EmployerTelephoneNumber = objCitizen.EmployerTelephoneNumber;
                    lockCitizen.SpouseSurname = objCitizen.SpouseSurname;
                    lockCitizen.SpouseFirstname = objCitizen.SpouseFirstname;
                    lockCitizen.SpouseMiddlename = objCitizen.SpouseMiddlename;
                    lockCitizen.SpouseExtensionname = objCitizen.SpouseExtensionname;
                    lockCitizen.SpouseOccupationId = objCitizen.SpouseOccupationId;
                    lockCitizen.SpouseEmployerName = objCitizen.SpouseEmployerName;
                    lockCitizen.SpouseEmployerAddress = objCitizen.SpouseEmployerAddress;
                    lockCitizen.FatherSurname = objCitizen.FatherSurname;
                    lockCitizen.FatherFirstname = objCitizen.FatherFirstname;
                    lockCitizen.FatherMiddlename = objCitizen.FatherMiddlename;
                    lockCitizen.FatherExtensionname = objCitizen.FatherExtensionname;
                    lockCitizen.MotherSurname = objCitizen.MotherSurname;
                    lockCitizen.MotherFirstname = objCitizen.MotherFirstname;
                    lockCitizen.MotherMiddlename = objCitizen.MotherMiddlename;
                    lockCitizen.MotherExtensionname = objCitizen.MotherExtensionname;
                    //lockCitizen.PictureURL = objCitizen.PictureURL;
                    lockCitizen.StatusId = objCitizen.StatusId;
                    lockCitizen.IsLocked = true;
                    lockCitizen.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    lockCitizen.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("unlock/{id}")]
        public HttpResponseMessage UnlockCitizen(String id)
        {
            try
            {
                var citizen = from d in db.MstCitizens
                              where d.Id == Convert.ToInt32(id)
                              select d;

                if (citizen.Any())
                {
                    if (citizen.FirstOrDefault().IsLocked == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Already unlocked!");
                    }

                    var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                    var unlockCitizen = citizen.FirstOrDefault();
                    unlockCitizen.IsLocked = false;
                    unlockCitizen.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    unlockCitizen.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteCitizen(String id)
        {
            try
            {
                var citizen = from d in db.MstCitizens
                              where d.Id == Convert.ToInt32(id)
                              select d;

                if (citizen.Any())
                {
                    if (citizen.FirstOrDefault().IsLocked == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Citizen cannot be deleted if locked.");
                    }

                    var deleteCitizen = citizen.FirstOrDefault();
                    db.MstCitizens.DeleteOnSubmit(deleteCitizen);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet, Route("search")]
        public List<Entities.MstCitizen> SearchCitizen(String keyword)
        {
            if (String.IsNullOrEmpty(keyword))
            {
                return new List<Entities.MstCitizen>();
            }

            var citizens = from d in db.MstCitizens
                           where d.Surname.Contains(keyword)
                           || d.Firstname.Contains(keyword)
                           || d.Middlename.Contains(keyword)
                           || d.Extensionname.Contains(keyword)
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

        [HttpPost, Route("uploadPhoto/{citizenId}")]
        public async Task<IHttpActionResult> UploadPhotoCitizen(String citizenId)
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                String cloudStorageConnectionString = ConfigurationManager.AppSettings["CloudStorageConnectionString"];
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);

                String cloudStorageContainerName = ConfigurationManager.AppSettings["CloudStorageContainerName"];
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(cloudStorageContainerName);

                AzureStorageMultipartFormDataStreamProvider provider = new AzureStorageMultipartFormDataStreamProvider(cloudBlobContainer);
                await Request.Content.ReadAsMultipartAsync(provider);

                String fileName = provider.FileData.FirstOrDefault()?.LocalFileName;
                if (String.IsNullOrEmpty(fileName))
                {
                    return BadRequest("An error has occured while uploading your file. Please try again.");
                }

                String imageURI = Azure.BlobStorage.BlobContainer.GetCloudBlockBlobImageURI(fileName);

                var citizen = from d in db.MstCitizens
                              where d.Id == Convert.ToInt32(citizenId)
                              select d;

                if (citizen.Any())
                {
                    var updateCitizen = citizen.FirstOrDefault();
                    updateCitizen.PictureURL = imageURI;
                    db.SubmitChanges();
                }

                return Ok(imageURI);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }
        }

        [HttpPut, Route("clearPhoto/{citizenId}")]
        public HttpResponseMessage ClearPhotoCitizen(String citizenId)
        {
            try
            {
                var citizen = from d in db.MstCitizens
                              where d.Id == Convert.ToInt32(citizenId)
                              select d;

                if (citizen.Any())
                {
                    var updateCitizen = citizen.FirstOrDefault();
                    updateCitizen.PictureURL = "";
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen not found!");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
