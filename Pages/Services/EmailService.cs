using MimeKit;
using ShriFoods.Model;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;


namespace ShriFoods.Pages.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendOrderEmail(
            string toEmail,
            string customerName,
            string orderNumber,
            int orderId,
            decimal total)
        {
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(
                    _settings.FromName,
                    _settings.FromEmail));

            message.To.Add(MailboxAddress.Parse(toEmail));

            message.Subject = $"Order Confirmation #{orderId}";

            message.Body = new TextPart("html")
            {
                Text = $@"
                    <h2>Thank You {customerName}</h2>
                    <p>Your order has been placed successfully.</p>

                    <p>
                        <strong>Order ID:</strong> #{orderId}
                    </p>
                     <p>
                        <strong>Order ID:</strong> #{orderNumber}
                    </p>
                    <p>
                        <strong>Total:</strong> ₹{total}
                    </p>
                    <br/>
                    <p>
                        Thanks for choosing Shri Suchi Foods.
                    </p>
                "
            };

            using var client = new SmtpClient();

            await client.ConnectAsync(
                _settings.Host,
                _settings.Port,
                false);

            await client.AuthenticateAsync(
                _settings.UserName,
                _settings.Password);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
}
