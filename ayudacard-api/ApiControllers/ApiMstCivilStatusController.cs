using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/civilStatus")]
    public class ApiMstCivilStatusController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstCivilStatus> CivilStatusList()
        {
            var civilStatuses = from d in db.MstCivilStatus
                                select new Entities.MstCivilStatus
                                {
                                    Id = d.Id,
                                    CivilStatus = d.CivilStatus
                                };

            return civilStatuses.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddCivilStatus(Entities.MstCivilStatus objCivilStatus)
        {
            try
            {
                Data.MstCivilStatus newCivilStatus = new Data.MstCivilStatus()
                {
                    CivilStatus = objCivilStatus.CivilStatus
                };

                db.MstCivilStatus.InsertOnSubmit(newCivilStatus);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateCivilStatus(String id, Entities.MstCivilStatus objCivilStatus)
        {
            try
            {
                var civilStatus = from d in db.MstCivilStatus
                                  where d.Id == Convert.ToInt32(id)
                                  select d;

                if (civilStatus.Any())
                {
                    var updateCivilStatus = civilStatus.FirstOrDefault();
                    updateCivilStatus.CivilStatus = objCivilStatus.CivilStatus;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Civil status not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteCivilStatus(String id)
        {
            try
            {
                var civilStatus = from d in db.MstCivilStatus
                                  where d.Id == Convert.ToInt32(id)
                                  select d;

                if (civilStatus.Any())
                {
                    var deleteCivilStatus = civilStatus.FirstOrDefault();
                    db.MstCivilStatus.DeleteOnSubmit(deleteCivilStatus);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Civil status not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
