using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/province")]
    public class ApiMstProvinceController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstProvince> ProvinceList()
        {
            var provinces = from d in db.MstProvinces
                            select new Entities.MstProvince
                            {
                                Id = d.Id,
                                Province = d.Province,
                                RegionId = d.RegionId,
                                Region = d.MstRegion.Region,
                                CountryId = d.MstRegion.CountryId,
                                Country = d.MstRegion.MstCountry.Country
                            };

            return provinces.OrderByDescending(d => d.Id).ToList();
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

        [HttpPost, Route("add")]
        public HttpResponseMessage AddProvince(Entities.MstProvince objProvince)
        {
            try
            {
                var country = from d in db.MstCountries where d.Id == objProvince.CountryId select d;
                if (country.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Country not found!");
                }

                var region = from d in db.MstRegions where d.Id == objProvince.RegionId select d;
                if (region.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Region not found!");
                }

                Data.MstProvince newProvince = new Data.MstProvince()
                {
                    Province = objProvince.Province,
                    RegionId = objProvince.RegionId
                };

                db.MstProvinces.InsertOnSubmit(newProvince);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateProvince(String id, Entities.MstProvince objProvince)
        {
            try
            {
                var country = from d in db.MstCountries where d.Id == objProvince.CountryId select d;
                if (country.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Country not found!");
                }

                var region = from d in db.MstRegions where d.Id == objProvince.RegionId select d;
                if (region.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Region not found!");
                }

                var province = from d in db.MstProvinces
                               where d.Id == Convert.ToInt32(id)
                               select d;

                if (province.Any())
                {
                    var updateProvince = province.FirstOrDefault();
                    updateProvince.Province = objProvince.Province;
                    updateProvince.RegionId = objProvince.RegionId;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Province not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteProvince(String id)
        {
            try
            {
                var province = from d in db.MstProvinces
                               where d.Id == Convert.ToInt32(id)
                               select d;

                if (province.Any())
                {
                    var deleteProvince = province.FirstOrDefault();
                    db.MstProvinces.DeleteOnSubmit(deleteProvince);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Province not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
