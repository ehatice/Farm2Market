using Farm2Market.Domain.Entities;
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
                return BadRequest(ApiResponse<string>.Failure("Product data is required."));
            }

            try
            {
                Guid userGuid;
                // Token'dan kullanıcı ID'sini al
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Unauthorized(ApiResponse<string>.Failure("Kullanıcı ID'si alınamadı."));
                }
                if (Guid.TryParse(userId, out userGuid))
                {

                   var addedProduct =  await _productService.AddProduct(userGuid, productDto);
                    return Ok(ApiResponse<ProductResponseDto>.Success(addedProduct));


                }
                else
                {
                    // Eğer çevrilemezse, uygun bir hata veya işlem yapabilirsiniz
                    return BadRequest(ApiResponse<string>.Failure("Geçersiz userId formatı. Lütfen doğru bir GUID girin."));
                }


            }
            catch (Exception ex)
            {
                // Hata durumunda 500 döndür
        return StatusCode(500, ApiResponse<string>.Failure($"Urun eklenirken hata olustu: {ex.Message}"));
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


		[HttpPost("UpdateProductQuantity")]
		public async Task<IActionResult> UpdateProductQuantity(int id, int amount)
		{
			try
			{
				bool result = await _productService.UpdateProductQuantity(id, amount);
				if (result)
				{
					return Ok("Ürün miktarı başarıyla güncellendi.");
				}
				else
				{
					return BadRequest("Ürün miktarı güncellenemedi.");
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}



	}
}
