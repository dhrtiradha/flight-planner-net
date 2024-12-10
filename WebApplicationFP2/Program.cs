
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using WebApplicationFP2.Database;
using WebApplicationFP2.Handler;
using WebApplicationFP2.Storage;

namespace WebApplicationFP2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            builder.Services.AddDbContext<FlightPlannerDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("flight-planner")));
            builder.Services.AddScoped<FlightStorage>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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
