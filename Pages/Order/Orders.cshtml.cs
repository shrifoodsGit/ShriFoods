using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ShriFoods.Model;
using Document = QuestPDF.Fluent.Document;
using IContainer = QuestPDF.Infrastructure.IContainer;


namespace ShriFoods.Pages.Order
{
    public class OrdersModel : PageModel
    {
        private readonly FoodDbContext _dbContext;

        public List<NewOrder> Orders { get; set; } = new();

        [BindProperty]
        public NewOrder? Order { get; set; }

        public OrdersModel(FoodDbContext context)
        {
            _dbContext = context;
        }

        private static IContainer CellStyle(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(8);
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Order = await GetOrderById(id);

            if (Order == null)
                return NotFound();

            Orders = await GetOrders(Order.UserId);

            return Page();
        }

        public async Task<NewOrder?> GetOrderById(int orderId)
        {
            return await _dbContext.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.OrderId == orderId);
        }

        public async Task<List<NewOrder>> GetOrders(string userId)
        {
            return await _dbContext.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OrderedDate)
                .ToListAsync();
        }

        public async Task<IActionResult> OnGetDownloadPdfAsync(int id)
        {
            var order = await GetOrderById(id);

            if (order == null)
                return NotFound();

            QuestPDF.Settings.License = LicenseType.Community;

            var pdfBytes = Document.Create(container =>
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

                        //column.Item().Text($"Customer: {order.CustomerName}");
                        column.Item().Text($"Phone: {order.PhoneNumber}");
                        //column.Item().Text($"Email: {order.Email}");
                        column.Item().Text($"Date: {order.OrderedDate:dd MMM yyyy hh:mm tt}");
                        column.Item().Text($"Status: {order.OrderStatus}");

                        column.Item().PaddingTop(10);

                        column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(4);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Item").Bold();
                            header.Cell().Element(CellStyle).Text("Qty").Bold();
                            header.Cell().Element(CellStyle).Text("Price").Bold();
                            header.Cell().Element(CellStyle).Text("Total").Bold();
                        });

                        foreach (var item in order.OrderDetails)
                        {
                            table.Cell().Element(CellStyle).Text(item.Product?.ProductName ?? "Product");
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

            return File(pdfBytes, "application/pdf", $"Order_{order.OrderId}.pdf");
        }
    }
}