using AutoMapper;
using FreeResponse.Application.MapperConfiguration;
using FreeResponse.Application.Projects;
using FreeResponse.Application.SdlcSystems;
using FreeResponse.EntityFrameworkCore.EntityFrameworkCore;
using FreeResponse.EntityFrameworkCore.EntityFrameworkCore.Seed;
using FreeResponse.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace FreeResponse.Web.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        
            services.AddMvc();

            var conn = Configuration["connectionStrings:sqlConnection"];
            services.AddDbContext<FreeResponseDbContext>(options => options
                    .UseSqlServer(conn, b => b.MigrationsAssembly("FreeResponse.EntityFrameworkCore")));

            services.AddScoped(typeof(ISdlcSystemAppService), typeof(SdlcSystemAppService));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IProjectAppService), typeof(ProjectAppService));


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeResponse API", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeResponse API V1");
                c.RoutePrefix = string.Empty;
            });

            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var dbContext = serviceScope.ServiceProvider.GetService<FreeResponseDbContext>();
            SeedHelper.SeedHostDb(dbContext);
        }
    }
}
