using AutoMapper;
using Sipay_Cohort_HW2.DTOs.Order;
using Sipay_Cohort_HW2.DTOs.User;
using Sipay_Cohort_HW2.Entities;

namespace Sipay_Cohort_HW2.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderCreateDTO,Order>().ReverseMap();
            CreateMap<Order,OrderDTO>().ForMember(dest=>dest.UserName,opt=>opt.MapFrom(src=>src.user.FirstName+" "+src.user.LastName));

            CreateMap<User, UserCreateDTO>().ReverseMap();

            CreateMap<Order,OrderUpdateDto>().ReverseMap();
        }
    }
}
