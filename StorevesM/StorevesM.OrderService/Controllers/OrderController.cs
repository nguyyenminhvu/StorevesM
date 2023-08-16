using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorevesM.OrderService.Service;

namespace StorevesM.OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("demo-get-products")]
        public async Task<IActionResult> DemoGetProducts()
        {
            try
            {
                var products = await _orderService.DemoGetProduct();
                return Ok(products);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("demo-update-quantity-products")]
        public async Task<IActionResult> DemoUpdateQuantityProduct()
        {
            try
            {
                var updated = await _orderService.DemoUpdateQuantityProduct();
                return updated ? Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("demo-clear-cartitem")]
        public async Task<IActionResult> DemoClearCartItem()
        {
            try
            {
                var updated = await _orderService.DemoClearCartItem();
                return updated ? Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
