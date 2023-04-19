using DataAccessLayer.DataProviders;
using DataAccessLayer;
using ServerDevelopment.Data;
using AutoMapper;
using ServerDevelopment.Mapper;
using FluentValidation;
using ServerDevelopment.Middleware;

var builder = WebApplication.CreateBuilder(args);

//services
var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddControllers();
builder.Services.AddScoped<ICustomerDataProvider, CustomerDataProvider>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<IValidator<CustomerDTO>>(serviceProvider =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    var oldName = httpContextAccessor?.HttpContext?.Request?.RouteValues?["name"]?.ToString();
    var customerService = serviceProvider.GetRequiredService<ICustomerService>();
    return new CustomerValidator(customerService, oldName);
});


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

app.UseExceptionHandlingMiddleware();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("MyApp");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
