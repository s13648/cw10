using Cw10.Middlewares;
using Cw10.Model;
using Cw10.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cw10
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
            services.AddTransient<IStudyDbService, StudyDbService>();
            services.AddTransient<IStudentDbService, StudentDbService>();
            services.AddTransient<IEnrollmentDbService, EnrollmentDbService>();

            services.AddTransient<IConfig,Config>(b => new Config  
            {
                ConnectionString = Configuration["DbConnectionString"]
            });

            services.AddDbContext<APBDContext>(options => options.UseSqlServer(Configuration["DbConnectionString"]));
            services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStudentDbService studentDbService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseMiddleware<LoggingMiddleware>();

            app.UseMiddleware<ExceptionMiddleware>();

            /*app.Use(async (context, next) =>
            {
                if (!context.Request.Headers.ContainsKey("Index"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Nie podałeś indeksu");
                    return;
                }

                var index = context.Request.Headers["Index"].ToString();
                var student = await studentDbService.GetByIndex(index);
                if (student == null)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Student not found");
                    return;
                }


                await next();
            });*/

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
