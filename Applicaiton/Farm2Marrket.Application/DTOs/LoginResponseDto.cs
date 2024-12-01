using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.DTOs
{
    public class LoginResponseDto
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public int ConfirmationNumber { get; set; }

    }
}
