using Farm2Market.Domain.Entities;
using Farm2Market.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farm2Market.Domain.Interfaces;
using Farm2Marrket.Application.Sevices;
using Farm2Marrket.Application.DTOs;
using Microsoft.EntityFrameworkCore;


namespace Farm2Marrket.Application.Manager
{
    public class ProductManager : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ProductResponseDto> AddProduct(Guid farmerId, ProductDto productDto)
        {
			// Resim verilerini byte dizilerine dönüştürme
			byte[] image1Bytes = null, image2Bytes = null, image3Bytes = null;

			if (!string.IsNullOrEmpty(productDto.Image1))
			{
				image1Bytes = Convert.FromBase64String(productDto.Image1);
			}
			if (!string.IsNullOrEmpty(productDto.Image2))
			{
				image2Bytes = Convert.FromBase64String(productDto.Image2);
			}
			if (!string.IsNullOrEmpty(productDto.Image3))
			{
				image3Bytes = Convert.FromBase64String(productDto.Image3);
			}

			// En az bir resmin zorunlu olduğu kontrolü
			if (image1Bytes == null && image2Bytes == null && image3Bytes == null)
			{
				throw new Exception("At least one product image is required.");
			}

			// Kategori kontrolü
			var category = await _productRepository.Categories.FirstOrDefaultAsync(c => c.Id == productDto.CategoryId);
			if (category == null)
			{
				throw new Exception("Category not found.");
			}

			// Yeni ürün oluşturma
			var product = new Product
			{
				Name = productDto.Name,
				Description = productDto.Description,
				WeightOrAmount = productDto.WeightOrAmount,
				Address = productDto.Address,
				FullAddress = productDto.FullAddress,
				Category = category,
				Quality = productDto.Quality,
				Price = productDto.Price,
				FarmerId = farmerId,
				Image1 = image1Bytes,
				Image2 = image2Bytes,
				Image3 = image3Bytes,
				UnitType = productDto.UnitType,
				CreatedDate = DateTime.Now,
			};

			// Veritabanına ekleme işlemi
			try
			{
				await _productRepository.AddAsync(product);
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error occurred while adding the product: " + ex.Message);
				throw;
			}

			// Dönüş objesi oluşturma
			var productResponse = new ProductResponseDto
			{
				Id = product.Id,
				Name = product.Name,
				WeightOrAmount = product.WeightOrAmount,
				Address = product.Address,
				Category = category.Name, // Kategori adı
				Quality = product.Quality,
				Price = product.Price,
				IsActive = product.IsActive,
			};

