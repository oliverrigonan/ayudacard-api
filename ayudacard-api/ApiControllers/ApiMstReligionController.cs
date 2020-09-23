using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/religion")]
    public class ApiMstReligionController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstReligion> ReligionList()
        {
            var religiones = from d in db.MstReligions
                             select new Entities.MstReligion
                             {
                                 Id = d.Id,
                                 Religion = d.Religion
                             };

            return religiones.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddReligion(Entities.MstReligion objReligion)
        {
            try
            {
                Data.MstReligion newReligion = new Data.MstReligion()
                {
                    Religion = objReligion.Religion
                };

                db.MstReligions.InsertOnSubmit(newReligion);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateReligion(String id, Entities.MstReligion objReligion)
        {
            try
            {
                var religion = from d in db.MstReligions
                               where d.Id == Convert.ToInt32(id)
                               select d;

                if (religion.Any())
                {
                    var updateReligion = religion.FirstOrDefault();
                    updateReligion.Religion = objReligion.Religion;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Gender not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteReligion(String id)
        {
            try
            {
                var religion = from d in db.MstReligions
                               where d.Id == Convert.ToInt32(id)
                               select d;

                if (religion.Any())
                {
                    var deleteReligion = religion.FirstOrDefault();
                    db.MstReligions.DeleteOnSubmit(deleteReligion);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Gender not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
