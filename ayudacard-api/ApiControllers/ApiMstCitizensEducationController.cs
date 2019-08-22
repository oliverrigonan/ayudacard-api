using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/citizen/education")]
    public class ApiMstCitizensEducationController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list/{citizenId}")]
        public List<Entities.MstCitizensEducation> CitizensEducationList(String citizenId)
        {
            var citizensEducation = from d in db.MstCitizensEducations
                                    where d.CitizenId == Convert.ToInt32(citizenId)
                                    select new Entities.MstCitizensEducation
                                    {
                                        Id = d.Id,
                                        CitizenId = d.CitizenId,
                                        EducationLevelId = d.EducationLevelId,
                                        EducationLevel = d.MstEducationLevel.EducationLevel,
                                        NameOfSchool = d.NameOfSchool,
                                        Degree = d.Degree,
                                        PeriodFrom = d.PeriodFrom.ToShortDateString(),
                                        PeriodTo = d.PeriodTo.ToShortDateString(),
                                        UnitsEarned = d.UnitsEarned,
                                        YearGraduated = d.YearGraduated
                                    };

            return citizensEducation.ToList();
        }

        [HttpGet, Route("educationLevel/dropdown/list")]
        public List<Entities.MstEducationLevel> EducationLevelDropdownList()
        {
            var educationLevels = from d in db.MstEducationLevels
                                  select new Entities.MstEducationLevel
                                  {
                                      Id = d.Id,
                                      EducationLevel = d.EducationLevel
                                  };

            return educationLevels.ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddCitizensEducation(Entities.MstCitizensEducation objCitizensEducation)
        {
            try
            {
                var citizen = from d in db.MstCitizens
                              where d.Id == objCitizensEducation.CitizenId
                              select d;

                if (!citizen.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen not found!");
                }

                var educationLevel = from d in db.MstEducationLevels
                                     where d.Id == objCitizensEducation.EducationLevelId
                                     select d;

                if (!educationLevel.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Education level not found!");
                }

                Data.MstCitizensEducation newEducation = new Data.MstCitizensEducation()
                {
                    CitizenId = objCitizensEducation.CitizenId,
                    EducationLevelId = objCitizensEducation.EducationLevelId,
                    NameOfSchool = objCitizensEducation.NameOfSchool,
                    Degree = objCitizensEducation.Degree,
                    PeriodFrom = Convert.ToDateTime(objCitizensEducation.PeriodFrom),
                    PeriodTo = Convert.ToDateTime(objCitizensEducation.PeriodTo),
                    UnitsEarned = objCitizensEducation.UnitsEarned,
                    YearGraduated = objCitizensEducation.YearGraduated
                };

                db.MstCitizensEducations.InsertOnSubmit(newEducation);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateCitizensEducation(String id, Entities.MstCitizensEducation objCitizensEducation)
        {
            try
            {
                var education = from d in db.MstCitizensEducations
                                where d.Id == Convert.ToInt32(id)
                                select d;

                if (education.Any())
                {
                    var updateEducation = education.FirstOrDefault();
                    updateEducation.EducationLevelId = objCitizensEducation.EducationLevelId;
                    updateEducation.NameOfSchool = objCitizensEducation.NameOfSchool;
                    updateEducation.Degree = objCitizensEducation.Degree;
                    updateEducation.PeriodFrom = Convert.ToDateTime(objCitizensEducation.PeriodFrom);
                    updateEducation.PeriodTo = Convert.ToDateTime(objCitizensEducation.PeriodTo);
                    updateEducation.UnitsEarned = objCitizensEducation.UnitsEarned;
                    updateEducation.YearGraduated = objCitizensEducation.YearGraduated;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Education not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteCitizensEducation(String id)
        {
            try
            {
                var education = from d in db.MstCitizensEducations
                                where d.Id == Convert.ToInt32(id)
                                select d;

                if (education.Any())
                {
                    var deleteEducation = education.FirstOrDefault();
                    db.MstCitizensEducations.DeleteOnSubmit(deleteEducation);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Education not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
