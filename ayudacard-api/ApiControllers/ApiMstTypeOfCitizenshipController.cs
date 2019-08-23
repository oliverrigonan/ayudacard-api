using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/typeOfCitizenship")]
    public class ApiMstTypeOfTypeOfCitizenshipController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstTypeOfCitizenship> TypeOfCitizenshipList()
        {
            var typeOfCitizenships = from d in db.MstTypeOfCitizenships
                                     select new Entities.MstTypeOfCitizenship
                                     {
                                         Id = d.Id,
                                         TypeOfCitizenship = d.TypeOfCitizenship
                                     };

            return typeOfCitizenships.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddTypeOfCitizenship(Entities.MstTypeOfCitizenship objTypeOfCitizenship)
        {
            try
            {
                Data.MstTypeOfCitizenship newTypeOfCitizenship = new Data.MstTypeOfCitizenship()
                {
                    TypeOfCitizenship = objTypeOfCitizenship.TypeOfCitizenship
                };

                db.MstTypeOfCitizenships.InsertOnSubmit(newTypeOfCitizenship);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateTypeOfCitizenship(String id, Entities.MstTypeOfCitizenship objTypeOfCitizenship)
        {
            try
            {
                var typeOfCitizenship = from d in db.MstTypeOfCitizenships
                                        where d.Id == Convert.ToInt32(id)
                                        select d;

                if (typeOfCitizenship.Any())
                {
                    var updateTypeOfCitizenship = typeOfCitizenship.FirstOrDefault();
                    updateTypeOfCitizenship.TypeOfCitizenship = objTypeOfCitizenship.TypeOfCitizenship;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizenship type not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteTypeOfCitizenship(String id)
        {
            try
            {
                var typeOfCitizenship = from d in db.MstTypeOfCitizenships
                                        where d.Id == Convert.ToInt32(id)
                                        select d;

                if (typeOfCitizenship.Any())
                {
                    var deleteTypeOfCitizenship = typeOfCitizenship.FirstOrDefault();
                    db.MstTypeOfCitizenships.DeleteOnSubmit(deleteTypeOfCitizenship);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizenship type not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
