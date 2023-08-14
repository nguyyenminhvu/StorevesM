using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorevesM.ProductService.Model.Request;
using StorevesM.ProductService.Service;

namespace StorevesM.ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> DemoSend()
        {
            return Ok(await _productService.SendMessageDemo());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromQuery] ProductCreateModel pcm)
        {
            try
            {
                var rs = await _productService.CreateProduct(pcm);
                if (rs != null)
                {
                    return StatusCode(StatusCodes.Status201Created, rs);
                }
                return BadRequest(new { Message = "Wrong categoryId mate" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
