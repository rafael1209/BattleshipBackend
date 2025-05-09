using BattleshipBackend.Databases;
using BattleshipBackend.Enums;
using BattleshipBackend.Interfaces;
using BattleshipBackend.Repositories;
using BattleshipBackend.Services;
using BattleshipBackend.Services.Auth;
using Microsoft.OpenApi.Models;
using x3rt.DiscordOAuth2;
namespace BattleshipBackend;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "SpCharity API", Version = "v1" });
            c.AddSecurityDefinition(
                "ApiKey",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description =
                        "API Key needed to access the endpoints. API Key must be in the 'Authorization' header. For public endpoints you can didn't use API Key, or just type '0'",
                    Name = "Authorization"
                }
            );
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" } }, [] }
            });
        });

        services.AddHttpContextAccessor();
        services.AddMemoryCache();

        services.AddSingleton<IMongoDbContext, MongoDbContext>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddKeyedScoped<IOAuthProvider, GoogleAuthService>(AuthStrategies.Google);
        services.AddKeyedScoped<IOAuthProvider, DiscordAuthService>(AuthStrategies.Discord);
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.WithMethods("POST", "GET", "PATCH", "PUT")
                    .AllowAnyOrigin()
                .AllowAnyHeader();
            });
        });

        DiscordOAuth.Configure(
            ulong.Parse(configuration["Discord:ClientId"] ?? throw new Exception("Discord ClientId is missing in the configuration.")),
            configuration["Discord:ClientSecret"] ?? throw new Exception("Discord ClientSecret is missing in the configuration.")
        );
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseHsts();
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors("AllowAll");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute().AllowAnonymous();
            endpoints.MapSwagger();
            endpoints.MapControllers().AllowAnonymous();
        });
    }
}