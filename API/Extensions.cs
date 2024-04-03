using API.Permissions;
using Application.AppConfigs;
using Application.Features.Identity.Queries;
using Application.Services.Employees;
using Application.Services.Identity;
using Common.Authorization;
using Common.Responses.Wrappers;
using Infrastructure.Context;
using Infrastructure.Models;
using Infrastructure.Services.Employees;
using Infrastructure.Services.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace API
{
	public static class Extensions
	{
		public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
		{
			using var scope = app.ApplicationServices.CreateScope();
			var seeder = scope.ServiceProvider.GetService<ApplicationDbSeeder>();
			seeder.SeedDatabaseAsync().GetAwaiter().GetResult(); // to wait for the seeding to finish
			return app;
		}
		public static IServiceCollection AddIdentitySettings(this IServiceCollection services)
		{
			services
				.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
				.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>()
				.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
			{
				opt.Password.RequiredLength = 6;
				opt.Password.RequireDigit = false;
				opt.Password.RequireLowercase = false;
				opt.Password.RequireUppercase = false;
				opt.Password.RequireNonAlphanumeric = false;
				opt.User.RequireUniqueEmail = true;
			})
			.AddEntityFrameworkStores<ApplicationDbContext>();
			return services;
		}
		public static AppConfiguration AddMyOptions(this IServiceCollection services, IConfiguration configuration)
		{
			var applicationSettingConfig = configuration.GetSection("JWT");
			services.Configure<AppConfiguration>(applicationSettingConfig);
			return applicationSettingConfig.Get<AppConfiguration>();
		}
		public static IServiceCollection AddJwtAuth(this IServiceCollection services, AppConfiguration config)
		{
			services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(opt =>
			{
				opt.RequireHttpsMetadata = false;
				opt.SaveToken = true;
				opt.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret)),
					ValidateIssuer = false,
					ValidateAudience = false,
					RoleClaimType = ClaimTypes.Role,
					ClockSkew = TimeSpan.Zero
				};
				opt.Events = new JwtBearerEvents
				{
					OnAuthenticationFailed = ctx =>
					{
						if (ctx.Exception is SecurityTokenExpiredException)
						{
							ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
							ctx.Response.ContentType = "application/json";
							var HandledResult = JsonSerializer.Serialize(ResponseWrapper<object>.Fail("Token is Expired"));
							return ctx.Response.WriteAsync(HandledResult);
						}
						ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
						ctx.Response.ContentType = "application/json";
						var result = JsonSerializer.Serialize(ResponseWrapper<object>.Fail("An Unhandled Exception Occured"));
						return ctx.Response.WriteAsync(result);
					},
					OnChallenge = ctx =>
					{ // excuted if the authentication is challenged (not authorized)
						ctx.HandleResponse();
						if (!ctx.Response.HasStarted)
						{
							ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
							ctx.Response.ContentType = "application/json";
							var result = JsonSerializer.Serialize(ResponseWrapper<object>.Fail("You are not Authorized"));
							return ctx.Response.WriteAsync(result);
						}
						return Task.CompletedTask;
					},
					OnForbidden = ctx =>
					{
						ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
						ctx.Response.ContentType = "application/json";	
						var result = JsonSerializer.Serialize(ResponseWrapper<object>.Fail("You are not Authorized to access this resource"));
						return ctx.Response.WriteAsync(result);
					}
				};
			});

			services.AddAuthorization(opt =>
			{
				foreach (var prop in typeof(AppPermissions).GetNestedTypes()
				.SelectMany(c =>
				c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
				{
					var propertyValue = prop.GetValue(null);
					if (propertyValue is not null)
					{
						opt.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(AppClaim.Permission, propertyValue.ToString()));
					}
				} // add all our authorization policies dynamically
			});
			return services;
		}
		public static IServiceCollection AddJWTSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "ASP_Security", Version = "v1" });
				options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
				{
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = JwtBearerDefaults.AuthenticationScheme,
					BearerFormat = "JWT",
					Description = "Input your Bearer Token in this format => Bearer {your token} to access this API"
				});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = JwtBearerDefaults.AuthenticationScheme
							},
							Scheme = "Oauth2",
							Name = JwtBearerDefaults.AuthenticationScheme,
							In = ParameterLocation.Header
						},new List<string>()
					}
				});
			});
			return services;

		}
		public static IServiceCollection AddAppServices(this IServiceCollection services)
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(GetTokenQueryHandler))))
			.AddAutoMapper(new[] { typeof(GetRefreshTokenQuery).Assembly, typeof(EmployeeRepository).Assembly });
			services.AddScoped<IEmployeeRepository, EmployeeRepository>()
					.AddScoped<ICurrentUserRepository, CurrentUserRepository>()
					.AddScoped<IRoleRepository, RoleRepository>()
					.AddHttpContextAccessor();
			return services;
		}
	}
}
