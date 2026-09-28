using System.Reflection.Metadata;
using System.Threading.RateLimiting;
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
using Microsoft.AspNetCore.Identity;
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
            // cfg.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
        }).AddXmlSerializerFormatters()
            .AddCustomCsvFormatter()
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

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

    public static void ConfigureOutputCaching(this IServiceCollection services) =>
        services.AddOutputCache(options =>
        {
            // options.AddBasePolicy(policy => policy.Expire(TimeSpan.FromSeconds(10)));
            options.AddPolicy("120SecondsDuration", p => p.Expire(TimeSpan.FromSeconds(120)));
        });

    public static void ConfigureRateLimitingOptions(this IServiceCollection services) =>
        services.AddRateLimiter(options =>
                {
                    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                            context => RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter",
                                partition => new FixedWindowRateLimiterOptions
                                {
                                    AutoReplenishment = true,
                                    PermitLimit = 5,
                                    QueueLimit = 2,
                                    Window = TimeSpan.FromMinutes(1)
                                }));

                    options.AddPolicy("SpecificPolicy", context =>

                        RateLimitPartition.GetFixedWindowLimiter("SpecificLimiter",
                                partition => new FixedWindowRateLimiterOptions
                                {
                                    AutoReplenishment = true,
                                    PermitLimit = 3,
                                    Window = TimeSpan.FromSeconds(10),
                                }));

                    options.OnRejected = async (context, token) =>
                    {
                        context.HttpContext.Response.StatusCode = 429;
                        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                            await context.HttpContext.Response.WriteAsync($"Too many request. Please try again after {retryAfter.TotalSeconds} seconds(s).", token);
                        else
                            await context.HttpContext.Response.WriteAsync($"Too many request. Please try again later.", token);
                    };
                });

    public static void ConfigureIdentity(this IServiceCollection services) =>
        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 10;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<RepositoryContext>()
        .AddDefaultTokenProviders();

    private static NewtonsoftJsonInputFormatter GetJsonPatchInputFormatter() =>
        new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
            .Services.BuildServiceProvider().GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
            .OfType<NewtonsoftJsonPatchInputFormatter>().First();
}
