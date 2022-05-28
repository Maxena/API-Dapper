using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using API.Data;
using API.Interfaces;
using API.Repo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API.Services;

public static class ServiceCollection
{
    public static IServiceCollection AddInfraStructure(this IServiceCollection services, IConfiguration configuration)
    {
        const string anyOriginPolicy = "AnyOriginPolicy";
        services.AddCors(options =>
            options.AddPolicy(anyOriginPolicy, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed(x => true);
            }));

        services.AddResponseCaching();

        services.Configure<GzipCompressionProviderOptions>(option => option.Level = CompressionLevel.Fastest);

        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
            options.Providers.Add<BrotliCompressionProvider>();
            options.MimeTypes =
                ResponseCompressionDefaults.MimeTypes.Concat(
                    new[]
                    {
                        "image/svg+xml",
                        "application/json",
                        "application/xml",
                        "text/css",
                        "text/json",
                        "text/plain",
                        "text/xml",
                        "application/vnd.android.package-archive"
                    });
        });

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                        expires > DateTime.UtcNow,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateActor = true,
                    ValidIssuer = configuration["JwtSetting:Issuer"],
                    ValidAudience = configuration["JwtSetting:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSetting:Key"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddControllers()
            .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddEndpointsApiExplorer();

        services.AddSingleton<ApplicationDbContext>();

        services.AddScoped<IRepository, Repository>();

        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = new HeaderApiVersionReader("api-version");
        });

        services.AddMvc();

        #region Swagger

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Dapper-API",
                Version = "v1",
                Description = "Task To build API with Dapper And Swagger in ASP.NET Core 6.0",
                TermsOfService = new Uri("https://amirhossein.com/terms-of-service"),
                Contact = new OpenApiContact
                {
                    Name = "Amirhossein Sami",
                    Email = "amirhossein.sami1275@gmail.com",
                    Url = new Uri("https://amirhossein.com/")
                },
                License = new OpenApiLicense
                {
                    Name = "Use under LICX",
                    Url = new Uri("https://amirhossein.com/license")
                }
            });

            c.OrderActionsBy(apiDesc =>
                $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.RelativePath}_{apiDesc.HttpMethod}");

            var securityScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Description =
                    "JWT Authorization header using the Bearer scheme." +
                    "\r\n\r\n" +
                    "Enter TOKEN in the text input below." +
                    "\r\n\r\n" +
                    "Example: 'a1.b2.c3'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            };

            c.AddSecurityDefinition("Bearer", securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    securityScheme, new[]
                    {
                        "Bearer"
                    }
                }
            };

            c.AddSecurityRequirement(securityRequirement);

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            c.IncludeXmlComments(xmlPath);

            c.UseInlineDefinitionsForEnums();
        });

        #endregion
        

        return services;
    }
}