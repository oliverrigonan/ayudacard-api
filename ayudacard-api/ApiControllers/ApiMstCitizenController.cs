using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/citizen")]
    public class ApiMstCitizenController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstCitizen> List()
        {
            var citizens = from d in db.MstCitizens
                           select new Entities.MstCitizen
                           {
                               Id = d.Id,
                               Surname = d.Surname,
                               Firstname = d.Firstname,
                               Middlename = d.Middlename ?? "",
                               Extensionname = d.Extensionname ?? "",
                               DateOfBirth = d.DateOfBirth.ToShortDateString(),
                               PlaceOfBirth = d.PlaceOfBirth,
                               Sex = d.MstSex.Sex,
                               CivilStatus = d.MstCivilStatus.CivilStatus,
                               Citizenship = d.MstCitizenship.Citizenship,
                               CitizenStatus = d.MstCitizensStatus.CitizenStatus,
                               MobileNumber = d.MobileNumber
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
    }
}
