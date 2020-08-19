using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using DatingApp.API.Helpers;
using AutoMapper;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //dependency injection container: iniettiamo il servizio nell'applicazione
        public void ConfigureServices(IServiceCollection services)
        {
            //aggiunta della stringa di connessione del db Sqlite
            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers().AddNewtonsoftJson(opt => // per la lettura dei json
            {
                opt.SerializerSettings.ReferenceLoopHandling =  // risolve l'errore: Self referencing loop detected for property 'user'
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddCors(); //permette di risolvere l'errore "blocked by CORS policy"
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings")); // recupera impostazioni cloudinary da appsettings.json
            services.AddAutoMapper(typeof(DatingRepository).Assembly); // permette l'uso di automapper

            services.AddTransient<Seed>();  // riferimento a classe seed.cs
            services.AddScoped<IAuthRepository, AuthRepository>();   //il servizio viene creato una volta per ogni richiesta, crea un istanza per ogni richesta http ma usa la stessa istanza all'interno della richiesta
            services.AddScoped<IDatingRepository, DatingRepository>(); 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                            ValidateIssuer = false,
                            ValidateAudience = false
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        { 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;  // recupera il codice dell'errore 500 (internal)

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);          //aggiunge un nuovo header alla risposta
                            await context.Response.WriteAsync(error.Error.Message);             //visualizza la descrizione dell'errore
                        }
                    });
                }); //aggiunge un middleware alla pipeline che cattura le exception...
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            //evitare errori CORS: permettiamo ogni origine, ogni metodo e ogni header (per ora mina la sicurezza)
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            //sostituisce UseMvc()
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
