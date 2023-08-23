﻿using Task_1.Models;

namespace Task_1.ViewModels
{
    public class StoreViewModel
    {
        public List<Product> Products { get; set; } = new();
        public List<string> Categories { get; set; } = new();
    }
}
