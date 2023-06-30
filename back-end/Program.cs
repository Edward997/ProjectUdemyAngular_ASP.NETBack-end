using back_end.Controllers;
using back_end.Filtros;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using System.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

namespace back_end
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));


            builder.Services.AddCors(options =>
            {
                var frontendURL = builder.Configuration.GetValue<string>("frontend_url");
                
                    //Metodo para mandar a llamar de otra forma sin usar <string>
                //var ffrontendURL = builder.Configuration["frontend_url"];
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader();
                });
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(MiFiltroDeAccion));
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            /*
            app.Use(async (context, next) =>
            {
                ILogger<Program> logger = context.RequestServices.GetService<ILogger<Program>>();

                using (var swapStream = new MemoryStream())
                {

                    var respuestaOriginal = context.Response.Body;
                    context.Response.Body = swapStream;

                    await next.Invoke();

                    swapStream.Seek(0, SeekOrigin.Begin);
                    string respuesta = new StreamReader(swapStream).ReadToEnd();
                    swapStream.Seek(0, SeekOrigin.Begin);

                    await swapStream.CopyToAsync(respuestaOriginal);
                    context.Response.Body = respuestaOriginal;

                    logger.LogInformation(respuesta);

                }
            }); */
             
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}