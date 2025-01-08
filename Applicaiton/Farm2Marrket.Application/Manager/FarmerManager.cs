using Farm2Market.Domain.Entities;
using Farm2Market.Domain.Interfaces;
using Farm2Marrket.Application.DTOs;
using Farm2Marrket.Application.Sevices;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.Manager
{
    public class FarmerManager : IFarmerService
    {
        private readonly IFarmerRepository _farmerRepository;
        private readonly IPasswordHasher<Farmer> _passwordHasher;
        public FarmerManager(IFarmerRepository farmerRepository)
        {
            _farmerRepository = farmerRepository;
        }

        public async Task<UpdateFarmerDto> GetFarmerByIdAsync(Guid farmerId)
        {

            var farmer = await _farmerRepository.GetByIdAsync(farmerId);


            if (farmer == null)
                return null;


            var farmerDto = new UpdateFarmerDto
            {
                FirstName = farmer.FirstName,
                LastName = farmer.LastName,
                UserName = farmer.UserName,
                Email = farmer.Email,
                Adress = farmer.Adress,
            };

            return farmerDto;
        }
        public async Task<bool> UpdateFarmerAsync(Guid userId, UpdateFarmerDto farmerDto)
        {
         
            var user = await _farmerRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

        
            user.FirstName = farmerDto.FirstName;
            user.LastName = farmerDto.LastName;
            user.UserName = farmerDto.UserName;
            user.Email = farmerDto.Email;
            user.Adress = farmerDto.Adress;
      
            return await _farmerRepository.UpdateAsync(user);
        }


    }
}
