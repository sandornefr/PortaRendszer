using Microsoft.EntityFrameworkCore;
using PortaRendszer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PortaRendszer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Kapcsolati karakterlánc lekérése
            var connectionString = builder.Configuration.GetConnectionString("MySql");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("A 'MySql' kapcsolati karakterlánc nem található.");
            }

            // DbContext regisztrálása
            builder.Services.AddDbContext<PortarendszerContext>(options =>
                options.UseMySQL(connectionString));

            // CORS engedélyezése
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // JWT beállítások
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            // Alap szolgáltatások
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Fejlesztési környezetben Swagger használata
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // CORS használata (engedélyezés a kérésekhez)
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            // Vezérlõk feltérképezése
            app.MapControllers();

            app.Run();
        }
    }
}
