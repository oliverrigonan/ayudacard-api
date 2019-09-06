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

            return citizensChildren.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddCitizensChildren(Entities.MstCitizensChildren objCitizensChildren)
        {
            try
            {
                var citizen = from d in db.MstCitizens
                              where d.Id == objCitizensChildren.CitizenId
                              select d;

                if (citizen.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen not found!");
                }

                Data.MstCitizensChildren newChildren = new Data.MstCitizensChildren()
                {
                    CitizenId = objCitizensChildren.CitizenId,
                    Fullname = objCitizensChildren.Fullname,
                    DateOfBirth = Convert.ToDateTime(objCitizensChildren.DateOfBirth)
                };

                db.MstCitizensChildrens.InsertOnSubmit(newChildren);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateCitizensChildren(String id, Entities.MstCitizensChildren objCitizensChildren)
        {
            try
            {
                var children = from d in db.MstCitizensChildrens
                               where d.Id == Convert.ToInt32(id)
                               select d;

                if (children.Any())
                {
                    var citizen = from d in db.MstCitizens
                                  where d.Id == objCitizensChildren.CitizenId
                                  select d;

                    if (citizen.Any() == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen not found!");
                    }

                    var updateChildren = children.FirstOrDefault();
                    updateChildren.Fullname = objCitizensChildren.Fullname;
                    updateChildren.DateOfBirth = Convert.ToDateTime(objCitizensChildren.DateOfBirth);
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
        public HttpResponseMessage DeleteCitizensChildren(String id)
        {
            try
            {
                var children = from d in db.MstCitizensChildrens
                               where d.Id == Convert.ToInt32(id)
                               select d;

                if (children.Any())
                {
                    var deleteChildren = children.FirstOrDefault();
                    db.MstCitizensChildrens.DeleteOnSubmit(deleteChildren);
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
