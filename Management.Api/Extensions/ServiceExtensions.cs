using System.Reflection.Metadata;
using Asp.Versioning;
using Management.Api.Formatter;
using Management.Api.Utility;
using Management.Contracts;
using Management.Entities.Models;
using Management.Presentation;
using Management.Repository;
using Management.Service;
using Management.Service.Contracts;
using Management.Service.Log;
using Management.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Management.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("X-Pagination")));

    public static void ConfigureLogging(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();

    public static void ConfigureEmployeeLinks(this IServiceCollection services) =>
        services.AddScoped<IEmployeeLinks, EmployeeLinks>();

    public static void ConfigureDataShaper(this IServiceCollection services) =>
        services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();

    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

    public static IMvcBuilder AddCustomCsvFormatter(this IMvcBuilder builder) =>
        builder.AddMvcOptions(cfg => cfg.OutputFormatters.Add(new CsvOutputFormatter()));

    public static void ConfigureControllers(this IServiceCollection services) =>
        services.AddControllers(cfg =>
        {
            cfg.RespectBrowserAcceptHeader = true;
            cfg.ReturnHttpNotAcceptable = true;
            cfg.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
        }).AddXmlSerializerFormatters()
            .AddCustomCsvFormatter()
            .AddApplicationPart(typeof(AssemblyReference).Assembly);

    public static void ConfigureAutoMapping(this IServiceCollection services) =>
        services.AddAutoMapper(cfg =>
        {
            cfg.CreateMap<Company, CompanyDto>()
                .ForMember(c => c.FullAddress, opt => opt.MapFrom(x => x.Address + " " + x.Country));
            cfg.CreateMap<CompanyForCreationDto, Company>();
            cfg.CreateMap<CompanyForUpdateDto, Company>();

            cfg.CreateMap<Employee, EmployeeDto>();
            cfg.CreateMap<EmployeeForCreationDto, Employee>();
            cfg.CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();
        }, typeof(Program));

    public static void ConfigureVersioning(this IServiceCollection services) =>
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        }).AddMvc(options =>
        {
            options.Conventions.Controller<CompaniesController>().HasApiVersion(new ApiVersion(1, 0));
            options.Conventions.Controller<CompaniesV2Controller>().HasDeprecatedApiVersion(new ApiVersion(2, 0));
        });

    public static void ConfigureResponseCaching(this IServiceCollection services) =>
        services.AddResponseCaching();

    private static NewtonsoftJsonInputFormatter GetJsonPatchInputFormatter() =>
        new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
            .Services.BuildServiceProvider().GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
            .OfType<NewtonsoftJsonPatchInputFormatter>().First();
}
