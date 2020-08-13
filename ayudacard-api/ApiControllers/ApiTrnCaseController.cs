using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ayudacard_api.Azure.AzureStorage;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace ayudacard_api.ApiControllers
{
    [RoutePrefix("api/trn/case")]
    public class ApiTrnCaseController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        public String FillLeadingZeroes(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

        [HttpGet, Route("serviceDepartment/dropdown/list")]
        public List<Entities.MstServiceDepartment> ServiceDepartmentDropdownList()
        {
            List<Entities.MstServiceDepartment> serviceDepartmentList = new List<Entities.MstServiceDepartment>();
            serviceDepartmentList.Add(new Entities.MstServiceDepartment()
            {
                Id = 0,
                ServiceDepartment = "ALL SERVICE DEPARTMENTS"
            });

            var serviceDepartments = from d in db.MstServiceDepartments
                                     select new Entities.MstServiceDepartment
                                     {
                                         Id = d.Id,
                                         ServiceDepartment = d.ServiceDepartment
                                     };

            if (serviceDepartments.Any())
            {
                foreach (var serviceDepartment in serviceDepartments)
                {
                    serviceDepartmentList.Add(serviceDepartment);
                }
            }

            return serviceDepartmentList.OrderBy(d => d.Id).ToList();
        }

        [HttpGet, Route("list/{serviceDepartmentId}/{startDate}/{endDate}")]
        public List<Entities.TrnCase> CasesList(String serviceDepartmentId, String startDate, String endDate)
        {
            if (Convert.ToInt32(serviceDepartmentId) == 0)
            {
                var cases = from d in db.TrnCases
                            where d.CaseDate >= Convert.ToDateTime(startDate)
                            && d.CaseDate <= Convert.ToDateTime(endDate)
                            select new Entities.TrnCase
                            {
                                Id = d.Id,
                                CaseNumber = d.CaseNumber,
                                CaseDate = d.CaseDate.ToShortDateString(),
                                CitizenId = d.CitizenId,
                                Citizen = d.MstCitizen.Surname + ", " + d.MstCitizen.Firstname + " " + d.MstCitizen.Middlename,
                                CitizenDateOfBirth = d.MstCitizen.DateOfBirth.ToShortDateString(),
                                CitizenAge = DateTime.Today.Year - d.MstCitizen.DateOfBirth.Year,
                                CitizenCivilStatus = d.MstCitizen.MstCivilStatus.CivilStatus,
                                CitizenEducationalAttainment = "None",
                                CitizenOccupation = d.MstCitizen.MstOccupation.Occupation,
                                CitizenReligion = "None",
                                CitizenAddress = d.MstCitizen.PermanentNumber + " " +
                                                 d.MstCitizen.PermanentStreet + " " +
                                                 d.MstCitizen.PermanentVillage + " " +
                                                 d.MstCitizen.MstBarangay.Barangay + " " +
                                                 d.MstCitizen.MstCity.City + " " +
                                                 d.MstCitizen.MstProvince.Province + " " +
                                                 d.MstCitizen.MstProvince.MstRegion.MstCountry.Country + " " +
                                                 d.MstCitizen.PermanentZipCode,
                                CitizenCardId = d.CitizenCardId,
                                CitizenCardNumber = d.MstCitizensCard.CardNumber,
                                ServiceId = d.ServiceId,
                                Service = d.MstService.Service,
                                ServiceGroup = d.MstService.MstServiceGroup.ServiceGroup,
                                Amount = d.Amount,
                                Problem = d.Problem,
                                Background = d.Background,
                                Recommendation = d.Recommendation,
                                PreparedById = d.PreparedById,
                                PreparedBy = d.MstUser.Fullname,
                                CheckedById = d.CheckedById,
                                CheckedBy = d.MstUser1.Fullname,
                                StatusId = d.StatusId,
                                Status = d.MstStatus.Status,
                                IsLocked = d.IsLocked,
                                CreatedByUserId = d.CreatedByUserId,
                                CreatedByUser = d.MstUser2.Fullname,
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedByUserId = d.UpdatedByUserId,
                                UpdatedByUser = d.MstUser3.Fullname,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                            };

                return cases.OrderByDescending(d => d.Id).ToList();
            }
            else
            {
                var cases = from d in db.TrnCases
                            where d.MstService.MstServiceGroup.ServiceDepartmentId == Convert.ToInt32(serviceDepartmentId)
                            && d.CaseDate >= Convert.ToDateTime(startDate)
                            && d.CaseDate <= Convert.ToDateTime(endDate)
                            select new Entities.TrnCase
                            {
                                Id = d.Id,
                                CaseNumber = d.CaseNumber,
                                CaseDate = d.CaseDate.ToShortDateString(),
                                CitizenId = d.CitizenId,
                                Citizen = d.MstCitizen.Surname + ", " + d.MstCitizen.Firstname + " " + d.MstCitizen.Middlename,
                                CitizenDateOfBirth = d.MstCitizen.DateOfBirth.ToShortDateString(),
                                CitizenAge = DateTime.Today.Year - d.MstCitizen.DateOfBirth.Year,
                                CitizenCivilStatus = d.MstCitizen.MstCivilStatus.CivilStatus,
                                CitizenEducationalAttainment = "None",
                                CitizenOccupation = d.MstCitizen.MstOccupation.Occupation,
                                CitizenReligion = "None",
                                CitizenAddress = d.MstCitizen.PermanentNumber + " " +
                                                 d.MstCitizen.PermanentStreet + " " +
                                                 d.MstCitizen.PermanentVillage + " " +
                                                 d.MstCitizen.MstBarangay.Barangay + " " +
                                                 d.MstCitizen.MstCity.City + " " +
                                                 d.MstCitizen.MstProvince.Province + " " +
                                                 d.MstCitizen.MstProvince.MstRegion.MstCountry.Country + " " +
                                                 d.MstCitizen.PermanentZipCode,
                                CitizenCardId = d.CitizenCardId,
                                CitizenCardNumber = d.MstCitizensCard.CardNumber,
                                ServiceId = d.ServiceId,
                                Service = d.MstService.Service,
                                ServiceGroup = d.MstService.MstServiceGroup.ServiceGroup,
                                Amount = d.Amount,
                                Problem = d.Problem,
                                Background = d.Background,
                                Recommendation = d.Recommendation,
                                PreparedById = d.PreparedById,
                                PreparedBy = d.MstUser.Fullname,
                                CheckedById = d.CheckedById,
                                CheckedBy = d.MstUser1.Fullname,
                                StatusId = d.StatusId,
                                Status = d.MstStatus.Status,
                                IsLocked = d.IsLocked,
                                CreatedByUserId = d.CreatedByUserId,
                                CreatedByUser = d.MstUser2.Fullname,
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedByUserId = d.UpdatedByUserId,
                                UpdatedByUser = d.MstUser3.Fullname,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                            };

                return cases.OrderByDescending(d => d.Id).ToList();
            }
        }

        [HttpGet, Route("detail/{id}")]
        public Entities.TrnCase CaseDetail(String id)
        {
            var detailCase = from d in db.TrnCases
                             where d.Id == Convert.ToInt32(id)
                             select new Entities.TrnCase
                             {
                                 Id = d.Id,
                                 CaseNumber = d.CaseNumber,
                                 CaseDate = d.CaseDate.ToShortDateString(),
                                 CitizenId = d.CitizenId,
                                 Citizen = d.MstCitizen.Surname + ", " + d.MstCitizen.Firstname + " " + d.MstCitizen.Middlename,
                                 CitizenDateOfBirth = d.MstCitizen.DateOfBirth.ToShortDateString(),
                                 CitizenAge = DateTime.Today.Year - d.MstCitizen.DateOfBirth.Year,
                                 CitizenCivilStatus = d.MstCitizen.MstCivilStatus.CivilStatus,
                                 CitizenEducationalAttainment = d.MstCitizen.MstCitizensEducations.Any() == true ? d.MstCitizen.MstCitizensEducations.OrderByDescending(e => e.Id).FirstOrDefault().Degree : "None",
                                 CitizenOccupation = d.MstCitizen.MstOccupation.Occupation,
                                 CitizenReligion = "None",
                                 CitizenAddress = d.MstCitizen.PermanentNumber + " " +
                                                  d.MstCitizen.PermanentStreet + " " +
                                                  d.MstCitizen.PermanentVillage + " " +
                                                  d.MstCitizen.MstBarangay.Barangay + " " +
                                                  d.MstCitizen.MstCity.City + " " +
                                                  d.MstCitizen.MstProvince.Province + " " +
                                                  d.MstCitizen.MstProvince.MstRegion.MstCountry.Country + " " +
                                                  d.MstCitizen.PermanentZipCode,
                                 CitizenCardId = d.CitizenCardId,
                                 CitizenCardNumber = d.MstCitizensCard.CardNumber,
                                 ServiceId = d.ServiceId,
                                 Service = d.MstService.Service,
                                 ServiceGroup = d.MstService.MstServiceGroup.ServiceGroup,
                                 Amount = d.Amount,
                                 Problem = d.Problem,
                                 Background = d.Background,
                                 Recommendation = d.Recommendation,
                                 PreparedById = d.PreparedById,
                                 PreparedBy = d.MstUser.Fullname,
                                 CheckedById = d.CheckedById,
                                 CheckedBy = d.MstUser1.Fullname,
                                 StatusId = d.StatusId,
                                 Status = d.MstStatus.Status,
                                 IsLocked = d.IsLocked,
                                 CreatedByUserId = d.CreatedByUserId,
                                 CreatedByUser = d.MstUser2.Fullname,
                                 CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                 UpdatedByUserId = d.UpdatedByUserId,
                                 UpdatedByUser = d.MstUser3.Fullname,
                                 UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                             };

            return detailCase.FirstOrDefault();
        }

        [HttpGet, Route("citizen/dropdown/list")]
        public List<Entities.MstCitizen> CitizenDropdownList()
        {
            var citizens = from d in db.MstCitizens
                           where d.IsLocked == true
                           select new Entities.MstCitizen
                           {
                               Id = d.Id,
                               Surname = d.Surname,
                               Firstname = d.Firstname,
                               Middlename = d.Middlename,
                               Extensionname = d.Extensionname,
                           };

            return citizens.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("citizensCard/dropdown/list/{citizenId}")]
        public List<Entities.MstCitizensCard> CitizensCardDropdownList(String citizenId)
        {
            var citizensCards = from d in db.MstCitizensCards
                                where d.CitizenId == Convert.ToInt32(citizenId)
                                select new Entities.MstCitizensCard
                                {
                                    Id = d.Id,
                                    CardNumber = d.CardNumber,
                                };

            return citizensCards.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("service/dropdown/list")]
        public List<Entities.MstService> ServiceDropdownList()
        {
            var services = from d in db.MstServices
                           where d.IsLocked == true
                           select new Entities.MstService
                           {
                               Id = d.Id,
                               Service = d.Service,
                               ServiceGroupId = d.ServiceGroupId
                           };

            return services.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("service/group/template/list/{serviceGroupId}")]
        public List<Entities.MstServiceGroupTemplate> ServiceGroupTemplateList(String serviceGroupId)
        {
            var serviceGroupTemplates = from d in db.MstServiceGroupTemplates
                                        where d.ServiceGroupId == Convert.ToInt32(serviceGroupId)
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

        [HttpGet, Route("user/dropdown/list")]
        public List<Entities.MstUser> UserDropdownList()
        {
            var users = from d in db.MstUsers
                        select new Entities.MstUser
                        {
                            Id = d.Id,
                            Fullname = d.Fullname,
                        };

            return users.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("status/dropdown/list")]
        public List<Entities.MstStatus> StatusDropdownList()
        {
            var statuses = from d in db.MstStatus
                           where d.Category.Equals("Case")
                           select new Entities.MstStatus
                           {
                               Id = d.Id,
                               Status = d.Status
                           };

            return statuses.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("supplier/dropdown/list")]
        public List<Entities.MstSupplier> SupplierDropdownList()
        {
            var suppliers = from d in db.MstSuppliers
                            select new Entities.MstSupplier
                            {
                                Id = d.Id,
                                Supplier = d.Supplier
                            };

            return suppliers.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("citizensCard/list")]
        public List<Entities.MstCitizensCard> CitizensCardList()
        {
            var citizensCards = from d in db.MstCitizensCards
                                select new Entities.MstCitizensCard
                                {
                                    Id = d.Id,
                                    CitizenId = d.CitizenId,
                                    Citizen = d.MstCitizen.Surname + ", " + d.MstCitizen.Firstname + " " + d.MstCitizen.Middlename,
                                    CardNumber = d.CardNumber,
                                    CitizensDateOfBirth = d.MstCitizen.DateOfBirth.ToShortDateString(),
                                    TotalBalance = d.TotalBalance,
                                    StatusId = d.StatusId,
                                    Status = d.MstStatus.Status
                                };

            return citizensCards.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("citizensCard/search/list")]
        public List<Entities.MstCitizensCard> CitizensCardSearchList(String keyWord)
        {
            var citizensCards = from d in db.MstCitizensCards
                                where d.CardNumber.Contains(keyWord)
                                || d.MstCitizen.Surname.Contains(keyWord)
                                || d.MstCitizen.Firstname.Contains(keyWord)
                                || d.MstCitizen.Middlename.Contains(keyWord)
                                || d.MstCitizen.Extensionname.Contains(keyWord)
                                select new Entities.MstCitizensCard
                                {
                                    Id = d.Id,
                                    CitizenId = d.CitizenId,
                                    Citizen = d.MstCitizen.Surname + ", " + d.MstCitizen.Firstname + " " + d.MstCitizen.Middlename,
                                    CardNumber = d.CardNumber,
                                    CitizensDateOfBirth = d.MstCitizen.DateOfBirth.ToShortDateString(),
                                    TotalBalance = d.TotalBalance,
                                    StatusId = d.StatusId,
                                    Status = d.MstStatus.Status
                                };

            return citizensCards.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add/{citizensCardId}")]
        public HttpResponseMessage AddCase(String citizensCardId)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                var citizensCard = from d in db.MstCitizensCards
                                   where d.Id == Convert.ToInt32(citizensCardId)
                                   select d;

                if (citizensCard.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen's card not found!");
                }
                else
                {
                    if (citizensCard.FirstOrDefault().MstCitizen.IsLocked == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen not found!");
                    }
                }

                var service = from d in db.MstServices where d.IsLocked == true select d;
                if (service.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service not found!");
                }

                var status = from d in db.MstStatus
                             where d.Category.Equals("Case")
                             select d;

                if (status.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Status not found!");
                }

                String caseNumber = "0000000001";

                var lastCase = from d in db.TrnCases.OrderByDescending(d => d.Id) select d;
                if (lastCase.Any())
                {
                    Int32 newCaseNumber = Convert.ToInt32(lastCase.FirstOrDefault().CaseNumber) + 1;
                    caseNumber = FillLeadingZeroes(newCaseNumber, 10);
                }

                Data.TrnCase newCase = new Data.TrnCase()
                {
                    CaseNumber = caseNumber,
                    CaseDate = DateTime.Today,
                    CitizenId = citizensCard.FirstOrDefault().CitizenId,
                    CitizenCardId = citizensCard.FirstOrDefault().Id,
                    ServiceId = service.FirstOrDefault().Id,
                    Amount = 0,
                    Problem = "",
                    Background = "",
                    Recommendation = "",
                    PreparedById = currentUser.FirstOrDefault().Id,
                    CheckedById = currentUser.FirstOrDefault().Id,
                    StatusId = status.FirstOrDefault().Id,
                    IsLocked = false,
                    CreatedByUserId = currentUser.FirstOrDefault().Id,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = currentUser.FirstOrDefault().Id,
                    UpdatedDateTime = DateTime.Now
                };

                db.TrnCases.InsertOnSubmit(newCase);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, newCase.Id);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("save/{id}")]
        public HttpResponseMessage SaveCase(String id, Entities.TrnCase objCase)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                var citizen = from d in db.MstCitizens
                              where d.Id == objCase.CitizenId
                              && d.IsLocked == true
                              select d;

                if (citizen.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen not found!");
                }

                var citizensCard = from d in db.MstCitizensCards
                                   where d.Id == objCase.CitizenCardId
                                   select d;

                if (citizensCard.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen's card not found!");
                }

                var service = from d in db.MstServices
                              where d.Id == objCase.ServiceId
                              && d.IsLocked == true
                              select d;

                if (service.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service not found!");
                }

                var status = from d in db.MstStatus
                             where d.Id == objCase.StatusId
                             && d.Category.Equals("Case")
                             select d;

                if (status.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Status not found!");
                }

                var preparedByUser = from d in db.MstUsers
                                     where d.Id == objCase.PreparedById
                                     select d;

                if (preparedByUser.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Prepared by user not found!");
                }

                var checkedByUser = from d in db.MstUsers
                                    where d.Id == objCase.CheckedById
                                    select d;

                if (checkedByUser.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Checked by user not found!");
                }

                var currentCase = from d in db.TrnCases
                                  where d.Id == Convert.ToInt32(id)
                                  select d;

                if (currentCase.Any())
                {
                    if (currentCase.FirstOrDefault().IsLocked == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Already locked!");
                    }

                    var saveCase = currentCase.FirstOrDefault();
                    saveCase.CaseDate = Convert.ToDateTime(objCase.CaseDate);
                    saveCase.CitizenId = objCase.CitizenId;
                    saveCase.CitizenCardId = objCase.CitizenCardId;
                    saveCase.ServiceId = objCase.ServiceId;
                    saveCase.Amount = objCase.Amount;
                    saveCase.Problem = objCase.Problem;
                    saveCase.Background = objCase.Background;
                    saveCase.Recommendation = objCase.Recommendation;
                    saveCase.StatusId = objCase.StatusId;
                    saveCase.PreparedById = objCase.PreparedById;
                    saveCase.CheckedById = objCase.CheckedById;
                    saveCase.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    saveCase.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Case not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("lock/{id}")]
        public HttpResponseMessage LockCase(String id, Entities.TrnCase objCase)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                var citizen = from d in db.MstCitizens
                              where d.Id == objCase.CitizenId
                              && d.IsLocked == true
                              select d;

                if (citizen.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen not found!");
                }

                var citizensCard = from d in db.MstCitizensCards
                                   where d.Id == objCase.CitizenCardId
                                   select d;

                if (citizensCard.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen's card not found!");
                }

                var service = from d in db.MstServices
                              where d.Id == objCase.ServiceId
                              && d.IsLocked == true
                              select d;

                if (service.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service not found!");
                }

                var status = from d in db.MstStatus
                             where d.Id == objCase.StatusId
                             && d.Category.Equals("Case")
                             select d;

                if (status.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Status not found!");
                }

                var preparedByUser = from d in db.MstUsers
                                     where d.Id == objCase.PreparedById
                                     select d;

                if (preparedByUser.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Prepared by user not found!");
                }

                var checkedByUser = from d in db.MstUsers
                                    where d.Id == objCase.CheckedById
                                    select d;

                if (checkedByUser.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Checked by user not found!");
                }

                var currentCase = from d in db.TrnCases
                                  where d.Id == Convert.ToInt32(id)
                                  select d;

                if (currentCase.Any())
                {
                    if (currentCase.FirstOrDefault().IsLocked == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Already locked!");
                    }

                    var lockCase = currentCase.FirstOrDefault();
                    lockCase.CaseDate = Convert.ToDateTime(objCase.CaseDate);
                    lockCase.CitizenId = objCase.CitizenId;
                    lockCase.CitizenCardId = objCase.CitizenCardId;
                    lockCase.ServiceId = objCase.ServiceId;
                    lockCase.Amount = objCase.Amount;
                    lockCase.Problem = objCase.Problem;
                    lockCase.Background = objCase.Background;
                    lockCase.Recommendation = objCase.Recommendation;
                    lockCase.StatusId = objCase.StatusId;
                    lockCase.PreparedById = objCase.PreparedById;
                    lockCase.CheckedById = objCase.CheckedById;
                    lockCase.IsLocked = true;
                    lockCase.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    lockCase.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Case not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("unlock/{id}")]
        public HttpResponseMessage UnlockCase(String id)
        {
            try
            {
                var currentCase = from d in db.TrnCases
                                  where d.Id == Convert.ToInt32(id)
                                  select d;

                if (currentCase.Any())
                {
                    if (currentCase.FirstOrDefault().IsLocked == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Already unlocked!");
                    }

                    var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                    var unlockCase = currentCase.FirstOrDefault();
                    unlockCase.IsLocked = false;
                    unlockCase.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    unlockCase.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Case not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteCase(String id)
        {
            try
            {
                var currentCase = from d in db.TrnCases
                                  where d.Id == Convert.ToInt32(id)
                                  select d;

                if (currentCase.Any())
                {
                    if (currentCase.FirstOrDefault().IsLocked == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Case cannot be deleted if locked.");
                    }

                    var deleteCitizen = currentCase.FirstOrDefault();
                    db.TrnCases.DeleteOnSubmit(deleteCitizen);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Case not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet, Route("print/{id}")]
        public HttpResponseMessage PrintCase(String id)
        {
            FontFactory.RegisterDirectories();

            Font fontTimesNewRoman11 = FontFactory.GetFont("Arial", 11);
            Font fontTimesNewRoman11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontTimesNewRoman12 = FontFactory.GetFont("Arial", 12);
            Font fontTimesNewRoman12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);

            Document document = new Document(PageSize.LETTER, 25f, 25f, 25f, 25f);
            MemoryStream workStream = new MemoryStream();

            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 30f, 30f, 30f);

            document.Open();

            var currentCase = from d in db.TrnCases
                              where d.Id == Convert.ToInt32(id)
                              && d.IsLocked == true
                              select d;

            if (currentCase.Any())
            {
                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

                Phrase phraseRepublic = new Phrase("Republic of the Philippines\n", fontTimesNewRoman11);
                Phrase phraseDepartment = new Phrase("City Social Welfare and Development Office\n\n", fontTimesNewRoman11);
                Phrase phraseCity = new Phrase("Danao City\n", fontTimesNewRoman11);
                Phrase phraseTitle = new Phrase("Case Summary Report", fontTimesNewRoman12Bold);

                Paragraph headerParagraph = new Paragraph
                {
                    phraseRepublic,
                    phraseCity,
                    phraseDepartment,
                    phraseTitle
                };

                headerParagraph.SetLeading(12f, 0);
                headerParagraph.Alignment = Element.ALIGN_CENTER;

                String logoPath = AppDomain.CurrentDomain.BaseDirectory + @"Images\Logo\danaocitylogo.png";
                Image imageLogo = Image.GetInstance(logoPath);
                imageLogo.ScaleToFit(1000f, 60f);

                PdfPTable pdfTableHeaderDetail = new PdfPTable(3);
                pdfTableHeaderDetail.SetWidths(new float[] { 30f, 40, 30f });
                pdfTableHeaderDetail.WidthPercentage = 100;
                pdfTableHeaderDetail.AddCell(new PdfPCell(imageLogo) { Border = 0, HorizontalAlignment = 2, PaddingRight = 15f });
                pdfTableHeaderDetail.AddCell(new PdfPCell(headerParagraph) { Border = 0, HorizontalAlignment = 1 });
                pdfTableHeaderDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });

                document.Add(pdfTableHeaderDetail);
                document.Add(line);

                String citizenName = currentCase.FirstOrDefault().MstCitizen.Surname + ", " +
                                     currentCase.FirstOrDefault().MstCitizen.Firstname + " " +
                                     currentCase.FirstOrDefault().MstCitizen.Middlename + " " +
                                     currentCase.FirstOrDefault().MstCitizen.Extensionname;
                String caseNumber = currentCase.FirstOrDefault().CaseNumber;
                String birthDate = currentCase.FirstOrDefault().MstCitizen.DateOfBirth.ToShortDateString();
                String caseDate = currentCase.FirstOrDefault().CaseDate.ToShortDateString();
                Int32 age = DateTime.Today.Year - currentCase.FirstOrDefault().MstCitizen.DateOfBirth.Year;
                String service = currentCase.FirstOrDefault().MstService.MstServiceGroup.ServiceGroup;
                String sex = currentCase.FirstOrDefault().MstCitizen.MstSex.Sex;
                String civilStatus = currentCase.FirstOrDefault().MstCitizen.MstCivilStatus.CivilStatus;
                String educationalAttainment = "None";
                if (currentCase.FirstOrDefault().MstCitizen.MstCitizensEducations.Any())
                {
                    educationalAttainment = currentCase.FirstOrDefault().MstCitizen.MstCitizensEducations.OrderByDescending(d => d.Id).FirstOrDefault().Degree;
                }
                String occupation = currentCase.FirstOrDefault().MstCitizen.MstOccupation.Occupation;
                String address = currentCase.FirstOrDefault().MstCitizen.PermanentNumber + " " +
                                 currentCase.FirstOrDefault().MstCitizen.PermanentStreet + " " +
                                 currentCase.FirstOrDefault().MstCitizen.PermanentVillage + " " +
                                 currentCase.FirstOrDefault().MstCitizen.MstBarangay.Barangay + " " +
                                 currentCase.FirstOrDefault().MstCitizen.MstCity.City + " " +
                                 currentCase.FirstOrDefault().MstCitizen.MstProvince.Province + " " +
                                 currentCase.FirstOrDefault().MstCitizen.MstProvince.MstRegion.MstCountry.Country + " " +
                                 currentCase.FirstOrDefault().MstCitizen.PermanentZipCode;
                String religion = "None";
                String problem = currentCase.FirstOrDefault().Problem;
                String background = currentCase.FirstOrDefault().Background;
                String recommendation = currentCase.FirstOrDefault().Recommendation;
                String preparedBy = currentCase.FirstOrDefault().MstUser.Fullname;
                String notedBy = currentCase.FirstOrDefault().MstService.MstServiceGroup.MstServiceDepartment.OfficerInCharge;

                PdfPTable pdfTableCaseDetail = new PdfPTable(4);
                pdfTableCaseDetail.SetWidths(new float[] { 20f, 40f, 15f, 25f });
                pdfTableCaseDetail.WidthPercentage = 100;
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman11)) { Colspan = 4, Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Identifying Information: ", fontTimesNewRoman11Bold)) { Border = 0, Colspan = 4, PaddingBottom = 10f });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Name: ", fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(citizenName, fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Case No.: ", fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(caseNumber, fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Birth Date: ", fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(birthDate, fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Case Date: ", fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(caseDate, fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Age: ", fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(age.ToString(), fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Service: ", fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(service, fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Sex: ", fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(sex, fontTimesNewRoman11)) { Colspan = 3, Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Civil Status: ", fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(civilStatus, fontTimesNewRoman11)) { Colspan = 3, Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Educ. Attainment: ", fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(educationalAttainment, fontTimesNewRoman11)) { Colspan = 3, Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Occupation: ", fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(occupation, fontTimesNewRoman11)) { Colspan = 3, Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Address: ", fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(address.TrimStart(), fontTimesNewRoman11)) { Colspan = 3, Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Religion: ", fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(religion, fontTimesNewRoman11)) { Colspan = 3, Border = 0 });

                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman11)) { Colspan = 4, Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(line)) { Colspan = 4, Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman11)) { Colspan = 4, Border = 0 });

                Phrase phraseProblemLabel = new Phrase("Problem: \n", fontTimesNewRoman11Bold);
                Phrase phraseProblemData = new Phrase(problem, fontTimesNewRoman11);
                Paragraph problemParagraph = new Paragraph { phraseProblemLabel, phraseProblemData };
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(problemParagraph)) { Colspan = 4, Border = 0 });

                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman11)) { Colspan = 4, Border = 0 });

                Phrase phraseBackgroundLabel = new Phrase("Background: \n", fontTimesNewRoman11Bold);
                Phrase phraseBackgroundData = new Phrase(background, fontTimesNewRoman11);
                Paragraph backgroundParagraph = new Paragraph { phraseBackgroundLabel, phraseBackgroundData };
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(backgroundParagraph)) { Colspan = 4, Border = 0 });

                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman11)) { Colspan = 4, Border = 0 });

                Phrase phraseRecommendationLabel = new Phrase("Assesment/Recommendation: \n", fontTimesNewRoman11Bold);
                Phrase phraseRecommendationData = new Phrase(recommendation, fontTimesNewRoman11);
                Paragraph recommendationParagraph = new Paragraph { phraseRecommendationLabel, phraseRecommendationData };
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(recommendationParagraph)) { Colspan = 4, Border = 0 });

                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman11)) { Colspan = 4, Border = 0 });

                document.Add(pdfTableCaseDetail);
                document.Add(line);

                PdfPTable pdfTableUsers = new PdfPTable(4);
                pdfTableUsers.SetWidths(new float[] { 40f, 10f, 40f, 20f });
                pdfTableUsers.WidthPercentage = 100;
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman11)) { Colspan = 4, Border = 0 });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman11)) { Colspan = 4, Border = 0 });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman11)) { Colspan = 4, Border = 0 });

                pdfTableUsers.AddCell(new PdfPCell(new Phrase("Prepared By: ", fontTimesNewRoman11Bold)) { Border = 0 });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("", fontTimesNewRoman11Bold)) { Border = 0 });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("Noted By: ", fontTimesNewRoman11Bold)) { Border = 0 });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("", fontTimesNewRoman11Bold)) { Border = 0 });

                pdfTableUsers.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman11)) { Colspan = 4, Border = 0 });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman11)) { Colspan = 4, Border = 0 });

                pdfTableUsers.AddCell(new PdfPCell(new Phrase(preparedBy, fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, Border = 0 });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("", fontTimesNewRoman11)) { Border = 0 });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(notedBy, fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, Border = 0 });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("", fontTimesNewRoman11Bold)) { Border = 0 });

                pdfTableUsers.AddCell(new PdfPCell(new Phrase("SWA", fontTimesNewRoman11)) { HorizontalAlignment = 1, Border = 1 });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("", fontTimesNewRoman11)) { Border = 0 });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("CSWDO", fontTimesNewRoman11)) { HorizontalAlignment = 1, Border = 1 });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("", fontTimesNewRoman11Bold)) { Border = 0 });

                document.Add(pdfTableUsers);
            }
            else
            {
                Paragraph emptyParagraph = new Paragraph("\n");
                document.Add(emptyParagraph);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            if (ContentDispositionHeaderValue.TryParse("inline; filename=case.pdf", out ContentDispositionHeaderValue contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }

        [HttpGet, Route("print/certificateOfEligibility/{id}")]
        public HttpResponseMessage PrintCertificateOfEligibility(String id)
        {
            FontFactory.RegisterDirectories();

            Font fontTimesNewRoman13 = FontFactory.GetFont("Times New Roman", 13);
            Font fontTimesNewRoman13Bold = FontFactory.GetFont("Times New Roman", 13, Font.BOLD);
            Font fontTimesNewRoman14 = FontFactory.GetFont("Times New Roman", 14);
            Font fontTimesNewRoman14Bold = FontFactory.GetFont("Times New Roman", 14, Font.BOLD);
            Font fontTimesNewRoman15 = FontFactory.GetFont("Times New Roman", 15);
            Font fontTimesNewRoman15Bold = FontFactory.GetFont("Times New Roman", 15, Font.BOLD);
            Font fontTimesNewRoman16 = FontFactory.GetFont("Times New Roman", 16);
            Font fontTimesNewRoman16Bold = FontFactory.GetFont("Times New Roman", 16, Font.BOLD);

            Document document = new Document(PageSize.LETTER, 50f, 50f, 25f, 25f);
            MemoryStream workStream = new MemoryStream();

            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(50f, 50f, 30f, 30f);

            document.Open();

            var currentCase = from d in db.TrnCases
                              where d.Id == Convert.ToInt32(id)
                              && d.IsLocked == true
                              select d;

            if (currentCase.Any())
            {
                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

                Phrase phraseRepublic = new Phrase("Republic of the Philippines\n", fontTimesNewRoman14Bold);
                Phrase phraseDepartment = new Phrase("CITY SOCIAL WELFARE AND DEVELOPMENT OFFICE\n", fontTimesNewRoman13);
                Phrase phraseCity = new Phrase("Danao City\n\n\n", fontTimesNewRoman14);
                Phrase phraseTitle = new Phrase("CERTIFICATE OF ELIGIBILITY", fontTimesNewRoman16Bold);

                Paragraph headerParagraph = new Paragraph
                {
                    phraseRepublic,
                    phraseDepartment,
                    phraseCity,
                    phraseTitle
                };

                headerParagraph.SetLeading(12f, 0);
                headerParagraph.Alignment = Element.ALIGN_CENTER;

                String logoPath = AppDomain.CurrentDomain.BaseDirectory + @"Images\Logo\danaocitylogo.png";
                Image imageLogo = Image.GetInstance(logoPath);
                imageLogo.ScaleToFit(1000f, 60f);

                PdfPTable pdfTableHeaderDetail = new PdfPTable(3);
                pdfTableHeaderDetail.SetWidths(new float[] { 30f, 160f, 30f });
                pdfTableHeaderDetail.WidthPercentage = 100;
                pdfTableHeaderDetail.AddCell(new PdfPCell(imageLogo) { HorizontalAlignment = 1, Border = 0 });
                pdfTableHeaderDetail.AddCell(new PdfPCell(headerParagraph) { HorizontalAlignment = 1, Border = 0 });
                pdfTableHeaderDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
                document.Add(pdfTableHeaderDetail);

                Data.MstCitizen citizen = currentCase.FirstOrDefault().MstCitizen;
                String citizensFullname = citizen.Surname + ", " + citizen.Firstname;
                String citizensAddress = citizen.MstBarangay.Barangay + ", " + citizen.MstCity.City;

                Phrase phrase1 = new Phrase("\nThis is to certify that " + citizensFullname + " of " + citizensAddress + " has been eligible for Financial Assistance under the Bureau of Assistance after interview and case study has been made.", fontTimesNewRoman15);
                Paragraph paragraph1 = new Paragraph { phrase1 };
                paragraph1.SetLeading(20f, 0);
                document.Add(paragraph1);

                String caseDate = currentCase.FirstOrDefault().CaseDate.ToLongDateString();
                String caseServiceDepartment = currentCase.FirstOrDefault().MstService.MstServiceGroup.MstServiceDepartment.ServiceDepartment;

                Phrase phrase2 = new Phrase("\nRecords of the case study date " + caseDate + " are in the Confidence file at Unit " + caseServiceDepartment + ".", fontTimesNewRoman15);
                Paragraph paragraph2 = new Paragraph { phrase2 };
                paragraph2.SetLeading(20f, 0);
                document.Add(paragraph2);

                String amount = Convert.ToString(Math.Round(currentCase.FirstOrDefault().Amount * 100) / 100);
                String amountInWords = GetMoneyWord(amount);

                if (currentCase.FirstOrDefault().Amount == 0)
                {
                    amountInWords = "Zero Pesos";
                }

                String service = currentCase.FirstOrDefault().MstService.MstServiceGroup.ServiceGroup;

                Phrase phrase3 = new Phrase("\nClient is recommended for assistance in the amount of " + amountInWords + " only for " + service + ".", fontTimesNewRoman15);
                Paragraph paragraph3 = new Paragraph { phrase3 };
                paragraph3.SetLeading(20f, 0);
                document.Add(paragraph3);

                Phrase phrase4 = new Phrase("\nRecords and Case Study Reviewed. \n\n", fontTimesNewRoman15);
                Paragraph paragraph4 = new Paragraph { phrase4 };
                paragraph4.SetLeading(20f, 0);
                document.Add(paragraph4);

                String preparedBy = currentCase.FirstOrDefault().MstUser.Fullname;
                String officerInCharge = currentCase.FirstOrDefault().MstService.MstServiceGroup.MstServiceDepartment.OfficerInCharge;

                String chiefOfOffice = "";
                var getChiefOfOffice = from d in db.SysSettings
                                       where d.Code == "CHIEFOFOFFICE"
                                       select d;

                if (getChiefOfOffice.Any())
                {
                    chiefOfOffice = getChiefOfOffice.FirstOrDefault().Value;
                }

                PdfPTable pdfTableUsers = new PdfPTable(3);
                pdfTableUsers.SetWidths(new float[] { 50f, 20f, 50f });
                pdfTableUsers.WidthPercentage = 100;

                pdfTableUsers.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman15)) { Colspan = 3, Border = 0 });

                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15Bold)) { Border = 0, PaddingBottom = 30f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15Bold)) { Border = 0, PaddingBottom = 30f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("Prepared By:", fontTimesNewRoman15Bold)) { Border = 0, PaddingBottom = 30f });

                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15Bold)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15Bold)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(preparedBy.ToUpper(), fontTimesNewRoman15Bold)) { HorizontalAlignment = 1, Border = 2, PaddingTop = 3f });

                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("SWA", fontTimesNewRoman15)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f });

                pdfTableUsers.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman15)) { Colspan = 3, Border = 0 });

                pdfTableUsers.AddCell(new PdfPCell(new Phrase("Noted By:", fontTimesNewRoman15Bold)) { Border = 0, PaddingBottom = 30f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15Bold)) { Border = 0, PaddingBottom = 30f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15Bold)) { Border = 0, PaddingBottom = 30f });

                pdfTableUsers.AddCell(new PdfPCell(new Phrase(officerInCharge.ToUpper(), fontTimesNewRoman15Bold)) { HorizontalAlignment = 1, Border = 2, PaddingTop = 3f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15Bold)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15Bold)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f });

                pdfTableUsers.AddCell(new PdfPCell(new Phrase("CSWDO", fontTimesNewRoman15)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f });

                pdfTableUsers.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman15)) { Colspan = 3, Border = 0 });

                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15Bold)) { Border = 0, PaddingBottom = 30f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15Bold)) { Border = 0, PaddingBottom = 30f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("Approved By:", fontTimesNewRoman15Bold)) { Border = 0, PaddingBottom = 30f });

                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15Bold)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(chiefOfOffice, fontTimesNewRoman15Bold)) { HorizontalAlignment = 1, Border = 2, PaddingTop = 3f });

                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman15)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("Chief of Office", fontTimesNewRoman15)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f });

                document.Add(pdfTableUsers);
            }
            else
            {
                Paragraph emptyParagraph = new Paragraph("\n");
                document.Add(emptyParagraph);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            if (ContentDispositionHeaderValue.TryParse("inline; filename=certificateOfEligibility.pdf", out ContentDispositionHeaderValue contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }

        [HttpGet, Route("print/disbursementVoucher/{id}")]
        public HttpResponseMessage PrintDisbursementVoucher(String id)
        {
            FontFactory.RegisterDirectories();

            Font fontTimesNewRoman09 = FontFactory.GetFont("Arial", 9);
            Font fontTimesNewRoman09Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontTimesNewRoman10 = FontFactory.GetFont("Arial", 10);
            Font fontTimesNewRoman10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Font fontTimesNewRoman11 = FontFactory.GetFont("Arial", 11);
            Font fontTimesNewRoman11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontTimesNewRoman13 = FontFactory.GetFont("Arial", 13);
            Font fontTimesNewRoman13Bold = FontFactory.GetFont("Arial", 13, Font.BOLD);
            Font fontTimesNewRoman14 = FontFactory.GetFont("Arial", 14);
            Font fontTimesNewRoman14Bold = FontFactory.GetFont("Arial", 14, Font.BOLD);
            Font fontTimesNewRoman15 = FontFactory.GetFont("Arial", 15);
            Font fontTimesNewRoman15Bold = FontFactory.GetFont("Arial", 15, Font.BOLD);
            Font fontTimesNewRoman16 = FontFactory.GetFont("Arial", 16);
            Font fontTimesNewRoman16Bold = FontFactory.GetFont("Arial", 16, Font.BOLD);

            Document document = new Document(PageSize.LETTER, 50f, 50f, 25f, 25f);
            MemoryStream workStream = new MemoryStream();

            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 30f, 30f, 30f);

            document.Open();

            var currentCase = from d in db.TrnCases
                              where d.Id == Convert.ToInt32(id)
                              && d.IsLocked == true
                              select d;

            if (currentCase.Any())
            {
                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

                Phrase phraseRepublic = new Phrase("Republic of the Philippines\n", fontTimesNewRoman15);
                Phrase phraseGovernment = new Phrase("DANAO CITY GOVERNMENT\n", fontTimesNewRoman16Bold);
                Phrase phraseCity = new Phrase("Danao City, Cebu\n", fontTimesNewRoman15);

                Paragraph headerParagraph = new Paragraph
                {
                    phraseRepublic,
                    phraseGovernment,
                    phraseCity
                };

                headerParagraph.SetLeading(12f, 0);
                headerParagraph.Alignment = Element.ALIGN_CENTER;

                Phrase phraseTitle = new Phrase("DISBURSEMENT VOUCHER", fontTimesNewRoman16Bold);
                Phrase phraseNo = new Phrase("No.", fontTimesNewRoman10);

                PdfPTable pdfTableHeaderDetail = new PdfPTable(2);
                pdfTableHeaderDetail.SetWidths(new float[] { 86.25f, 28.75f });
                pdfTableHeaderDetail.WidthPercentage = 100;
                pdfTableHeaderDetail.AddCell(new PdfPCell(headerParagraph) { HorizontalAlignment = 1, PaddingBottom = 6f, Colspan = 2 });
                pdfTableHeaderDetail.AddCell(new PdfPCell(phraseTitle) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableHeaderDetail.AddCell(new PdfPCell(phraseNo) { PaddingBottom = 6f, PaddingTop = 6f });
                document.Add(pdfTableHeaderDetail);

                Phrase phraseModeOfPayment = new Phrase("Mode of Payment", fontTimesNewRoman11);
                Phrase phrasePayments = new Phrase("___ Check     ___ Cash     ___ Others", fontTimesNewRoman11);

                PdfPTable pdfTableModeOfPaymentDetail = new PdfPTable(2);
                pdfTableModeOfPaymentDetail.SetWidths(new float[] { 15f, 100f });
                pdfTableModeOfPaymentDetail.WidthPercentage = 100;
                pdfTableModeOfPaymentDetail.AddCell(new PdfPCell(phraseModeOfPayment) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableModeOfPaymentDetail.AddCell(new PdfPCell(phrasePayments) { HorizontalAlignment = 1, PaddingBottom = 6f, PaddingTop = 3f });
                document.Add(pdfTableModeOfPaymentDetail);

                Phrase phrasePayee = new Phrase("Payee", fontTimesNewRoman11);
                Phrase phrasePayeeName = new Phrase(currentCase.FirstOrDefault().MstCitizen.Firstname + " " + currentCase.FirstOrDefault().MstCitizen.Middlename + " " + currentCase.FirstOrDefault().MstCitizen.Surname, fontTimesNewRoman11Bold);
                Phrase phraseTINEmployeeNo = new Phrase("TIN/Employee No.", fontTimesNewRoman10);
                Phrase phraseObligation = new Phrase("Obligation Request No.", fontTimesNewRoman10);

                PdfPTable pdfTablePayeeDetail = new PdfPTable(4);
                pdfTablePayeeDetail.SetWidths(new float[] { 15f, 42.5f, 28.75f, 28.75f });
                pdfTablePayeeDetail.WidthPercentage = 100;
                pdfTablePayeeDetail.AddCell(new PdfPCell(phrasePayee) { HorizontalAlignment = 1, PaddingBottom = 6f, PaddingTop = 7f });
                pdfTablePayeeDetail.AddCell(new PdfPCell(phrasePayeeName) { PaddingBottom = 6f, PaddingTop = 7f });
                pdfTablePayeeDetail.AddCell(new PdfPCell(phraseTINEmployeeNo) { PaddingBottom = 20f });
                pdfTablePayeeDetail.AddCell(new PdfPCell(phraseObligation) { PaddingBottom = 20f });
                document.Add(pdfTablePayeeDetail);

                Phrase phraseAddress = new Phrase("Address", fontTimesNewRoman11);
                Phrase phraseContactNumber = new Phrase("Contact No.", fontTimesNewRoman11);
                Phrase phraseContactNumberValue = new Phrase(currentCase.FirstOrDefault().MstCitizen.MobileNumber, fontTimesNewRoman11);
                Phrase phraseAddressValue = new Phrase(currentCase.FirstOrDefault().MstCitizen.MstBarangay1.Barangay + ", " + currentCase.FirstOrDefault().MstCitizen.MstCity1.City, fontTimesNewRoman11Bold);
                Phrase phraseResponsibilityCenter = new Phrase("Responsibility Center", fontTimesNewRoman10);

                Phrase phraseOfficeUnitProject = new Phrase("Office Unit/Project\n", fontTimesNewRoman10);
                Phrase phraseOfficeUnitProjectValue = new Phrase("CSWD", fontTimesNewRoman11Bold);

                Paragraph passOfficeUnitProjectParagraph = new Paragraph
                {
                    phraseOfficeUnitProject,
                    phraseOfficeUnitProjectValue
                };

                Phrase phraseCode = new Phrase("Code\n", fontTimesNewRoman10);
                Phrase phraseCodeValue = new Phrase("Code", fontTimesNewRoman11Bold);

                Paragraph passCodeParagraph = new Paragraph
                {
                    phraseCode,
                    phraseCodeValue
                };

                PdfPTable pdfTableAddressDetail = new PdfPTable(4);
                pdfTableAddressDetail.SetWidths(new float[] { 15f, 42.5f, 28.75f, 28.75f });
                pdfTableAddressDetail.WidthPercentage = 100;
                pdfTableAddressDetail.AddCell(new PdfPCell(phraseAddress) { HorizontalAlignment = 1, PaddingBottom = 6f, PaddingTop = 6f, Rowspan = 2 });
                pdfTableAddressDetail.AddCell(new PdfPCell(phraseAddressValue) { PaddingBottom = 10f, PaddingTop = 7f, Rowspan = 2 });
                pdfTableAddressDetail.AddCell(new PdfPCell(phraseResponsibilityCenter) { PaddingBottom = 3f, Colspan = 2 });
                pdfTableAddressDetail.AddCell(new PdfPCell(passOfficeUnitProjectParagraph) { PaddingBottom = 3f, Rowspan = 2 });
                pdfTableAddressDetail.AddCell(new PdfPCell(passCodeParagraph) { PaddingBottom = 3f, Rowspan = 2 });
                pdfTableAddressDetail.AddCell(new PdfPCell(phraseContactNumber) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAddressDetail.AddCell(new PdfPCell(phraseContactNumberValue) { PaddingBottom = 6f });
                document.Add(pdfTableAddressDetail);

                Phrase phraseExplanation = new Phrase("EXPLANATION", fontTimesNewRoman11Bold);
                Phrase phraseAmount = new Phrase("Amount", fontTimesNewRoman11Bold);

                PdfPTable pdfTableExplanationDetail = new PdfPTable(2);
                pdfTableExplanationDetail.SetWidths(new float[] { 86.25f, 28.75f });
                pdfTableExplanationDetail.WidthPercentage = 100;
                pdfTableExplanationDetail.AddCell(new PdfPCell(phraseExplanation) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableExplanationDetail.AddCell(new PdfPCell(phraseAmount) { HorizontalAlignment = 1, PaddingBottom = 6f });

                String amount = Convert.ToString(Math.Round(currentCase.FirstOrDefault().Amount * 100) / 100);

                String amountInWords = GetMoneyWord(amount);
                if (currentCase.FirstOrDefault().Amount == 0)
                {
                    amountInWords = "Zero Pesos Only";
                }

                String service = currentCase.FirstOrDefault().MstService.MstServiceGroup.ServiceGroup;

                String explanationValue = "To Payment of Financial Assitance for " + service + " in the amount of " + amountInWords + " as per supporting papers here to attached.";

                pdfTableExplanationDetail.AddCell(new PdfPCell(new Phrase(explanationValue, fontTimesNewRoman11)) { PaddingTop = 10f, PaddingLeft = 10f, PaddingRight = 10f, PaddingBottom = 70f });
                pdfTableExplanationDetail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().Amount.ToString("#,##0.00"), fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 70f });
                pdfTableExplanationDetail.AddCell(new PdfPCell(new Phrase(amountInWords, fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableExplanationDetail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().Amount.ToString("#,##0.00"), fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                document.Add(pdfTableExplanationDetail);

                Phrase phraseCertifiedLetterA = new Phrase("A", fontTimesNewRoman11Bold);
                Phrase phraseCertifiedValueA = new Phrase("Certified\n\n", fontTimesNewRoman11Bold);
                Phrase phraseAmountObligated = new Phrase("___ Allotment obiligated for the purpose as indicated above. \n\n", fontTimesNewRoman11);
                Phrase phraseSupportingDocuments = new Phrase("___ Supporting documents complete.\n\n", fontTimesNewRoman11);

                Paragraph certifiedAValueParagraph = new Paragraph
                {
                    phraseCertifiedValueA,
                    phraseAmountObligated,
                    phraseSupportingDocuments
                };

                Phrase phraseCertifiedLetterB = new Phrase("B", fontTimesNewRoman11Bold);
                Phrase phraseCertifiedValueB = new Phrase("Certified\n\n", fontTimesNewRoman11Bold);
                Phrase phraseFunds = new Phrase("Funds Available", fontTimesNewRoman11);

                Paragraph certifiedBValueParagraph = new Paragraph
                {
                    phraseCertifiedValueB,
                    phraseFunds
                };

                PdfPTable pdfTableCertifiedDetail = new PdfPTable(4);
                pdfTableCertifiedDetail.SetWidths(new float[] { 7.5f, 50f, 7.5f, 50f });
                pdfTableCertifiedDetail.WidthPercentage = 100;
                pdfTableCertifiedDetail.AddCell(new PdfPCell(phraseCertifiedLetterA) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableCertifiedDetail.AddCell(new PdfPCell(certifiedAValueParagraph) { Border = Rectangle.RIGHT_BORDER, PaddingLeft = 3f, PaddingBottom = 6f, Rowspan = 2 });
                pdfTableCertifiedDetail.AddCell(new PdfPCell(phraseCertifiedLetterB) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableCertifiedDetail.AddCell(new PdfPCell(certifiedBValueParagraph) { Border = Rectangle.RIGHT_BORDER, PaddingLeft = 3f, PaddingBottom = 6f, Rowspan = 2 });
                pdfTableCertifiedDetail.AddCell(new PdfPCell(new Phrase("")) { PaddingBottom = 6f, Border = Rectangle.LEFT_BORDER });
                pdfTableCertifiedDetail.AddCell(new PdfPCell(new Phrase("")) { PaddingBottom = 6f, Border = Rectangle.LEFT_BORDER });
                document.Add(pdfTableCertifiedDetail);

                Phrase phraseSignature = new Phrase("Signature", fontTimesNewRoman11Bold);

                PdfPTable pdfTableSignatureDetail = new PdfPTable(4);
                pdfTableSignatureDetail.SetWidths(new float[] { 15f, 42.5f, 15f, 42.5f });
                pdfTableSignatureDetail.WidthPercentage = 100;
                pdfTableSignatureDetail.AddCell(new PdfPCell(phraseSignature) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 9f, PaddingBottom = 12f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase("")) { PaddingBottom = 12f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(phraseSignature) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 9f, PaddingBottom = 12f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase("")) { PaddingBottom = 12f });
                document.Add(pdfTableSignatureDetail);

                String cityAccoutant = "";
                var getCityAccountant = from d in db.SysSettings
                                        where d.Code == "CITYACCOUNTANT"
                                        select d;

                if (getCityAccountant.Any())
                {
                    cityAccoutant = getCityAccountant.FirstOrDefault().Value;
                }

                Phrase phrasePrintedNameA = new Phrase("Printed Name", fontTimesNewRoman11);
                Phrase phrasePrintedNameValueA = new Phrase(cityAccoutant, fontTimesNewRoman11Bold);
                Phrase phrasePrintedDateA = new Phrase("Date", fontTimesNewRoman10);

                String cityTreasurer = "";
                var getCityTreasurer = from d in db.SysSettings
                                       where d.Code == "CITYTREASURER"
                                       select d;

                if (getCityTreasurer.Any())
                {
                    cityTreasurer = getCityTreasurer.FirstOrDefault().Value;
                }

                Phrase phrasePrintedNameB = new Phrase("Printed Name", fontTimesNewRoman11);
                Phrase phrasePrintedNameValueB = new Phrase(cityTreasurer, fontTimesNewRoman11Bold);
                Phrase phrasePrintedDateB = new Phrase("Date", fontTimesNewRoman10);

                PdfPTable pdfTablePrintedDetail = new PdfPTable(6);
                pdfTablePrintedDetail.SetWidths(new float[] { 15f, 32.5f, 10f, 15f, 32.5f, 10f });
                pdfTablePrintedDetail.WidthPercentage = 100;
                pdfTablePrintedDetail.AddCell(new PdfPCell(phrasePrintedNameA) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 9f, PaddingBottom = 12f, Rowspan = 2 });
                pdfTablePrintedDetail.AddCell(new PdfPCell(phrasePrintedNameValueA) { PaddingTop = 5f, PaddingBottom = 7f, HorizontalAlignment = 1 });
                pdfTablePrintedDetail.AddCell(new PdfPCell(phrasePrintedDateA) { PaddingLeft = 3f, PaddingBottom = 12f, Rowspan = 2 });
                pdfTablePrintedDetail.AddCell(new PdfPCell(phrasePrintedNameB) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 9f, PaddingBottom = 12f, Rowspan = 2 });
                pdfTablePrintedDetail.AddCell(new PdfPCell(phrasePrintedNameValueB) { PaddingTop = 5f, PaddingBottom = 7f, HorizontalAlignment = 1 });
                pdfTablePrintedDetail.AddCell(new PdfPCell(phrasePrintedDateB) { PaddingLeft = 3f, PaddingBottom = 12f, Rowspan = 2 });
                pdfTablePrintedDetail.AddCell(new PdfPCell(new Phrase("City Accountant", fontTimesNewRoman10)) { PaddingTop = 3f, HorizontalAlignment = 1 });
                pdfTablePrintedDetail.AddCell(new PdfPCell(new Phrase("OIC City Treasurer", fontTimesNewRoman10)) { PaddingTop = 3f, HorizontalAlignment = 1 });
                document.Add(pdfTablePrintedDetail);

                PdfPTable pdfTableApprovedAndReceivedDetail = new PdfPTable(4);
                pdfTableApprovedAndReceivedDetail.SetWidths(new float[] { 7.5f, 50f, 7.5f, 50f });
                pdfTableApprovedAndReceivedDetail.WidthPercentage = 100;
                pdfTableApprovedAndReceivedDetail.AddCell(new PdfPCell(new Phrase("C", fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableApprovedAndReceivedDetail.AddCell(new PdfPCell(new Phrase("APPROVED FOR PAYMENT", fontTimesNewRoman11Bold)) { PaddingLeft = 3f, PaddingBottom = 6f, Rowspan = 2 });
                pdfTableApprovedAndReceivedDetail.AddCell(new PdfPCell(new Phrase("D", fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableApprovedAndReceivedDetail.AddCell(new PdfPCell(new Phrase("RECEVIED PAYMENT", fontTimesNewRoman11Bold)) { PaddingLeft = 3f, PaddingBottom = 6f, Rowspan = 2 });
                document.Add(pdfTableApprovedAndReceivedDetail);

                PdfPTable pdfTableMayorSignatureAndBankDetail = new PdfPTable(6);
                pdfTableMayorSignatureAndBankDetail.SetWidths(new float[] { 15f, 32.5f, 10f, 15f, 32.5f, 10f });
                pdfTableMayorSignatureAndBankDetail.WidthPercentage = 100;
                pdfTableMayorSignatureAndBankDetail.AddCell(new PdfPCell(new Phrase("Signature", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 18f, Rowspan = 2 });
                pdfTableMayorSignatureAndBankDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { PaddingLeft = 3f, PaddingTop = 18f, Rowspan = 2, Colspan = 2 });
                pdfTableMayorSignatureAndBankDetail.AddCell(new PdfPCell(new Phrase("Checked No.", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndBankDetail.AddCell(new PdfPCell(new Phrase("Bank Name", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndBankDetail.AddCell(new PdfPCell(new Phrase("Date", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndBankDetail.AddCell(new PdfPCell(new Phrase("Signature", fontTimesNewRoman09)) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 6f, PaddingBottom = 10f });
                pdfTableMayorSignatureAndBankDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndBankDetail.AddCell(new PdfPCell(new Phrase("Date", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                document.Add(pdfTableMayorSignatureAndBankDetail);

                String cityMayor = "";
                var getCityMayor = from d in db.SysSettings
                                   where d.Code == "CITYMAYOR"
                                   select d;

                if (getCityMayor.Any())
                {
                    cityMayor = getCityMayor.FirstOrDefault().Value;
                }

                Phrase phraseCityMayor = new Phrase(cityMayor + "\n", fontTimesNewRoman11Bold);
                Phrase phraseCityMayorLabel = new Phrase("City Mayor", fontTimesNewRoman11);
                Paragraph cityMayorParagraph = new Paragraph
                {
                    phraseCityMayor,
                    phraseCityMayorLabel
                };

                PdfPTable pdfTableMayorSignatureAndCheckSignatureDetail = new PdfPTable(6);
                pdfTableMayorSignatureAndCheckSignatureDetail.SetWidths(new float[] { 15f, 32.5f, 10f, 15f, 32.5f, 10f });
                pdfTableMayorSignatureAndCheckSignatureDetail.WidthPercentage = 100;
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(new Phrase("Printed Name", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 20f, Rowspan = 2 });
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(cityMayorParagraph) { HorizontalAlignment = 1, PaddingTop = 15f, PaddingLeft = 3f, Rowspan = 2 });
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(new Phrase("Date", fontTimesNewRoman11)) { PaddingLeft = 3f, Rowspan = 2 });
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(new Phrase("Printed Name", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().MstCitizen.Firstname + " " + currentCase.FirstOrDefault().MstCitizen.Middlename + " " + currentCase.FirstOrDefault().MstCitizen.Surname, fontTimesNewRoman09Bold)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(new Phrase("OR/Other Documents", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(new Phrase("JEV No.", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(new Phrase("Date", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                document.Add(pdfTableMayorSignatureAndCheckSignatureDetail);
            }
            else
            {
                Paragraph emptyParagraph = new Paragraph("\n");
                document.Add(emptyParagraph);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            if (ContentDispositionHeaderValue.TryParse("inline; filename=certificateOfEligibility.pdf", out ContentDispositionHeaderValue contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }

        [HttpGet, Route("print/disbursementVoucher/supplier/{id}/{supplierId}")]
        public HttpResponseMessage PrintDisbursementVoucherSupplier(String id, String supplierId)
        {
            FontFactory.RegisterDirectories();

            Font fontTimesNewRoman09 = FontFactory.GetFont("Arial", 9);
            Font fontTimesNewRoman09Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontTimesNewRoman10 = FontFactory.GetFont("Arial", 10);
            Font fontTimesNewRoman10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Font fontTimesNewRoman11 = FontFactory.GetFont("Arial", 11);
            Font fontTimesNewRoman11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontTimesNewRoman13 = FontFactory.GetFont("Arial", 13);
            Font fontTimesNewRoman13Bold = FontFactory.GetFont("Arial", 13, Font.BOLD);
            Font fontTimesNewRoman14 = FontFactory.GetFont("Arial", 14);
            Font fontTimesNewRoman14Bold = FontFactory.GetFont("Arial", 14, Font.BOLD);
            Font fontTimesNewRoman15 = FontFactory.GetFont("Arial", 15);
            Font fontTimesNewRoman15Bold = FontFactory.GetFont("Arial", 15, Font.BOLD);
            Font fontTimesNewRoman16 = FontFactory.GetFont("Arial", 16);
            Font fontTimesNewRoman16Bold = FontFactory.GetFont("Arial", 16, Font.BOLD);

            Document document = new Document(PageSize.LETTER, 50f, 50f, 25f, 25f);
            MemoryStream workStream = new MemoryStream();

            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 30f, 30f, 30f);

            document.Open();

            var currentCase = from d in db.TrnCases
                              where d.Id == Convert.ToInt32(id)
                              && d.IsLocked == true
                              select d;

            if (currentCase.Any())
            {
                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

                Phrase phraseRepublic = new Phrase("Republic of the Philippines\n", fontTimesNewRoman15);
                Phrase phraseGovernment = new Phrase("DANAO CITY GOVERNMENT\n", fontTimesNewRoman16Bold);
                Phrase phraseCity = new Phrase("Danao City, Cebu\n", fontTimesNewRoman15);

                Paragraph headerParagraph = new Paragraph
                {
                    phraseRepublic,
                    phraseGovernment,
                    phraseCity
                };

                headerParagraph.SetLeading(12f, 0);
                headerParagraph.Alignment = Element.ALIGN_CENTER;

                Phrase phraseTitle = new Phrase("DISBURSEMENT VOUCHER", fontTimesNewRoman16Bold);
                Phrase phraseNo = new Phrase("No.", fontTimesNewRoman10);

                PdfPTable pdfTableHeaderDetail = new PdfPTable(2);
                pdfTableHeaderDetail.SetWidths(new float[] { 86.25f, 28.75f });
                pdfTableHeaderDetail.WidthPercentage = 100;
                pdfTableHeaderDetail.AddCell(new PdfPCell(headerParagraph) { HorizontalAlignment = 1, PaddingBottom = 6f, Colspan = 2 });
                pdfTableHeaderDetail.AddCell(new PdfPCell(phraseTitle) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableHeaderDetail.AddCell(new PdfPCell(phraseNo) { PaddingBottom = 6f, PaddingTop = 6f });
                document.Add(pdfTableHeaderDetail);

                Phrase phraseModeOfPayment = new Phrase("Mode of Payment", fontTimesNewRoman11);
                Phrase phrasePayments = new Phrase("___ Check     ___ Cash     ___ Others", fontTimesNewRoman11);

                PdfPTable pdfTableModeOfPaymentDetail = new PdfPTable(2);
                pdfTableModeOfPaymentDetail.SetWidths(new float[] { 15f, 100f });
                pdfTableModeOfPaymentDetail.WidthPercentage = 100;
                pdfTableModeOfPaymentDetail.AddCell(new PdfPCell(phraseModeOfPayment) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableModeOfPaymentDetail.AddCell(new PdfPCell(phrasePayments) { HorizontalAlignment = 1, PaddingBottom = 6f, PaddingTop = 3f });
                document.Add(pdfTableModeOfPaymentDetail);

                String supplierName = "";
                String supplierAddres = "";
                Boolean isVAT = false;
                Decimal VATRate = 0;
                Boolean isWithheld = false;
                Decimal withholdingRate = 0;
                Boolean isCityTax = false;
                Decimal cityTaxRate = 0;

                var supplier = from d in db.MstSuppliers
                               where d.Id == Convert.ToInt32(supplierId)
                               select d;

                if (supplier.Any())
                {
                    supplierName = supplier.FirstOrDefault().Supplier;
                    supplierAddres = supplier.FirstOrDefault().Address;
                    isVAT = supplier.FirstOrDefault().IsVAT;
                    VATRate = supplier.FirstOrDefault().VATRate;
                    isWithheld = supplier.FirstOrDefault().IsWithheld;
                    withholdingRate = supplier.FirstOrDefault().WithholdingRate;
                    isCityTax = supplier.FirstOrDefault().IsCityTax;
                    cityTaxRate = supplier.FirstOrDefault().CityTaxRate;
                }

                Phrase phrasePayee = new Phrase("Payee", fontTimesNewRoman11);
                Phrase phrasePayeeName = new Phrase(supplierName, fontTimesNewRoman11Bold);
                Phrase phraseTINEmployeeNo = new Phrase("TIN/Employee No.", fontTimesNewRoman10);
                Phrase phraseObligation = new Phrase("Obligation Request No.", fontTimesNewRoman10);

                PdfPTable pdfTablePayeeDetail = new PdfPTable(4);
                pdfTablePayeeDetail.SetWidths(new float[] { 15f, 42.5f, 28.75f, 28.75f });
                pdfTablePayeeDetail.WidthPercentage = 100;
                pdfTablePayeeDetail.AddCell(new PdfPCell(phrasePayee) { HorizontalAlignment = 1, PaddingBottom = 6f, PaddingTop = 7f });
                pdfTablePayeeDetail.AddCell(new PdfPCell(phrasePayeeName) { PaddingBottom = 6f, PaddingTop = 7f });
                pdfTablePayeeDetail.AddCell(new PdfPCell(phraseTINEmployeeNo) { PaddingBottom = 20f });
                pdfTablePayeeDetail.AddCell(new PdfPCell(phraseObligation) { PaddingBottom = 20f });
                document.Add(pdfTablePayeeDetail);

                Phrase phraseAddress = new Phrase("Address", fontTimesNewRoman11);
                Phrase phraseContactNumber = new Phrase("Contact No.", fontTimesNewRoman11);
                Phrase phraseContactNumberValue = new Phrase("", fontTimesNewRoman11);
                Phrase phraseAddressValue = new Phrase(supplierAddres, fontTimesNewRoman11Bold);
                Phrase phraseResponsibilityCenter = new Phrase("Responsibility Center", fontTimesNewRoman10);

                Phrase phraseOfficeUnitProject = new Phrase("Office Unit/Project\n", fontTimesNewRoman10);
                Phrase phraseOfficeUnitProjectValue = new Phrase("CSWD", fontTimesNewRoman11Bold);

                Paragraph passOfficeUnitProjectParagraph = new Paragraph
                {
                    phraseOfficeUnitProject,
                    phraseOfficeUnitProjectValue
                };

                Phrase phraseCode = new Phrase("Code\n", fontTimesNewRoman10);
                Phrase phraseCodeValue = new Phrase("Code", fontTimesNewRoman11Bold);

                Paragraph passCodeParagraph = new Paragraph
                {
                    phraseCode,
                    phraseCodeValue
                };

                PdfPTable pdfTableAddressDetail = new PdfPTable(4);
                pdfTableAddressDetail.SetWidths(new float[] { 15f, 42.5f, 28.75f, 28.75f });
                pdfTableAddressDetail.WidthPercentage = 100;
                pdfTableAddressDetail.AddCell(new PdfPCell(phraseAddress) { HorizontalAlignment = 1, PaddingBottom = 6f, PaddingTop = 6f, Rowspan = 2 });
                pdfTableAddressDetail.AddCell(new PdfPCell(phraseAddressValue) { PaddingBottom = 10f, PaddingTop = 7f, Rowspan = 2 });
                pdfTableAddressDetail.AddCell(new PdfPCell(phraseResponsibilityCenter) { PaddingBottom = 3f, Colspan = 2 });
                pdfTableAddressDetail.AddCell(new PdfPCell(passOfficeUnitProjectParagraph) { PaddingBottom = 3f, Rowspan = 2 });
                pdfTableAddressDetail.AddCell(new PdfPCell(passCodeParagraph) { PaddingBottom = 3f, Rowspan = 2 });
                pdfTableAddressDetail.AddCell(new PdfPCell(phraseContactNumber) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAddressDetail.AddCell(new PdfPCell(phraseContactNumberValue) { PaddingBottom = 6f });
                document.Add(pdfTableAddressDetail);

                Phrase phraseExplanation = new Phrase("EXPLANATION", fontTimesNewRoman11Bold);
                Phrase phraseAmount = new Phrase("Amount", fontTimesNewRoman11Bold);

                PdfPTable pdfTableExplanationDetail = new PdfPTable(2);
                pdfTableExplanationDetail.SetWidths(new float[] { 86.25f, 28.75f });
                pdfTableExplanationDetail.WidthPercentage = 100;
                pdfTableExplanationDetail.AddCell(new PdfPCell(phraseExplanation) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableExplanationDetail.AddCell(new PdfPCell(phraseAmount) { HorizontalAlignment = 1, PaddingBottom = 6f });

                String amount = Convert.ToString(Math.Round(currentCase.FirstOrDefault().Amount * 100) / 100);

                String amountInWords = GetMoneyWord(amount);
                if (currentCase.FirstOrDefault().Amount == 0)
                {
                    amountInWords = "Zero Pesos Only";
                }

                Decimal LessVAT = currentCase.FirstOrDefault().Amount;
                if (isVAT)
                {
                    LessVAT = currentCase.FirstOrDefault().Amount * (VATRate / 100);
                }

                Decimal LessVATCITW = currentCase.FirstOrDefault().Amount;
                if (isWithheld)
                {
                    LessVATCITW = currentCase.FirstOrDefault().Amount * (withholdingRate / 100);
                }

                Decimal BusinessTax = currentCase.FirstOrDefault().Amount;
                if (isCityTax)
                {
                    BusinessTax = currentCase.FirstOrDefault().Amount * (cityTaxRate / 100);
                }

                String citizensName = currentCase.FirstOrDefault().MstCitizen.Firstname + " " + currentCase.FirstOrDefault().MstCitizen.Middlename + " " + currentCase.FirstOrDefault().MstCitizen.Surname;
                String service = currentCase.FirstOrDefault().MstService.MstServiceGroup.ServiceGroup;

                String explanationValue = "To Payment of Financial Assitance of " + citizensName + " for " + service + " in the amount of " + amountInWords + " as per supporting papers here to attached.\n\n"
                    + "LESS VAT: " + currentCase.FirstOrDefault().Amount.ToString("#,##0.00") + " x " + VATRate.ToString("#,##0.00") + "% = " + LessVAT.ToString("#,##0.00") + "\n"
                    + "LESS VAT: CITW = " + currentCase.FirstOrDefault().Amount.ToString("#,##0.00") + " x " + withholdingRate.ToString("#,##0.00") + "% = " + LessVATCITW.ToString("#,##0.00") + "\n"
                    + "BUSINESS TAX: " + currentCase.FirstOrDefault().Amount.ToString("#,##0.00") + " x " + cityTaxRate.ToString("#,##0.00") + "% = " + BusinessTax.ToString("#,##0.00");

                pdfTableExplanationDetail.AddCell(new PdfPCell(new Phrase(explanationValue, fontTimesNewRoman11)) { PaddingTop = 10f, PaddingLeft = 10f, PaddingRight = 10f, PaddingBottom = 60f });
                pdfTableExplanationDetail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().Amount.ToString("#,##0.00"), fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 60f });
                pdfTableExplanationDetail.AddCell(new PdfPCell(new Phrase(amountInWords, fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableExplanationDetail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().Amount.ToString("#,##0.00"), fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                document.Add(pdfTableExplanationDetail);

                Phrase phraseCertifiedLetterA = new Phrase("A", fontTimesNewRoman11Bold);
                Phrase phraseCertifiedValueA = new Phrase("Certified\n\n", fontTimesNewRoman11Bold);
                Phrase phraseAmountObligated = new Phrase("___ Allotment obiligated for the purpose as indicated above.\n\n", fontTimesNewRoman11);
                Phrase phraseSupportingDocuments = new Phrase("___ Supporting documents complete.\n\n", fontTimesNewRoman11);

                Paragraph certifiedAValueParagraph = new Paragraph
                {
                    phraseCertifiedValueA,
                    phraseAmountObligated,
                    phraseSupportingDocuments
                };

                Phrase phraseCertifiedLetterB = new Phrase("B", fontTimesNewRoman11Bold);
                Phrase phraseCertifiedValueB = new Phrase("Certified\n\n", fontTimesNewRoman11Bold);
                Phrase phraseFunds = new Phrase("Funds Available", fontTimesNewRoman11);

                Paragraph certifiedBValueParagraph = new Paragraph
                {
                    phraseCertifiedValueB,
                    phraseFunds
                };

                PdfPTable pdfTableCertifiedDetail = new PdfPTable(4);
                pdfTableCertifiedDetail.SetWidths(new float[] { 7.5f, 50f, 7.5f, 50f });
                pdfTableCertifiedDetail.WidthPercentage = 100;
                pdfTableCertifiedDetail.AddCell(new PdfPCell(phraseCertifiedLetterA) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableCertifiedDetail.AddCell(new PdfPCell(certifiedAValueParagraph) { Border = Rectangle.RIGHT_BORDER, PaddingLeft = 3f, PaddingBottom = 6f, Rowspan = 2 });
                pdfTableCertifiedDetail.AddCell(new PdfPCell(phraseCertifiedLetterB) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableCertifiedDetail.AddCell(new PdfPCell(certifiedBValueParagraph) { Border = Rectangle.RIGHT_BORDER, PaddingLeft = 3f, PaddingBottom = 6f, Rowspan = 2 });
                pdfTableCertifiedDetail.AddCell(new PdfPCell(new Phrase("")) { PaddingBottom = 6f, Border = Rectangle.LEFT_BORDER });
                pdfTableCertifiedDetail.AddCell(new PdfPCell(new Phrase("")) { PaddingBottom = 6f, Border = Rectangle.LEFT_BORDER });
                document.Add(pdfTableCertifiedDetail);

                Phrase phraseSignature = new Phrase("Signature", fontTimesNewRoman11Bold);

                PdfPTable pdfTableSignatureDetail = new PdfPTable(4);
                pdfTableSignatureDetail.SetWidths(new float[] { 15f, 42.5f, 15f, 42.5f });
                pdfTableSignatureDetail.WidthPercentage = 100;
                pdfTableSignatureDetail.AddCell(new PdfPCell(phraseSignature) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 9f, PaddingBottom = 12f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase("")) { PaddingBottom = 12f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(phraseSignature) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 9f, PaddingBottom = 12f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase("")) { PaddingBottom = 12f });
                document.Add(pdfTableSignatureDetail);

                String cityAccoutant = "";
                var getCityAccountant = from d in db.SysSettings
                                        where d.Code == "CITYACCOUNTANT"
                                        select d;

                if (getCityAccountant.Any())
                {
                    cityAccoutant = getCityAccountant.FirstOrDefault().Value;
                }

                Phrase phrasePrintedNameA = new Phrase("Printed Name", fontTimesNewRoman11);
                Phrase phrasePrintedNameValueA = new Phrase(cityAccoutant, fontTimesNewRoman11Bold);
                Phrase phrasePrintedDateA = new Phrase("Date", fontTimesNewRoman10);

                String cityTreasurer = "";
                var getCityTreasurer = from d in db.SysSettings
                                       where d.Code == "CITYTREASURER"
                                       select d;

                if (getCityTreasurer.Any())
                {
                    cityTreasurer = getCityTreasurer.FirstOrDefault().Value;
                }

                Phrase phrasePrintedNameB = new Phrase("Printed Name", fontTimesNewRoman11);
                Phrase phrasePrintedNameValueB = new Phrase(cityTreasurer, fontTimesNewRoman11Bold);
                Phrase phrasePrintedDateB = new Phrase("Date", fontTimesNewRoman10);

                PdfPTable pdfTablePrintedDetail = new PdfPTable(6);
                pdfTablePrintedDetail.SetWidths(new float[] { 15f, 32.5f, 10f, 15f, 32.5f, 10f });
                pdfTablePrintedDetail.WidthPercentage = 100;
                pdfTablePrintedDetail.AddCell(new PdfPCell(phrasePrintedNameA) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 9f, PaddingBottom = 12f, Rowspan = 2 });
                pdfTablePrintedDetail.AddCell(new PdfPCell(phrasePrintedNameValueA) { PaddingTop = 5f, PaddingBottom = 7f, HorizontalAlignment = 1 });
                pdfTablePrintedDetail.AddCell(new PdfPCell(phrasePrintedDateA) { PaddingLeft = 3f, PaddingBottom = 12f, Rowspan = 2 });
                pdfTablePrintedDetail.AddCell(new PdfPCell(phrasePrintedNameB) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 9f, PaddingBottom = 12f, Rowspan = 2 });
                pdfTablePrintedDetail.AddCell(new PdfPCell(phrasePrintedNameValueB) { PaddingTop = 5f, PaddingBottom = 7f, HorizontalAlignment = 1 });
                pdfTablePrintedDetail.AddCell(new PdfPCell(phrasePrintedDateB) { PaddingLeft = 3f, PaddingBottom = 12f, Rowspan = 2 });
                pdfTablePrintedDetail.AddCell(new PdfPCell(new Phrase("City Accountant", fontTimesNewRoman10)) { PaddingTop = 3f, HorizontalAlignment = 1 });
                pdfTablePrintedDetail.AddCell(new PdfPCell(new Phrase("OIC City Treasurer", fontTimesNewRoman10)) { PaddingTop = 3f, HorizontalAlignment = 1 });
                document.Add(pdfTablePrintedDetail);

                PdfPTable pdfTableApprovedAndReceivedDetail = new PdfPTable(4);
                pdfTableApprovedAndReceivedDetail.SetWidths(new float[] { 7.5f, 50f, 7.5f, 50f });
                pdfTableApprovedAndReceivedDetail.WidthPercentage = 100;
                pdfTableApprovedAndReceivedDetail.AddCell(new PdfPCell(new Phrase("C", fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableApprovedAndReceivedDetail.AddCell(new PdfPCell(new Phrase("APPROVED FOR PAYMENT", fontTimesNewRoman11Bold)) { PaddingLeft = 3f, PaddingBottom = 6f, Rowspan = 2 });
                pdfTableApprovedAndReceivedDetail.AddCell(new PdfPCell(new Phrase("D", fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableApprovedAndReceivedDetail.AddCell(new PdfPCell(new Phrase("RECEVIED PAYMENT", fontTimesNewRoman11Bold)) { PaddingLeft = 3f, PaddingBottom = 6f, Rowspan = 2 });
                document.Add(pdfTableApprovedAndReceivedDetail);

                PdfPTable pdfTableMayorSignatureAndBankDetail = new PdfPTable(6);
                pdfTableMayorSignatureAndBankDetail.SetWidths(new float[] { 15f, 32.5f, 10f, 15f, 32.5f, 10f });
                pdfTableMayorSignatureAndBankDetail.WidthPercentage = 100;
                pdfTableMayorSignatureAndBankDetail.AddCell(new PdfPCell(new Phrase("Signature", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 18f, Rowspan = 2 });
                pdfTableMayorSignatureAndBankDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { PaddingLeft = 3f, PaddingTop = 18f, Rowspan = 2, Colspan = 2 });
                pdfTableMayorSignatureAndBankDetail.AddCell(new PdfPCell(new Phrase("Checked No.", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndBankDetail.AddCell(new PdfPCell(new Phrase("Bank Name", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndBankDetail.AddCell(new PdfPCell(new Phrase("Date", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndBankDetail.AddCell(new PdfPCell(new Phrase("Signature", fontTimesNewRoman09)) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 6f, PaddingBottom = 10f });
                pdfTableMayorSignatureAndBankDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndBankDetail.AddCell(new PdfPCell(new Phrase("Date", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                document.Add(pdfTableMayorSignatureAndBankDetail);

                String cityMayor = "";
                var getCityMayor = from d in db.SysSettings
                                   where d.Code == "CITYMAYOR"
                                   select d;

                if (getCityMayor.Any())
                {
                    cityMayor = getCityMayor.FirstOrDefault().Value;
                }

                Phrase phraseCityMayor = new Phrase(cityMayor + "\n", fontTimesNewRoman11Bold);
                Phrase phraseCityMayorLabel = new Phrase("City Mayor", fontTimesNewRoman11);
                Paragraph cityMayorParagraph = new Paragraph
                {
                    phraseCityMayor,
                    phraseCityMayorLabel
                };

                PdfPTable pdfTableMayorSignatureAndCheckSignatureDetail = new PdfPTable(6);
                pdfTableMayorSignatureAndCheckSignatureDetail.SetWidths(new float[] { 15f, 32.5f, 10f, 15f, 32.5f, 10f });
                pdfTableMayorSignatureAndCheckSignatureDetail.WidthPercentage = 100;
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(new Phrase("Printed Name", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 20f, Rowspan = 2 });
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(cityMayorParagraph) { HorizontalAlignment = 1, PaddingTop = 15f, PaddingLeft = 3f, Rowspan = 2 });
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(new Phrase("Date", fontTimesNewRoman11)) { PaddingLeft = 3f, Rowspan = 2 });
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(new Phrase("Printed Name", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().MstCitizen.Firstname + " " + currentCase.FirstOrDefault().MstCitizen.Middlename + " " + currentCase.FirstOrDefault().MstCitizen.Surname, fontTimesNewRoman09Bold)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(new Phrase("OR/Other Documents", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(new Phrase("JEV No.", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                pdfTableMayorSignatureAndCheckSignatureDetail.AddCell(new PdfPCell(new Phrase("Date", fontTimesNewRoman09)) { PaddingLeft = 3f, PaddingBottom = 15f });
                document.Add(pdfTableMayorSignatureAndCheckSignatureDetail);
            }
            else
            {
                Paragraph emptyParagraph = new Paragraph("\n");
                document.Add(emptyParagraph);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            if (ContentDispositionHeaderValue.TryParse("inline; filename=certificateOfEligibility.pdf", out ContentDispositionHeaderValue contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }

        [HttpGet, Route("print/obligationRequest/{id}")]
        public HttpResponseMessage PrintObligationRequest(String id)
        {
            FontFactory.RegisterDirectories();

            Font fontTimesNewRoman09 = FontFactory.GetFont("Arial", 9);
            Font fontTimesNewRoman09Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontTimesNewRoman10 = FontFactory.GetFont("Arial", 10);
            Font fontTimesNewRoman10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Font fontTimesNewRoman11 = FontFactory.GetFont("Arial", 11);
            Font fontTimesNewRoman11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontTimesNewRoman13 = FontFactory.GetFont("Arial", 13);
            Font fontTimesNewRoman13Bold = FontFactory.GetFont("Arial", 13, Font.BOLD);
            Font fontTimesNewRoman14 = FontFactory.GetFont("Arial", 14);
            Font fontTimesNewRoman14Bold = FontFactory.GetFont("Arial", 14, Font.BOLD);
            Font fontTimesNewRoman15 = FontFactory.GetFont("Arial", 15);
            Font fontTimesNewRoman15Bold = FontFactory.GetFont("Arial", 15, Font.BOLD);
            Font fontTimesNewRoman16 = FontFactory.GetFont("Arial", 16);
            Font fontTimesNewRoman16Bold = FontFactory.GetFont("Arial", 16, Font.BOLD);

            Document document = new Document(PageSize.LETTER, 50f, 50f, 25f, 25f);
            MemoryStream workStream = new MemoryStream();

            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 30f, 30f, 30f);

            document.Open();

            var currentCase = from d in db.TrnCases
                              where d.Id == Convert.ToInt32(id)
                              && d.IsLocked == true
                              select d;

            if (currentCase.Any())
            {
                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

                Phrase phraseRepublic = new Phrase("Republic of the Philippines\n", fontTimesNewRoman15);
                Phrase phraseGovernment = new Phrase("DANAO CITY GOVERNMENT\n", fontTimesNewRoman16Bold);
                Phrase phraseCity = new Phrase("Danao City, Cebu\n", fontTimesNewRoman15);

                Paragraph headerParagraph = new Paragraph
                {
                    phraseRepublic,
                    phraseGovernment,
                    phraseCity
                };

                headerParagraph.SetLeading(12f, 0);
                headerParagraph.Alignment = Element.ALIGN_CENTER;

                Phrase phraseTitle = new Phrase("OBLIGATION REQUEST", fontTimesNewRoman16Bold);
                Phrase phraseNo = new Phrase("No.", fontTimesNewRoman10);

                PdfPTable pdfTableHeaderDetail = new PdfPTable(2);
                pdfTableHeaderDetail.SetWidths(new float[] { 86.25f, 28.75f });
                pdfTableHeaderDetail.WidthPercentage = 100;
                pdfTableHeaderDetail.AddCell(new PdfPCell(headerParagraph) { HorizontalAlignment = 1, PaddingBottom = 6f, Colspan = 2 });
                pdfTableHeaderDetail.AddCell(new PdfPCell(phraseTitle) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableHeaderDetail.AddCell(new PdfPCell(phraseNo) { PaddingBottom = 6f, PaddingTop = 6f });
                document.Add(pdfTableHeaderDetail);

                String citizensName = currentCase.FirstOrDefault().MstCitizen.Firstname + " " + currentCase.FirstOrDefault().MstCitizen.Middlename + " " + currentCase.FirstOrDefault().MstCitizen.Surname;
                String citizensAddress = currentCase.FirstOrDefault().MstCitizen.MstBarangay1.Barangay + ", " + currentCase.FirstOrDefault().MstCitizen.MstCity1.City;

                Phrase phrasePayee = new Phrase("Payee", fontTimesNewRoman11);
                Phrase phrasePayeeValue = new Phrase(citizensName, fontTimesNewRoman11Bold);
                Phrase phraseOffice = new Phrase("Office", fontTimesNewRoman11);
                Phrase phraseOfficeValue = new Phrase(currentCase.FirstOrDefault().MstService.MstServiceGroup.MstServiceDepartment.ServiceDepartment, fontTimesNewRoman11Bold);
                Phrase phraseAddress = new Phrase("Address", fontTimesNewRoman11);
                Phrase phraseAddressValue = new Phrase(citizensAddress, fontTimesNewRoman11Bold);

                PdfPTable pdfTablePayeeDetail = new PdfPTable(2);
                pdfTablePayeeDetail.SetWidths(new float[] { 15f, 100f });
                pdfTablePayeeDetail.WidthPercentage = 100;
                pdfTablePayeeDetail.AddCell(new PdfPCell(phrasePayee) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTablePayeeDetail.AddCell(new PdfPCell(phrasePayeeValue) { HorizontalAlignment = 0, PaddingBottom = 6f, PaddingTop = 3f });
                pdfTablePayeeDetail.AddCell(new PdfPCell(phraseOffice) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTablePayeeDetail.AddCell(new PdfPCell(phraseOfficeValue) { HorizontalAlignment = 0, PaddingBottom = 6f, PaddingTop = 3f });
                pdfTablePayeeDetail.AddCell(new PdfPCell(phraseAddress) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTablePayeeDetail.AddCell(new PdfPCell(phraseAddressValue) { HorizontalAlignment = 0, PaddingBottom = 6f, PaddingTop = 3f });
                document.Add(pdfTablePayeeDetail);

                Phrase phraseResponsibilityCenter = new Phrase("Responsibility Center", fontTimesNewRoman11);
                Phrase phraseResponsibilityCenterValue = new Phrase(" ", fontTimesNewRoman11);
                Phrase phraseAllotmentClass = new Phrase("Allotment Class", fontTimesNewRoman11);
                Phrase phraseFPP = new Phrase("F.P.P", fontTimesNewRoman11);
                Phrase phraseAccountCode = new Phrase("Account Code", fontTimesNewRoman11);
                Phrase phraseAmount = new Phrase("Amount", fontTimesNewRoman11);

                PdfPTable pdfTableResponsibilityCenterDetail = new PdfPTable(6);
                pdfTableResponsibilityCenterDetail.SetWidths(new float[] { 15f, 42.5f, 20f, 8.75f, 15f, 13.75f });
                pdfTableResponsibilityCenterDetail.WidthPercentage = 100;
                pdfTableResponsibilityCenterDetail.AddCell(new PdfPCell(phraseResponsibilityCenter) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableResponsibilityCenterDetail.AddCell(new PdfPCell(phraseResponsibilityCenterValue) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableResponsibilityCenterDetail.AddCell(new PdfPCell(phraseAllotmentClass) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableResponsibilityCenterDetail.AddCell(new PdfPCell(phraseFPP) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableResponsibilityCenterDetail.AddCell(new PdfPCell(phraseAccountCode) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableResponsibilityCenterDetail.AddCell(new PdfPCell(phraseAmount) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableResponsibilityCenterDetail.AddCell(new PdfPCell(new Phrase("CSWDO", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 50f });
                pdfTableResponsibilityCenterDetail.AddCell(new PdfPCell(new Phrase("Assistamce for Indigent. " + currentCase.FirstOrDefault().MstService.MstServiceGroup.ServiceGroup.ToUpper() + " \n\nTo payment for Financial Assitance.", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 50f });
                pdfTableResponsibilityCenterDetail.AddCell(new PdfPCell(new Phrase("200", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 50f });
                pdfTableResponsibilityCenterDetail.AddCell(new PdfPCell(new Phrase("7611", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 50f });
                pdfTableResponsibilityCenterDetail.AddCell(new PdfPCell(new Phrase("5-02-99-990", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 50f });
                pdfTableResponsibilityCenterDetail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().Amount.ToString("#,##0.00"), fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 100f });

                String amount = Convert.ToString(Math.Round(currentCase.FirstOrDefault().Amount * 100) / 100);
                String amountInWords = GetMoneyWord(amount);

                if (currentCase.FirstOrDefault().Amount == 0)
                {
                    amountInWords = "Zero Pesos Only.";
                }

                pdfTableResponsibilityCenterDetail.AddCell(new PdfPCell(new Phrase(amountInWords, fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f, Colspan = 4 });
                pdfTableResponsibilityCenterDetail.AddCell(new PdfPCell(new Phrase("TOTAL", fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableResponsibilityCenterDetail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().Amount.ToString("#,##0.00"), fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                document.Add(pdfTableResponsibilityCenterDetail);

                Phrase phraseCertifiedLetterA = new Phrase("A", fontTimesNewRoman11Bold);
                Phrase phraseCertifiedValueA = new Phrase("Certified\n\n", fontTimesNewRoman11Bold);
                Phrase phraseAmountObligated = new Phrase("___ Charges to approporiation/allotment necessary, lawful and under my direct supervision. \n", fontTimesNewRoman11);
                Phrase phraseSupportingDocuments = new Phrase("___ Supporting documents valid, proper and legal", fontTimesNewRoman11);

                Paragraph certifiedAValueParagraph = new Paragraph
                {
                    phraseCertifiedValueA,
                    phraseAmountObligated,
                    phraseSupportingDocuments
                };

                Phrase phraseCertifiedLetterB = new Phrase("B", fontTimesNewRoman11Bold);
                Phrase phraseCertifiedValueB = new Phrase("Certified\n\n", fontTimesNewRoman11Bold);
                Phrase phraseFunds = new Phrase("Existence of available apprporiation", fontTimesNewRoman11);

                Paragraph certifiedBValueParagraph = new Paragraph
                {
                    phraseCertifiedValueB,
                    phraseFunds
                };

                PdfPTable pdfTableCertifiedDetail = new PdfPTable(4);
                pdfTableCertifiedDetail.SetWidths(new float[] { 7.5f, 50f, 7.5f, 50f });
                pdfTableCertifiedDetail.WidthPercentage = 100;
                pdfTableCertifiedDetail.AddCell(new PdfPCell(phraseCertifiedLetterA) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableCertifiedDetail.AddCell(new PdfPCell(certifiedAValueParagraph) { Border = Rectangle.RIGHT_BORDER, PaddingLeft = 3f, PaddingBottom = 6f, Rowspan = 2 });
                pdfTableCertifiedDetail.AddCell(new PdfPCell(phraseCertifiedLetterB) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableCertifiedDetail.AddCell(new PdfPCell(certifiedBValueParagraph) { Border = Rectangle.RIGHT_BORDER, PaddingLeft = 3f, PaddingBottom = 6f, Rowspan = 2 });
                pdfTableCertifiedDetail.AddCell(new PdfPCell(new Phrase("")) { PaddingBottom = 6f, Border = Rectangle.LEFT_BORDER });
                pdfTableCertifiedDetail.AddCell(new PdfPCell(new Phrase("")) { PaddingBottom = 6f, Border = Rectangle.LEFT_BORDER });
                document.Add(pdfTableCertifiedDetail);

                Phrase phraseSignature = new Phrase("Signature", fontTimesNewRoman11Bold);

                PdfPTable pdfTableSignatureDetail = new PdfPTable(4);
                pdfTableSignatureDetail.SetWidths(new float[] { 15f, 42.5f, 15f, 42.5f });
                pdfTableSignatureDetail.WidthPercentage = 100;
                pdfTableSignatureDetail.AddCell(new PdfPCell(phraseSignature) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 9f, PaddingBottom = 12f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase("")) { PaddingBottom = 12f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(phraseSignature) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 9f, PaddingBottom = 12f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase("")) { PaddingBottom = 12f });
                document.Add(pdfTableSignatureDetail);

                Phrase phrasePrintedNameA = new Phrase("Printed Name", fontTimesNewRoman11);
                Phrase phrasePrintedNameValueA = new Phrase(currentCase.FirstOrDefault().MstService.MstServiceGroup.MstServiceDepartment.OfficerInCharge, fontTimesNewRoman11Bold);

                String budgetOfficer = "";
                String budgetOfficerPosition = "";
                var getBudgetOfficer = from d in db.SysSettings
                                       where d.Code == "CITYBUDGETOFFICER"
                                       select d;

                if (getBudgetOfficer.Any())
                {
                    budgetOfficer = getBudgetOfficer.FirstOrDefault().Value;
                    budgetOfficerPosition = getBudgetOfficer.FirstOrDefault().Category;
                }

                Phrase phrasePrintedNameB = new Phrase("Printed Name", fontTimesNewRoman11);
                Phrase phrasePrintedNameValueB = new Phrase(budgetOfficer, fontTimesNewRoman11Bold);

                PdfPTable pdfTablePrintedDetail = new PdfPTable(4);
                pdfTablePrintedDetail.SetWidths(new float[] { 15f, 42.5f, 15f, 42.5f });
                pdfTablePrintedDetail.WidthPercentage = 100;
                pdfTablePrintedDetail.AddCell(new PdfPCell(phrasePrintedNameA) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 9f, PaddingBottom = 12f });
                pdfTablePrintedDetail.AddCell(new PdfPCell(phrasePrintedNameValueA) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 9f, PaddingBottom = 3f });
                pdfTablePrintedDetail.AddCell(new PdfPCell(phrasePrintedNameB) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 9f, PaddingBottom = 12f });
                pdfTablePrintedDetail.AddCell(new PdfPCell(phrasePrintedNameValueB) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 9f, PaddingBottom = 3f });
                document.Add(pdfTablePrintedDetail);

                Phrase phrasePositionNameA = new Phrase("Position", fontTimesNewRoman11);
                Phrase phrasePositionNameValueA = new Phrase("CSWDO", fontTimesNewRoman11);

                Phrase phrasePositionNameB = new Phrase("Position", fontTimesNewRoman11);
                Phrase phrasePositionNameValueB = new Phrase("OIC City Budget Officer", fontTimesNewRoman11);

                PdfPTable pdfTablePositionDetail = new PdfPTable(4);
                pdfTablePositionDetail.SetWidths(new float[] { 15f, 42.5f, 15f, 42.5f });
                pdfTablePositionDetail.WidthPercentage = 100;
                pdfTablePositionDetail.AddCell(new PdfPCell(phrasePositionNameA) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 9f, PaddingBottom = 18f, Rowspan = 2 });
                pdfTablePositionDetail.AddCell(new PdfPCell(phrasePositionNameValueA) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingBottom = 6f });
                pdfTablePositionDetail.AddCell(new PdfPCell(phrasePositionNameB) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingTop = 9f, PaddingBottom = 18f, Rowspan = 2 });
                pdfTablePositionDetail.AddCell(new PdfPCell(phrasePositionNameValueB) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingBottom = 6f });
                pdfTablePositionDetail.AddCell(new PdfPCell(new Phrase("Head, Requesting Office / Authorized Representative", fontTimesNewRoman09)) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingBottom = 6f });
                pdfTablePositionDetail.AddCell(new PdfPCell(new Phrase("Head, Unit/ Authorized Representative", fontTimesNewRoman09)) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingBottom = 6f });
                document.Add(pdfTablePositionDetail);

                PdfPTable pdfTableSignatureDateDetail = new PdfPTable(4);
                pdfTableSignatureDateDetail.SetWidths(new float[] { 15f, 42.5f, 15f, 42.5f });
                pdfTableSignatureDateDetail.WidthPercentage = 100;
                pdfTableSignatureDateDetail.AddCell(new PdfPCell(new Phrase("Date", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingBottom = 6f });
                pdfTableSignatureDateDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { PaddingLeft = 3f, PaddingBottom = 6f });
                pdfTableSignatureDateDetail.AddCell(new PdfPCell(new Phrase("Date", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingBottom = 6f });
                pdfTableSignatureDateDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { PaddingLeft = 3f, PaddingBottom = 6f });
                document.Add(pdfTableSignatureDateDetail);

                PdfPTable pdfTableStatusDetail = new PdfPTable(1);
                pdfTableStatusDetail.SetWidths(new float[] { 115f });
                pdfTableStatusDetail.WidthPercentage = 100;
                pdfTableStatusDetail.AddCell(new PdfPCell(new Phrase("STATUS OF OBLIGATIONS AND ALLOTMENT BALANCE", fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingBottom = 6f });
                document.Add(pdfTableStatusDetail);

                PdfPTable pdfTableStatusObligationValueDetail = new PdfPTable(6);
                pdfTableStatusObligationValueDetail.SetWidths(new float[] { 15f, 27.5f, 25f, 15.75f, 18f, 13.75f });
                pdfTableStatusObligationValueDetail.WidthPercentage = 100;
                pdfTableStatusObligationValueDetail.AddCell(new PdfPCell(new Phrase("DATE", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f, Rowspan = 2 });
                pdfTableStatusObligationValueDetail.AddCell(new PdfPCell(new Phrase("PARTICULARS", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f, Rowspan = 2 });
                pdfTableStatusObligationValueDetail.AddCell(new PdfPCell(new Phrase("AMOUNT OBLIGATED AND ALLOTMENT BALANCE", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f, Colspan = 3 });
                pdfTableStatusObligationValueDetail.AddCell(new PdfPCell(new Phrase("POSTED BY", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f, Rowspan = 2 });
                pdfTableStatusObligationValueDetail.AddCell(new PdfPCell(new Phrase("ALLOTMENT", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableStatusObligationValueDetail.AddCell(new PdfPCell(new Phrase("OBLIGATION", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableStatusObligationValueDetail.AddCell(new PdfPCell(new Phrase("BALANCE", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableStatusObligationValueDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 50f });
                pdfTableStatusObligationValueDetail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().MstService.MstServiceGroup.ServiceGroup.ToUpper(), fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 50f });
                pdfTableStatusObligationValueDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 50f });
                pdfTableStatusObligationValueDetail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().Amount.ToString("#,##0.00"), fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 50f });
                pdfTableStatusObligationValueDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 50f });
                pdfTableStatusObligationValueDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 50f });
                document.Add(pdfTableStatusObligationValueDetail);
            }
            else
            {
                Paragraph emptyParagraph = new Paragraph("\n");
                document.Add(emptyParagraph);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            if (ContentDispositionHeaderValue.TryParse("inline; filename=certificateOfEligibility.pdf", out ContentDispositionHeaderValue contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }

        [HttpGet, Route("print/journalEntryVoucher/{id}")]
        public HttpResponseMessage PrintJournalEntryVoucher(String id)
        {
            FontFactory.RegisterDirectories();

            Font fontTimesNewRoman09 = FontFactory.GetFont("Times New Roman", 9);
            Font fontTimesNewRoman09Bold = FontFactory.GetFont("Times New Roman", 9, Font.BOLD);
            Font fontTimesNewRoman10 = FontFactory.GetFont("Times New Roman", 10);
            Font fontTimesNewRoman10Bold = FontFactory.GetFont("Times New Roman", 10, Font.BOLD);
            Font fontTimesNewRoman11 = FontFactory.GetFont("Times New Roman", 11);
            Font fontTimesNewRoman11Bold = FontFactory.GetFont("Times New Roman", 11, Font.BOLD);
            Font fontTimesNewRoman12 = FontFactory.GetFont("Times New Roman", 12);
            Font fontTimesNewRoman12Bold = FontFactory.GetFont("Times New Roman", 12, Font.BOLD);
            Font fontTimesNewRoman13 = FontFactory.GetFont("Times New Roman", 13);
            Font fontTimesNewRoman13Bold = FontFactory.GetFont("Times New Roman", 13, Font.BOLD);
            Font fontTimesNewRoman14 = FontFactory.GetFont("Times New Roman", 14);
            Font fontTimesNewRoman14Bold = FontFactory.GetFont("Times New Roman", 14, Font.BOLD);
            Font fontTimesNewRoman15 = FontFactory.GetFont("Times New Roman", 15);
            Font fontTimesNewRoman15Bold = FontFactory.GetFont("Times New Roman", 15, Font.BOLD);
            Font fontTimesNewRoman16 = FontFactory.GetFont("Times New Roman", 16);
            Font fontTimesNewRoman16Bold = FontFactory.GetFont("Times New Roman", 16, Font.BOLD);

            Document document = new Document(PageSize.LETTER, 50f, 50f, 25f, 25f);
            MemoryStream workStream = new MemoryStream();

            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 30f, 30f, 30f);

            document.Open();

            var currentCase = from d in db.TrnCases
                              where d.Id == Convert.ToInt32(id)
                              && d.IsLocked == true
                              select d;

            if (currentCase.Any())
            {
                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

                Phrase phraseTitle = new Phrase("JOURNAL ENTRY VOUCHER", fontTimesNewRoman13);

                Paragraph headerParagraph = new Paragraph
                {
                    phraseTitle,
                };

                headerParagraph.SetLeading(12f, 0);
                headerParagraph.Alignment = Element.ALIGN_CENTER;

                PdfPTable pdfTableHeaderDetail = new PdfPTable(2);
                pdfTableHeaderDetail.SetWidths(new float[] { 86.25f, 28.75f });
                pdfTableHeaderDetail.WidthPercentage = 100;
                pdfTableHeaderDetail.AddCell(new PdfPCell(headerParagraph) { HorizontalAlignment = 1, PaddingBottom = 6f, Colspan = 2 });
                pdfTableHeaderDetail.AddCell(new PdfPCell(new Phrase("DANAO CITY GOVERNMENT\nLGU", fontTimesNewRoman12)) { Border = Rectangle.LEFT_BORDER, HorizontalAlignment = 1, PaddingBottom = 6f, PaddingTop = 10f });
                pdfTableHeaderDetail.AddCell(new PdfPCell(new Phrase("No.\nDate", fontTimesNewRoman12)) { Border = Rectangle.RIGHT_BORDER, PaddingBottom = 6f, PaddingTop = 10f });
                document.Add(pdfTableHeaderDetail);

                Phrase phraseCollection = new Phrase("Collection       ___ Check Disbursement   ___ Cash Disbursement     ___ Others", fontTimesNewRoman11);

                PdfPTable pdfTableCollectionDetail = new PdfPTable(1);
                pdfTableCollectionDetail.SetWidths(new float[] { 115f });
                pdfTableCollectionDetail.WidthPercentage = 100;
                pdfTableCollectionDetail.AddCell(new PdfPCell(phraseCollection) { HorizontalAlignment = 1, PaddingBottom = 6f, Colspan = 2 });
                document.Add(pdfTableCollectionDetail);

                PdfPTable pdfTableAccountingEntriesDetail = new PdfPTable(6);
                pdfTableAccountingEntriesDetail.SetWidths(new float[] { 15f, 40.5f, 20.625f, 10.75f, 14.375f, 14.375f });
                pdfTableAccountingEntriesDetail.WidthPercentage = 100;
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("Resposibility Center", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f, Rowspan = 3 });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("ACCOUNTING ENTRIES", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f, Colspan = 5 });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("Accounting and Explanation", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f, Rowspan = 2 });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("Accounting Code", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f, Rowspan = 2 });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("PR", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f, Rowspan = 2 });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("Amount", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f, Colspan = 2 });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("Debit", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("Credit", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("CSWDO", fontTimesNewRoman11)) { PaddingTop = 10f, HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("Other Maintenance & Operating Expenses", fontTimesNewRoman11)) { PaddingTop = 10f, HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("7611-5-02-99-990", fontTimesNewRoman11)) { PaddingTop = 10f, HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("(1)", fontTimesNewRoman11)) { PaddingTop = 10f, HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().Amount.ToString("#,##0.00"), fontTimesNewRoman11)) { PaddingTop = 10f, HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { PaddingTop = 10f, HorizontalAlignment = 2, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("CASH IN BANK", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("1010 20 10", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("(1)", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().Amount.ToString("#,##0.00"), fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("To Record Payment for financial Assitance", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("for " + currentCase.FirstOrDefault().MstService.MstServiceGroup.ServiceGroup, fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });

                String citizensName = currentCase.FirstOrDefault().MstCitizen.Firstname + " " + currentCase.FirstOrDefault().MstCitizen.Middlename + " " + currentCase.FirstOrDefault().MstCitizen.Surname;

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("RE: " + citizensName, fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 2, PaddingBottom = 6f });

                String amount = Convert.ToString(Math.Round(currentCase.FirstOrDefault().Amount * 100) / 100);
                String amountInWords = GetMoneyWord(amount);

                if (currentCase.FirstOrDefault().Amount == 0)
                {
                    amountInWords = "Zero Pesos Only.";
                }

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(amountInWords, fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f, Colspan = 3 });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("TOTAL", fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().Amount.ToString("#,##0.00"), fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().Amount.ToString("#,##0.00"), fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f, Colspan = 6 });

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("Prepared By", fontTimesNewRoman11)) { PaddingBottom = 50f, Colspan = 2 });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("Approved By", fontTimesNewRoman11)) { PaddingBottom = 50f, Colspan = 4 });

                String cityAccoutant = "";
                var getCityAccountant = from d in db.SysSettings
                                        where d.Code == "CITYACCOUNTANT"
                                        select d;

                if (getCityAccountant.Any())
                {
                    cityAccoutant = getCityAccountant.FirstOrDefault().Value;
                }

                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().MstUser.Username, fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f, Colspan = 2 });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase(cityAccoutant, fontTimesNewRoman11Bold)) { HorizontalAlignment = 1, PaddingBottom = 6f, Colspan = 4 });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("SWA", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f, Colspan = 2 });
                pdfTableAccountingEntriesDetail.AddCell(new PdfPCell(new Phrase("City Accountant", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingBottom = 6f, Colspan = 4 });
                document.Add(pdfTableAccountingEntriesDetail);
            }
            else
            {
                Paragraph emptyParagraph = new Paragraph("\n");
                document.Add(emptyParagraph);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            if (ContentDispositionHeaderValue.TryParse("inline; filename=certificateOfEligibility.pdf", out ContentDispositionHeaderValue contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }

        [HttpGet, Route("print/CSWDForm200/{id}")]
        public HttpResponseMessage PrintCSWDForm200(String id)
        {
            FontFactory.RegisterDirectories();

            Font fontTimesNewRoman09 = FontFactory.GetFont("Times New Roman", 9);
            Font fontTimesNewRoman09Bold = FontFactory.GetFont("Times New Roman", 9, Font.BOLD);
            Font fontTimesNewRoman10 = FontFactory.GetFont("Times New Roman", 10);
            Font fontTimesNewRoman10Bold = FontFactory.GetFont("Times New Roman", 10, Font.BOLD);
            Font fontTimesNewRoman11 = FontFactory.GetFont("Times New Roman", 11);
            Font fontTimesNewRoman11Bold = FontFactory.GetFont("Times New Roman", 11, Font.BOLD);
            Font fontTimesNewRoman13 = FontFactory.GetFont("Times New Roman", 13);
            Font fontTimesNewRoman13Bold = FontFactory.GetFont("Times New Roman", 13, Font.BOLD);
            Font fontTimesNewRoman14 = FontFactory.GetFont("Times New Roman", 14);
            Font fontTimesNewRoman14Bold = FontFactory.GetFont("Times New Roman", 14, Font.BOLD);
            Font fontTimesNewRoman15 = FontFactory.GetFont("Times New Roman", 15);
            Font fontTimesNewRoman15Bold = FontFactory.GetFont("Times New Roman", 15, Font.BOLD);
            Font fontTimesNewRoman16 = FontFactory.GetFont("Times New Roman", 16);
            Font fontTimesNewRoman16Bold = FontFactory.GetFont("Times New Roman", 16, Font.BOLD);

            Document document = new Document(PageSize.LETTER, 50f, 50f, 25f, 25f);
            MemoryStream workStream = new MemoryStream();

            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 30f, 30f, 30f);

            document.Open();

            var currentCase = from d in db.TrnCases
                              where d.Id == Convert.ToInt32(id)
                              && d.IsLocked == true
                              select d;

            if (currentCase.Any())
            {
                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

                Phrase phraseRepublic = new Phrase("Republic of the Philippines\n", fontTimesNewRoman11);
                Phrase phraseDepartment = new Phrase("City Social Welfare & Development Office\n", fontTimesNewRoman11);
                Phrase phraseCity = new Phrase("Danao City", fontTimesNewRoman11);

                Paragraph headerParagraph = new Paragraph
                {
                    phraseRepublic,
                    phraseDepartment,
                    phraseCity
                };

                PdfPTable pdfTableHeaderDetail = new PdfPTable(1);
                pdfTableHeaderDetail.SetWidths(new float[] { 100f });
                pdfTableHeaderDetail.WidthPercentage = 100;
                pdfTableHeaderDetail.AddCell(new PdfPCell(headerParagraph) { Border = 0, HorizontalAlignment = 1 });
                document.Add(pdfTableHeaderDetail);
                document.Add(new Paragraph("\n"));

                PdfPTable pdfTableCSWDForm200Detail = new PdfPTable(5);
                pdfTableCSWDForm200Detail.SetWidths(new float[] { 50f, 50f, 10f, 50f, 50f });
                pdfTableCSWDForm200Detail.WidthPercentage = 100;
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase("CSWD FORM 200", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase("Name of Disaster", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase("Material Aid", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase("", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase("Fire", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase("Labor Relief", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase("Typhoon", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase("Flood", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase("Earthquake", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase("Date of Occurence:", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase("Name of Family Head:", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().MstCitizen.Firstname + " " + currentCase.FirstOrDefault().MstCitizen.Middlename + " " + currentCase.FirstOrDefault().MstCitizen.Surname, fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase("Age:", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase((DateTime.Now.Year - currentCase.FirstOrDefault().MstCitizen.DateOfBirth.Year).ToString(), fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase("Address:", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().MstCitizen.MstBarangay1.Barangay + ", " + currentCase.FirstOrDefault().MstCitizen.MstCity1.City, fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase("Occupation:", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(currentCase.FirstOrDefault().MstCitizen.MstOccupation.Occupation, fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase("Educational Attainment:", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                pdfTableCSWDForm200Detail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 0, PaddingBottom = 0 });
                document.Add(pdfTableCSWDForm200Detail);
                document.Add(new Paragraph("\n"));

                PdfPTable pdfTableDependentsDetail = new PdfPTable(4);
                pdfTableDependentsDetail.SetWidths(new float[] { 50f, 35f, 20f, 50f });
                pdfTableDependentsDetail.WidthPercentage = 100;
                pdfTableDependentsDetail.AddCell(new PdfPCell(new Phrase("Dependents", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingRight = 3f, PaddingBottom = 6f });
                pdfTableDependentsDetail.AddCell(new PdfPCell(new Phrase("Relationship", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingRight = 3f, PaddingBottom = 6f });
                pdfTableDependentsDetail.AddCell(new PdfPCell(new Phrase("Age", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingRight = 3f, PaddingBottom = 6f });
                pdfTableDependentsDetail.AddCell(new PdfPCell(new Phrase("Educational Attainment", fontTimesNewRoman11)) { HorizontalAlignment = 1, PaddingLeft = 3f, PaddingRight = 3f, PaddingBottom = 6f });

                var citizensChildren = from d in db.MstCitizensChildrens
                                       where d.CitizenId == currentCase.FirstOrDefault().CitizenId
                                       select d;

                if (citizensChildren.Any())
                {
                    foreach (var child in citizensChildren)
                    {
                        pdfTableDependentsDetail.AddCell(new PdfPCell(new Phrase(child.Fullname, fontTimesNewRoman11)) { PaddingLeft = 3f, PaddingRight = 3f });
                        pdfTableDependentsDetail.AddCell(new PdfPCell(new Phrase("Child", fontTimesNewRoman11)) { PaddingLeft = 3f, PaddingRight = 3f });
                        pdfTableDependentsDetail.AddCell(new PdfPCell(new Phrase((DateTime.Now.Year - child.DateOfBirth.Year).ToString(), fontTimesNewRoman11)) { PaddingLeft = 3f, PaddingRight = 3f });
                        pdfTableDependentsDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { PaddingLeft = 3f, PaddingRight = 3f });
                    }
                }
                else
                {
                    for (int i = 0; i < 10; i++)
                    {
                        pdfTableDependentsDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { PaddingLeft = 3f, PaddingRight = 3f });
                        pdfTableDependentsDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { PaddingLeft = 3f, PaddingRight = 3f });
                        pdfTableDependentsDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { PaddingLeft = 3f, PaddingRight = 3f });
                        pdfTableDependentsDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { PaddingLeft = 3f, PaddingRight = 3f });
                    }
                }

                document.Add(pdfTableDependentsDetail);
                document.Add(new Paragraph("\n"));

                PdfPTable pdfTableOwnerDetail = new PdfPTable(5);
                pdfTableOwnerDetail.SetWidths(new float[] { 25f, 50f, 15f, 30f, 50f });
                pdfTableOwnerDetail.WidthPercentage = 100;
                pdfTableOwnerDetail.AddCell(new PdfPCell(new Phrase("House Owner:", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableOwnerDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableOwnerDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableOwnerDetail.AddCell(new PdfPCell(new Phrase("Totally Damaged:", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableOwnerDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableOwnerDetail.AddCell(new PdfPCell(new Phrase("House Owner:", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableOwnerDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableOwnerDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableOwnerDetail.AddCell(new PdfPCell(new Phrase("Partially Damaged:", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableOwnerDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f });
                document.Add(pdfTableOwnerDetail);
                document.Add(new Paragraph("\n"));

                PdfPTable pdfTableSignatureDetail = new PdfPTable(3);
                pdfTableSignatureDetail.SetWidths(new float[] { 50f, 20f, 50f });
                pdfTableSignatureDetail.WidthPercentage = 100;
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase("Signature over Printed Name of Client", fontTimesNewRoman11)) { HorizontalAlignment = 1, Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase("Signature over Printed Name of SWA", fontTimesNewRoman11)) { HorizontalAlignment = 1, Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableSignatureDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                document.Add(pdfTableSignatureDetail);
                document.Add(new Paragraph("\n"));

                document.Add(new Paragraph(new Phrase("Registered _____________________________  20 _____", fontTimesNewRoman11)));
                document.Add(new Paragraph("\n"));

                PdfPTable pdfTableEmptyTablesDetail = new PdfPTable(3);
                pdfTableEmptyTablesDetail.SetWidths(new float[] { 50f, 50f, 50f });
                pdfTableEmptyTablesDetail.WidthPercentage = 100;

                for (int i = 0; i < 3; i++)
                {
                    pdfTableEmptyTablesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { PaddingLeft = 3f, PaddingRight = 3f });
                    pdfTableEmptyTablesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { PaddingLeft = 3f, PaddingRight = 3f });
                    pdfTableEmptyTablesDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { PaddingLeft = 3f, PaddingRight = 3f });
                }

                document.Add(pdfTableEmptyTablesDetail);
                document.Add(new Paragraph("\n"));

                String amount = Convert.ToString(Math.Round(currentCase.FirstOrDefault().Amount * 100) / 100);
                String amountInWords = GetMoneyWord(amount);

                if (currentCase.FirstOrDefault().Amount == 0)
                {
                    amountInWords = "Zero Pesos Only.";
                }

                document.Add(new Paragraph(new Phrase("Recommendation:   The client is recommended to avail " + currentCase.FirstOrDefault().MstService.MstServiceGroup.ServiceGroup + ".", fontTimesNewRoman11)));
                document.Add(new Paragraph(new Phrase("Remarks:   Worth " + amountInWords + " (" + currentCase.FirstOrDefault().Amount.ToString("#,##0.00") + ").", fontTimesNewRoman11)));
                document.Add(new Paragraph("\n"));

                String CSWDOOIC = currentCase.FirstOrDefault().MstService.MstServiceGroup.MstServiceDepartment.OfficerInCharge;

                PdfPTable pdfTableNotedDetail = new PdfPTable(3);
                pdfTableNotedDetail.SetWidths(new float[] { 50f, 50f, 50f });
                pdfTableNotedDetail.WidthPercentage = 100;
                pdfTableNotedDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableNotedDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableNotedDetail.AddCell(new PdfPCell(new Phrase("Noted By:", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableNotedDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableNotedDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableNotedDetail.AddCell(new PdfPCell(new Phrase(CSWDOOIC, fontTimesNewRoman11)) { HorizontalAlignment = 1, Border = Rectangle.BOTTOM_BORDER, PaddingLeft = 3f, PaddingRight = 3f, PaddingTop = 10f });
                pdfTableNotedDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableNotedDetail.AddCell(new PdfPCell(new Phrase(" ", fontTimesNewRoman11)) { Border = 0, PaddingLeft = 3f, PaddingRight = 3f });
                pdfTableNotedDetail.AddCell(new PdfPCell(new Phrase("CSWDO", fontTimesNewRoman11)) { HorizontalAlignment = 1, Border = Rectangle.TOP_BORDER, PaddingLeft = 3f, PaddingRight = 3f });
                document.Add(pdfTableNotedDetail);
            }
            else
            {
                Paragraph emptyParagraph = new Paragraph("\n");
                document.Add(emptyParagraph);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            if (ContentDispositionHeaderValue.TryParse("inline; filename=certificateOfEligibility.pdf", out ContentDispositionHeaderValue contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }

        public static String GetMoneyWord(String input)
        {
            String decimals = "";
            if (input.Contains("."))
            {
                decimals = input.Substring(input.IndexOf(".") + 1);
                input = input.Remove(input.IndexOf("."));
            }

            String strWords = GetMoreThanThousandNumberWords(input);
            if (decimals.Length > 0)
            {
                if (Convert.ToDecimal(decimals) > 0)
                {
                    String getFirstRoundedDecimals = new String(decimals.Take(2).ToArray());
                    strWords += " Pesos And " + GetMoreThanThousandNumberWords(getFirstRoundedDecimals) + " Cents Only";
                }
                else
                {
                    strWords += " Pesos Only";
                }
            }
            else
            {
                strWords += " Pesos Only";
            }

            return strWords;
        }

        private static String GetMoreThanThousandNumberWords(string input)
        {
            try
            {
                String[] seperators = { "", " Thousand ", " Million ", " Billion " };

                int i = 0;

                String strWords = "";

                while (input.Length > 0)
                {
                    String _3digits = input.Length < 3 ? input : input.Substring(input.Length - 3);
                    input = input.Length < 3 ? "" : input.Remove(input.Length - 3);

                    Int32 no = Int32.Parse(_3digits);
                    _3digits = GetHundredNumberWords(no);

                    _3digits += seperators[i];
                    strWords = _3digits + strWords;

                    i++;
                }

                return strWords;
            }
            catch
            {
                return "Invalid Amount";
            }
        }

        private static String GetHundredNumberWords(Int32 no)
        {
            String[] Ones =
            {
                "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
                "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Ninteen"
            };

            String[] Tens = { "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            String word = "";

            if (no > 99 && no < 1000)
            {
                Int32 i = no / 100;
                word = word + Ones[i - 1] + " Hundred ";
                no = no % 100;
            }

            if (no > 19 && no < 100)
            {
                Int32 i = no / 10;
                word = word + Tens[i - 1] + " ";
                no = no % 10;
            }

            if (no > 0 && no < 20)
            {
                word = word + Ones[no - 1];
            }

            return word;
        }

        [HttpPost, Route("add/new/citizen/card")]
        public HttpResponseMessage AddCitizen(Entities.MstCitizen objCitizen)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                var sex = from d in db.MstSexes select d;
                if (sex.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Sex not found!");
                }

                var civilStatus = from d in db.MstCivilStatus select d;
                if (civilStatus.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Civil status not found!");
                }

                var bloodType = from d in db.MstBloodTypes select d;
                if (bloodType.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Blood type not found!");
                }

                var citizenship = from d in db.MstCitizenships select d;
                if (citizenship.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizenship not found!");
                }

                var barangay = from d in db.MstBarangays where d.Id == objCitizen.ResidentialBarangayId select d;
                if (barangay.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Barangay not found!");
                }

                var city = from d in db.MstCities select d;
                if (city.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "City not found!");
                }

                var province = from d in db.MstProvinces select d;
                if (province.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Province not found!");
                }

                var occupation = from d in db.MstOccupations select d;
                if (occupation.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Occupation not found!");
                }

                var status = from d in db.MstStatus
                             where d.Category.Equals("Citizen")
                             select d;

                if (status.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Status not found!");
                }

                Data.MstCitizen newCitizen = new Data.MstCitizen()
                {
                    Surname = objCitizen.Surname,
                    Firstname = objCitizen.Firstname,
                    Middlename = objCitizen.Middlename,
                    Extensionname = objCitizen.Extensionname,
                    DateOfBirth = DateTime.Today,
                    PlaceOfBirth = "",
                    SexId = sex.FirstOrDefault().Id,
                    CivilStatusId = civilStatus.FirstOrDefault().Id,
                    Height = 0,
                    Weight = 0,
                    BloodTypeId = bloodType.FirstOrDefault().Id,
                    GSISNumber = "",
                    HDMFNumber = "",
                    PhilHealthNumber = "",
                    SSSNumber = "",
                    TINNumber = "",
                    AgencyEmployeeNumber = "",
                    CitizenshipId = citizenship.FirstOrDefault().Id,
                    TypeOfCitizenshipId = null,
                    DualCitizenshipCountry = "",
                    ResidentialNumber = "",
                    ResidentialStreet = "",
                    ResidentialVillage = "",
                    ResidentialBarangayId = barangay.FirstOrDefault().Id,
                    ResidentialCityId = city.FirstOrDefault().Id,
                    ResidentialProvinceId = province.FirstOrDefault().Id,
                    ResidentialZipCode = "",
                    ResidentialPrecinctNumber = "",
                    PermanentNumber = "",
                    PermanentStreet = "",
                    PermanentVillage = "",
                    PermanentBarangayId = barangay.FirstOrDefault().Id,
                    PermanentCityId = city.FirstOrDefault().Id,
                    PermanentProvinceId = province.FirstOrDefault().Id,
                    PermanentZipCode = "",
                    PermanentPrecinctNumber = "",
                    TelephoneNumber = "",
                    MobileNumber = "",
                    EmailAddress = "",
                    OccupationId = occupation.FirstOrDefault().Id,
                    EmployerName = "",
                    EmployerAddress = "",
                    EmployerTelephoneNumber = "",
                    SpouseSurname = "",
                    SpouseFirstname = "",
                    SpouseMiddlename = "",
                    SpouseExtensionname = "",
                    SpouseOccupationId = occupation.FirstOrDefault().Id,
                    SpouseEmployerName = "",
                    SpouseEmployerAddress = "",
                    FatherSurname = "",
                    FatherFirstname = "",
                    FatherMiddlename = "",
                    FatherExtensionname = "",
                    MotherSurname = "",
                    MotherFirstname = "",
                    MotherMiddlename = "",
                    MotherExtensionname = "",
                    PictureURL = objCitizen.PictureURL,
                    StatusId = status.FirstOrDefault().Id,
                    IsLocked = true,
                    CreatedByUserId = currentUser.FirstOrDefault().Id,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = currentUser.FirstOrDefault().Id,
                    UpdatedDateTime = DateTime.Now
                };

                db.MstCitizens.InsertOnSubmit(newCitizen);
                db.SubmitChanges();

                var cardStatus = from d in db.MstStatus
                                 where d.Category.Equals("Card")
                                 select d;

                if (cardStatus.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Card status not found!");
                }

                String cardNumber = "0000000001";
                var lastCard = from d in db.MstCitizensCards.OrderByDescending(d => d.Id) select d;
                if (lastCard.Any())
                {
                    Int32 newCardNumber = Convert.ToInt32(lastCard.FirstOrDefault().CardNumber) + 1;
                    cardNumber = FillLeadingZeroes(newCardNumber, 10);
                }

                Data.MstCitizensCard newCitizensCard = new Data.MstCitizensCard()
                {
                    CitizenId = newCitizen.Id,
                    CardNumber = cardNumber,
                    TotalBalance = 0,
                    StatusId = cardStatus.FirstOrDefault().Id
                };

                db.MstCitizensCards.InsertOnSubmit(newCitizensCard);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, newCitizensCard.Id.ToString());
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost, Route("uploadPhoto")]
        public async Task<HttpResponseMessage> UploadPhotoCitizen()
        {
            try
            {
                app_configuration app_config;
                using (StreamReader r = new StreamReader(HttpContext.Current.Server.MapPath("~/app_configuration.json")))
                {
                    string json = r.ReadToEnd();
                    app_config = JsonConvert.DeserializeObject<app_configuration>(json);
                }

                String StoragePath = app_config.PhotosBackUpStoragePath;

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                String root = HttpContext.Current.Server.MapPath("~/Uploads/Photos");
                MultipartFormDataStreamProvider provider = new MultipartFormDataStreamProvider(root);

                await Request.Content.ReadAsMultipartAsync(provider);

                MultipartFileData fileData = provider.FileData.FirstOrDefault();
                if (fileData != null)
                {
                    Trace.WriteLine(fileData.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + fileData.LocalFileName);

                    if (String.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
                    }

                    String fileName = fileData.LocalFileName;

                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }

                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }

                    var file = Path.Combine(StoragePath, fileName);
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }

                    File.Copy(fileData.LocalFileName, file);

                    return Request.CreateResponse(HttpStatusCode.OK, Url.Content(string.Format("~/Uploads/Photos/{0}", Path.GetFileName(fileData.LocalFileName))));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Image provider not found.");
                }

                //if (!Request.Content.IsMimeMultipartContent("form-data"))
                //{
                //    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                //}

                //String cloudStorageConnectionString = ConfigurationManager.AppSettings["CloudStorageConnectionString"];
                //CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);

                //String cloudStorageContainerName = ConfigurationManager.AppSettings["CloudStorageContainerName"];
                //CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                //CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(cloudStorageContainerName);

                //AzureStorageMultipartFormDataStreamProvider provider = new AzureStorageMultipartFormDataStreamProvider(cloudBlobContainer);
                //await Request.Content.ReadAsMultipartAsync(provider);

                //String fileName = provider.FileData.FirstOrDefault()?.LocalFileName;
                //if (String.IsNullOrEmpty(fileName))
                //{
                //    return BadRequest("An error has occured while uploading your file. Please try again.");
                //}

                //String imageURI = Azure.BlobStorage.BlobContainer.GetCloudBlockBlobImageURI(fileName);

                //return Ok(imageURI);
            }
            catch (Exception ex)
            {
                //return BadRequest($"An error has occured. Details: {ex.Message}");
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet, Route("barangay/dropdown/list")]
        public List<Entities.MstBarangay> BarangayDropdownList()
        {
            var barangays = from d in db.MstBarangays
                            select new Entities.MstBarangay
                            {
                                Id = d.Id,
                                Barangay = d.Barangay
                            };

            return barangays.OrderBy(d => d.Barangay).ToList();
        }
    }
}
