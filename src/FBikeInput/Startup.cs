using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FBikeInput
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<FBikeMonitor>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, FBikeMonitor monitor)
        {
            // monitor.SampleProcessed += (e, average) => { Console.WriteLine($"{average}"); };
            monitor.OneRotationDetected += (e, average) => { Console.WriteLine($"Rotate! {average}"); };
            monitor.Monitor(Configuration.GetValue<int>("DeviceId"));

            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}