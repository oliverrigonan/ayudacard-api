using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/barangay")]
    public class ApiMstBarangayController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstBarangay> BarangayList()
        {
            var barangays = from d in db.MstBarangays
                            select new Entities.MstBarangay
                            {
                                Id = d.Id,
                                Barangay = d.Barangay,
                                BarangayChairman = d.BarangayChairman,
                                CityId = d.CityId,
                                City = d.MstCity.City,
                                ProvinceId = d.MstCity.ProvinceId,
                                Province = d.MstCity.MstProvince.Province,
                                RegionId = d.MstCity.MstProvince.RegionId,
                                Region = d.MstCity.MstProvince.MstRegion.Region,
                                CountryId = d.MstCity.MstProvince.MstRegion.CountryId,
                                Country = d.MstCity.MstProvince.MstRegion.MstCountry.Country
                            };

            return barangays.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("country/dropdown/list")]
        public List<Entities.MstCountry> CountryDropdownList()
        {
            var countries = from d in db.MstCountries
                            select new Entities.MstCountry
                            {
                                Id = d.Id,
                                Country = d.Country
                            };

            return countries.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("region/dropdown/list/{countryId}")]
        public List<Entities.MstRegion> RegionDropdownList(String countryId)
        {
            var regions = from d in db.MstRegions
                          where d.CountryId == Convert.ToInt32(countryId)
                          select new Entities.MstRegion
                          {
                              Id = d.Id,
                              Region = d.Region
                          };

            return regions.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("province/dropdown/list/{regionId}")]
        public List<Entities.MstProvince> ProvinceDropdownList(String regionId)
        {
            var provinces = from d in db.MstProvinces
                            where d.RegionId == Convert.ToInt32(regionId)
                            select new Entities.MstProvince
                            {
                                Id = d.Id,
                                Province = d.Province
                            };

            return provinces.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("city/dropdown/list/{provinceId}")]
        public List<Entities.MstCity> CityDropdownList(String provinceId)
        {
            var cities = from d in db.MstCities
                         where d.ProvinceId == Convert.ToInt32(provinceId)
                         select new Entities.MstCity
                         {
                             Id = d.Id,
                             City = d.City
                         };

            return cities.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddBarangay(Entities.MstBarangay objBarangay)
        {
            try
            {
                var country = from d in db.MstCountries where d.Id == objBarangay.CountryId select d;
                if (country.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Country not found!");
                }

                var region = from d in db.MstRegions where d.Id == objBarangay.RegionId select d;
                if (region.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Region not found!");
                }

                var province = from d in db.MstProvinces where d.Id == objBarangay.ProvinceId select d;
                if (province.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Province not found!");
                }

                var city = from d in db.MstCities where d.Id == objBarangay.CityId select d;
                if (city.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "City not found!");
                }

                Data.MstBarangay newBarangay = new Data.MstBarangay()
                {
                    Barangay = objBarangay.Barangay,
                    BarangayChairman = objBarangay.BarangayChairman,
                    CityId = objBarangay.CityId
                };

                db.MstBarangays.InsertOnSubmit(newBarangay);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateBarangay(String id, Entities.MstBarangay objBarangay)
        {
            try
            {
                var country = from d in db.MstCountries where d.Id == objBarangay.CountryId select d;
                if (country.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Country not found!");
                }

                var region = from d in db.MstRegions where d.Id == objBarangay.RegionId select d;
                if (region.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Region not found!");
                }

                var province = from d in db.MstProvinces where d.Id == objBarangay.ProvinceId select d;
                if (province.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Province not found!");
                }

                var city = from d in db.MstCities where d.Id == objBarangay.CityId select d;
                if (city.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "City not found!");
                }

                var barangay = from d in db.MstBarangays
                               where d.Id == Convert.ToInt32(id)
                               select d;

                if (barangay.Any())
                {
                    var updateBarangay = barangay.FirstOrDefault();
                    updateBarangay.Barangay = objBarangay.Barangay;
                    updateBarangay.BarangayChairman = objBarangay.BarangayChairman;
                    updateBarangay.CityId = objBarangay.CityId;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Barangay not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteBarangay(String id)
        {
            try
            {
                var barangay = from d in db.MstBarangays
                               where d.Id == Convert.ToInt32(id)
                               select d;

                if (barangay.Any())
                {
                    var deleteBarangay = barangay.FirstOrDefault();
                    db.MstBarangays.DeleteOnSubmit(deleteBarangay);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Barangay not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}