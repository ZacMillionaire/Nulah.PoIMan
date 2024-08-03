using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Nulah.PoIMan.Api.Middleware;
using Nulah.PoIMan.Api.Models;
using Nulah.PoIMan.Data;
using Nulah.PoIMan.Domain.Interfaces;

namespace Nulah.PoIMan.Api;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddScoped<IFeatureRepository, FeatureRepository>();
		builder.Services.AddScoped<IUserRepository, UserRepository>();
		
		builder.Configuration.AddEnvironmentVariables(prefix: "NulahPoIMan_");
		builder.Services.Configure<AppSettings>(builder.Configuration);

		builder.Services.AddDbContext<PoIManDbContext>((serviceProvider, dbContextOptions) =>
			{
				var appsettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>();
				//docker run --name some-postgis -p 55432:5432 -e POSTGRES_PASSWORD=mysecretpassword -d postgis/postgis
				dbContextOptions.UseNpgsql(appsettings.Value.ConnectionStrings.Postgres,
					// required to have postgis functionality
					x => x.UseNetTopologySuite());
			}
		);

		ConfigureAuthentication(builder);

		builder.Services.AddControllers(opts =>
			{
				var policy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();
				opts.Filters.Add(new AuthorizeFilter(policy));
			})
			.AddJsonOptions(opts =>
			{
				var enumConverter = new JsonStringEnumConverter();
				opts.JsonSerializerOptions.Converters.Add(enumConverter);
			});

#if DEBUG
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(c =>
		{
			var securityScheme = new OpenApiSecurityScheme
			{
				Name = ApiKeyAuthenticationOptions.PolicyName,
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.Http,
				Scheme = "Bearer",
				Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

				Reference = new OpenApiReference
				{
					Id = ApiKeyAuthenticationOptions.PolicyName,
					Type = ReferenceType.SecurityScheme
				}
			};

			c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

			c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{ securityScheme, Array.Empty<string>() }
			});
		});
#endif

		var app = builder.Build();
		
#if DEBUG
		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}
#endif
		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}

	private static void ConfigureAuthentication(WebApplicationBuilder builder)
	{
		builder.Services.AddAuthentication(ApiKeyAuthenticationHandler.AuthenticationHandlerName)
			.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationHandler.AuthenticationHandlerName, null);

		builder.Services.AddAuthorization(options =>
		{
			options.AddPolicy(ApiKeyAuthenticationOptions.PolicyName, policy =>
			{
				policy.AddAuthenticationSchemes(ApiKeyAuthenticationHandler.AuthenticationHandlerName);
				policy.RequireAuthenticatedUser();
			});
		});
	}
}