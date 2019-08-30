using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/users")]
    public class ApiMstUserController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstUser> UserList()
        {
            var users = from d in db.MstUsers
                        select new Entities.MstUser
                        {
                            Id = d.Id,
                            Username = d.Username,
                            Password = d.Password,
                            Fullname = d.Fullname
                        };

            return users.OrderByDescending(d => d.Id).ToList();
        }

        [HttpPut, Route("update/{id}")]
        public HttpResponseMessage UpdateUser(String id, Entities.MstUser objUser)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.Id == Convert.ToInt32(id)
                                  select d;

                if (currentUser.Any())
                {
                    var updateUser = currentUser.FirstOrDefault();
                    updateUser.Fullname = objUser.Fullname;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
