using System.Reflection.Metadata;
using Management.Contracts;
using Management.Entities.Models;
using Management.Repository;
using Management.Service;
using Management.Service.Contracts;
using Management.Service.Log;
using Management.Shared.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Management.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()));

    public static void ConfigureLogging(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();

    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

    public static void ConfigureControllers(this IServiceCollection services) =>
        services.AddControllers(cfg =>
        {
            cfg.RespectBrowserAcceptHeader = true;
            cfg.ReturnHttpNotAcceptable = true;
        }).AddXmlSerializerFormatters()
            .AddApplicationPart(typeof(AssemblyReference).Assembly);

    public static void ConfigureAutoMapping(this IServiceCollection services) =>
        services.AddAutoMapper(cfg =>
        {
            cfg.CreateMap<Company, CompanyDto>()
            .ForMember(c => c.FullAddress, opt => opt.MapFrom(x => x.Address + " " + x.Country));

            cfg.CreateMap<Employee, EmployeeDto>();
        }, typeof(Program));
}
