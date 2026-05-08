using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ShriFoods.Model;
using System.ComponentModel;
using System.Drawing;
using System.Reflection.Metadata;
using Document = QuestPDF.Fluent.Document;
using IContainer = QuestPDF.Infrastructure.IContainer;

namespace ShriFoods.Pages.Order
{
    public class OrdersModel : PageModel
    {
        public OrderModel Order { get; set; } = new();


        private static IContainer CellStyle(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(8);
        }

        public void OnGet(int id)
        {
            // Sample Data
            //Order = GetSampleOrder(id);

        }



        //public IActionResult OnGetDownloadPdf(int id)
        //{
        //    //var order = GetSampleOrder(id);

        //    QuestPDF.Settings.License = LicenseType.Community;

        //    var pdfBytes = Document.Create(container =>
        //    {
        //        container.Page(page =>
        //        {
        //            page.Margin(40);

        //            page.Header()
        //                .Text($"Order Summary - #{order.Id}")
        //                .FontSize(24)
        //                .Bold();

        //            page.Content().Column(column =>
        //            {
        //                column.Spacing(15);

        //                column.Item().Text($"Customer: {order.CustomerName}");
        //                column.Item().Text($"Phone: {order.Phone}");
        //                column.Item().Text($"Email: {order.Email}");
        //                column.Item().Text($"Date: {order.OrderDate:dd MMM yyyy}");

        //                column.Item().Table(table =>
        //                {
        //                    table.ColumnsDefinition(columns =>
        //                    {
        //                        columns.RelativeColumn(4);
        //                        columns.RelativeColumn(1);
        //                        columns.RelativeColumn(2);
        //                        columns.RelativeColumn(2);
        //                    });

        //                    table.Header(header =>
        //                    {
        //                        header.Cell().Element(CellStyle).Text("Item").Bold();
        //                        header.Cell().Element(CellStyle).Text("Qty").Bold();
        //                        header.Cell().Element(CellStyle).Text("Price").Bold();
        //                        header.Cell().Element(CellStyle).Text("Total").Bold();
        //                    });

        //                    foreach (var item in order.Items)
        //                    {
        //                        table.Cell().Element(CellStyle).Text(item.Name);
        //                        table.Cell().Element(CellStyle).Text(item.Quantity.ToString());
        //                        table.Cell().Element(CellStyle).Text($"₹{item.Price}");
        //                        table.Cell().Element(CellStyle).Text($"₹{item.Price * item.Quantity}");
        //                    }
        //                });

        //                column.Item().PaddingTop(20);

        //                column.Item().AlignRight().Text($"Subtotal: ₹{order.Subtotal}").Bold();
        //                column.Item().AlignRight().Text($"GST: ₹{order.GST}").Bold();
        //                column.Item().AlignRight().Text($"Grand Total: ₹{order.Total}")
        //                    .FontSize(18)
        //                    .Bold();
        //            });

        //            page.Footer()
        //                .AlignCenter()
        //                .Text(text =>
        //                {
        //                    text.Span("Thank you for choosing ShriGo");
        //                });
        //        });
        //    }).GeneratePdf();

        //    return File(pdfBytes, "application/pdf", $"Order_{order.Id}.pdf");
        //}



        //private Order GetSampleOrder(int id)
        //{
        //    return new Order
        //    {
        //        Id = id,
        //        CustomerName = "Pavan Bandi",
        //        Phone = "+91 9876543210",
        //        Email = "customer@shrigo.in",
        //        OrderDate = DateTime.Now,
        //        Status = "Completed",
        //        Items = new List<OrderItem>
        //        {
        //            new OrderItem
        //            {
        //                Name = "Car Full Service",
        //                Quantity = 1,
        //                Price = 2999
        //            },
        //            new OrderItem
        //            {
        //                Name = "Interior Cleaning",
        //                Quantity = 1,
        //                Price = 999
        //            }
        //        }
        //    };
        //}
    }
}

