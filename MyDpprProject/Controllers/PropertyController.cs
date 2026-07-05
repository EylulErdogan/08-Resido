using Dapper;
using Microsoft.AspNetCore.Mvc;
using MyDpprProject.Filters;
using MyDpprProject.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MyDpprProject.Controllers
{
    [LoginAuthorize]
    public class PropertyController : Controller
    {
        public IActionResult Index()
        {
            var values = Context.List<Property>("PropertyViewAll").ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult AddProperty()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProperty(Property property)
        {
            DynamicParameters param = new DynamicParameters();

            param.Add("@Title", property.Title);
            param.Add("@Description", property.Description);
            param.Add("@Price", property.Price);
            param.Add("@City", property.City);
            param.Add("@District", property.District);
            param.Add("@Address", property.Address);
            param.Add("@BedCount", property.BedCount);
            param.Add("@BathCount", property.BathCount);
            param.Add("@SquareMeter", property.SquareMeter);
            param.Add("@ImageUrl", property.ImageUrl);
            param.Add("@PropertyTypeId", property.PropertyTypeId);

            Context.ExecuteReturn("PropertyInsert", param);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UpdateProperty(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@PropertyId", id);

            var value = Context.List<Property>("PropertyViewById", param).FirstOrDefault();

            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateProperty(Property property)
        {
            DynamicParameters param = new DynamicParameters();

            param.Add("@PropertyId", property.PropertyId);
            param.Add("@Title", property.Title);
            param.Add("@Description", property.Description);
            param.Add("@Price", property.Price);
            param.Add("@City", property.City);
            param.Add("@District", property.District);
            param.Add("@Address", property.Address);
            param.Add("@BedCount", property.BedCount);
            param.Add("@BathCount", property.BathCount);
            param.Add("@SquareMeter", property.SquareMeter);
            param.Add("@ImageUrl", property.ImageUrl);
            param.Add("@PropertyTypeId", property.PropertyTypeId);

            Context.ExecuteReturn("PropertyUpdate", param);

            return RedirectToAction("Index");
        }
        public IActionResult PropertyViewById(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PropertyId", id);

            var value = Context.List<Property>("PropertyViewById", parameters)
                               .FirstOrDefault();

            if (value == null)
            {
                return NotFound();
            }

            return View(value);
        }

        public IActionResult DeleteProperty(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@PropertyId", id);

            Context.ExecuteReturn("PropertyDelete", param);

            return RedirectToAction("Index");
        }

        public IActionResult ExportToPdf()
        {
            var properties = Context.List<Property>("PropertyViewAll").ToList();

            var pdfDocument = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    page.Header()
                        .Text("İlan Listesi Raporu")
                        .SemiBold()
                        .FontSize(20)
                        .FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingTop(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);
                                columns.RelativeColumn();
                                columns.ConstantColumn(90);
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(70);
                                columns.ConstantColumn(50);
                                columns.ConstantColumn(50);
                                columns.ConstantColumn(60);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("ID").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Başlık").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Fiyat").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Şehir").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("İlçe").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Tür").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Yatak").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Banyo").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("m²").Bold();
                            });

                            foreach (var item in properties)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.PropertyId.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Title);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Price.ToString("N2"));
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.City);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.District);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.TypeName);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.BedCount.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.BathCount.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.SquareMeter.ToString());
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Sayfa ");
                            x.CurrentPageNumber();
                        });
                });
            });

            var pdfBytes = pdfDocument.GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Ilan_Listesi_{DateTime.Now:yyyyMMdd}.pdf");
        }
        public IActionResult ExportToExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Backend softito");

            var properties = Context.List<Property>("PropertyViewAll").ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("İlan Listesi");

                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Başlık";
                worksheet.Cells[1, 3].Value = "Fiyat";
                worksheet.Cells[1, 4].Value = "Şehir";
                worksheet.Cells[1, 5].Value = "İlçe";
                worksheet.Cells[1, 6].Value = "Tür";
                worksheet.Cells[1, 7].Value = "Yatak";
                worksheet.Cells[1, 8].Value = "Banyo";
                worksheet.Cells[1, 9].Value = "Metrekare";

                using (var range = worksheet.Cells[1, 1, 1, 9])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(41, 128, 185));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                int rowNumber = 2;

                foreach (var item in properties)
                {
                    worksheet.Cells[rowNumber, 1].Value = item.PropertyId;
                    worksheet.Cells[rowNumber, 2].Value = item.Title;
                    worksheet.Cells[rowNumber, 3].Value = item.Price;
                    worksheet.Cells[rowNumber, 4].Value = item.City;
                    worksheet.Cells[rowNumber, 5].Value = item.District;
                    worksheet.Cells[rowNumber, 6].Value = item.TypeName;
                    worksheet.Cells[rowNumber, 7].Value = item.BedCount;
                    worksheet.Cells[rowNumber, 8].Value = item.BathCount;
                    worksheet.Cells[rowNumber, 9].Value = item.SquareMeter;

                    rowNumber++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var fileBytes = package.GetAsByteArray();
                string fileName = $"Ilan_Listesi_{DateTime.Now:yyyyMMdd}.xlsx";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}