using Hymson.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Hymson.MES.SignalRHub.Extensions.DependencyInjection
{
    public static class AuthServiceExtensions
    {
        public static IServiceCollection AddCostomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtOptions jwtOptions = new JwtOptions();
            configuration.GetSection("JwtOptions").Bind(jwtOptions);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(delegate (JwtBearerOptions options)
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = false,
                    ValidAudience = jwtOptions.Audience,
                    //ValidateLifetime = jwtOptions.ValidateLifetime,
                    RequireExpirationTime = jwtOptions.RequireExpirationTime,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SymmetricSecurityKeyString))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/api2"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            return services;
        }

    }
}
