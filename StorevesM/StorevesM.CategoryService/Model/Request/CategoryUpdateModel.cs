using System.ComponentModel.DataAnnotations;

namespace StorevesM.CategoryService.Model.Request
{
    public class CategoryUpdateModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
