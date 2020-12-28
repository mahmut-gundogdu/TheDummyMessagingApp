using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using MessagingService.Core;
using MessagingService.Helpers;
using MessagingService.Services;

namespace MessagingService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<AppSettings>(Configuration);
           
            services.AddTransient(typeof(IRepository<>),typeof(Repository<>));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMessageService,MessageService>();
            services.AddTransient<ISessionManager, SessionManager>();
            
            services.AddSingleton<IJwtManager, JwtManager>();
            services.AddAutoMapper(typeof(Startup));
            services.AddHttpContextAccessor();
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
            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();
            // custom global error handler middleware
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
