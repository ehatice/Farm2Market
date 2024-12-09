﻿using Farm2Market.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.DTOs
{
    public class ProductDto
    {
        public  int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int WeightOrAmount { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }
		public int CategoryId { get; set; }
		public string Quality { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<string> Image { get; set; }
        public string UnitType { get; set; }
        public bool IsActive { get; set; }
    }
}
