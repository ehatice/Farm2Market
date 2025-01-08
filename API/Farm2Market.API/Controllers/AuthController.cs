using Farm2Market.Domain.Entities;
using Farm2Marrket.Application.DTOs;
using Farm2Marrket.Application.Manager;
using Farm2Marrket.Application.Sevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Farm2Market.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase

    {
        private readonly IAppUserService _appUserService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
		private readonly IEmailService _emailService;
		private readonly IUserService _userService;
        private readonly IFarmerService _farmerService;
        private readonly IMarketService _marketService;

        public AuthController(UserManager<AppUser> service, SignInManager<AppUser> identityUser, IEmailService emailService,IUserService userService, IAppUserService appUserService, IFarmerService farmerService, IMarketService marketService)
        {
            _userManager = service;
            _signInManager = identityUser;
			_emailService = emailService;
            _userService = userService;
            _appUserService = appUserService;
            _farmerService = farmerService;
            _marketService = marketService;
        }

        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost()]
		public async Task<IActionResult> SendMail()
		{
            Random random = new Random();
            int number = random.Next(1000, 10000);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userExists = await _userManager.FindByIdAsync(userId);

            //await _emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body);
            await _emailService.SendEmailAsync(userExists.Email, "emailverificationcode", number.ToString());
            return Ok("Email sent successfully!");
		}
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> ConfirmMail(int number)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized("User is not logged in.");
			}
			var bisey = await _userService.ConfirmNumber(userId, number);
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
            var userExists = await _userManager.FindByNameAsync(model.Email);
            if (userExists != null)
                return Conflict(ApiResponse<string>.Failure("Bu kullanýcý adý zaten mevcut."));

           
			// Yeni kullanýcý oluþtur
			var user = new Farmer
            {
               FirstName = model.FirstName,
               LastName = model.LastName,
               UserName = model.UserName,
               Email = model.Email,
               Adress=model.Adress,
               UserRole = RoleConsts.Farmer, 
              
            };

			//await _emailService.SendEmailAsync(model.Email, "emailverificationcode", number.ToString());
			//var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			//var confirmationlink = Url.Action("ConfirmMail", "Auth", new { token , email = user.Email, userId = user.Id });

			var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(500, ApiResponse<string>.Failure("Kullanýcý oluþturulamadý."));

            var responseDto = new FarmerRegisterResponseDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
            };
            return Ok(ApiResponse<object>.Success(responseDto));
        }
        [HttpPost]
        public async Task<IActionResult> MarketRegister(MarketReceiverDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kullanýcý var mý kontrol et
            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return Conflict(ApiResponse<string>.Failure("Bu kullanýcý adý zaten mevcut."));

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
            };
            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var confirmationlink = Url.Action("ConfirmMail", "Auth", new {token , email = user.Email});

            // Kullanýcýyý kaydet
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(500, ApiResponse<string>.Failure("Kullanýcý oluþturulamadý."));
            var responseDto = new MarketReceiverResponseDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                MarketName = user.MarketName,
                CompanyType= user.CompanyType,
            };
            return Ok(ApiResponse<MarketReceiverResponseDto>.Success(responseDto));

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Failure("Geçersiz model."));


            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(ApiResponse<string>.Failure("Geçersiz kullanýcý adý veya þifre."));


            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

            if (!result.Succeeded)
                return Unauthorized(ApiResponse<string>.Failure("Geçersiz kullanýcý adý veya þifre."));
			HttpContext.Session.SetString("UserId", user.Id);

			if (!user.EmailConfirmed)
            {
                Random random = new Random();
                int number = random.Next(1000, 10000);
                user.ConfirmationNumber = number;

                // Email gönderme iþlemi
                await _emailService.SendEmailAsync(model.Email, "emailverificationcode", number.ToString());
            }
            var tokenn = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationlink = Url.Action("ConfirmMail", "Auth", new { tokenn , email = user.Email, userId = user.Id });

            var token = await _appUserService.GenerateToken(user);
            var updateResult = await _userManager.UpdateAsync(user);
			HttpContext.Session.SetString("UserId", user.Id);

			var LoginResponse = new LoginResponseDto
            {
                UserName = user.UserName,
                EmailConfirmed = user.EmailConfirmed,
                UserRole = user.UserRole,
                Token = token,
                Email=user.Email,
                ConfirmationNumber = user.ConfirmationNumber,
            };
            return Ok(ApiResponse<LoginResponseDto>.Success(LoginResponse));
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut()]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized("Geçerli bir kullanýcý bulunamadý.");
            var farmer = await _userManager.FindByIdAsync(userId);


            var result = await _userManager.ChangePasswordAsync(farmer,changePasswordDto.CurrentPassword,changePasswordDto.NewPassword);


            if (result.Succeeded)
            {
                return Ok("Kullanýcý bilgileri baþarýyla güncellendi.");
            }


            return BadRequest("Kullanýcý bulunamadý veya güncelleme baþarýsýz oldu.");
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet()]
        public async Task<IActionResult> GetFarmerProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var farmerId))
                return Unauthorized("User ID not found or invalid.");

            var farmer = await _farmerService.GetFarmerByIdAsync(farmerId);
            if (farmer == null)
                return NotFound("Farmer not found.");

            return Ok(farmer);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut()]
        public async Task<IActionResult> UpdateFarmerProfile([FromBody] UpdateFarmerDto farmerDto)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized("Geçerli bir kullanýcý bulunamadý.");


            var result = await _farmerService.UpdateFarmerAsync(parsedUserId, farmerDto);


            if (result)
            {
                return Ok("Kullanýcý bilgileri baþarýyla güncellendi.");
            }


            return BadRequest("Kullanýcý bulunamadý veya güncelleme baþarýsýz oldu.");
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet()]
        public async Task<IActionResult> GetMarketProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var marketId))
                return Unauthorized("User ID not found or invalid.");

            var market = await _marketService.GetMarketByIdAsync(marketId);
            if (market == null)
                return NotFound("Market not found.");
            return Ok(market);
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut()]
        public async Task<IActionResult> UpdateMarketProfile([FromBody] UpdateMarketDto marketDto)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized("Geçerli bir kullanýcý bulunamadý.");


            var result = await _marketService.UpdateMarketAsync(parsedUserId, marketDto);


            if (result)
            {
                return Ok("Kullanýcý bilgileri baþarýyla güncellendi.");
            }


            return BadRequest("Kullanýcý bulunamadý veya güncelleme baþarýsýz oldu.");
        }

        //admin 
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet()]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetActiveUsersAsync();

            if (users == null || !users.Any())
            {
                return NotFound("No active users found.");
            }

            return Ok(users);
        }

    }

}





