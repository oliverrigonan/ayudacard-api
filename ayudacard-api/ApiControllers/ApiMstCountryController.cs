using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/country")]
    public class ApiMstCountryController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstCountry> CountryList()
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
        public HttpResponseMessage AddCountry(Entities.MstCountry objCountry)
        {
            try
            {
                Data.MstCountry newCountry = new Data.MstCountry()
                {
                    Country = objCountry.Country
                };

                db.MstCountries.InsertOnSubmit(newCountry);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateCountry(String id, Entities.MstCountry objCountry)
        {
            try
            {
                var country = from d in db.MstCountries
                              where d.Id == Convert.ToInt32(id)
                              select d;

                if (country.Any())
                {
                    var updateCountry = country.FirstOrDefault();
                    updateCountry.Country = objCountry.Country;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Country not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteCountry(String id)
        {
            try
            {
                var country = from d in db.MstCountries
                              where d.Id == Convert.ToInt32(id)
                              select d;

                if (country.Any())
                {
                    var deleteCountry = country.FirstOrDefault();
                    db.MstCountries.DeleteOnSubmit(deleteCountry);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Country not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
