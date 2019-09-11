using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/citizen/card")]
    public class ApiMstCitizensCardController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstCitizensCard> CitizensCardList()
        {
            var citizensCards = from d in db.MstCitizensCards
                                select new Entities.MstCitizensCard
                                {
                                    Id = d.Id,
                                    CitizenId = d.CitizenId,
                                    Citizen = d.MstCitizen.Surname + ", " + d.MstCitizen.Firstname + " " + d.MstCitizen.Middlename,
                                    CardNumber = d.CardNumber,
                                    TotalBalance = d.TotalBalance,
                                    StatusId = d.StatusId,
                                    Status = d.MstStatus.Status
                                };

            return citizensCards.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("citizen/list")]
        public List<Entities.MstCitizen> ListCitizen()
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
                               DateOfBirth = d.DateOfBirth.ToShortDateString(),
                               PlaceOfBirth = d.PlaceOfBirth,
                               Sex = d.MstSex.Sex
                           };

            return citizens.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("citizen/search")]
        public List<Entities.MstCitizen> SearchCitizen(String keyword)
        {
            if (String.IsNullOrEmpty(keyword))
            {
                return new List<Entities.MstCitizen>();
            }

            var citizens = from d in db.MstCitizens
                           where d.IsLocked == true
                           && (d.Surname.Contains(keyword)
                           || d.Firstname.Contains(keyword)
                           || d.Middlename.Contains(keyword)
                           || d.Extensionname.Contains(keyword))
                           select new Entities.MstCitizen
                           {
                               Id = d.Id,
                               Surname = d.Surname,
                               Firstname = d.Firstname,
                               Middlename = d.Middlename,
                               Extensionname = d.Extensionname,
                               DateOfBirth = d.DateOfBirth.ToShortDateString(),
                               PlaceOfBirth = d.PlaceOfBirth,
                               Sex = d.MstSex.Sex
                           };

            return citizens.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("status/dropdown/list")]
        public List<Entities.MstStatus> StatusDropdownList()
        {
            var statuses = from d in db.MstStatus
                           where d.Category.Equals("Card")
                           select new Entities.MstStatus
                           {
                               Id = d.Id,
                               Status = d.Status
                           };

            return statuses.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddCitizensCard(Entities.MstCitizensCard objCitizensCard)
        {
            try
            {
                var citizen = from d in db.MstCitizens where d.Id == objCitizensCard.CitizenId select d;
                if (citizen.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen not found!");
                }

                var status = from d in db.MstStatus
                             where d.Id == objCitizensCard.StatusId
                             && d.Category.Equals("Card")
                             select d;

                if (status.Any() == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Status not found!");
                }

                Data.MstCitizensCard newCitizensCard = new Data.MstCitizensCard()
                {
                    CitizenId = objCitizensCard.CitizenId,
                    CardNumber = objCitizensCard.CardNumber,
                    TotalBalance = objCitizensCard.TotalBalance,
                    StatusId = objCitizensCard.StatusId
                };

                db.MstCitizensCards.InsertOnSubmit(newCitizensCard);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateCitizensCard(String id, Entities.MstCitizensCard objCitizensCard)
        {
            try
            {
                var citizensCard = from d in db.MstCitizensCards
                                   where d.Id == Convert.ToInt32(id)
                                   select d;

                if (citizensCard.Any())
                {
                    var citizen = from d in db.MstCitizens where d.Id == objCitizensCard.CitizenId select d;
                    if (citizen.Any() == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Citizen not found!");
                    }

                    var status = from d in db.MstStatus
                                 where d.Id == objCitizensCard.StatusId
                                 && d.Category.Equals("Card")
                                 select d;

                    if (status.Any() == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Status not found!");
                    }

                    var updateCitizensCard = citizensCard.FirstOrDefault();
                    updateCitizensCard.CitizenId = objCitizensCard.CitizenId;
                    updateCitizensCard.CardNumber = objCitizensCard.CardNumber;
                    updateCitizensCard.TotalBalance = objCitizensCard.TotalBalance;
                    updateCitizensCard.StatusId = objCitizensCard.StatusId;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Card not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteCitizensCard(String id)
        {
            try
            {
                var citizensCard = from d in db.MstCitizensCards
                                   where d.Id == Convert.ToInt32(id)
                                   select d;

                if (citizensCard.Any())
                {
                    var deleteCitizensCard = citizensCard.FirstOrDefault();
                    db.MstCitizensCards.DeleteOnSubmit(deleteCitizensCard);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Card not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
