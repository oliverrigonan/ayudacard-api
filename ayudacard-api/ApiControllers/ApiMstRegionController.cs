using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/region")]
    public class ApiMstRegionController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstRegion> RegionList()
        {
            var regions = from d in db.MstRegions
                          select new Entities.MstRegion
                          {
                              Id = d.Id,
                              Region = d.Region,
                              CountryId = d.CountryId,
                              Country = d.MstCountry.Country
                          };

            return regions.OrderByDescending(d => d.Id).ToList();
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

        [HttpPost, Route("add")]
        public HttpResponseMessage AddRegion(Entities.MstRegion objRegion)
        {
            try
            {
                var country = from d in db.MstCountries where d.Id == objRegion.CountryId select d;
                if (country.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Country not found!");
                }

                Data.MstRegion newRegion = new Data.MstRegion()
                {
                    Region = objRegion.Region,
                    CountryId = objRegion.CountryId
                };

                db.MstRegions.InsertOnSubmit(newRegion);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateRegion(String id, Entities.MstRegion objRegion)
        {
            try
            {
                var country = from d in db.MstCountries where d.Id == objRegion.CountryId select d;
                if (country.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Country not found!");
                }

                var region = from d in db.MstRegions
                             where d.Id == Convert.ToInt32(id)
                             select d;

                if (region.Any())
                {
                    var updateRegion = region.FirstOrDefault();
                    updateRegion.Region = objRegion.Region;
                    updateRegion.CountryId = objRegion.CountryId;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Region not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteRegion(String id)
        {
            try
            {
                var region = from d in db.MstRegions
                             where d.Id == Convert.ToInt32(id)
                             select d;

                if (region.Any())
                {
                    var deleteRegion = region.FirstOrDefault();
                    db.MstRegions.DeleteOnSubmit(deleteRegion);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Region not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
