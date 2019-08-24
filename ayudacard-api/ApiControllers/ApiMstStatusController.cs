using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/status")]
    public class ApiMstStatusController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstStatus> StatusList()
        {
            var statuses = from d in db.MstStatus
                           select new Entities.MstStatus
                           {
                               Id = d.Id,
                               Status = d.Status,
                               Category = d.Category
                           };

            return statuses.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddStatus(Entities.MstStatus objStatus)
        {
            try
            {
                Data.MstStatus newStatus = new Data.MstStatus()
                {
                    Status = objStatus.Status,
                    Category = objStatus.Category
                };

                db.MstStatus.InsertOnSubmit(newStatus);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateStatus(String id, Entities.MstStatus objStatus)
        {
            try
            {
                var status = from d in db.MstStatus
                             where d.Id == Convert.ToInt32(id)
                             select d;

                if (status.Any())
                {
                    var updateStatus = status.FirstOrDefault();
                    updateStatus.Status = objStatus.Status;
                    updateStatus.Category = objStatus.Category;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Status not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteStatus(String id)
        {
            try
            {
                var status = from d in db.MstStatus
                             where d.Id == Convert.ToInt32(id)
                             select d;

                if (status.Any())
                {
                    var deleteStatus = status.FirstOrDefault();
                    db.MstStatus.DeleteOnSubmit(deleteStatus);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Status not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
