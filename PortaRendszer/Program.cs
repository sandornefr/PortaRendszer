using Microsoft.EntityFrameworkCore;
using PortaRendszer.Models;

namespace PortaRendszer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Kapcsolati karakterl�nc lek�r�se
            var connectionString = builder.Configuration.GetConnectionString("MySql");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("A 'MySql' kapcsolati karakterl�nc nem tal�lhat�.");
            }

            // DbContext regisztr�l�sa
            builder.Services.AddDbContext<PortarendszerContext>(options =>
                options.UseMySQL(connectionString));

            // CORS enged�lyez�se
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // Alap szolg�ltat�sok
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Fejleszt�si k�rnyezetben Swagger haszn�lata
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // CORS haszn�lata (enged�lyez�s a k�r�sekhez)
            app.UseCors();

            app.UseAuthorization();

            // Vez�rl�k felt�rk�pez�se
            app.MapControllers();

            app.Run();
        }
    }
}
