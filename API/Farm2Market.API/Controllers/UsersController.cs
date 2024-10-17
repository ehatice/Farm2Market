using Farm2Market.Application.Dtos;
using Farm2Market.Domain.Entities;
using Farm2Market.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Farm2Market.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly AppDbContext dbContext;

		public UsersController(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}
		[HttpPost]
		[Route("Registration")]
		public IActionResult Registration(User userDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var objuser = dbContext.Users.FirstOrDefault(x => x.Email == userDTO.Email);
			if (objuser == null)
			{
				dbContext.Users.Add(new User
				{
					UserName = userDTO.UserName,
					Password = userDTO.Password,
					Email = userDTO.Email,
				});

				dbContext.SaveChanges();
				return Ok("User registered");
			}
			else { 
				return BadRequest("User already exist");
			}

		}


		[HttpPost]
		[Route("Login")]
		public IActionResult Login(LoginDTO loginDTO)
		{
			var user = dbContext.Users.FirstOrDefault(x =>x.Email == loginDTO.Email && x.Password == loginDTO.Password);
            if (user != null)
            {
				return Ok(user);
            }
			return NoContent();
        }

		[HttpGet]
		[Route("GetUsers")]

		public IActionResult GetUsers()
		{
			return Ok(dbContext.Users.ToList());
		}

		[HttpGet]
		[Route("GetUser")]

		public IActionResult GetUser(int id)
		{
			var user = dbContext.Users.FirstOrDefault(x => x.Id == id);
			if (user != null)
			{
				return Ok(user);
			}
			else return NoContent();
		}

	}
}
