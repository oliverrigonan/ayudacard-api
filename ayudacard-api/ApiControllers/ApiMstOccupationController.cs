using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/occupation")]
    public class ApiMstOccupationController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstOccupation> OccupationList()
        {
            var occupations = from d in db.MstOccupations
                              select new Entities.MstOccupation
                              {
                                  Id = d.Id,
                                  Occupation = d.Occupation
                              };

            return occupations.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddOccupation(Entities.MstOccupation objOccupation)
        {
            try
            {
                Data.MstOccupation newOccupation = new Data.MstOccupation()
                {
                    Occupation = objOccupation.Occupation
                };

                db.MstOccupations.InsertOnSubmit(newOccupation);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateOccupation(String id, Entities.MstOccupation objOccupation)
        {
            try
            {
                var occupation = from d in db.MstOccupations
                                 where d.Id == Convert.ToInt32(id)
                                 select d;

                if (occupation.Any())
                {
                    var updateOccupation = occupation.FirstOrDefault();
                    updateOccupation.Occupation = objOccupation.Occupation;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Occupation not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteOccupation(String id)
        {
            try
            {
                var occupation = from d in db.MstOccupations
                                 where d.Id == Convert.ToInt32(id)
                                 select d;

                if (occupation.Any())
                {
                    var deleteOccupation = occupation.FirstOrDefault();
                    db.MstOccupations.DeleteOnSubmit(deleteOccupation);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Occupation not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
