using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriFoods.Model;
using Twilio;
using Twilio.Rest.Verify.V2.Service;

namespace ShriFoods.Pages
{
    public class SignUpModel : PageModel
    {
        private const string smsSent = "Success";
        private readonly FoodsDBContext _dBContext;
        private readonly IConfiguration _config;

        public PhoneVerify twilo;
        public List<UserModel> listUserModel = new List<UserModel>();


        [BindProperty]
        public UserModel NewUserModel { get; set; }

        private static readonly Random _random = new Random();
        public int UniqueNumber { get; set; }



        //Constructor
        public SignUpModel(FoodsDBContext context, IConfiguration config)
        {
            _dBContext = context;
            _config = config;
        }

        public void OnGet()
        {
        }
        //public PhoneVerify Twilo(string phone)
        //{
        //    if (string.IsNullOrEmpty(phone))
        //    {
        //        return "Invalid"; // Path 1
        //    }
        //    var accountSid = _config["TwiloConnection.accountSid"];
        //    var authToken = _config["TwiloConnection.AuthToken"];
        //    TwilioClient.Init(accountSid, authToken);

        //    var verification = VerificationResource.Create(
        //        to: phone,//"+918374499001",
        //        channel: "sms",
        //        _config["TwiloConnection.pathServiceSid"]
        //        );

        //    Console.WriteLine(verification.Sid);
        //    // Missing return here!
        //    return smsSent; // Add this

        //}
        public IActionResult OnPostUser()
        {

            //DriverId
            NewUserModel.UserId = (_dBContext.UserTb.Max(r => r.UserId))+1;

            //DriverUniqueId
            //string driverLastName = _dBContext.DriversTb.Where(x=>x.DriverId == NewDriverModel.DriverId).Select(u => u.DriverLastName).FirstOrDefault();
            // Generate a random number between 1,000,000 and 9,999,999
            UniqueNumber = _random.Next(10000, 100000);

            //If the number doesn't need to be purely numeric or short, use a GUID for guaranteed uniqueness: 
            //string uniqueId = Guid.NewGuid().ToString("N");


            NewUserModel.UserUniqueId = UniqueNumber.ToString();

            // DriverReg Date only 
            NewUserModel.UserRegDate = DateOnly.FromDateTime(DateTime.Today);

            _dBContext.UserTb.Add(NewUserModel);

            if (_dBContext.SaveChanges() ==1)
            {
                ViewData["Message"]= "Your details have been saved successfully!";
                return RedirectToPage("/SignIn");
            }
            else
            {
                return RedirectToPage("/Index");
            }


        }


    }
}
