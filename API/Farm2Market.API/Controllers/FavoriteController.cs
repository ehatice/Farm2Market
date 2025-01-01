using Farm2Market.Domain.Entities;
using Farm2Marrket.Application.Manager;
using Farm2Marrket.Application.Sevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Farm2Market.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IMarketFavoriteService _favoriteManager;

        public FavoriteController(IMarketFavoriteService favoriteManager)
        {
            _favoriteManager = favoriteManager;
        }

        
        private Guid? GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var marketReceiverId))
            {
                return null;
            }
            return marketReceiverId;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("add")]
        public async Task<IActionResult> AddFavorite(int productId)
        {
            var marketReceiverId = GetUserIdFromClaims();
            if (!marketReceiverId.HasValue)
            {
                return Unauthorized(new { message = "Invalid or missing user ID." });
            }

            try
            {
                await _favoriteManager.AddFavoriteAsync(marketReceiverId.Value, productId);
                return Ok(new { message = "Favorite added successfully." });
            }
            catch (InvalidOperationException ex)
            {
                // Ürün zaten favorilere eklenmiş
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Diğer tüm hatalar
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFavorite(int productId)
        {
            var marketReceiverId = GetUserIdFromClaims();
            if (!marketReceiverId.HasValue)
            {
                return Unauthorized(new { message = "Invalid or missing user ID." });
            }

            await _favoriteManager.RemoveFavoriteAsync(marketReceiverId.Value, productId);
            return Ok(new { message = "Favorite removed successfully." });
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavorites()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var marketReceiverId))
            {
                return Unauthorized(new { message = "Invalid or missing user ID." });
            }

            var favorites = await _favoriteManager.GetFavoritesByMarketAsync(marketReceiverId);
            return Ok(favorites);
        }
    }
}
