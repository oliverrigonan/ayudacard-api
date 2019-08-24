using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/bloodType")]
    public class ApiMstBloodTypeController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstBloodType> BloodTypeList()
        {
            var bloodTypes = from d in db.MstBloodTypes
                             select new Entities.MstBloodType
                             {
                                 Id = d.Id,
                                 BloodType = d.BloodType
                             };

            return bloodTypes.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddBloodType(Entities.MstBloodType objBloodType)
        {
            try
            {
                Data.MstBloodType newBloodType = new Data.MstBloodType()
                {
                    BloodType = objBloodType.BloodType
                };

                db.MstBloodTypes.InsertOnSubmit(newBloodType);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateBloodType(String id, Entities.MstBloodType objBloodType)
        {
            try
            {
                var bloodType = from d in db.MstBloodTypes
                                where d.Id == Convert.ToInt32(id)
                                select d;

                if (bloodType.Any())
                {
                    var updateBloodType = bloodType.FirstOrDefault();
                    updateBloodType.BloodType = objBloodType.BloodType;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Blood type not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteBloodType(String id)
        {
            try
            {
                var bloodType = from d in db.MstBloodTypes
                                where d.Id == Convert.ToInt32(id)
                                select d;

                if (bloodType.Any())
                {
                    var deleteBloodType = bloodType.FirstOrDefault();
                    db.MstBloodTypes.DeleteOnSubmit(deleteBloodType);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Blood type not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
