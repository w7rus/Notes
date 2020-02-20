using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotesWebAPI.Data;
using NotesWebAPI.Extensions;
using NotesWebAPI.Repository.Users;
using NotesWebAPI.Repository.Users.Implementation;
using NotesWebAPI.Services.Users;
using NotesWebAPI.Services.Users.Implementation;

namespace NotesWebAPI
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
            services.AddDbContext<NotesWebAPIContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddScoped<>
            
            services.ConfigureAuthentication();
            services.ConfigureCors();
            services.ConfigureControllers();
            services.AddScoped<IUsersService, CUsersService>();
            services.AddScoped<IUsersRepository, CUsersRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseAuthorization();

            app.ConfigureCors();
            app.ConfigureAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}