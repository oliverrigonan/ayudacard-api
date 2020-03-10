using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Web.Http;
using Zen.Barcode;

namespace ayudacard_api.ApiControllers
{
    [Authorize, RoutePrefix("api/mst/citizen/card")]
    public class ApiMstCitizensCardController : ApiController
    {
        public Data.ayudacarddbDataContext db = new Data.ayudacarddbDataContext();

        public String FillZeroes(Int32 number, Int32 length)
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

                String cardNumber = "0000000001";
                var lastCard = from d in db.MstCitizensCards.OrderByDescending(d => d.Id) select d;
                if (lastCard.Any())
                {
                    Int32 newCardNumber = Convert.ToInt32(lastCard.FirstOrDefault().CardNumber) + 1;
                    cardNumber = FillZeroes(newCardNumber, 10);
                }

                Data.MstCitizensCard newCitizensCard = new Data.MstCitizensCard()
                {
                    CitizenId = objCitizensCard.CitizenId,
                    CardNumber = cardNumber,
                    TotalBalance = objCitizensCard.TotalBalance,
                    StatusId = objCitizensCard.StatusId
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
            try
            {
                FontFactory.RegisterDirectories();

                Font fontArial6 = FontFactory.GetFont("Arial", 6);
                Font fontArial6Bold = FontFactory.GetFont("Arial", 6, Font.BOLD);

                Font fontArial7 = FontFactory.GetFont("Arial", 7);
                Font fontArial7Bold = FontFactory.GetFont("Arial", 7, Font.BOLD);

                Font fontArial8Bold = FontFactory.GetFont("Arial", 8, Font.BOLD);

                Font fontArial20Bold = FontFactory.GetFont("Arial", 20, Font.BOLD);

                Rectangle cardSize = new Rectangle(Utilities.MillimetersToPoints(85), Utilities.MillimetersToPoints(54));
                Document document = new Document(cardSize, 5f, 5f, 5f, 5f);
                MemoryStream workStream = new MemoryStream();

                PdfWriter pdfWriter = PdfWriter.GetInstance(document, workStream);
                pdfWriter.CloseStream = false;

                document.Open();

                String cardBackgroundPath = AppDomain.CurrentDomain.BaseDirectory + @"Images\CardBackground\ayudaCardBackground.png";
                Image cardBackground = Image.GetInstance(cardBackgroundPath);
                cardBackground.Alignment = Image.UNDERLYING;

                var pageWidth = document.PageSize.Width;
                var pageHeight = document.PageSize.Height;

                cardBackground.SetAbsolutePosition(0, 0);
                cardBackground.ScaleToFit(pageWidth, pageHeight);
                document.Add(cardBackground);

                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

                var citizensCard = from d in db.MstCitizensCards
                                   where d.Id == Convert.ToInt32(id)
                                   select d;

                if (citizensCard.Any())
                {
                    PdfPTable pdfTableSpace = new PdfPTable(1);
                    pdfTableSpace.SetWidths(new float[] { 100f });
                    pdfTableSpace.WidthPercentage = 100;
                    pdfTableSpace.AddCell(new PdfPCell(new Phrase(" ", fontArial7Bold)) { Border = 0, HorizontalAlignment = 1, PaddingBottom = 15f });
                    document.Add(pdfTableSpace);

                    Phrase phraseLastNameLabel = new Phrase("Last Name: ", fontArial7Bold);
                    Phrase phraseLastNameData = new Phrase(citizensCard.FirstOrDefault().MstCitizen.Surname + "\n", fontArial7);

                    Phrase phraseFirstNameLabel = new Phrase("First Name: ", fontArial7Bold);
                    Phrase phraseFirstNameData = new Phrase(citizensCard.FirstOrDefault().MstCitizen.Firstname + "\n", fontArial7);

                    Phrase phraseMiddleNameLabel = new Phrase("Middle Name: ", fontArial7Bold);
                    Phrase phraseMiddleNameData = new Phrase(citizensCard.FirstOrDefault().MstCitizen.Middlename + "\n", fontArial7);

                    Phrase phraseGenderLabel = new Phrase("Gender: ", fontArial7Bold);
                    Phrase phraseGenderData = new Phrase(citizensCard.FirstOrDefault().MstCitizen.MstSex.Sex + "\n", fontArial7);

                    Phrase phraseBirthDateLabel = new Phrase("Date of Birth: ", fontArial7Bold);
                    Phrase phraseBirthDateData = new Phrase(citizensCard.FirstOrDefault().MstCitizen.DateOfBirth.ToString("MMMM dd, yyyy") + "\n", fontArial7);

                    Phrase phraseAddressLabel = new Phrase("Address: ", fontArial7Bold);
                    String address = citizensCard.FirstOrDefault().MstCitizen.PermanentNumber + " " + citizensCard.FirstOrDefault().MstCitizen.PermanentStreet + " " + citizensCard.FirstOrDefault().MstCitizen.PermanentVillage + " " + citizensCard.FirstOrDefault().MstCitizen.MstCity.City + " " + citizensCard.FirstOrDefault().MstCitizen.MstProvince.Province + " " + citizensCard.FirstOrDefault().MstCitizen.PermanentZipCode + "\n";
                    Phrase phraseAddressData = new Phrase(address.TrimStart(), fontArial7);

                    Phrase phraseIDNumberLabel = new Phrase("ID No.: ", fontArial6Bold);
                    Phrase phraseIDNumberData = new Phrase(citizensCard.FirstOrDefault().CardNumber, fontArial6);
                    Paragraph IDNumberParagraph = new Paragraph { phraseIDNumberLabel, phraseIDNumberData };

                    Phrase phrasePrecinctNumberLabel = new Phrase("Precinct No.: ", fontArial6Bold);
                    Phrase phrasePrecinctNumberData = new Phrase(citizensCard.FirstOrDefault().MstCitizen.PermanentPrecinctNumber, fontArial6);
                    Paragraph precinctNumberParagraph = new Paragraph { phrasePrecinctNumberLabel, phrasePrecinctNumberData };

                    Code128BarcodeDraw barcode = BarcodeDrawFactory.Code128WithChecksum;
                    System.Drawing.Image imageDrawing = barcode.Draw(citizensCard.FirstOrDefault().CardNumber, 35);
                    Image barcodeImageDrawing = Image.GetInstance(imageDrawing, BaseColor.BLACK);
                    PdfPCell barcodeImage = new PdfPCell(barcodeImageDrawing, true) { };

                    //PdfContentByte pdfContentByte = pdfWriter.DirectContent;
                    //Barcode39 barcode39 = new Barcode39
                    //{
                    //    AltText = " ",
                    //    Code = citizensCard.FirstOrDefault().CardNumber
                    //};

                    //Image image39 = barcode39.CreateImageWithBarcode(pdfContentByte, null, null);
                    //image39.ScaleAbsoluteHeight(30f);
                    //image39.ScaleAbsoluteWidth(65f);
                    //PdfPCell barcodeImage = new PdfPCell(barcodeImageDrawing, true) { };

                    // Table
                    PdfPTable pdfTableCitizensDetail = new PdfPTable(3);
                    pdfTableCitizensDetail.SetWidths(new float[] { 35f, 70f, 50f });
                    pdfTableCitizensDetail.WidthPercentage = 93;

                    // Picture Side
                    pdfTableCitizensDetail.AddCell(new PdfPCell(new Phrase(" ", fontArial6Bold)) { Border = 0, PaddingTop = 20f, Colspan = 2 });

                    if (String.IsNullOrEmpty(citizensCard.FirstOrDefault().MstCitizen.PictureURL) == true)
                    {
                        pdfTableCitizensDetail.AddCell(new PdfPCell(new Phrase(" ", fontArial7Bold)) { Border = 5, Rowspan = 6, HorizontalAlignment = 0, Padding = 1f });
                    }
                    else
                    {
                        Image citizensPhoto = Image.GetInstance(new Uri(citizensCard.FirstOrDefault().MstCitizen.PictureURL));
                        //citizensPhoto.ScaleToFit(1000f, 40f);
                        citizensPhoto.ScaleAbsolute(170f, 155f);
                        PdfPCell citizensPhotoCell = new PdfPCell(citizensPhoto) { FixedHeight = 2f };

                        pdfTableCitizensDetail.AddCell(new PdfPCell(citizensPhotoCell) { Border = 0, Rowspan = 5, HorizontalAlignment = 0, Padding = 1f });
                    }

                    pdfTableCitizensDetail.AddCell(new PdfPCell(phraseLastNameLabel) { Border = 0, Padding = 1f });
                    pdfTableCitizensDetail.AddCell(new PdfPCell(phraseLastNameData) { Border = 0, Padding = 1f });

                    pdfTableCitizensDetail.AddCell(new PdfPCell(phraseFirstNameLabel) { Border = 0, Padding = 1f });
                    pdfTableCitizensDetail.AddCell(new PdfPCell(phraseFirstNameData) { Border = 0, Padding = 1f });

                    pdfTableCitizensDetail.AddCell(new PdfPCell(phraseMiddleNameLabel) { Border = 0, Padding = 1f });
                    pdfTableCitizensDetail.AddCell(new PdfPCell(phraseMiddleNameData) { Border = 0, Padding = 1f });

                    pdfTableCitizensDetail.AddCell(new PdfPCell(phraseGenderLabel) { Border = 0, Padding = 1f });
                    pdfTableCitizensDetail.AddCell(new PdfPCell(phraseGenderData) { Border = 0, Padding = 1f });

                    // ID Number && Precinct No. Side
                    pdfTableCitizensDetail.AddCell(new PdfPCell(phraseBirthDateLabel) { Border = 0, Padding = 1f });
                    pdfTableCitizensDetail.AddCell(new PdfPCell(phraseBirthDateData) { Border = 0, Padding = 1f });
                    pdfTableCitizensDetail.AddCell(new PdfPCell(IDNumberParagraph) { Border = 0, Padding = 1f });

                    pdfTableCitizensDetail.AddCell(new PdfPCell(phraseAddressLabel) { Border = 0, Padding = 1f, Rowspan = 3 });
                    pdfTableCitizensDetail.AddCell(new PdfPCell(phraseAddressData) { Border = 0, Padding = 1f, Rowspan = 3 });

                    pdfTableCitizensDetail.AddCell(new PdfPCell(precinctNumberParagraph) { Border = 0 });
                    pdfTableCitizensDetail.AddCell(new PdfPCell(barcodeImage) { Border = 0, Padding = 1f, Rowspan = 2 });

                    document.Add(pdfTableCitizensDetail);

                    // back page...
                    document.NewPage();

                    PdfPTable pdfTableTermsAndConditions = new PdfPTable(2);
                    pdfTableTermsAndConditions.SetWidths(new float[] { 5f, 100f });
                    pdfTableTermsAndConditions.WidthPercentage = 95;

                    pdfTableTermsAndConditions.AddCell(new PdfPCell(new Phrase("TERMS AND CONDITIONS", fontArial8Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 20f, PaddingBottom = 5f, Colspan = 2 });

                    pdfTableTermsAndConditions.AddCell(new PdfPCell(new Phrase("•", fontArial7Bold)) { Border = 0, HorizontalAlignment = 2, Padding = 2f });
                    pdfTableTermsAndConditions.AddCell(new PdfPCell(new Phrase("Present this card when availing educational, medical and burial assistance.", fontArial7)) { Border = 0, HorizontalAlignment = 0, Padding = 2f });

                    pdfTableTermsAndConditions.AddCell(new PdfPCell(new Phrase("•", fontArial7Bold)) { Border = 0, HorizontalAlignment = 2, Padding = 2f });
                    pdfTableTermsAndConditions.AddCell(new PdfPCell(new Phrase("Use of this card is governed by Terms and Conditions under AYUDA Program agreements with its partner establishments.", fontArial7)) { Border = 0, HorizontalAlignment = 0, Padding = 2f });

                    pdfTableTermsAndConditions.AddCell(new PdfPCell(new Phrase("•", fontArial7Bold)) { Border = 0, HorizontalAlignment = 2, Padding = 2f });
                    pdfTableTermsAndConditions.AddCell(new PdfPCell(new Phrase("This card is non-transferable, non-seleable.", fontArial7)) { Border = 0, HorizontalAlignment = 0, Padding = 2f });

                    pdfTableTermsAndConditions.AddCell(new PdfPCell(new Phrase("•", fontArial7Bold)) { Border = 0, HorizontalAlignment = 2, Padding = 2f });
                    pdfTableTermsAndConditions.AddCell(new PdfPCell(new Phrase("If found, please return to AYUDA Office, Ground Floor of Legislative Bldg., Danao City Hall, Danao City.", fontArial7)) { Border = 0, HorizontalAlignment = 0, Padding = 2f });

                    pdfTableTermsAndConditions.AddCell(new PdfPCell(new Phrase("•", fontArial7Bold)) { Border = 0, HorizontalAlignment = 2, Padding = 2f });
                    pdfTableTermsAndConditions.AddCell(new PdfPCell(new Phrase("For inquiries, you may call AYUDA Office at 255-5373; CSWD Office a2 260-1172; City Health Office at 261-8386", fontArial7)) { Border = 0, HorizontalAlignment = 0, Padding = 2f });

                    document.Add(pdfTableTermsAndConditions);
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
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
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
