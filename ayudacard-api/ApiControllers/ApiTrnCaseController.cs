﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/trn/case")]
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
            var serviceDepartments = from d in db.MstServiceDepartments
                                     select new Entities.MstServiceDepartment
                                     {
                                         Id = d.Id,
                                         ServiceDepartment = d.ServiceDepartment
                                     };

            return serviceDepartments.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("list/{serviceDepartmentId}/{startDate}/{endDate}")]
        public List<Entities.TrnCase> CasesList(String serviceDepartmentId, String startDate, String endDate)
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

            Font fontTimesNewRoman11 = FontFactory.GetFont("Times New Roman", 11);
            Font fontTimesNewRoman11Bold = FontFactory.GetFont("Times New Roman", 11, Font.BOLD);
            Font fontTimesNewRoman12 = FontFactory.GetFont("Times New Roman", 12);
            Font fontTimesNewRoman12Bold = FontFactory.GetFont("Times New Roman", 12, Font.BOLD);

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
                Phrase phraseCity = new Phrase("City of Danao\n", fontTimesNewRoman11);
                Phrase phraseDepartment = new Phrase(currentCase.FirstOrDefault().MstService.MstServiceGroup.MstServiceDepartment.ServiceDepartment + "\n\n", fontTimesNewRoman11);
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
                pdfTableHeaderDetail.AddCell(new PdfPCell(imageLogo) { Border = 0 });
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
                String service = currentCase.FirstOrDefault().MstService.Service;
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
                String checkedBy = currentCase.FirstOrDefault().MstUser1.Fullname;

                PdfPTable pdfTableCaseDetail = new PdfPTable(4);
                pdfTableCaseDetail.SetWidths(new float[] { 20f, 40f, 15f, 25f });
                pdfTableCaseDetail.WidthPercentage = 100;
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman11)) { Colspan = 4, Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Name: ", fontTimesNewRoman11Bold)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(citizenName, fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Case No.: ", fontTimesNewRoman11Bold)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(caseNumber, fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Birth Date: ", fontTimesNewRoman11Bold)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(birthDate, fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Case Date: ", fontTimesNewRoman11Bold)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(caseDate, fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Age: ", fontTimesNewRoman11Bold)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(age.ToString(), fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Service: ", fontTimesNewRoman11Bold)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(service, fontTimesNewRoman11)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Sex: ", fontTimesNewRoman11Bold)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(sex, fontTimesNewRoman11)) { Colspan = 3, Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Civil Status: ", fontTimesNewRoman11Bold)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(civilStatus, fontTimesNewRoman11)) { Colspan = 3, Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Educ. Attainment: ", fontTimesNewRoman11Bold)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(educationalAttainment, fontTimesNewRoman11)) { Colspan = 3, Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Occupation: ", fontTimesNewRoman11Bold)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(occupation, fontTimesNewRoman11)) { Colspan = 3, Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Address: ", fontTimesNewRoman11Bold)) { Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(address, fontTimesNewRoman11)) { Colspan = 3, Border = 0 });
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("Religion: ", fontTimesNewRoman11Bold)) { Border = 0 });
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

                Phrase phraseRecommendationLabel = new Phrase("Recommendation: \n", fontTimesNewRoman11Bold);
                Phrase phraseRecommendationData = new Phrase(recommendation, fontTimesNewRoman11);
                Paragraph recommendationParagraph = new Paragraph { phraseRecommendationLabel, phraseRecommendationData };
                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase(recommendationParagraph)) { Colspan = 4, Border = 0 });

                pdfTableCaseDetail.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman11)) { Colspan = 4, Border = 0 });

                document.Add(pdfTableCaseDetail);
                document.Add(line);

                PdfPTable pdfTableUsers = new PdfPTable(4);
                pdfTableUsers.SetWidths(new float[] { 15f, 25f, 15f, 45f });
                pdfTableUsers.WidthPercentage = 100;
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("\n", fontTimesNewRoman11)) { Colspan = 4, Border = 0 });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("Prepared By: ", fontTimesNewRoman11Bold)) { Border = 0 });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(preparedBy, fontTimesNewRoman11)) { Border = 0 });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase("Checked By: ", fontTimesNewRoman11Bold)) { Border = 0 });
                pdfTableUsers.AddCell(new PdfPCell(new Phrase(checkedBy, fontTimesNewRoman11)) { Border = 0 });

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
    }
}
