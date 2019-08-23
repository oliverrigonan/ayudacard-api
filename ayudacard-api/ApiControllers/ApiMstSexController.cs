using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/sex")]
    public class ApiMstSexController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstSex> SexList()
        {
            var sexes = from d in db.MstSexes
                        select new Entities.MstSex
                        {
                            Id = d.Id,
                            Sex = d.Sex
                        };

            return sexes.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddSex(Entities.MstSex objSex)
        {
            try
            {
                Data.MstSex newSex = new Data.MstSex()
                {
                    Sex = objSex.Sex
                };

                db.MstSexes.InsertOnSubmit(newSex);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateSex(String id, Entities.MstSex objSex)
        {
            try
            {
                var sex = from d in db.MstSexes
                          where d.Id == Convert.ToInt32(id)
                          select d;

                if (sex.Any())
                {
                    var updateSex = sex.FirstOrDefault();
                    updateSex.Sex = objSex.Sex;
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
        public HttpResponseMessage DeleteSex(String id)
        {
            try
            {
                var sex = from d in db.MstSexes
                          where d.Id == Convert.ToInt32(id)
                          select d;

                if (sex.Any())
                {
                    var deleteSex = sex.FirstOrDefault();
                    db.MstSexes.DeleteOnSubmit(deleteSex);
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
