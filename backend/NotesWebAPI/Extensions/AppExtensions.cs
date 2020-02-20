using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
