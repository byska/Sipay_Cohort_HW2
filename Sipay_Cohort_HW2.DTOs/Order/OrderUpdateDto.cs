using HW_Cohorts_1.Enums;

namespace Sipay_Cohort_HW2.DTOs.Order
{
    public class OrderUpdateDto
    {
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public string Description { get; set; }
        public Status status { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }
}
