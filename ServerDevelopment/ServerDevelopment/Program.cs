using DataAccessLayer.DataProviders;
using DataAccessLayer;
using ServerDevelopment.Data;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using DataAccessLayer.Models;
using ServerDevelopment.Mapper;

var builder = WebApplication.CreateBuilder(args);

//services
var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddControllers();
builder.Services.AddScoped<ICustomerDataProvider, CustomerDataProvider>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddCors(options => options.AddPolicy(name: "MyApp",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    }));

var app = builder.Build();

// HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}





//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseCors("MyApp");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
