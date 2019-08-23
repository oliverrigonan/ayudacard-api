using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/citizenship")]
    public class ApiMstCitizenshipController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstCitizenship> CitizenshipList()
        {
            var citizenships = from d in db.MstCitizenships
                               select new Entities.MstCitizenship
                               {
                                   Id = d.Id,
                                   Citizenship = d.Citizenship
                               };

            return citizenships.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddCitizenship(Entities.MstCitizenship objCitizenship)
        {
            try
            {
                Data.MstCitizenship newCitizenship = new Data.MstCitizenship()
                {
                    Citizenship = objCitizenship.Citizenship
                };

                db.MstCitizenships.InsertOnSubmit(newCitizenship);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateCitizenship(String id, Entities.MstCitizenship objCitizenship)
        {
            try
            {
                var citizenship = from d in db.MstCitizenships
                                  where d.Id == Convert.ToInt32(id)
                                  select d;

                if (citizenship.Any())
                {
                    var updateCitizenship = citizenship.FirstOrDefault();
                    updateCitizenship.Citizenship = objCitizenship.Citizenship;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizenship not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteCitizenship(String id)
        {
            try
            {
                var citizenship = from d in db.MstCitizenships
                                  where d.Id == Convert.ToInt32(id)
                                  select d;

                if (citizenship.Any())
                {
                    var deleteCitizenship = citizenship.FirstOrDefault();
                    db.MstCitizenships.DeleteOnSubmit(deleteCitizenship);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizenship not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
