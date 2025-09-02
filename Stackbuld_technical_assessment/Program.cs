using Api.config;
using Core.Interface;
using Infrastracture.Data;
using Infrastracture.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MyDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("StackbuldConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient(typeof(IGenericRepo<>), typeof(GenericService<>));
builder.Services.AddTransient<IProductRepo, ProductServices>();
builder.Services.AddTransient<IOrderRepo, OrderServies>();
builder.Services.AddTransient<ICostumerRepo, CostumerServices>();
builder.Services.AddHostedService<DbSeed>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
