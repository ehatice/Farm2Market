﻿using Farm2Market.Domain.Entities;
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
using Farm2Market.Domain.Enums;

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
            byte[]? imageBytes = null;
            if (!string.IsNullOrEmpty(productDto.Image))
            {
                try
                {
                    imageBytes = Convert.FromBase64String(productDto.Image);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Invalid base64 string: " + ex.Message);
                }
            }

            // Category'yi enum türüne dönüştür
            ProductCategory categoryEnum;
            if (Enum.TryParse(productDto.Category, out categoryEnum))
            {
                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    WeightOrAmount = productDto.WeightOrAmount,
                    Address = productDto.Address,
                    FullAddress = productDto.FullAddress,
                    Category = categoryEnum,  // Enum olarak kaydediliyor
                    Quality = productDto.Quality,
                    Price = productDto.Price,
                    FarmerId = farmerId,
                    Image = imageBytes ?? new byte[0],
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
                    Category = product.Category.ToString(),  // Enum'u string'e dönüştür
                    Quality = product.Quality,
                    Price = product.Price,
                    IsActive = product.IsActive,
                };

                return productResponse;  // Burada return ekliyoruz
            }
            else
            {
                throw new ArgumentException("Invalid category value");
            }
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
                Name = product.Name,
                Description = product.Description,
                WeightOrAmount = product.WeightOrAmount,
                Address = product.Address,
                FullAddress = product.FullAddress,
                Category = product.Category.ToString(),
                Quality = product.Quality,
                Quantity = product.Quantity,
                Price = product.Price,
                Image = product.Image != null ? Convert.ToBase64String(product.Image) : string.Empty, // byte[] -> Base64
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
                Name = product.Name,
                Description = product.Description,
                WeightOrAmount = product.WeightOrAmount,
                Address = product.Address,
                FullAddress = product.FullAddress,
                Category = product.Category.ToString(),
                Quality = product.Quality,
                Quantity = product.Quantity,
                Price = product.Price,
                Image = product.Image != null ? Convert.ToBase64String(product.Image) : string.Empty, // byte[] -> Base64
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

    }
}

