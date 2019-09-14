using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/service")]
    public class ApiMstServiceController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstService> ServiceList()
        {
            var services = from d in db.MstServices
                           select new Entities.MstService
                           {
                               Id = d.Id,
                               Service = d.Service,
                               Description = d.Description,
                               ServiceDepartmentId = d.MstServiceGroup.ServiceDepartmentId,
                               ServiceDepartment = d.MstServiceGroup.MstServiceDepartment.ServiceDepartment,
                               ServiceGroupId = d.ServiceGroupId,
                               ServiceGroup = d.MstServiceGroup.ServiceGroup,
                               DateEncoded = d.DateEncoded.ToShortDateString(),
                               DateExpiry = d.DateExpiry != null ? Convert.ToDateTime(d.DateExpiry).ToShortDateString() : null,
                               LimitAmount = d.LimitAmount,
                               IsMultipleUse = d.IsMultipleUse,
                               StatusId = d.StatusId,
                               Status = d.MstStatus.Status,
                               IsLocked = d.IsLocked,
                               CreatedByUserId = d.CreatedByUserId,
                               CreatedByUser = d.MstUser.Fullname,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedByUserId = d.UpdatedByUserId,
                               UpdatedByUser = d.MstUser1.Fullname,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };

            return services.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("detail/{id}")]
        public Entities.MstService ServiceDetail(String id)
        {
            var service = from d in db.MstServices
                          where d.Id == Convert.ToInt32(id)
                          select new Entities.MstService
                          {
                              Id = d.Id,
                              Service = d.Service,
                              Description = d.Description,
                              ServiceDepartmentId = d.MstServiceGroup.ServiceDepartmentId,
                              ServiceDepartment = d.MstServiceGroup.MstServiceDepartment.ServiceDepartment,
                              ServiceGroupId = d.ServiceGroupId,
                              ServiceGroup = d.MstServiceGroup.ServiceGroup,
                              DateEncoded = d.DateEncoded.ToShortDateString(),
                              DateExpiry = d.DateExpiry != null ? Convert.ToDateTime(d.DateExpiry).ToShortDateString() : null,
                              LimitAmount = d.LimitAmount,
                              IsMultipleUse = d.IsMultipleUse,
                              StatusId = d.StatusId,
                              Status = d.MstStatus.Status,
                              IsLocked = d.IsLocked,
                              CreatedByUserId = d.CreatedByUserId,
                              CreatedByUser = d.MstUser.Fullname,
                              CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                              UpdatedByUserId = d.UpdatedByUserId,
                              UpdatedByUser = d.MstUser1.Fullname,
                              UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                          };

            return service.FirstOrDefault();
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

        [HttpGet, Route("serviceGroup/dropdown/list/{serviceDepartmentId}")]
        public List<Entities.MstServiceGroup> ServiceGroupDropdownList(String serviceDepartmentId)
        {
            var serviceGroup = from d in db.MstServiceGroups
                               where d.ServiceDepartmentId == Convert.ToInt32(serviceDepartmentId)
                               select new Entities.MstServiceGroup
                               {
                                   Id = d.Id,
                                   ServiceGroup = d.ServiceGroup
                               };

            return serviceGroup.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("status/dropdown/list")]
        public List<Entities.MstStatus> StatusDropdownList()
        {
            var status = from d in db.MstStatus
                         where d.Category.Equals("Service")
                         select new Entities.MstStatus
                         {
                             Id = d.Id,
                             Status = d.Status
                         };

            return status.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddService()
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                var serviceDepartment = from d in db.MstServiceDepartments select d;
                if (serviceDepartment.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service department not found!");
                }

                var serviceGroup = from d in db.MstServiceGroups select d;
                if (serviceGroup.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service group not found!");
                }

                var status = from d in db.MstStatus
                             where d.Category.Equals("Service")
                             select d;

                if (status.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Status not found!");
                }

                Data.MstService newService = new Data.MstService()
                {
                    Service = "",
                    Description = "",
                    ServiceGroupId = serviceGroup.FirstOrDefault().Id,
                    DateEncoded = DateTime.Today,
                    DateExpiry = DateTime.Today.AddYears(1),
                    LimitAmount = 0,
                    IsMultipleUse = false,
                    StatusId = status.FirstOrDefault().Id,
                    IsLocked = false,
                    CreatedByUserId = currentUser.FirstOrDefault().Id,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = currentUser.FirstOrDefault().Id,
                    UpdatedDateTime = DateTime.Now
                };

                db.MstServices.InsertOnSubmit(newService);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, newService.Id);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("save/{id}")]
        public HttpResponseMessage SaveService(String id, Entities.MstService objService)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                var serviceDepartment = from d in db.MstServiceDepartments where d.Id == objService.ServiceDepartmentId select d;
                if (serviceDepartment.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service department not found!");
                }

                var serviceGroup = from d in db.MstServiceGroups where d.Id == objService.ServiceGroupId select d;
                if (serviceGroup.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service group not found!");
                }

                var status = from d in db.MstStatus
                             where d.Id == objService.StatusId
                             && d.Category.Equals("Service")
                             select d;

                if (status.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Status not found!");
                }

                var service = from d in db.MstServices
                              where d.Id == Convert.ToInt32(id)
                              select d;

                if (service.Any())
                {
                    DateTime? dateExpiry = null;
                    if (objService.DateExpiry != null)
                    {
                        dateExpiry = Convert.ToDateTime(objService.DateExpiry);
                    }

                    var saveService = service.FirstOrDefault();
                    saveService.Service = objService.Service;
                    saveService.Description = objService.Description;
                    saveService.ServiceGroupId = objService.ServiceGroupId;
                    saveService.DateEncoded = Convert.ToDateTime(objService.DateEncoded);
                    saveService.DateExpiry = dateExpiry;
                    saveService.LimitAmount = objService.LimitAmount;
                    saveService.IsMultipleUse = objService.IsMultipleUse;
                    saveService.StatusId = objService.StatusId;
                    saveService.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    saveService.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service  not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("lock/{id}")]
        public HttpResponseMessage LockService(String id, Entities.MstService objService)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                var serviceDepartment = from d in db.MstServiceDepartments where d.Id == objService.ServiceDepartmentId select d;
                if (serviceDepartment.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service department not found!");
                }

                var serviceGroup = from d in db.MstServiceGroups where d.Id == objService.ServiceGroupId select d;
                if (serviceGroup.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service group not found!");
                }

                var status = from d in db.MstStatus
                             where d.Id == objService.StatusId
                             && d.Category.Equals("Service")
                             select d;

                if (status.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Status not found!");
                }

                var service = from d in db.MstServices
                              where d.Id == Convert.ToInt32(id)
                              select d;

                if (service.Any())
                {
                    if (service.FirstOrDefault().IsLocked == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Already locked!");
                    }

                    DateTime? dateExpiry = null;
                    if (objService.DateExpiry != null)
                    {
                        dateExpiry = Convert.ToDateTime(objService.DateExpiry);
                    }

                    var lockService = service.FirstOrDefault();
                    lockService.Service = objService.Service;
                    lockService.Description = objService.Description;
                    lockService.ServiceGroupId = objService.ServiceGroupId;
                    lockService.DateEncoded = Convert.ToDateTime(objService.DateEncoded);
                    lockService.DateExpiry = dateExpiry;
                    lockService.LimitAmount = objService.LimitAmount;
                    lockService.IsMultipleUse = objService.IsMultipleUse;
                    lockService.StatusId = objService.StatusId;
                    lockService.IsLocked = true;
                    lockService.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    lockService.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service  not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("unlock/{id}")]
        public HttpResponseMessage UnlockService(String id)
        {
            try
            {
                var service = from d in db.MstServices
                              where d.Id == Convert.ToInt32(id)
                              select d;

                if (service.Any())
                {
                    if (service.FirstOrDefault().IsLocked == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Already unlocked!");
                    }

                    var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                    var unlockService = service.FirstOrDefault();
                    unlockService.IsLocked = false;
                    unlockService.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    unlockService.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteService(String id)
        {
            try
            {
                var service = from d in db.MstServices
                              where d.Id == Convert.ToInt32(id)
                              select d;

                if (service.Any())
                {
                    if (service.FirstOrDefault().IsLocked == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Service cannot be deleted if locked.");
                    }

                    var deleteService = service.FirstOrDefault();
                    db.MstServices.DeleteOnSubmit(deleteService);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
