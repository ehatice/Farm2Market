using Farm2Market.Domain.Entities;
using Farm2Marrket.Application.DTOs;
using Farm2Marrket.Application.Manager;
using Farm2Marrket.Application.Sevices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Farm2Market.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
		private readonly IEmailService _emailService;
		private readonly IUserService _userService;
		public AuthController(UserManager<AppUser> service, SignInManager<AppUser> identityUser, IEmailService emailService,IUserService userService)
        {
            _userManager = service;
            _signInManager = identityUser;
			_emailService = emailService;
            _userService = userService;
		}


		[HttpPost("SendMail")]
		public async Task<IActionResult> SendMail([FromBody] EmailRequest emailRequest)
		{
			await _emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body);
			return Ok("Email sent successfully!");
		}


		[HttpGet]
        public async Task<IActionResult> ConfirmMail(string Id,int number)
        {
			var bisey = await _userService.ConfirmNumber(Id, number);
            if (bisey)
            {
				return Ok();
			}
            else { return BadRequest(false); }
            
		}

        [HttpPost]
        public async Task<IActionResult> FarmerRegister(RegisterFarmerDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kullanýcý var mý kontrol et
            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return StatusCode(500, new { message = "Kullanýcý zaten mevcut." });

			Random random = new Random();
			int number = random.Next(1000, 10000);

			// Yeni kullanýcý oluþtur
			var user = new Farmer
            {
               FirstName = model.FirstName,
               LastName = model.LastName,
               UserName = model.UserName,
               Email = model.Email,
               Adress=model.Adress,
               UserRole = RoleConsts.Farmer, 
               ConfirmationNumber = number,
            };

			await _emailService.SendEmailAsync(model.Email, "emailverificationcode", number.ToString());
			//var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			//var confirmationlink = Url.Action("ConfirmMail", "Auth", new { token , email = user.Email, userId = user.Id });
			


			// Kullanýcýyý kaydet
			var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(500, new { message = "Kullanýcý oluþturulamadý.", errors = result.Errors });

            return Ok(new {  });
        }
        [HttpPost]
        public async Task<IActionResult> MarketRegister(MarketReceiverDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kullanýcý var mý kontrol et
            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return StatusCode(500, new { message = "Kullanýcý zaten mevcut." });

			// Yeni kullanýcý oluþtur

			Random random = new Random();
			int number = random.Next(1000, 10000);

			var user = new MarketReceiver
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Adress = model.Adress,
                CompanyType = model.CompanyType,
                MarketName = model.MarketName,
                UserRole = RoleConsts.MarketReceiver,
                ConfirmationNumber = number,
                
            };


            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationlink = Url.Action("ConfirmMail", "Auth", new {token , email = user.Email});




            // Kullanýcýyý kaydet
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(500, new { message = "Kullanýcý oluþturulamadý.", errors = result.Errors });

            return Ok(new { message = "Kayýt baþarýlý!" });
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

   
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return Unauthorized(new { message = "Geçersiz kullanýcý adý veya þifre." });

         
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

            if (!result.Succeeded)
                return Unauthorized(new { message = "Geçersiz kullanýcý adý veya þifre." });

            var ecem = new LoginResponseDto
            {
                UserName = user.UserName,
                EmailConfirmed = user.EmailConfirmed,
                UserRole = user.UserRole,
            };

            // Giriþ baþarýlý
            return Ok(ecem);
        }
    }
}
public class EmailRequest
{
	public string ToEmail { get; set; }
	public string Subject { get; set; }
	public string Body { get; set; }
}





