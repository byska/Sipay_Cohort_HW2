﻿namespace Sipay_Cohort_HW2.DTOs.Order
{
    public class OrderCreateDTO
    {
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public string Description { get; set; }
    }
}
