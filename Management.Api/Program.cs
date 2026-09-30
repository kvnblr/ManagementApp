using Management.Api.Extensions;
using Management.Contracts;
using Management.Presentation.ActionFilters;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.ConfigureAutoMapping();
builder.Services.ConfigureCors();
builder.Services.ConfigureLogging();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureDataShaper();
builder.Services.ConfigureEmployeeLinks();
builder.Services.ConfigureVersioning();
// builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureOutputCaching();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureJwtSettings(builder.Configuration);
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.ConfigureControllers();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });
app.UseRateLimiter();
app.UseCors("CorsPolicy");
// app.UseResponseCaching();
app.UseOutputCache();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

