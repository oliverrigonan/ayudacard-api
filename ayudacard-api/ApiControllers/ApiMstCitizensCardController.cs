using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        [HttpGet, Route("print/{id}")]
        public HttpResponseMessage PrintCitizensCard(String id)
        {
            FontFactory.RegisterDirectories();

            Font fontArial6 = FontFactory.GetFont("Arial", 6);
            Font fontArial6Bold = FontFactory.GetFont("Arial", 6, Font.BOLD);
            Font fontArial20Bold = FontFactory.GetFont("Arial", 20, Font.BOLD);

            Rectangle cardSize = new Rectangle(202.5F, 127.5F)
            {
                BackgroundColor = BaseColor.LIGHT_GRAY
            };

            Document document = new Document(cardSize, 5f, 5f, 5f, 5f);
            MemoryStream workStream = new MemoryStream();

            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            var citizensCard = from d in db.MstCitizensCards
                               where d.Id == Convert.ToInt32(id)
                               select d;

            if (citizensCard.Any())
            {
                Phrase phraseRepublic = new Phrase("Republic of the Philippines\n", fontArial6);
                Phrase phraseCity = new Phrase("City of Danao\n", fontArial6);
                Phrase phraseTitle = new Phrase("AYUDA", fontArial20Bold);

                Paragraph headerParagraph = new Paragraph
                {
                    phraseRepublic,
                    phraseCity,
                    phraseTitle
                };

                headerParagraph.Alignment = Element.ALIGN_CENTER;

                String logoPath = AppDomain.CurrentDomain.BaseDirectory + @"Images\Logo\danaocitylogo.png";
                Image imageLogo = Image.GetInstance(logoPath);
                imageLogo.ScaleToFit(1000f, 40f);

                PdfPTable pdfTableHeaderDetail = new PdfPTable(3);
                pdfTableHeaderDetail.SetWidths(new float[] { 30f, 50, 30f });
                pdfTableHeaderDetail.WidthPercentage = 100;
                pdfTableHeaderDetail.AddCell(new PdfPCell(imageLogo) { Border = 0, PaddingBottom = 2f });
                pdfTableHeaderDetail.AddCell(new PdfPCell(headerParagraph) { Border = 0, HorizontalAlignment = 1, PaddingBottom = 2f });
                pdfTableHeaderDetail.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 2f });
                document.Add(pdfTableHeaderDetail);

                Phrase phraseLastNameLabel = new Phrase("Last Name: ", fontArial6Bold);
                Phrase phraseLastNameData = new Phrase(citizensCard.FirstOrDefault().MstCitizen.Surname + "\n", fontArial6);
                Phrase phraseFirstNameLabel = new Phrase("First Name: ", fontArial6Bold);
                Phrase phraseFirstNameData = new Phrase(citizensCard.FirstOrDefault().MstCitizen.Firstname + "\n", fontArial6);
                Phrase phraseMiddleNameLabel = new Phrase("Middle Name: ", fontArial6Bold);
                Phrase phraseMiddleNameData = new Phrase(citizensCard.FirstOrDefault().MstCitizen.Middlename + "\n", fontArial6);
                Phrase phraseGenderLabel = new Phrase("Gender: ", fontArial6Bold);
                Phrase phraseGenderData = new Phrase(citizensCard.FirstOrDefault().MstCitizen.MstSex.Sex + "\n", fontArial6);
                Phrase phraseBirthDateLabel = new Phrase("Date of Birth: ", fontArial6Bold);
                Phrase phraseBirthDateData = new Phrase(citizensCard.FirstOrDefault().MstCitizen.DateOfBirth.ToString("MMMM dd, yyyy") + "\n", fontArial6);
                Phrase phraseAddressLabel = new Phrase("Address: ", fontArial6Bold);
                Phrase phraseAddressData = new Phrase(citizensCard.FirstOrDefault().MstCitizen.PermanentNumber + " " + citizensCard.FirstOrDefault().MstCitizen.PermanentStreet + " " + citizensCard.FirstOrDefault().MstCitizen.PermanentVillage + " " + citizensCard.FirstOrDefault().MstCitizen.MstCity.City + " " + citizensCard.FirstOrDefault().MstCitizen.MstProvince.Province + " " + citizensCard.FirstOrDefault().MstCitizen.PermanentZipCode + "\n", fontArial6);

                Phrase phraseIDNumberLabel = new Phrase("ID No.: ", fontArial6Bold);
                Phrase phraseIDNumberData = new Phrase(citizensCard.FirstOrDefault().CardNumber, fontArial6);
                Paragraph IDNumberParagraph = new Paragraph { phraseIDNumberLabel, phraseIDNumberData };

                Phrase phrasePrecinctNumberLabel = new Phrase("Precinct No.: ", fontArial6Bold);
                Phrase phrasePrecinctNumberData = new Phrase("0128C", fontArial6);
                Paragraph precinctNumberParagraph = new Paragraph { phrasePrecinctNumberLabel, phrasePrecinctNumberData };

                PdfPTable pdfTableCitizensDetail = new PdfPTable(3);
                pdfTableCitizensDetail.SetWidths(new float[] { 35f, 70f, 50f });
                pdfTableCitizensDetail.WidthPercentage = 100;
                pdfTableCitizensDetail.AddCell(new PdfPCell(phraseLastNameLabel) { Border = 0, Padding = 1f });
                pdfTableCitizensDetail.AddCell(new PdfPCell(phraseLastNameData) { Border = 0, Padding = 1f });
                pdfTableCitizensDetail.AddCell(new PdfPCell(new Phrase(" ", fontArial6Bold)) { Border = 0, Rowspan = 4, HorizontalAlignment = 1, Padding = 1f });
                pdfTableCitizensDetail.AddCell(new PdfPCell(phraseFirstNameLabel) { Border = 0, Padding = 1f });
                pdfTableCitizensDetail.AddCell(new PdfPCell(phraseFirstNameData) { Border = 0, Padding = 1f });
                pdfTableCitizensDetail.AddCell(new PdfPCell(phraseMiddleNameLabel) { Border = 0, Padding = 1f });
                pdfTableCitizensDetail.AddCell(new PdfPCell(phraseMiddleNameData) { Border = 0, Padding = 1f });
                pdfTableCitizensDetail.AddCell(new PdfPCell(phraseGenderLabel) { Border = 0, Padding = 1f });
                pdfTableCitizensDetail.AddCell(new PdfPCell(phraseGenderData) { Border = 0, Padding = 1f });
                pdfTableCitizensDetail.AddCell(new PdfPCell(phraseBirthDateLabel) { Border = 0, Padding = 1f });
                pdfTableCitizensDetail.AddCell(new PdfPCell(phraseBirthDateData) { Border = 0, Padding = 1f });
                pdfTableCitizensDetail.AddCell(new PdfPCell(IDNumberParagraph) { Border = 0, Padding = 1f });
                pdfTableCitizensDetail.AddCell(new PdfPCell(phraseAddressLabel) { Border = 0, Padding = 1f });
                pdfTableCitizensDetail.AddCell(new PdfPCell(phraseAddressData) { Border = 0, Padding = 1f });
                pdfTableCitizensDetail.AddCell(new PdfPCell(precinctNumberParagraph) { Border = 0, Padding = 1f });
                document.Add(pdfTableCitizensDetail);
            }
            else
            {
                document.Add(line);
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

        public System.Drawing.Image DownloadImageFromUrl(string imageUrl)
        {
            System.Drawing.Image image = null;

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                WebResponse webResponse = webRequest.GetResponse();

                Stream stream = webResponse.GetResponseStream();

                image = System.Drawing.Image.FromStream(stream);

                webResponse.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return image;
        }
    }
}
