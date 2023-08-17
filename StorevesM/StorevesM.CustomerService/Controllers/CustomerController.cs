using Microsoft.AspNetCore.Mvc;
using StorevesM.CustomerService.Model.Request;
using StorevesM.CustomerService.Service;

namespace StorevesM.CustomerService.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            try
            {
                var rs = await _customerService.GetCustomer(id);
                return rs != null ? Ok(rs) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers([FromQuery] CustomerFilterModel cfm)
        {
            try
            {
                var rs = await _customerService.GetCustomers(cfm);
                return rs != null ? Ok(rs) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCustomer([FromRoute] int id)
        {
            try
            {
                var rs = await _customerService.RemoveCustomer(id);
                return rs ? StatusCode(StatusCodes.Status204NoContent) : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
