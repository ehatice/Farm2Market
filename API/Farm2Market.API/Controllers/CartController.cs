﻿
using Farm2Market.Domain.Entities;
using Farm2Marrket.Application.DTOs;
using Farm2Marrket.Application.Manager;
using Farm2Marrket.Application.Sevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace Farm2Market.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]

        // POST: api/cart/add
        [HttpPost("Add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto model)
        {
            if (model == null || model.WeightOrAmount <= 0 || model.ProdcutId <= 0)
            {
                return BadRequest("Geçersiz ürün bilgileri.");
            }

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Kullanıcı kimliği bulunamadı.");
                }

                Guid marketReceiverId = Guid.Parse(userId);
                await _cartService.AddToCart(marketReceiverId, model);
                return Ok("Ürün sepete başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hata: {ex.Message}");
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Kullanıcı kimliği bulunamadı.");
            }

            
            if (!Guid.TryParse(userId, out var userGuid))
            {
                return BadRequest("Geçersiz kullanıcı kimliği.");
            }

            var cart = await _cartService.GetCartAsync(userGuid);

            if (cart == null)
            {
                return NotFound("Sepet bulunamadı.");
            }

            return Ok(cart);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Kullanıcı kimliği bulunamadı.");
            }

            if (cartItemId <= 0)
            {
                return BadRequest("Geçersiz CartItemId.");
            }

            try
            {
                await _cartService.RemoveCartItemAsync(cartItemId);
                return Ok("CartItem başarıyla silindi.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hata: {ex.Message}");
            }
        }



    }
}
