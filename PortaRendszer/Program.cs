using Microsoft.EntityFrameworkCore;
using PortaRendszer.Models;

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

            app.UseAuthorization();

            // Vezérlõk feltérképezése
            app.MapControllers();

            app.Run();
        }
    }
}
