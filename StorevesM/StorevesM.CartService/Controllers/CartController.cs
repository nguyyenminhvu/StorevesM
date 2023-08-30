using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using StorevesM.CartService.Model.Request;
using StorevesM.CartService.Service.Interface;

namespace StorevesM.CartService.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ICartService _cartService;

        public CartController(ICartService cartService, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _cartService = cartService;
        }


        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var headers = HttpContext.Request.Headers.Authorization;
                if (StringValues.IsNullOrEmpty(headers)) return Unauthorized(new { Message = "Unauthorized" });
                var customerId = _tokenService.GetId(headers!);
                if (customerId == null) return Unauthorized(new { Message = "Unauthorized" });
                return Ok(await _cartService.GetCart(Convert.ToInt32(customerId)));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCart(CartUpdateModel cum)
        {
            var headers = HttpContext.Request.Headers.Authorization;
            if (StringValues.IsNullOrEmpty(headers)) return Unauthorized(new { Message = "Unauthorized" });
            var customerId = _tokenService.GetId(headers!);
            if (customerId == null) return Unauthorized(new { Message = "Unauthorized" });
            try
            {
                var rs = await _cartService.UpdateCart(cum, Convert.ToInt32(customerId));
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
