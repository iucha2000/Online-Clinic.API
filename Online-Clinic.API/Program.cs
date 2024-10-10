using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Middlewares;
using Online_Clinic.API.Models;
using Online_Clinic.API.Repositories.Oracle;

namespace Online_Clinic.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Change the class objects when switching to different repository
            builder.Services.AddScoped<IRepository<Doctor>, PKG_DOCTORS>();
            builder.Services.AddScoped<IRepository<Patient>, PKG_PATIENTS>();
            builder.Services.AddScoped<IRepository<Reservation>, PKG_RESERVATIONS>();

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
