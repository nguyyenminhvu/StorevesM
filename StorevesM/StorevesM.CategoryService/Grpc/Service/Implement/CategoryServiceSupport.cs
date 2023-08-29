using Grpc.Net.Client;
using StorevesM.ProductService.Grpc.Service.Interface;

namespace StorevesM.ProductService.Grpc.Service.Implement
{
    public class CategoryServiceSupport : ICategoryServiceSupport
    {
        private readonly IConfiguration _configuration;
        private readonly GrpcChannel _channel;

        public CategoryServiceSupport(IConfiguration configuration)
        {
            _configuration = configuration;
            _channel = GrpcChannel.ForAddress(_configuration["CategoryAddress"]!);
        }
      
    }
}
