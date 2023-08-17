using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StorevesM.CustomerService.Entity;
using StorevesM.CustomerService.Model.Request;
using StorevesM.CustomerService.Model.View;
using StorevesM.CustomerService.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StorevesM.CustomerService.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly CustomerDbContext _context;
        private readonly Repository<Customer> _customerRepository;

        public CustomerService(CustomerDbContext customerDbContext, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _mapper = mapper;
            _context = customerDbContext;
            _customerRepository = new Repository<Customer>(_context);
        }
        public async Task<CustomerViewModel> CreateCustomer(CustomerCreateModel ccm)
        {
            if (ccm != null)
            {
                var customer = await _customerRepository.FirstOrDefaultAsync(x => x.Username == ccm.Username);
                if (customer != null)
                {
                    return null!;
                }
                Customer customer1 = new();
                customer1.Name = ccm.Name;
                customer1.Address = ccm.Address ?? null!;
                customer1.Username = ccm.Username;
                customer1.Password = ccm.Password;
                customer1.IsActive = true;
                customer1.CreateAt = DateTime.Now;
                await _customerRepository.AddAsync(customer1);
                await _context.SaveChangesAsync();
                return await GetCustomer(customer1.Id);
            }
            return null!;
        }

        public async Task<List<CustomerViewModel>> GetCustomers(CustomerFilterModel? customerFilterModel)
        {
            var queryable = _customerRepository.GetAll();
            if (customerFilterModel != null && queryable.Count() > 0)
            {
                if (customerFilterModel.Name != null)
                {
                    queryable = queryable.Where(x => x.Name.ToLower().Contains(customerFilterModel.Name.ToLower()));
                }
                if (customerFilterModel.Username != null)
                {
                    queryable = queryable.Where(x => x.Username.ToLower().Contains(customerFilterModel.Username.ToLower()));
                }
            }
            if (queryable.Count() <= 0)
            {
                return null!;
            }
            return await queryable.ProjectTo<CustomerViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<CustomerViewModel> GetCustomer(int id)
        {
            var customer = await _customerRepository.FirstOrDefaultAsync(x => x.Id == id);

            return customer != null ? _mapper.Map<CustomerViewModel>(customer) : null!;
        }

        public async Task<string> Login(CustomerLoginModel customerLogin)
        {
            var customer = await _customerRepository.FirstOrDefaultAsync(x => x.Username == customerLogin.Username && x.Password == customerLogin.Password);
            if (customer != null)
            {
                return GetToken(customer);
            }
            return null!;
        }

        public async Task<CustomerViewModel> UpdateCustomer(CustomerUpdateModel customerUpdate, int id)
        {
            var customer = await _customerRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (customer == null)
            {
                return null!;
            }
            customer.Name = customerUpdate.Name ?? customer.Name;
            customer.Password = customerUpdate.Password ?? customer.Password;
            customer.Address = customerUpdate.Address ?? customer.Address;
            await _context.SaveChangesAsync();
            return await GetCustomer(id);
        }

        private string GetToken(Customer customer)
        {
            var key = System.Text.Encoding.UTF8.GetBytes(_configuration["SecretKeytoken"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, customer.Username),
                    new Claim("role", "customer"),
                    new Claim("id", customer.Id.ToString())
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddDays(1)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> RemoveCustomer(int id)
        {
            var customer = await _customerRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (customer != null!)
            {
                customer.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
