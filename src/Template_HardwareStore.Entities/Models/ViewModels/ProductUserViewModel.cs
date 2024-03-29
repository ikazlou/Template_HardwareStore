﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template_HardwareStore.Entities.Models.ViewModels
{
    public class ProductUserViewModel
    {
        public ProductUserViewModel()
        {
            ProductList = new List<Product>();
        }

        public IList<Product> ProductList { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
