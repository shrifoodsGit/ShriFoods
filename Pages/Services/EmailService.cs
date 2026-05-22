using MimeKit;
using ShriFoods.Model;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;


namespace ShriFoods.Pages.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        private readonly EmailSettings _settings;

        public EmailService(IConfiguration config, EmailSettings settings)
        {
            _config = config;
            _settings = settings;
        }

        //Basic one 
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
                        <strong>Order Number:</strong> #{orderNumber}
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

        //Send Email to Customer on successfully placing order  
        public async Task SendOrderEmailWithPdf_ToCustomer(
          string toEmail,
          string subject,
          string body,
          byte[] pdfBytes)
        {
            var email = new MimeMessage();

            email.From.Add(
                MailboxAddress.Parse("orders@shrifoods.in"));

            email.To.Add(
                MailboxAddress.Parse(toEmail));

            email.Subject = subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = body;

            builder.Attachments.Add(
                "OrderInvoice.pdf",
                pdfBytes,
                ContentType.Parse("application/pdf"));

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                  _settings.Host,
                  _settings.Port,
                  false);

            await smtp.AuthenticateAsync(
                _settings.UserName,
                _settings.Password);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }

        //Send Email to Admin on successfully placing order  
        public async Task SendOrderEmailWithPdf_ToAdmin(
          string toEmail,
          string subject,
          string body,
          byte[] pdfBytes)
        {
            var email = new MimeMessage();

            email.From.Add(
                MailboxAddress.Parse("orders@shrifoods.in"));

            email.To.Add(
                MailboxAddress.Parse(toEmail));

            email.Subject = subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = body;

            builder.Attachments.Add(
                "OrderInvoice.pdf",
                pdfBytes,
                ContentType.Parse("application/pdf"));

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                  _settings.Host,
                  _settings.Port,
                  false);

            await smtp.AuthenticateAsync(
                _settings.UserName,
                _settings.Password);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }

        public async Task SendOTPEmail(string email, string otp)
        {
            var mail = new MimeMessage();
     

            mail.From.Add(MailboxAddress.Parse("Otp@shrifoods.in"));

            mail.To.Add(
                MailboxAddress.Parse(email));

            mail.Subject = "ShriFoods Password Reset OTP";

            mail.Body =new TextPart("html")
            {
                Text = $@"
                <p>Your OTP for password reset is: #{otp}</p>
                <br />
                <p>This OTP will expire in 10 minutes</p>
                "
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                  _settings.Host,
                  _settings.Port,
                  false);

            await smtp.AuthenticateAsync(
                _settings.UserName,
                _settings.Password);
            //smtp.EnableSsl = true;
            await smtp.SendAsync(mail);
        }
    }
}