			return productResponse;

		}


        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new Exception("Ürün bulunamadı.");
            }
            if (product.IsActive)
            {
                throw new Exception("Ürün zaten pasif durumda.");
            }
            await _productRepository.DeleteProductAsync(product);  // DeleteProductAsync'i repository'de çağırıyoruz.
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByFarmerIdAsync(Guid farmerId)
        {
            //repodan alıyoruz
            var products = await _productRepository.GetProductsByFarmerIdAsync(farmerId);


            var productDtos = products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                WeightOrAmount = product.WeightOrAmount,
                Address = product.Address,
                FullAddress = product.FullAddress,
                CategoryId = product.CategoryId,
                Quality = product.Quality,
                Quantity = product.Quantity,
                Price = product.Price,
				Image1 = product.Image1 != null ? Convert.ToBase64String(product.Image1) : null,
				Image2 = product.Image2 != null ? Convert.ToBase64String(product.Image2) : null,
				Image3 = product.Image3 != null ? Convert.ToBase64String(product.Image3) : null,
				UnitType = product.UnitType,
                //FarmerId = product.FarmerId,
                //IsActive = product.IsActive
            }); 

            return productDtos;
        }

        public async Task<IEnumerable<ProductDto>> GetProductAsync()
        {
            //repodan alıyoruz
            var products = await _productRepository.GetProductsAsync();

            var productDtos = products.Select(product => new ProductDto
            {
				Id = product.Id,
				Name = product.Name,
                Description = product.Description,
                WeightOrAmount = product.WeightOrAmount,
                Address = product.Address,
                FullAddress = product.FullAddress,
                CategoryId = product.CategoryId,
                Quality = product.Quality,
                Quantity = product.Quantity,
                Price = product.Price,
				Image1 = product.Image1 != null ? Convert.ToBase64String(product.Image1) : null,
				Image2 = product.Image2 != null ? Convert.ToBase64String(product.Image2) : null,
				Image3 = product.Image3 != null ? Convert.ToBase64String(product.Image3) : null,
				UnitType = product.UnitType,
                //FarmerId = product.FarmerId,
                //IsActive = product.IsActive
            });

            return productDtos;
        }
        public async Task<bool> UpdateProductQuantity(int id, int amount)
        {
            // Ürünü ID ile al
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new Exception("Product bulunamadı");
            }

            // Geçerli miktar kontrolü
            if (amount <= 0)
            {
                throw new Exception("Girdiğiniz miktar 0 veya negatif olamaz");
            }

            if (product.WeightOrAmount < amount)
            {
                throw new Exception("Girdiğiniz miktar ürünün mevcut adedinden fazla olamaz");
            }

            // Yeni miktarı hesapla
            int newQuantity = product.WeightOrAmount - amount;

            // Eğer yeni miktar 0 olursa ürünü sil
            if (newQuantity == 0)
            {
                await _productRepository.DeleteProductAsync(product);
            }
          
                
                await _productRepository.UpdateProductQuantity(product.Id, newQuantity);
            

            return true;
        }

		public async Task<bool> UpdateProductAsync(ProductDto productDto)
		{
			var product = await _productRepository.GetByIdAsync(productDto.Id);
			if (product == null)
			{
				return false;
			}

			// DTO'dan alınan verilerle mevcut ürün güncelleniyor
			product.Name = productDto.Name;
			product.Description = productDto.Description;
			product.WeightOrAmount = productDto.WeightOrAmount;
			product.Address = productDto.Address;
			product.FullAddress = productDto.FullAddress;
			product.CategoryId = productDto.CategoryId;
			product.Quality = productDto.Quality;
			product.Quantity = productDto.Quantity;
			product.Price = productDto.Price;
			if (!string.IsNullOrEmpty(productDto.Image1))
			{
				product.Image1 = Convert.FromBase64String(productDto.Image1);
			}

			if (!string.IsNullOrEmpty(productDto.Image2))
			{
				product.Image2 = Convert.FromBase64String(productDto.Image2);
			}

			if (!string.IsNullOrEmpty(productDto.Image3))
			{
				product.Image3 = Convert.FromBase64String(productDto.Image3);
			}
			product.UnitType = productDto.UnitType;
			product.IsActive = productDto.IsActive;

			// Veritabanında güncelleme işlemi
			await _productRepository.UpdateAsync(product);
			return true;
		}


		//public async Task<bool> UpdateAsync(Product product)
		//{
		//	// Veritabanında güncelleme işlemi
		//	var existingProduct = await _productRepository.GetByIdAsync(product.Id);
		//	if (existingProduct == null)
		//	{
		//		return false; // Ürün bulunamadı
		//	}

		//	existingProduct.Name = product.Name;
		//	existingProduct.Description = product.Description;
		//	existingProduct.WeightOrAmount = product.WeightOrAmount;
		//	existingProduct.Address = product.Address;
		//	existingProduct.FullAddress = product.FullAddress;
		//	existingProduct.Category = product.Category;
		//	existingProduct.Quality = product.Quality;
		//	existingProduct.Quantity = product.Quantity;
		//	existingProduct.Price = product.Price;
		//	existingProduct.Image = product.Image;
		//	existingProduct.UnitType = product.UnitType;
		//	existingProduct.IsActive = product.IsActive;

		//	await _productRepository.UpdateAsync(existingProduct);
		//	return true;
		//}



		public async Task<int> AddCategoryAsync(string categoryName)
		{
			if (string.IsNullOrEmpty(categoryName))
				throw new ArgumentException("Category name cannot be null or empty.");

			// Yeni kategori nesnesi oluştur
			var category = new Category
			{
				Name = categoryName
			};

			// Veritabanına ekle
			await _productRepository.AddCategoryAsync(category);

			return category.Id; // Eklenen kategorinin ID'sini döndür
		}

		public async Task<IEnumerable<Category>> GetCategory()
		{
			return await _productRepository.GetCategory();
		}


		public async Task<Order> GetOrderByIdAsync(int orderId)
		{
			return await _productRepository.GetOrderByIdAsync(orderId);
		}

		public async Task<bool> UpdateOrderStatus(int orderId, string status)
		{
			var order = await _productRepository.GetOrderByIdAsync(orderId);
			if (order == null) return false;

			order.Status = status;
			await _productRepository.UpdateAsync(order);
			return true;
		}
	}
}

