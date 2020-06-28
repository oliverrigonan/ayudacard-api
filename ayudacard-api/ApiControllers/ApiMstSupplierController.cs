using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/supplier")]
    public class ApiMstSupplierController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list")]
        public List<Entities.MstSupplier> SupplierList()
        {
            var suppliers = from d in db.MstSuppliers
                            select new Entities.MstSupplier
                            {
                                Id = d.Id,
                                Supplier = d.Supplier,
                                Address = d.Address,
                                IsVAT = d.IsVAT,
                                VATRate = d.VATRate,
                                IsWithheld = d.IsWithheld,
                                WithholdingRate = d.WithholdingRate,
                                IsCityTax = d.IsCityTax,
                                CityTaxRate = d.CityTaxRate,
                                IsLocked = d.IsLocked,
                                CreatedByUserId = d.CreatedByUserId,
                                CreatedByUser = d.MstUser.Fullname,
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedByUserId = d.UpdatedByUserId,
                                UpdatedByUser = d.MstUser1.Fullname,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                            };

            return suppliers.OrderByDescending(d => d.Id).ToList();
        }

        [HttpGet, Route("detail/{id}")]
        public Entities.MstSupplier SupplierDetail(String id)
        {
            var supplier = from d in db.MstSuppliers
                           where d.Id == Convert.ToInt32(id)
                           select new Entities.MstSupplier
                           {
                               Id = d.Id,
                               Supplier = d.Supplier,
                               Address = d.Address,
                               IsVAT = d.IsVAT,
                               VATRate = d.VATRate,
                               IsWithheld = d.IsWithheld,
                               WithholdingRate = d.WithholdingRate,
                               IsCityTax = d.IsCityTax,
                               CityTaxRate = d.CityTaxRate,
                               IsLocked = d.IsLocked,
                               CreatedByUserId = d.CreatedByUserId,
                               CreatedByUser = d.MstUser.Fullname,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedByUserId = d.UpdatedByUserId,
                               UpdatedByUser = d.MstUser1.Fullname,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };

            return supplier.FirstOrDefault();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddSupplier()
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                Data.MstSupplier newSupplier = new Data.MstSupplier()
                {
                    Supplier = "",
                    Address = "",
                    IsVAT = false,
                    VATRate = 0,
                    IsWithheld = false,
                    WithholdingRate = 0,
                    IsCityTax = false,
                    CityTaxRate = 0,
                    IsLocked = false,
                    CreatedByUserId = currentUser.FirstOrDefault().Id,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = currentUser.FirstOrDefault().Id,
                    UpdatedDateTime = DateTime.Now
                };

                db.MstSuppliers.InsertOnSubmit(newSupplier);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, newSupplier.Id);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("save/{id}")]
        public HttpResponseMessage SaveSupplier(String id, Entities.MstSupplier objSupplier)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                var supplier = from d in db.MstSuppliers
                               where d.Id == Convert.ToInt32(id)
                               select d;

                if (supplier.Any())
                {
                    var saveSupplier = supplier.FirstOrDefault();
                    saveSupplier.Supplier = objSupplier.Supplier;
                    saveSupplier.Address = objSupplier.Address;
                    saveSupplier.IsVAT = objSupplier.IsVAT;
                    saveSupplier.VATRate = objSupplier.VATRate;
                    saveSupplier.IsWithheld = objSupplier.IsWithheld;
                    saveSupplier.WithholdingRate = objSupplier.WithholdingRate;
                    saveSupplier.IsCityTax = objSupplier.IsCityTax;
                    saveSupplier.CityTaxRate = objSupplier.CityTaxRate;
                    saveSupplier.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    saveSupplier.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Supplier  not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("lock/{id}")]
        public HttpResponseMessage LockSupplier(String id, Entities.MstSupplier objSupplier)
        {
            try
            {
                var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                var supplier = from d in db.MstSuppliers
                               where d.Id == Convert.ToInt32(id)
                               select d;

                if (supplier.Any())
                {
                    if (supplier.FirstOrDefault().IsLocked == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Already locked!");
                    }

                    var lockSupplier = supplier.FirstOrDefault();
                    lockSupplier.Supplier = objSupplier.Supplier;
                    lockSupplier.Address = objSupplier.Address;
                    lockSupplier.IsVAT = objSupplier.IsVAT;
                    lockSupplier.VATRate = objSupplier.VATRate;
                    lockSupplier.IsWithheld = objSupplier.IsWithheld;
                    lockSupplier.WithholdingRate = objSupplier.WithholdingRate;
                    lockSupplier.IsCityTax = objSupplier.IsCityTax;
                    lockSupplier.CityTaxRate = objSupplier.CityTaxRate;
                    lockSupplier.IsLocked = true;
                    lockSupplier.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    lockSupplier.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Supplier  not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut, Route("unlock/{id}")]
        public HttpResponseMessage UnlockSupplier(String id)
        {
            try
            {
                var supplier = from d in db.MstSuppliers
                               where d.Id == Convert.ToInt32(id)
                               select d;

                if (supplier.Any())
                {
                    if (supplier.FirstOrDefault().IsLocked == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Already unlocked!");
                    }

                    var currentUser = from d in db.MstUsers where d.AspNetUserId == User.Identity.GetUserId() select d;

                    var unlockSupplier = supplier.FirstOrDefault();
                    unlockSupplier.IsLocked = false;
                    unlockSupplier.UpdatedByUserId = currentUser.FirstOrDefault().Id;
                    unlockSupplier.UpdatedDateTime = DateTime.Now;
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Supplier not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteSupplier(String id)
        {
            try
            {
                var supplier = from d in db.MstSuppliers
                               where d.Id == Convert.ToInt32(id)
                               select d;

                if (supplier.Any())
                {
                    if (supplier.FirstOrDefault().IsLocked == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Supplier cannot be deleted if locked.");
                    }

                    var deleteSupplier = supplier.FirstOrDefault();
                    db.MstSuppliers.DeleteOnSubmit(deleteSupplier);
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Supplier not found!");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
