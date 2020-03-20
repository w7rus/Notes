using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace NotesWebAPI.Extensions
{
    public static class AppExtensions
    {
        public static void ConfigureCors(this IApplicationBuilder app)
        {
            app.UseCors("DefaultCorsPolicy");
        }
        public static void ConfigureAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();
        }

        public static void EnsureMigrationOfContext<T>(this IApplicationBuilder app) where T : DbContext
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<T>();

                context.Database.EnsureCreated();
                //context.Database.Migrate();
            }
        }
    }
}
