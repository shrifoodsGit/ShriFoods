using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ShriFoods.Model;
using Document = QuestPDF.Fluent.Document;
using IContainer = QuestPDF.Infrastructure.IContainer;

namespace ShriFoods.Pages.Services
{
    public class PdfService
    {

        private static IContainer CellStyle(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(8);
        }
        public byte[] GenerateOrderPdf(NewOrder order)
        {

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
         
                    page.Header()
                        .Text($"Order Summary - #{order.OrderId}")
                        .FontSize(24)
                        .Bold();

                    page.Content().Column(column =>
                    {
                        column.Spacing(12);
                
                        column.Item().Text($"Customer: {order.UserFirstName}");
                        column.Item().Text($"Phone: {order.PhoneNumber}");
                        column.Item().Text($"Address: {order.ShippingAddress}");
                        column.Item().Text($"Order Number-#:{order.OrderNumber}");
                        column.Item().Text($"Date: {order.OrderedDate:dd MMM yyyy hh:mm tt}");
                        column.Item().Text($"Payment Status: {order.PaymentStatus}");
                        column.Item().Text($"Payment Method: {order.PaymentMethod}");                
  


                        column.Item().PaddingTop(10);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(4);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Item").Bold();
                                header.Cell().Element(CellStyle).Text("Weight").Bold();
                                header.Cell().Element(CellStyle).Text("Qty").Bold();
                                header.Cell().Element(CellStyle).Text("Price").Bold();
                                header.Cell().Element(CellStyle).Text("Total").Bold();
                            });

                            foreach (var item in order.OrderDetails)
                            {
                                table.Cell().Element(CellStyle).Text(item.Product?.ProductName ?? "Product");
                                table.Cell().Element(CellStyle).Text(item.ProductWeight.ToString());
                                table.Cell().Element(CellStyle).Text(item.Quantity.ToString());
                                table.Cell().Element(CellStyle).Text($"₹{item.UnitPrice:N2}");
                                table.Cell().Element(CellStyle).Text($"₹{(item.Quantity * item.UnitPrice):N2}");
                            }
                        });

                        column.Item().PaddingTop(20);

                        column.Item()
                            .AlignRight()
                            .Text($"Grand Total: ₹{order.TotalAmount:N2}")
                            .FontSize(18)
                            .Bold();
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("Thank you for choosing Shri Suchi Foods");
                });
            }).GeneratePdf();
            //return Document.Create(container =>
            //{
            //    container.Page(page =>
            //    {
            //        page.Margin(30);

            //        page.Header()
            //            .Text("Shri Foods Order")
            //            .FontSize(22)
            //            .Bold();

            //        page.Content().Column(col =>
            //        {
            //            col.Item().Text($"Order ID: {order.OrderId}");
            //            col.Item().Text($"Customer: {order.UserFirstName}");
            //            col.Item().Text($"Phone: {order.PhoneNumber}");
            //            col.Item().Text($"Address: {order.ShippingAddress}");
            //            col.Item().Text($"Total: ₹{order.TotalAmount}");
            //            col.Item().Text($"Date: {DateTime.Now}");
            //        });

            //        page.Footer()
            //            .AlignCenter()
            //            .Text("www.shrifoods.in");
            //    });
            //}).GeneratePdf();
        }
    }
}
