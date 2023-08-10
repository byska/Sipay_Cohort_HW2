using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sipay_Cohort_HW2.Business.Services.Order;
using Sipay_Cohort_HW2.DataAccess.Context;
using Sipay_Cohort_HW2.DTOs.Order;
using Sipay_Cohort_HW2.DTOs.User;
using Sipay_Cohort_HW2.Entities;

namespace Sipay_Cohort_HW2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly HW2DbContext _context;
        private readonly IMapper _mapper;
        private readonly IOrderService _service;
        public OrderController(HW2DbContext context, IMapper mapper, IOrderService service)
        {
            _context = context;
            _mapper = mapper;
            _service = service;
        }

        /// <summary>
        /// Lists all orders
        /// </summary>
        /// <returns>List of OrderDTO</returns>
        [HttpGet]
        [Route("Get")]
        public ActionResult<List<OrderDTO>> Get()
        {
            var orders = _context.Set<Order>().Include(x => x.user).ToList();
            var mapped = _mapper.Map<List<OrderDTO>>(orders);
            return mapped;
        }

        /// <summary>
        /// Filters orders by user and sorts by date added 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>List of OrderDTO</returns>
        [HttpGet]
        [Route("GetOrderByUserId")]
        public ActionResult<List<OrderDTO>> GetOrderByUserId([FromQuery] int UserId)
        {
            if (UserId <= 0)
                return BadRequest("Gelen id değeri 0 veya 0'dan küçük.");
            try
            {
                var orders = _context.Set<Order>().Include(x => x.user).Where(x => x.UserId == UserId).OrderBy(x => x.AddedDate).ToList();
                var mapped = _mapper.Map<List<OrderDTO>>(orders);
                return mapped;
            }
            catch (Exception)
            {

                return NotFound();
            }
        }

        /// <summary>
        /// Brings the relevant order according to the order number
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns>OrderDTO</returns>
        [HttpGet]
        [Route("GetByOrderId/{OrderId}")]
        public ActionResult<OrderDTO> GetByOrderId(int OrderId)
        {
            if (OrderId <= 0)
                return BadRequest("Gelen id değeri 0 veya 0'dan küçük.");
            try
            {
                var order = _context.Set<Order>().Include(x => x.user).FirstOrDefault(x => x.Id == OrderId);
                var mapped = _mapper.Map<OrderDTO>(order);
                return mapped;
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Adds orders by taking information from body
        /// </summary>
        /// <param name="orderCreate"></param>
        /// <returns>Result of the transaction</returns>
        [HttpPost]
        public IActionResult AddOrder([FromBody] OrderCreateDTO orderCreate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var CreateOrder = _mapper.Map<OrderCreateDTO, Order>(orderCreate);
                    CreateOrder.UserId = 1;
                    _context.Set<Order>().Add(CreateOrder);
                    _context.SaveChanges();
                    return Ok(orderCreate);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            else return BadRequest("Lütfen girdiğiniz bilgileri kontrol ediniz.");
        }

        /// <summary>
        /// Adds users by taking information from query
        /// </summary>
        /// <param name="userCreate"></param>
        /// <returns>Result of the transaction</returns>
        [HttpGet]
        public IActionResult AddUser([FromQuery] UserCreateDTO userCreate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var CreateUser = _mapper.Map<UserCreateDTO, User>(userCreate);
                    _context.Set<User>().Add(CreateUser);
                    _context.SaveChanges();
                    return Ok(CreateUser);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            else return BadRequest("Lütfen girdiğiniz bilgileri kontrol ediniz.");
        }

        /// <summary>
        /// Updates the order
        /// </summary>
        /// <param name="id">Order id</param>
        /// <param name="orderUpdate"></param>
        /// <returns>Result of the transaction</returns>
        [HttpPut("{id}")]
        public IActionResult OrderUpdate(int id, [FromBody] OrderUpdateDto orderUpdate)
        {
            if (ModelState.IsValid)
            {
                var order = _context.Orders.Find(id);
                if (order == null) return StatusCode(404,"Sipariş bulunamadı.");
                try
                {
                    order.UnitPrice = orderUpdate.UnitPrice;
                    order.status = orderUpdate.status;
                    order.ModifiedDate = DateTime.Now;
                    order.Quantity = orderUpdate.Quantity;
                    order.Description = orderUpdate.Description;
                    order.IsActive = orderUpdate.IsActive;
                    _context.SaveChanges();
                    return StatusCode(200,order);
                }
                catch (Exception)
                {
                    return StatusCode(400,"İşlem gerçekleştirilemedi.");
                }
            }
            else return StatusCode(400,"Lütfen girdiğiniz bilgileri kontrol ediniz.");
        }

        /// <summary>
        /// Updates only the given parts of the order
        /// </summary>
        /// <param name="id">Order id</param>
        /// <param name="orderUpdate"></param>
        /// <returns>Result of the transaction</returns>
        [HttpPatch]
        public IActionResult OrderPartialUpdate(int id, [FromBody] OrderPartialUpdate orderPartialUpdate)
        {
            if (ModelState.IsValid)
            {
                var order = _context.Orders.Find(id);
                if (order == null) return StatusCode(404,"Kullanıcı bulunamadı.");
                try
                {
                    if(orderPartialUpdate.UnitPrice != null)
                    {
                        order.UnitPrice=orderPartialUpdate.UnitPrice;
                    }

                    if (orderPartialUpdate.Quantity != null)
                    {
                        order.Quantity = orderPartialUpdate.Quantity;
                    }

                    if (!string.IsNullOrEmpty(orderPartialUpdate.Description))
                    {
                        order.Description = orderPartialUpdate.Description;
                    }

                    _context.SaveChanges();
                    return StatusCode(200, order);
                }
                catch (Exception)
                {
                    return StatusCode(500,"İşlem sırasında bir hata oluştu.");
                }
            }
            else return StatusCode(400,"Lütfen girdiğiniz bilgileri kontrol ediniz.");
        }

        /// <summary>
        /// Delete the order
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>Result of the transaction</returns>
        [HttpDelete]
        public IActionResult DeleteOrder(int id)
        {
            var entity = _context.Set<Order>().Find(id);
            if (entity == null) return StatusCode(404,"Sipariş bulunamadı.");
            _context.Set<Order>().Remove(entity);
            _context.SaveChanges();
            return StatusCode(200,entity);
        }
    }
}
