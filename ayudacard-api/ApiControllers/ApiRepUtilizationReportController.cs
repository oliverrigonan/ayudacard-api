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
    [RoutePrefix("api/rep/utilization/report")]
    public class ApiRepUtilizationReportController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        [HttpGet, Route("list/{startDate}/{endDate}")]
        public List<Entities.RepUtilizationReport> UtilizationReportList(String startDate, String endDate)
        {
            List<Entities.RepUtilizationReport> newUtilizationReportList = new List<Entities.RepUtilizationReport>();

            var cases = from d in db.TrnCases
                        where d.CaseDate >= Convert.ToDateTime(startDate)
                        && d.CaseDate <= Convert.ToDateTime(endDate)
                        group d by new
                        {
                            TypeOfAssistance = d.MstService.Service
                        } into g
                        select new
                        {
                            TypeOfAssistance = g.Key.TypeOfAssistance,
                            NumberOfBeneficiaries = g.Count(),
                            Amount = g.Sum(d => d.Amount)
                        };

            if (cases.ToList().Any())
            {
                foreach (var obj in cases)
                {
                    newUtilizationReportList.Add(new Entities.RepUtilizationReport()
                    {
                        TypeOfAssistance = obj.TypeOfAssistance,
                        NumberOfBeneficiaries = obj.NumberOfBeneficiaries,
                        Amount = obj.Amount
                    });
                }
            }

            return newUtilizationReportList;
        }
    }
}
