﻿using Farm2Market.Domain;
using Farm2Market.Domain.Entities;
using Farm2Marrket.Application.Sevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.Manager
{
    public class UserManager : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task AddUser(User user)
        {
            await _userRepository.AddAsync(user);

        }
		public async Task<bool> ConfirmNumber(string id,int number)
        {
            var idnum = await _userRepository.GetByIdAsync(id);
			var existnumber = await _userRepository.GetConfirmNumber(id);
            if (existnumber == number)
            {
                await _userRepository.GetConfirmedEmail(id);
                return true;
            }
            else { return false; }
        }

	}
}
