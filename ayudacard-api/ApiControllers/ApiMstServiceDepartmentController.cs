using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/serviceDepartment")]
    public class ApiMstServiceDepartmentController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstServiceDepartment> ServiceDepartmentList()
        {
            var serviceDepartments = from d in db.MstServiceDepartments
                                     select new Entities.MstServiceDepartment
                                     {
                                         Id = d.Id,
                                         ServiceDepartment = d.ServiceDepartment,
                                         OfficerInCharge = d.OfficerInCharge,
                                         ContactNumber = d.ContactNumber,
                                         EmailAddress = d.EmailAddress
                                     };

            return serviceDepartments.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddServiceDepartment(Entities.MstServiceDepartment objServiceDepartment)
        {
            try
            {
                Data.MstServiceDepartment newServiceDepartment = new Data.MstServiceDepartment()
                {
                    ServiceDepartment = objServiceDepartment.ServiceDepartment,
                    OfficerInCharge = objServiceDepartment.OfficerInCharge,
                    ContactNumber = objServiceDepartment.ContactNumber,
                    EmailAddress = objServiceDepartment.EmailAddress
                };

                db.MstServiceDepartments.InsertOnSubmit(newServiceDepartment);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateServiceDepartment(String id, Entities.MstServiceDepartment objServiceDepartment)
        {
            try
            {
                var serviceDepartment = from d in db.MstServiceDepartments
                                        where d.Id == Convert.ToInt32(id)
                                        select d;

                if (serviceDepartment.Any())
                {
                    var updateServiceDepartment = serviceDepartment.FirstOrDefault();
                    updateServiceDepartment.ServiceDepartment = objServiceDepartment.ServiceDepartment;
                    updateServiceDepartment.OfficerInCharge = objServiceDepartment.OfficerInCharge;
                    updateServiceDepartment.ContactNumber = objServiceDepartment.ContactNumber;
                    updateServiceDepartment.EmailAddress = objServiceDepartment.EmailAddress;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service department not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteServiceDepartment(String id)
        {
            try
            {
                var serviceDepartment = from d in db.MstServiceDepartments
                                        where d.Id == Convert.ToInt32(id)
                                        select d;

                if (serviceDepartment.Any())
                {
                    var deleteServiceDepartment = serviceDepartment.FirstOrDefault();
                    db.MstServiceDepartments.DeleteOnSubmit(deleteServiceDepartment);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service department not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
