using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/city")]
    public class ApiMstCityController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstCity> CityList()
        {
            var cities = from d in db.MstCities
                         select new Entities.MstCity
                         {
                             Id = d.Id,
                             City = d.City,
                             ProvinceId = d.ProvinceId,
                             Province = d.MstProvince.Province,
                             RegionId = d.MstProvince.RegionId,
                             Region = d.MstProvince.MstRegion.Region,
                             CountryId = d.MstProvince.MstRegion.CountryId,
                             Country = d.MstProvince.MstRegion.MstCountry.Country
                         };

            return cities.OrderByDescending(d => d.Id).ToList();
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

        [HttpPost, Route("add")]
        public HttpResponseMessage AddCity(Entities.MstCity objCity)
        {
            try
            {
                var country = from d in db.MstCountries where d.Id == objCity.CountryId select d;
                if (country.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Country not found!");
                }

                var region = from d in db.MstRegions where d.Id == objCity.RegionId select d;
                if (region.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Region not found!");
                }

                var province = from d in db.MstProvinces where d.Id == objCity.ProvinceId select d;
                if (province.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Province not found!");
                }

                Data.MstCity newCity = new Data.MstCity()
                {
                    City = objCity.City,
                    ProvinceId = objCity.ProvinceId
                };

                db.MstCities.InsertOnSubmit(newCity);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateCity(String id, Entities.MstCity objCity)
        {
            try
            {
                var country = from d in db.MstCountries where d.Id == objCity.CountryId select d;
                if (country.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Country not found!");
                }

                var region = from d in db.MstRegions where d.Id == objCity.RegionId select d;
                if (region.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Region not found!");
                }

                var province = from d in db.MstProvinces where d.Id == objCity.ProvinceId select d;
                if (province.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Province not found!");
                }

                var city = from d in db.MstCities
                           where d.Id == Convert.ToInt32(id)
                           select d;

                if (city.Any())
                {
                    var updateCity = city.FirstOrDefault();
                    updateCity.City = objCity.City;
                    updateCity.ProvinceId = objCity.ProvinceId;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "City not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteCity(String id)
        {
            try
            {
                var city = from d in db.MstCities
                           where d.Id == Convert.ToInt32(id)
                           select d;

                if (city.Any())
                {
                    var deleteCity = city.FirstOrDefault();
                    db.MstCities.DeleteOnSubmit(deleteCity);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "City not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
