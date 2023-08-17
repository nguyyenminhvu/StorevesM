using Microsoft.AspNetCore.Mvc;
using StorevesM.CustomerService.Model.Request;
using StorevesM.CustomerService.Service;

namespace StorevesM.CustomerService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public AuthController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerCreateModel ccm)
        {
            try
            {
                var rs = await _customerService.CreateCustomer(ccm);
                return rs != null! ? StatusCode(StatusCodes.Status201Created, rs) : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(CustomerLoginModel clm)
        {
            try
            {
                var rs = await _customerService.Login(clm);
                return rs != null! ? Ok(rs) : StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
