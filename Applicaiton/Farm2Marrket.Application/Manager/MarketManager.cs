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
    public class MarketManager : IMarketService
    {
        private readonly IMarketRepository _marketRepository;
        private readonly IPasswordHasher<MarketReceiver> _passwordHasher;

        public MarketManager(IMarketRepository marketRepository)
        {
            _marketRepository = marketRepository;
        }

        public async Task<UpdateMarketDto> GetMarketByIdAsync(Guid marketId)
        {

            var market = await _marketRepository.GetByIdAsync(marketId);


            if (market == null)
                return null;


            var marketDto = new UpdateMarketDto
            {
                FirstName = market.FirstName,
                LastName = market.LastName,
                UserName = market.UserName,
                Email = market.Email,
                Adress = market.Adress,
                MarketName = market.MarketName,
                CompanyType = market.CompanyType,
            };

            return marketDto;
        }

        public async Task<bool> UpdateMarketAsync(Guid marketId, UpdateMarketDto marketDto)
        {

            var user = await _marketRepository.GetByIdAsync(marketId);
            if (user == null)
                return false;


            user.FirstName = marketDto.FirstName;
            user.LastName = marketDto.LastName;
            user.UserName = marketDto.UserName;
            user.Email = marketDto.Email;
            user.Adress = marketDto.Adress;
            user.CompanyType = marketDto.CompanyType;
            user.MarketName = marketDto.MarketName;

            return await _marketRepository.UpdateAsync(user);
        }
    }
}
