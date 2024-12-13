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
			List<byte[]> imageBytesList = new List<byte[]>();

			if (productDto.Image != null && productDto.Image.Any())
			{
				foreach (var base64Image in productDto.Image)
				{
					if (!string.IsNullOrEmpty(base64Image))
					{
						try
						{
							var imageBytes = Convert.FromBase64String(base64Image);
							imageBytesList.Add(imageBytes);
						}
						catch (FormatException ex)
						{
							Console.WriteLine("Invalid base64 string: " + ex.Message);
						}
					}
				}
			}

			// Category'yi enum türüne dönüştür
			//ProductCategory categoryEnum;
			var category = await _productRepository.Categories.FirstOrDefaultAsync(c => c.Id == productDto.CategoryId);

			var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    WeightOrAmount = productDto.WeightOrAmount,
                    Address = productDto.Address,
                    FullAddress = productDto.FullAddress,
                    Category = category,  // Enum olarak kaydediliyor
                    Quality = productDto.Quality,
                    Price = productDto.Price,
                    FarmerId = farmerId,
                    Image = imageBytesList,
                    UnitType = productDto.UnitType,
                    CreatedDate = DateTime.Now,
                };

                try
                {
                    await _productRepository.AddAsync(product);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while adding the product: " + ex.Message);
                    throw;
                }

                var productResponse = new ProductResponseDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    WeightOrAmount = product.WeightOrAmount,
                    Address = product.Address,
                    Category = category.Name,  // Enum'u string'e dönüştür
                    Quality = product.Quality,
                    Price = product.Price,
                    IsActive = product.IsActive,
                };

                return productResponse;  // Burada return ekliyoruz

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
				Image = product.Image != null && product.Image.Any()
		? product.Image.Select(image => Convert.ToBase64String(image)).ToList() // byte[] -> Base64 for each image
		: new List<string>(), // byte[] -> Base64
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
				Image = product.Image != null && product.Image.Any()
		? product.Image.Select(image => Convert.ToBase64String(image)).ToList() // Her bir byte[] -> Base64
		: new List<string>(), // byte[] -> Base64
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
			// Enum dönüşümü
			//if (Enum.TryParse<ProductCategory>(productDto.Category, out var category))
			//{
			//	product.Category = category;
			//}
			//else
			//{
			//	throw new Exception("Invalid category value.");
			//}

			product.Quality = productDto.Quality;
			product.Quantity = productDto.Quantity;
			product.Price = productDto.Price;
			if (productDto.Image != null && productDto.Image.Any())
			{
				product.Image = productDto.Image
					.Where(image => !string.IsNullOrEmpty(image)) // Geçerli resim değerlerini al
					.Select(image => Convert.FromBase64String(image)) // Base64 -> byte[]
					.ToList();
			} // Resim değişmemişse mevcut resmi korur
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

	}
}

