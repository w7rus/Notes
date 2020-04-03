using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Notes.Logic.Data;
using Notes.Logic.Repositories.Attachment;
using Notes.Logic.Repositories.Attachment.Implementation;
using Notes.Logic.Repositories.Notes;
using Notes.Logic.Repositories.Notes.Implementation;
using Notes.Logic.Repositories.Shares;
using Notes.Logic.Repositories.Shares.Implementation;
using Notes.Logic.Repositories.Users;
using Notes.Logic.Repositories.Users.Implementation;
using Notes.Logic.Services.Attachments;
using Notes.Logic.Services.Attachments.Implementation;
using Notes.Logic.Services.Notes;
using Notes.Logic.Services.Notes.Implementation;
using Notes.Logic.Services.Shares;
using Notes.Logic.Services.Shares.Implementation;
using Notes.Logic.Services.Users;
using Notes.Logic.Services.Users.Implementation;
using NotesWebAPI.Extensions;

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

            services.ConfigureAuthentication(Configuration);
            services.ConfigureCors();
            services.ConfigureControllers();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<INotesService, NotesService>();
            services.AddScoped<INotesRepository, NotesRepository>();
            services.AddScoped<ISharesService, SharesService>();
            services.AddScoped<ISharesRepository, SharesRepository>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseAuthorization();

            app.ConfigureCors();

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });

            app.ConfigureAuthentication();

            app.EnsureMigrationOfContext<NotesWebAPIContext>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}