using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Pegazus.Core;
using Pegazus.Core.Interfaces;
using Pegazus.Domain.Models;
using Pegazus.Logic;
using Pegazus.Logic.Interfaces;
using Pegazus.Repository;
using Pegazus.Repository.Interfaces;

namespace Pegazus.API
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
            // Inject auto mapper. See cref https://code-maze.com/automapper-net-core/
            // Should make 1 line per assembly where automapper profiles are present.
            services.AddAutoMapper(
                Assembly.GetAssembly(typeof(Startup)),
                Assembly.GetAssembly(typeof(CustomerLogic)));

            // Dependency Injection for the CommandCenter.Repository project.
            // See cref https://www.tutorialsteacher.com/core/dependency-injection-in-aspnet-core
            RegisterRepositoryProviders(services);

            // Dependency Injection for the CommandCenter.Repository project.
            // See cref https://www.tutorialsteacher.com/core/dependency-injection-in-aspnet-core
            RegisterLogicProviders(services);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pegazus.API", Version = "v1" });
            });
        }

        private void RegisterLogicProviders(IServiceCollection services)
        {
            services.AddScoped<ICustomerLogic, CustomerLogic>();
        }

        private void RegisterRepositoryProviders(IServiceCollection services)
        {
            services.AddDbContext<PegazusContext>(option => {
                option.UseSqlServer(Configuration["database:connection"]);
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IUnitOfWork<PegazusContext>, UnitOfWork<PegazusContext>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("./v1/swagger.json", "Pegazus.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }
    }
}
