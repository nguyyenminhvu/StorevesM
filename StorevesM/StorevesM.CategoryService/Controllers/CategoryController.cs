using Microsoft.AspNetCore.Mvc;
using StorevesM.CategoryService.Model.Request;
using StorevesM.CategoryService.Service;

namespace StorevesM.CategoryService.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var rs = await _categoryService.GetCategory(id);
                return rs != null ? Ok(rs) : NotFound(new { Message = "Not found category" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] CategoryFilter category)
        {
            try
            {
                var rs = await _categoryService.GetCategories(category);
                return rs != null ? Ok(rs) : NotFound(new { Message = "Not found category" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreateModel cm)
        {
            try
            {
                var rs = await _categoryService.CreateCategory(cm);
                return rs != null ? StatusCode(StatusCodes.Status201Created, rs) : NotFound(new { Message = "Not found category" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateModel cum)
        {
            try
            {
                var rs = await _categoryService.UpdateCategory(cum);
                return rs != null ? Ok(rs) : NotFound(new { Message = "Not found category" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
