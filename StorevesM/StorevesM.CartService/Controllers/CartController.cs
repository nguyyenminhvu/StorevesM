using Microsoft.AspNetCore.Mvc;
using StorevesM.CartService.Model.Request;
using StorevesM.CartService.Service.Interface;

namespace StorevesM.CartService.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            int customerId = 1;
            try
            {
                return Ok(await _cartService.GetCart(customerId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCart(CartUpdateModel cum)
        {
            int customerId = 1;
            try
            {
                var rs = await _cartService.UpdateCart(cum, customerId);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("demo-get-customer-{id}")]
        public async Task<IActionResult> GetCustomerDemo([FromRoute] int id)
        {
            try
            {
                var rs = await _cartService.DemoCallCustomer(id);
                return rs != null ? Ok(rs) : BadRequest(new { Message = "Wrong wrong" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
