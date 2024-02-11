using Serilog;
using Carter;
using FinanceManagerAPI.Application;
using FinanceManagerAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FinanceManagerAPI.WebApi;

public class Startup
{
    private IConfiguration Configuration { get; }
    
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContextPool<FinanceAPIDbContext>(options => options
            .UseSqlServer(Configuration.GetConnectionString("SQLServer")));
        
        services.RegisterApplicationDependency();
        
        services.AddCarter();
        
        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Finance Manager API", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSerilogRequestLogging();
        
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        
            // Swagger is enabled only in a development environment. May be turned-off if needed.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Finance Manager API V1");
            });
        }
        
        app.UseRouting();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapCarter();
        });
    }
}