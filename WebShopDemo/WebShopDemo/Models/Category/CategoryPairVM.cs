﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebShopDemo.Models.Category
{
    public class CategoryPairVM
    {
        public int Id { get; set; }
        [Display(Name = "Category")]
        public string Name { get; set; }
    }
}
