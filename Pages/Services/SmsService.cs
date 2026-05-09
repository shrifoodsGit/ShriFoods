using RestSharp;
using System.Text.Json;
namespace ShriFoods.Pages.Services
{
    public class SmsService
    {
        private readonly IConfiguration _config;

        public SmsService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendSms(
            string mobile,
            string customerName,
            int orderId)
        {
            var authKey = _config["MSG91:AuthKey"];

            var client = new RestClient(
                "https://control.msg91.com/api/v5/flow/");

            var request = new RestRequest("", Method.Post);

            request.AddHeader("authkey", authKey);

            request.AddJsonBody(new
            {
                flow_id = _config["MSG91:FlowId"],
                sender = "SHRIFO",
                mobiles = $"91{mobile}",
                customer_name = customerName,
                order_id = orderId.ToString()
            });

            await client.ExecuteAsync(request);
        }
    }
}
