using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OmniKassa.Samples.DotNet50.Configuration;

namespace example_dotnet50
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConfigurationParameters = InitializeConfigurationParameters(configuration);
        }

        public IConfiguration Configuration { get; }
        
        public ConfigurationParameters ConfigurationParameters { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton(ConfigurationParameters);
        }
        
        private static ConfigurationParameters InitializeConfigurationParameters(IConfiguration configuration)
        {
            var refreshToken = configuration.GetValue<string>("RefreshToken");
            var signingKey = configuration.GetValue<string>("SigningKey");
            var callbackUrl = configuration.GetValue<string>("CallbackUrl", "http://localhost:52060/Home/Callback/");
            var baseUrl = configuration.GetValue<string>("BaseUrl");

            return new ConfigurationParameters(refreshToken, signingKey, callbackUrl, baseUrl);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
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
