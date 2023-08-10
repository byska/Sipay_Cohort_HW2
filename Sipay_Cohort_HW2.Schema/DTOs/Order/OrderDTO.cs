using Sipay_Cohort_HW2.Enums;

namespace Sipay_Cohort_HW2.DTOs.Order
{
    public class OrderDTO
    {
        public bool IsActive { get; set; } = true;
        public Status status { get; set; } = 0;
        public string UserName { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public string Description { get; set; }
    }
}
