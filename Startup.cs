using Interfaces;
using Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class OperationService
    {

          
        public IOperationTransient Transient { get; }
        public IOperationScoped Scoped { get; }
        public IOperationSingleton Singleton { get; }
        public IOperationSingletonInstance SingletonInstance { get; }
        public OperationService(IOperationTransient OTransient,IOperationScoped OScoped,IOperationSingleton OSingleton, IOperationSingletonInstance OSingletonInstance)
        {
            Transient = OTransient;
            Scoped = OScoped;
            Singleton = OSingleton;
            SingletonInstance = OSingletonInstance;
        }

    }
}

namespace Interfaces
{
    public interface IOperation
    {
        Guid OperationId { get;  }
    }

    public interface IOperationTransient : IOperation
    {
    }

    public interface IOperationScoped: IOperation
    {
    }

    public interface IOperationSingleton : IOperation
    {
    }

    public interface IOperationSingletonInstance: IOperation
    {
    }

    public class Operation : IOperationTransient, IOperationScoped, IOperationSingleton, IOperationSingletonInstance
    {
        public Operation()
        {
            OperationId = Guid.NewGuid();
        }
        public Operation(Guid ParameterGuid)
        {
            OperationId = ParameterGuid;
        }
        public Guid OperationId { get; }
    }
}

namespace WebApplication3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddTransient<IOperationTransient, Operation>();
            services.AddScoped<IOperationScoped, Operation>();
            services.AddSingleton<IOperationSingleton, Operation>();
            services.AddSingleton<IOperationSingletonInstance>(new Operation(Guid.Empty));
            services.AddTransient<OperationService, OperationService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
