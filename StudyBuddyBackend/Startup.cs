using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudyBuddyBackend.Database.Contexts;
using StudyBuddyBackend.Hubs;

namespace StudyBuddyBackend
{
    public class Startup
    {
        private readonly string _connectionString;

        public Startup(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("StudyBuddy");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(_connectionString).UseLazyLoadingProxies());
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                builder.AddUserSecrets<Startup>();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}
