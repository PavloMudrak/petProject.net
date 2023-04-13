using DataAccessLayer.DataProviders;
using DataAccessLayer;
using ServerDevelopment.Data;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //services
        builder.Services.AddControllers();
        builder.Services.AddScoped<ICustomerDataProvider, CustomerDataProvider>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddDbContext<AppDbContext>();
        builder.Services.AddCors(o => o.AddPolicy("All", builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                //Always allow the debug version in browser
                .SetIsOriginAllowed(origin => origin == "http://localhost:4200");
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

        app.UseHttpsRedirection();
        app.UseCors("All");
        app.UseStaticFiles();
        app.UseRouting();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        app.Run();
    }
}
