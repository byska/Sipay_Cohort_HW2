using Sipay_Cohort_HW2.DTOs.Order;

namespace Sipay_Cohort_HW2.DTOs.User
{
    public class UserDTO
    {
        public bool IsActive { get; set; } = true;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public virtual List<OrderDTO> Orders { get; set; }

    }
}
