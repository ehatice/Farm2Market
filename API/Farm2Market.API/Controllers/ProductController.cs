﻿using Farm2Market.Domain.Entities;
using Farm2Marrket.Application.DTOs;
using Farm2Marrket.Application.Manager;
using Farm2Marrket.Application.Sevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Farm2Market.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController: ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product data is required.");
            }

            try
            {
                Guid userGuid;
                // Token'dan kullanıcı ID'sini al
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Unauthorized("Kullanıcı ID'si alınamadı.");
                }
                if (Guid.TryParse(userId, out userGuid))
                {
                    // Eğer userId başarılı bir şekilde GUID'e çevrilirse, işlemi burada yapabilirsiniz
                    await _productService.AddProduct(userGuid, productDto);
                }
                else
                {
                    // Eğer çevrilemezse, uygun bir hata veya işlem yapabilirsiniz
                    throw new ArgumentException("Geçersiz userId formatı. Lütfen doğru bir GUID girin.");
                }
                // Ürünü ekle
    

                return Ok(new { message = "Product added successfully." });
            }
            catch (Exception ex)
            {
                // Hata durumunda 500 döndür
                return StatusCode(500, new { error = "An error occurred while adding the product.", details = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return Ok("Ürün başarıyla pasifleştirildi.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetProductsByFarmer()
        {
            try
            {
                var farmerId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var products = await _productService.GetProductsByFarmerIdAsync(farmerId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }



    }
}
