using Microsoft.AspNetCore.Mvc;
using StorevesM.ProductService.Model.Request;
using StorevesM.ProductService.Service;

namespace StorevesM.ProductService.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        //[HttpGet]
        //public async Task<IActionResult> DemoSend()
        //{
        //    return Ok(await _productService.SendMessageDemo());
        //}

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
                return BadRequest(new { Message = "Wrong CategoryId" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            try
            {
                var product = await _productService.GetProduct(id);
                return product != null ? Ok(product) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductFilterModel pfm)
        {
            try
            {
                var products = await _productService.GetProducts(pfm);
                return products != null ? Ok(products) : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateModel pum, [FromRoute] int id)
        {
            try
            {
                var product = await _productService.UpdateProduct(pum, id);
                return product != null ? Ok(product) : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            try
            {
                var deleted = await _productService.RemoveProduct(id);
                return deleted == true ? StatusCode(StatusCodes.Status204NoContent) : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }
    }
}
