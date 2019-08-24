using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/educationLevel")]
    public class ApiMstEducationLevelController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstEducationLevel> EducationLevelList()
        {
            var educationLevels = from d in db.MstEducationLevels
                                  select new Entities.MstEducationLevel
                                  {
                                      Id = d.Id,
                                      EducationLevel = d.EducationLevel
                                  };

            return educationLevels.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddEducationLevel(Entities.MstEducationLevel objEducationLevel)
        {
            try
            {
                Data.MstEducationLevel newEducationLevel = new Data.MstEducationLevel()
                {
                    EducationLevel = objEducationLevel.EducationLevel
                };

                db.MstEducationLevels.InsertOnSubmit(newEducationLevel);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateEducationLevel(String id, Entities.MstEducationLevel objEducationLevel)
        {
            try
            {
                var educationLevel = from d in db.MstEducationLevels
                                     where d.Id == Convert.ToInt32(id)
                                     select d;

                if (educationLevel.Any())
                {
                    var updateEducationLevel = educationLevel.FirstOrDefault();
                    updateEducationLevel.EducationLevel = objEducationLevel.EducationLevel;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Education level not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteEducationLevel(String id)
        {
            try
            {
                var educationLevel = from d in db.MstEducationLevels
                                     where d.Id == Convert.ToInt32(id)
                                     select d;

                if (educationLevel.Any())
                {
                    var deleteEducationLevel = educationLevel.FirstOrDefault();
                    db.MstEducationLevels.DeleteOnSubmit(deleteEducationLevel);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Education level not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
