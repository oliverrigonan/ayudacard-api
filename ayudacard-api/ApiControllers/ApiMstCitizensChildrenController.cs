using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/citizen/children")]
    public class ApiMstCitizensChildrenController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list/{citizenId}")]
        public List<Entities.MstCitizensChildren> CitizensChildrenList(String citizenId)
        {
            var citizensChildren = from d in db.MstCitizensChildrens
                                   where d.CitizenId == Convert.ToInt32(citizenId)
                                   select new Entities.MstCitizensChildren
                                   {
                                       Id = d.Id,
                                       CitizenId = d.CitizenId,
                                       Fullname = d.Fullname,
                                       DateOfBirth = d.DateOfBirth.ToShortDateString()
                                   };

            return citizensChildren.ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddCitizensChild(Entities.MstCitizensChildren objCitizensChildren)
        {
            try
            {
                var citizen = from d in db.MstCitizens
                              where d.Id == objCitizensChildren.CitizenId
                              select d;

                if (!citizen.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen not found!");
                }

                Data.MstCitizensChildren newChild = new Data.MstCitizensChildren()
                {
                    CitizenId = objCitizensChildren.CitizenId,
                    Fullname = objCitizensChildren.Fullname,
                    DateOfBirth = Convert.ToDateTime(objCitizensChildren.DateOfBirth)
                };

                db.MstCitizensChildrens.InsertOnSubmit(newChild);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateCitizensChild(String id, Entities.MstCitizensChildren objCitizensChildren)
        {
            try
            {
                var child = from d in db.MstCitizensChildrens
                            where d.Id == Convert.ToInt32(id)
                            select d;

                if (child.Any())
                {
                    var updateChild = child.FirstOrDefault();
                    updateChild.Fullname = objCitizensChildren.Fullname;
                    updateChild.DateOfBirth = Convert.ToDateTime(objCitizensChildren.DateOfBirth);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Child not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteCitizensChild(String id)
        {
            try
            {
                var child = from d in db.MstCitizensChildrens
                            where d.Id == Convert.ToInt32(id)
                            select d;

                if (child.Any())
                {
                    var deleteChild = child.FirstOrDefault();
                    db.MstCitizensChildrens.DeleteOnSubmit(deleteChild);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Child not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
