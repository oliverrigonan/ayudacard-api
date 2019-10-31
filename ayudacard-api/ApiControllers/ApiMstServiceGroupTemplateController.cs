using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/service/group/template")]
    public class ApiMstServiceGroupTemplateController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstServiceGroupTemplate> ServiceGroupTemplateList()
        {
            var serviceGroupTemplates = from d in db.MstServiceGroupTemplates
                                        select new Entities.MstServiceGroupTemplate
                                        {
                                            Id = d.Id,
                                            ServiceGroupId = d.ServiceGroupId,
                                            ServiceGroup = d.MstServiceGroup.ServiceGroup,
                                            TemplateName = d.TemplateName,
                                            Problem = d.Problem,
                                            Background = d.Background,
                                            Recommendation = d.Recommendation
                                        };

            return serviceGroupTemplates.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("serviceGroup/dropdown/list")]
        public List<Entities.MstServiceGroup> ServiceGroupDropdownList()
        {
            var serviceGroup = from d in db.MstServiceGroups
                               select new Entities.MstServiceGroup
                               {
                                   Id = d.Id,
                                   ServiceGroup = d.ServiceGroup
                               };

            return serviceGroup.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddServiceGroupTemplate(Entities.MstServiceGroupTemplate objServiceGroupTemplate)
        {
            try
            {
                var serviceGroup = from d in db.MstServiceGroups where d.Id == objServiceGroupTemplate.ServiceGroupId select d;
                if (serviceGroup.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service group not found!");
                }

                Data.MstServiceGroupTemplate newServiceGroupTemplate = new Data.MstServiceGroupTemplate()
                {
                    ServiceGroupId = objServiceGroupTemplate.ServiceGroupId,
                    TemplateName = objServiceGroupTemplate.TemplateName,
                    Problem = objServiceGroupTemplate.Problem,
                    Background = objServiceGroupTemplate.Background,
                    Recommendation = objServiceGroupTemplate.Recommendation,
                };

                db.MstServiceGroupTemplates.InsertOnSubmit(newServiceGroupTemplate);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateServiceGroupTemplate(String id, Entities.MstServiceGroupTemplate objServiceGroupTemplate)
        {
            try
            {
                var serviceGroupTemplate = from d in db.MstServiceGroupTemplates
                                           where d.Id == Convert.ToInt32(id)
                                           select d;

                if (serviceGroupTemplate.Any())
                {
                    var serviceGroup = from d in db.MstServiceGroups where d.Id == objServiceGroupTemplate.ServiceGroupId select d;
                    if (serviceGroup.Any() == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Service group not found!");
                    }

                    var updateServiceGroupTemplate = serviceGroupTemplate.FirstOrDefault();
                    updateServiceGroupTemplate.ServiceGroupId = objServiceGroupTemplate.ServiceGroupId;
                    updateServiceGroupTemplate.TemplateName = objServiceGroupTemplate.TemplateName;
                    updateServiceGroupTemplate.Problem = objServiceGroupTemplate.Problem;
                    updateServiceGroupTemplate.Background = objServiceGroupTemplate.Background;
                    updateServiceGroupTemplate.Recommendation = objServiceGroupTemplate.Recommendation;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service group template not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteServiceGroupTemplate(String id)
        {
            try
            {
                var serviceGroupTemplate = from d in db.MstServiceGroupTemplates
                                           where d.Id == Convert.ToInt32(id)
                                           select d;

                if (serviceGroupTemplate.Any())
                {
                    var deleteServiceGroupTemplate = serviceGroupTemplate.FirstOrDefault();
                    db.MstServiceGroupTemplates.DeleteOnSubmit(deleteServiceGroupTemplate);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service group template not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
