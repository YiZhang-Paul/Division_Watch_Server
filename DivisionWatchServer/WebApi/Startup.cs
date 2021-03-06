using Core.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Repositories;
using Service.Services;

namespace WebApi
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
            services.AddCors(options =>
            {
                options.AddPolicy("pom-cors", _ => _.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod());
            });

            services.AddControllers();
            services.AddSingleton<AppSettingsService, AppSettingsService>();
            services.AddSingleton<DailyPlanService, DailyPlanService>();
            services.AddSingleton<CategoryService, CategoryService>();
            services.AddSingleton<TaskItemService, TaskItemService>();
            services.AddScoped<AppSettingsRepository, AppSettingsRepository>();
            services.AddScoped<DailyPlanRepository, DailyPlanRepository>();
            services.AddScoped<CategoryRepository, CategoryRepository>();
            services.AddScoped<TaskItemRepository, TaskItemRepository>();
            services.Configure<DatabaseConfiguration>(Configuration.GetSection(DatabaseConfiguration.Key));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("pom-cors");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
