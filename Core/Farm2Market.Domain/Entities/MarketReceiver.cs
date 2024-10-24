using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Market.Domain.Entities
{
    public class MarketReceiver:AppUser
    {
        public string MarketName {  get; set; }
        public string CompanyType { get; set; }
    }
}
