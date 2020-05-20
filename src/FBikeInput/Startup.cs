using System;
using Microsoft.AspNetCore.Builder;
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
            services.AddSignalR();
            services.AddSingleton<FBikeMonitor>();
            services.AddSingleton<FBikeHub>();
        }

        public void Configure(IApplicationBuilder app, FBikeHub hub, FBikeMonitor monitor)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<FBikeHub>("/fbikeHub");
            });

            monitor.OneRotationDetected += (e, average) =>
            {
                Console.WriteLine($"Rotation detected because average was: {average}");
                hub.RotationDetected(average);
            };

            monitor.Monitor(Configuration.GetValue<int>("DeviceId"));
        }
    }
}