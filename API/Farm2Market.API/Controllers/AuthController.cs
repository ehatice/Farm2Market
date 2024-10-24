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

            // Kullanýcý var mý kontrol et
            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return StatusCode(500, new { message = "Kullanýcý zaten mevcut." });

            // Yeni kullanýcý oluþtur
            var user = new Farmer
            {
               FirstName = model.FirstName,
               LastName = model.LastName,
               UserName = model.UserName,
               Email = model.Email,
               Adress=model.Adress,
            };

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
                EmailConfirmed = user.EmailConfirmed
            };

            // Giriþ baþarýlý
            return Ok(ecem);
        }
    }
}






