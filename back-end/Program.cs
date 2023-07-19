using back_end.Controllers;
using back_end.Filtros;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using System.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using back_end.Utilidades;

namespace back_end
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

            /*
            builder.Services.AddCors(options =>
            {
                var frontendURL = builder.Configuration.GetValue<string>("frontend_url");

                //Metodo para mandar a llamar de otra forma sin usar <string>
                //var ffrontendURL = builder.Configuration["frontend_url"];
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader()
                    .WithExposedHeaders(new string[] { "cantidadTotalRegistros" });
                });
            });
            */

            
            builder.Services.AddCors(o => o.AddPolicy("corsApp", builder =>
            {
                builder.WithOrigins("*")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyHeader()
                .WithExposedHeaders(new string[] { "cantidadTotalRegistros" });
            }));
            
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

            //cors
            app.UseCors("corsApp");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}