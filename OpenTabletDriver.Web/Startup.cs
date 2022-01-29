using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octokit;
using OpenTabletDriver.Web.Controllers;
using OpenTabletDriver.Web.Core;
using OpenTabletDriver.Web.Core.Contracts;
using OpenTabletDriver.Web.Core.Framework;
using OpenTabletDriver.Web.Core.GitHub.Services;
using OpenTabletDriver.Web.Core.Plugins;
using OpenTabletDriver.Web.Core.Services;
using ProductHeaderValue = Octokit.ProductHeaderValue;

namespace OpenTabletDriver.Web
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
            services.AddControllersWithViews();

            services.AddSingleton<IRepositoryService, GitHubRepositoryService>()
                .AddSingleton<IReleaseService, GitHubReleaseService>()
                .AddSingleton<IGitHubClient, GitHubClient>(AuthenticateGitHub)
                .AddSingleton<ITabletService, GitHubTabletService>()
                .AddSingleton<IPluginMetadataService, GitHubPluginMetadataService>()
                .AddSingleton<IFrameworkService, DotnetCoreService>();

            services.AddHttpClient<IRepositoryService, GitHubRepositoryService>(SetupHttpClient);

            services.AddMemoryCache();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
            });
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }

        private GitHubClient AuthenticateGitHub(IServiceProvider serviceProvider)
        {
            const string productHeader = "OpenTabletDriver-Web";
            string apiKey = Environment.GetEnvironmentVariable("GITHUB_TOKEN");

            var clientHeader = ProductHeaderValue.Parse(productHeader);
            return new GitHubClient(clientHeader)
            {
                Credentials = string.IsNullOrWhiteSpace(apiKey) ? Credentials.Anonymous : new Credentials(apiKey)
            };
        }

        private void SetupHttpClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("User-Agent", "OpenTabletDriver-Web");
        }
    }
}