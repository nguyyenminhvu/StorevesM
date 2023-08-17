using StorevesM.CustomerService.Model.Request;
using StorevesM.CustomerService.Model.View;

namespace StorevesM.CustomerService.Service
{
    public interface ICustomerService
    {
        Task<List<CustomerViewModel>> GetCustomers(CustomerFilterModel? customerFilterModel);
        Task<CustomerViewModel> GetCustomer(int id);
        Task<CustomerViewModel> UpdateCustomer(CustomerUpdateModel customerUpdate, int id);
        Task<string> Login(CustomerLoginModel customerLogin);
        Task<CustomerViewModel> CreateCustomer(CustomerCreateModel ccm);
        Task<bool> RemoveCustomer(int id);
    }
}
