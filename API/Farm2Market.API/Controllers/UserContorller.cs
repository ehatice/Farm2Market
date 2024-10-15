using Farm2Market.Domain.Entities;
using Farm2Marrket.Application.Sevices;
using Microsoft.AspNetCore.Mvc;

namespace Farm2Market.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            await _userService.AddUser(user);
            return Ok(user);
        }

    }
}
