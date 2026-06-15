using CouponAuthAPI.Model;
using CouponAuthAPI.Repository;
using CouponAuthAPI.Services;
using CouponAuthAPI.Services.IService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserRepository>();
builder.Services.Configure<JWTOption>(builder.Configuration.GetSection("ApiSetting:JwtOptions"));
builder.Services.AddScoped<IJWT, JWT>();
builder.Services.Configure<JWTOption>(
builder.Configuration.GetSection("JWTOption"));
//builder.Services.AddScoped<IJWT, JWT>();
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
