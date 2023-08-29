using Grpc.Net.Client;
using StorevesM.ProductService.Grpc.Protos;
using StorevesM.ProductService.Grpc.Service.Interface;
using StorevesM.ProductService.Model.DTOMessage;

namespace StorevesM.ProductService.Grpc.Service.Implement
{
    public class CategoryServiceSupport : ICategoryServiceSupport
    {
        private readonly IConfiguration _configuration;
        private readonly GrpcChannel _channel;
        private readonly Category.CategoryClient _categoryGrpc;


        public CategoryServiceSupport(IConfiguration configuration)
        {
            _configuration = configuration;
            _channel = GrpcChannel.ForAddress(_configuration["CategoryAddress"]!);
            _categoryGrpc = new Category.CategoryClient(_channel);
        }

        public async Task<CategoryDTO> GetCategory(int id)
        {
            if (id < 0) { return null!; }
            var category = await _categoryGrpc.GetCategoryAsync(new GetCategoryRequest { Id = id });
            return category != null ? new CategoryDTO { Id = id, Name = category.Name } : null!;
        }
    }
}
