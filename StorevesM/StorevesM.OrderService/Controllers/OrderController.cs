using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorevesM.OrderService.Model.DTOMessage;
using StorevesM.OrderService.Service;

namespace StorevesM.OrderService.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CartDTO cartDTO)
        {
            try
            {
                var rs = await _orderService.CreateOrder(cartDTO);
                return rs != null ? Ok(rs) : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var rs = await _orderService.GetOrders();
                return rs != null ? Ok(rs) : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            try
            {
                var rs = await _orderService.GetOrder(id);
                return rs != null ? Ok(rs) : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] int id, [FromQuery] string status)
        {
            try
            {
                var rs = await _orderService.UpdateOrder(new Model.Request.OrderUpdateModel { Id = id, Status = status });
                return rs != null ? Ok(rs) : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
