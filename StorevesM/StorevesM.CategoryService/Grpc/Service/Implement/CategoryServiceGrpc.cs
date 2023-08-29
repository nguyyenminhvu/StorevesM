using Grpc.Core;
using StorevesM.CategoryService.Grpc.Protos;
using StorevesM.CategoryService.Service;

namespace StorevesM.CategoryService.Grpc.Service.Implement
{
    public class CategoryServiceGrpc : Category.CategoryBase
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CategoryServiceGrpc(IServiceScopeFactory serviceScopeFactory)
        {
            _scopeFactory = serviceScopeFactory;
        }
        public override async Task<GetCategoryResponse> GetCategory(GetCategoryRequest request, ServerCallContext context)
        {
            using (var _scope = _scopeFactory.CreateScope())
            {
                var categoryService = _scope.ServiceProvider.GetRequiredService<ICategoryService>();
                var category = await categoryService.GetCategory(request.Id);
                return category != null ? await Task.FromResult(new GetCategoryResponse { Id = request.Id, Name = category.Name }) : throw new RpcException(new Status(StatusCode.NotFound,"Not Found"));
            }
        }
    }
}
