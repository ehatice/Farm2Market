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
        public AuthController(UserManager<AppUser> service, SignInManager<AppUser> identityUser)
        {
            _userManager = service;
            _signInManager = identityUser;
        }


        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("pong");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFarmerDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kullan�c� var m� kontrol et
            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return StatusCode(500, new { message = "Kullan�c� zaten mevcut." });

            // Yeni kullan�c� olu�tur
            var user = new Farmer
            {
               FirstName = model.FirstName,
               LastName = model.LastName,
               UserName = model.UserName,
               Email = model.Email,
               Adress=model.Adress,
            };

            // Kullan�c�y� kaydet
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(500, new { message = "Kullan�c� olu�turulamad�.", errors = result.Errors });

            return Ok(new { message = "Kay�t ba�ar�l�!" });
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

   
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return Unauthorized(new { message = "Ge�ersiz kullan�c� ad� veya �ifre." });

         
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

            if (!result.Succeeded)
                return Unauthorized(new { message = "Ge�ersiz kullan�c� ad� veya �ifre." });

            var ecem = new LoginResponseDto
            {
                UserName = user.UserName,
                EmailConfirmed = user.EmailConfirmed
            };

            // Giri� ba�ar�l�
            return Ok(ecem);
        }
    }
}






