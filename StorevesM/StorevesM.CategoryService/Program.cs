using StorevesM.CategoryService.ApplicationConfig;
using StorevesM.CategoryService.Grpc.Service.Implement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.InjectionDependency(builder.Configuration);
builder.Services.AddGrpc();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGrpcService<CategoryServiceGrpc>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
