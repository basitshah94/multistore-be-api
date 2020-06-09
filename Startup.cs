using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotnet.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.Data.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;
using Newtonsoft.Json.Serialization;
using dotnet.Hubs;
namespace dotnet {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            //token 
            services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer (option => option.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],

                        IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (Configuration["Jwt:key"]))
                });
            services.AddDbContext<Context> (options => options.UseMySQL (Configuration.GetConnectionString ("DefaultConnection")));
            // services.AddDbContext<ApplicationDbContext>();
            services.AddControllers ();
            services.AddSwaggerGen ((options) => {
                options.SwaggerDoc ("v1", new OpenApiInfo { Title = "myApi", Version = "v1" });
            });
            services.AddCors ();
            services.AddSignalR();

            services.AddControllersWithViews ()
                .AddNewtonsoftJson (options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            //  services.AddCors(options => options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().Build()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            //web sockets
            // app.UseWebSockets ();
            // app.Use (async (context, next) => {
            //     if (context.Request.Path == "/ws") {
            //         if (context.WebSockets.IsWebSocketRequest) {
            //             WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync ();
            //             await Echo (context, webSocket);
            //         } else {
            //             context.Response.StatusCode = 400;
            //         }
            //     } else {
            //         await next ();
            //     }

            // });
            
            // end web sockets
            app.UseSwagger ();
            app.UseSwaggerUI (c => {
                c.SwaggerEndpoint ("/swagger/v1/swagger.json", "My API V1");
            });
            // app.UseCors (builder => {
            //     builder.AllowAnyOrigin ().AllowAnyHeader ().AllowAnyMethod ();
                //.AllowCredentials ();     
            // });
            app.UseCors(
               options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
           );
        
            

            app.UseStaticFiles ();
            app.UseStaticFiles (new StaticFileOptions () {
                FileProvider = new PhysicalFileProvider (Path.Combine (Directory.GetCurrentDirectory (), @"Resources")),
                    RequestPath = new PathString ("/Resources")
            });

            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            // app.UseMvc();
            app.UseHttpsRedirection ();

            app.UseRouting ();

            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
                endpoints.MapHub<OrderHub>("/orderHub");
            });
//             app.UseSignalR(routes =>
// {
//     routes.MapHub<OrderHub>("/order");
// });
        }
    }
}