using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/service/group")]
    public class ApiMstServiceGroupController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstServiceGroup> ServiceGroupList()
        {
            var serviceGroups = from d in db.MstServiceGroups
                                select new Entities.MstServiceGroup
                                {
                                    Id = d.Id,
                                    ServiceGroup = d.ServiceGroup,
                                    Description = d.Description,
                                    ServiceDepartmentId = d.ServiceDepartmentId,
                                    ServiceDepartment = d.MstServiceDepartment.ServiceDepartment
                                };

            return serviceGroups.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("detail/{id}")]
        public Entities.MstServiceGroup ServiceGroupDetail(String id)
        {
            var serviceGroup = from d in db.MstServiceGroups
                               where d.Id == Convert.ToInt32(id)
                               select new Entities.MstServiceGroup
                               {
                                   Id = d.Id,
                                   ServiceGroup = d.ServiceGroup,
                                   Description = d.Description,
                                   ServiceDepartmentId = d.ServiceDepartmentId,
                                   ServiceDepartment = d.MstServiceDepartment.ServiceDepartment
                               };

            return serviceGroup.FirstOrDefault();
        }

        [HttpGet, Route("serviceDepartment/dropdown/list")]
        public List<Entities.MstServiceDepartment> ServiceDepartmentDropdownList()
        {
            var serviceDepartment = from d in db.MstServiceDepartments
                                    select new Entities.MstServiceDepartment
                                    {
                                        Id = d.Id,
                                        ServiceDepartment = d.ServiceDepartment
                                    };

            return serviceDepartment.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddServiceGroup()
        {
            try
            {
                var serviceDepartment = from d in db.MstServiceDepartments select d;
                if (serviceDepartment.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service department not found!");
                }

                Data.MstServiceGroup newServiceGroup = new Data.MstServiceGroup()
                {
                    ServiceGroup = "",
                    Description = "",
                    ServiceDepartmentId = serviceDepartment.FirstOrDefault().Id
                };

                db.MstServiceGroups.InsertOnSubmit(newServiceGroup);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, newServiceGroup.Id);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateServiceGroup(String id, Entities.MstServiceGroup objServiceGroup)
        {
            try
            {
                var serviceGroup = from d in db.MstServiceGroups
                                   where d.Id == Convert.ToInt32(id)
                                   select d;

                if (serviceGroup.Any())
                {
                    var serviceDepartment = from d in db.MstServiceDepartments where d.Id == objServiceGroup.ServiceDepartmentId select d;
                    if (serviceDepartment.Any() == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Service department not found!");
                    }

                    var updateServiceGroup = serviceGroup.FirstOrDefault();
                    updateServiceGroup.ServiceGroup = objServiceGroup.ServiceGroup;
                    updateServiceGroup.Description = objServiceGroup.Description;
                    updateServiceGroup.ServiceDepartmentId = objServiceGroup.ServiceDepartmentId;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service group not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteServiceGroup(String id)
        {
            try
            {
                var serviceGroup = from d in db.MstServiceGroups
                                   where d.Id == Convert.ToInt32(id)
                                   select d;

                if (serviceGroup.Any())
                {
                    var deleteServiceGroup = serviceGroup.FirstOrDefault();
                    db.MstServiceGroups.DeleteOnSubmit(deleteServiceGroup);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service group not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
