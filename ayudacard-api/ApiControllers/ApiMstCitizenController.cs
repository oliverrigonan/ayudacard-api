using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/citizen")]
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
                               TypeOfCitizenship = d.MstTypeOfCitizenship.TypeOfCitizenship,
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
                               CitizenStatusId = d.CitizenStatusId,
                               CitizenStatus = d.MstCitizensStatus.CitizenStatus,
                               CreatedByUserId = d.CreatedByUserId,
                               CreatedByUser = d.MstUser.Fullname,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedByUserId = d.UpdatedByUserId,
                               UpdatedByUser = d.MstUser1.Fullname,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };

            return citizens.ToList();
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

            return sexes.ToList();
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

            return civilStatus.ToList();
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

            return bloodTypes.ToList();
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

            return citizenships.ToList();
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

            return typeOfCitizenships.ToList();
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

            return barangays.ToList();
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

            return cities.ToList();
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

            return provinces.ToList();
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

            return regions.ToList();
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

            return occupations.ToList();
        }

        [HttpGet, Route("citizenStatus/dropdown/list")]
        public List<Entities.MstCitizenStatus> CitizenStatusDropdownList()
        {
            var citizenStatus = from d in db.MstCitizensStatus
                                select new Entities.MstCitizenStatus
                                {
                                    Id = d.Id,
                                    CitizenStatus = d.CitizenStatus
                                };

            return citizenStatus.ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddCitizen(Entities.MstCitizen objCitizen)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                var sex = from d in db.MstSexes where d.Id == objCitizen.SexId select d;
                if (!sex.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Sex not found!");
                }

                var civilStatus = from d in db.MstCivilStatus where d.Id == objCitizen.CivilStatusId select d;
                if (!civilStatus.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Civil status not found!");
                }

                var bloodType = from d in db.MstBloodTypes where d.Id == objCitizen.BloodTypeId select d;
                if (!bloodType.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Blood type not found!");
                }

                var citizenship = from d in db.MstCitizenships where d.Id == objCitizen.CitizenshipId select d;
                if (!citizenship.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizenship not found!");
                }

                var typeOfCitizenship = from d in db.MstTypeOfCitizenships where d.Id == objCitizen.TypeOfCitizenshipId select d;
                if (!typeOfCitizenship.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Type of citizenship not found!");
                }

                var barangay = from d in db.MstBarangays where d.Id == objCitizen.ResidentialBarangayId && d.Id == objCitizen.PermanentBarangayId select d;
                if (!barangay.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Barangay not found!");
                }

                var city = from d in db.MstCities where d.Id == objCitizen.ResidentialCityId && d.Id == objCitizen.PermanentCityId select d;
                if (!city.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "City not found!");
                }

                var province = from d in db.MstProvinces where d.Id == objCitizen.ResidentialProvinceId && d.Id == objCitizen.PermanentProvinceId select d;
                if (!province.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Province not found!");
                }

                var occupation = from d in db.MstOccupations where d.Id == objCitizen.OccupationId && d.Id == objCitizen.SpouseOccupationId select d;
                if (!occupation.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Occupation not found!");
                }

                var citizenStatus = from d in db.MstCitizensStatus where d.Id == objCitizen.CitizenStatusId select d;
                if (!citizenStatus.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen status not found!");
                }

                Data.MstCitizen newCitizen = new Data.MstCitizen()
                {
                    Surname = objCitizen.Surname,
                    Firstname = objCitizen.Firstname,
                    Middlename = objCitizen.Middlename,
                    Extensionname = objCitizen.Extensionname,
                    DateOfBirth = Convert.ToDateTime(objCitizen.DateOfBirth),
                    PlaceOfBirth = objCitizen.PlaceOfBirth,
                    SexId = objCitizen.SexId,
                    CivilStatusId = objCitizen.CivilStatusId,
                    Height = objCitizen.Height,
                    Weight = objCitizen.Weight,
                    BloodTypeId = objCitizen.BloodTypeId,
                    GSISNumber = objCitizen.GSISNumber,
                    HDMFNumber = objCitizen.HDMFNumber,
                    PhilHealthNumber = objCitizen.PhilHealthNumber,
                    SSSNumber = objCitizen.SSSNumber,
                    TINNumber = objCitizen.TINNumber,
                    AgencyEmployeeNumber = objCitizen.AgencyEmployeeNumber,
                    CitizenshipId = objCitizen.CitizenshipId,
                    TypeOfCitizenshipId = objCitizen.TypeOfCitizenshipId,
                    DualCitizenshipCountry = objCitizen.DualCitizenshipCountry,
                    ResidentialNumber = objCitizen.ResidentialNumber,
                    ResidentialStreet = objCitizen.ResidentialStreet,
                    ResidentialVillage = objCitizen.ResidentialVillage,
                    ResidentialBarangayId = objCitizen.ResidentialBarangayId,
                    ResidentialCityId = objCitizen.ResidentialCityId,
                    ResidentialProvinceId = objCitizen.ResidentialProvinceId,
                    ResidentialZipCode = objCitizen.ResidentialZipCode,
                    PermanentNumber = objCitizen.PermanentNumber,
                    PermanentStreet = objCitizen.PermanentStreet,
                    PermanentVillage = objCitizen.PermanentVillage,
                    PermanentBarangayId = objCitizen.PermanentBarangayId,
                    PermanentCityId = objCitizen.PermanentCityId,
                    PermanentProvinceId = objCitizen.PermanentProvinceId,
                    PermanentZipCode = objCitizen.PermanentZipCode,
                    TelephoneNumber = objCitizen.TelephoneNumber,
                    MobileNumber = objCitizen.MobileNumber,
                    EmailAddress = objCitizen.EmailAddress,
                    OccupationId = objCitizen.OccupationId,
                    EmployerName = objCitizen.EmployerName,
                    EmployerAddress = objCitizen.EmployerAddress,
                    EmployerTelephoneNumber = objCitizen.EmployerTelephoneNumber,
                    SpouseSurname = objCitizen.SpouseSurname,
                    SpouseFirstname = objCitizen.SpouseFirstname,
                    SpouseMiddlename = objCitizen.SpouseMiddlename,
                    SpouseExtensionname = objCitizen.SpouseExtensionname,
                    SpouseOccupationId = objCitizen.SpouseOccupationId,
                    SpouseEmployerName = objCitizen.SpouseEmployerName,
                    SpouseEmployerAddress = objCitizen.SpouseEmployerAddress,
                    FatherSurname = objCitizen.FatherSurname,
                    FatherFirstname = objCitizen.FatherFirstname,
                    FatherMiddlename = objCitizen.FatherMiddlename,
                    FatherExtensionname = objCitizen.FatherExtensionname,
                    MotherSurname = objCitizen.MotherSurname,
                    MotherFirstname = objCitizen.MotherFirstname,
                    MotherMiddlename = objCitizen.MotherMiddlename,
                    MotherExtensionname = objCitizen.MotherExtensionname,
                    PictureURL = objCitizen.PictureURL,
                    CitizenStatusId = objCitizen.CitizenStatusId,
                    CreatedByUserId = currentUser.FirstOrDefault().Id,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = currentUser.FirstOrDefault().Id,
                    UpdatedDateTime = DateTime.Now
                };

                db.MstCitizens.InsertOnSubmit(newCitizen);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateCitizen(String id, Entities.MstCitizen objCitizen)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                var sex = from d in db.MstSexes where d.Id == objCitizen.SexId select d;
                if (!sex.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Sex not found!");
                }

                var civilStatus = from d in db.MstCivilStatus where d.Id == objCitizen.CitizenStatusId select d;
                if (!civilStatus.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Civil status not found!");
                }

                var bloodType = from d in db.MstBloodTypes where d.Id == objCitizen.BloodTypeId select d;
                if (!bloodType.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Blood type not found!");
                }

                var citizenship = from d in db.MstCitizenships where d.Id == objCitizen.CitizenshipId select d;
                if (!citizenship.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizenship not found!");
                }

                var typeOfCitizenship = from d in db.MstTypeOfCitizenships where d.Id == objCitizen.TypeOfCitizenshipId select d;
                if (!typeOfCitizenship.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Type of citizenship not found!");
                }

                var barangay = from d in db.MstBarangays where d.Id == objCitizen.ResidentialBarangayId && d.Id == objCitizen.PermanentBarangayId select d;
                if (!barangay.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Barangay not found!");
                }

                var city = from d in db.MstCities where d.Id == objCitizen.ResidentialCityId && d.Id == objCitizen.PermanentCityId select d;
                if (!city.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "City not found!");
                }

                var province = from d in db.MstProvinces where d.Id == objCitizen.ResidentialProvinceId && d.Id == objCitizen.PermanentProvinceId select d;
                if (!province.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Province not found!");
                }

                var occupation = from d in db.MstOccupations where d.Id == objCitizen.OccupationId && d.Id == objCitizen.SpouseOccupationId select d;
                if (!occupation.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Occupation not found!");
                }

                var citizenStatus = from d in db.MstCitizensStatus where d.Id == objCitizen.CitizenStatusId select d;
                if (!citizenStatus.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen status not found!");
                }

                var citizen = from d in db.MstCitizens
                              where d.Id == Convert.ToInt32(id)
                              select d;

                if (citizen.Any())
                {
                    var updateCitizen = citizen.FirstOrDefault();
                    updateCitizen.Surname = objCitizen.Surname;
                    updateCitizen.Firstname = objCitizen.Firstname;
                    updateCitizen.Middlename = objCitizen.Middlename;
                    updateCitizen.Extensionname = objCitizen.Extensionname;
                    updateCitizen.DateOfBirth = Convert.ToDateTime(objCitizen.DateOfBirth);
                    updateCitizen.PlaceOfBirth = objCitizen.PlaceOfBirth;
                    updateCitizen.SexId = objCitizen.SexId;
                    updateCitizen.CivilStatusId = objCitizen.CivilStatusId;
                    updateCitizen.Height = objCitizen.Height;
                    updateCitizen.Weight = objCitizen.Weight;
                    updateCitizen.BloodTypeId = objCitizen.BloodTypeId;
                    updateCitizen.GSISNumber = objCitizen.GSISNumber;
                    updateCitizen.HDMFNumber = objCitizen.HDMFNumber;
                    updateCitizen.PhilHealthNumber = objCitizen.PhilHealthNumber;
                    updateCitizen.SSSNumber = objCitizen.SSSNumber;
                    updateCitizen.TINNumber = objCitizen.TINNumber;
                    updateCitizen.AgencyEmployeeNumber = objCitizen.AgencyEmployeeNumber;
                    updateCitizen.CitizenshipId = objCitizen.CitizenshipId;
                    updateCitizen.TypeOfCitizenshipId = objCitizen.TypeOfCitizenshipId;
                    updateCitizen.DualCitizenshipCountry = objCitizen.DualCitizenshipCountry;
                    updateCitizen.ResidentialNumber = objCitizen.ResidentialNumber;
                    updateCitizen.ResidentialStreet = objCitizen.ResidentialStreet;
                    updateCitizen.ResidentialVillage = objCitizen.ResidentialVillage;
                    updateCitizen.ResidentialBarangayId = objCitizen.ResidentialBarangayId;
                    updateCitizen.ResidentialCityId = objCitizen.ResidentialCityId;
                    updateCitizen.ResidentialProvinceId = objCitizen.ResidentialProvinceId;
                    updateCitizen.ResidentialZipCode = objCitizen.ResidentialZipCode;
                    updateCitizen.PermanentNumber = objCitizen.PermanentNumber;
                    updateCitizen.PermanentStreet = objCitizen.PermanentStreet;
                    updateCitizen.PermanentVillage = objCitizen.PermanentVillage;
                    updateCitizen.PermanentBarangayId = objCitizen.PermanentBarangayId;
                    updateCitizen.PermanentCityId = objCitizen.PermanentCityId;
                    updateCitizen.PermanentProvinceId = objCitizen.PermanentProvinceId;
                    updateCitizen.PermanentZipCode = objCitizen.PermanentZipCode;
                    updateCitizen.TelephoneNumber = objCitizen.TelephoneNumber;
                    updateCitizen.MobileNumber = objCitizen.MobileNumber;
                    updateCitizen.EmailAddress = objCitizen.EmailAddress;
                    updateCitizen.OccupationId = objCitizen.OccupationId;
                    updateCitizen.EmployerName = objCitizen.EmployerName;
                    updateCitizen.EmployerAddress = objCitizen.EmployerAddress;
                    updateCitizen.EmployerTelephoneNumber = objCitizen.EmployerTelephoneNumber;
                    updateCitizen.SpouseSurname = objCitizen.SpouseSurname;
                    updateCitizen.SpouseFirstname = objCitizen.SpouseFirstname;
                    updateCitizen.SpouseMiddlename = objCitizen.SpouseMiddlename;
                    updateCitizen.SpouseExtensionname = objCitizen.SpouseExtensionname;
                    updateCitizen.SpouseOccupationId = objCitizen.SpouseOccupationId;
                    updateCitizen.SpouseEmployerName = objCitizen.SpouseEmployerName;
                    updateCitizen.SpouseEmployerAddress = objCitizen.SpouseEmployerAddress;
                    updateCitizen.FatherSurname = objCitizen.FatherSurname;
                    updateCitizen.FatherFirstname = objCitizen.FatherFirstname;
                    updateCitizen.FatherMiddlename = objCitizen.FatherMiddlename;
                    updateCitizen.FatherExtensionname = objCitizen.FatherExtensionname;
                    updateCitizen.MotherSurname = objCitizen.MotherSurname;
                    updateCitizen.MotherFirstname = objCitizen.MotherFirstname;
                    updateCitizen.MotherMiddlename = objCitizen.MotherMiddlename;
                    updateCitizen.MotherExtensionname = objCitizen.MotherExtensionname;
                    updateCitizen.PictureURL = objCitizen.PictureURL;
                    updateCitizen.CitizenStatusId = objCitizen.CitizenStatusId;
                    updateCitizen.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    updateCitizen.UpdatedDateTime = DateTime.Now;
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
    }
}
