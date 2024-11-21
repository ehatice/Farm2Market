using Farm2Market.Domain.Entities;
using Farm2Marrket.Application.DTOs;
using Farm2Marrket.Application.Manager;
using Farm2Marrket.Application.Sevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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
       // [Authorize(AuthenticationSchemes = "Bearer", Roles = "Farmer")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            
            if (productDto == null)
            {
                return BadRequest("Product data is required.");
            }

            try
            {
               
                await _productService.AddProduct(productDto);
                return Ok(new { message = "Product added successfully." });
            }
            catch (Exception ex)
            {
                
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
        [HttpGet("GetProductsByFarmer/{farmerId}")]
        public async Task<IActionResult> GetProductsByFarmer(Guid farmerId)
        {
            try
            {
                // _productManager yerine _productService kullanıyoruz
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
