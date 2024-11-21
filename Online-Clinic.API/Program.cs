using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using My_Login_App.API.Auth;
using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Middlewares;
using Online_Clinic.API.Models;
using Online_Clinic.API.Profiles;
using Online_Clinic.API.Repositories.Oracle;
using Online_Clinic.API.Services;
using System.Text;

namespace Online_Clinic.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Change the class objects when switching to different repository
            builder.Services.AddScoped<IJwtManager, JwtManager>();
            builder.Services.AddScoped<IAccountRepository, PKG_ACCOUNTS>();
            builder.Services.AddScoped<IDoctorRepository, PKG_DOCTORS>();
            builder.Services.AddScoped<IPatientRepository, PKG_PATIENTS>();
            builder.Services.AddScoped<IReservationRepository, PKG_RESERVATIONS>();
            builder.Services.AddScoped<IEmailConfirmationRepository, PKG_EMAIL_CONFIRMATION>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ICVService, CVService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllCors", config =>
                {
                    config.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();

                });
            });

            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
            });

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            builder.Services.AddResponseCaching();

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAllCors");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
