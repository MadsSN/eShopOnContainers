using Autofac.Extensions.DependencyInjection;
using Template1.API.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF;
using Microsoft.eShopOnContainers.Services.Template1.API;
using Microsoft.eShopOnContainers.Services.Template1.API.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Reflection;

namespace Template1.FunctionalTests
{
    public class Template1ScenariosBase
    {
        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(Template1ScenariosBase))
              .Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables();
                })
                .UseStartup<Startup>();


            var testServer = new TestServer(hostBuilder);

            testServer.Host
                .MigrateDbContext<Template1Context>((context, services) =>
                {
                    var env = services.GetService<IWebHostEnvironment>();
                    var settings = services.GetService<IOptions<Template1Settings>>();
                    var logger = services.GetService<ILogger<Template1ContextSeed>>();

                    new Template1ContextSeed()
                    .SeedAsync(context, env, settings, logger)
                    .Wait();
                })
                .MigrateDbContext<IntegrationEventLogContext>((_, __) => { });

            return testServer;
        }

        public static class Get
        {
            private const int PageIndex = 0;
            private const int PageCount = 4;

            public static string Items(bool paginated = false)
            {
                return paginated
                    ? "api/v1/catalog/items" + Paginated(PageIndex, PageCount)
                    : "api/v1/catalog/items";
            }

            public static string ItemById(int id)
            {
                return $"api/v1/catalog/items/{id}";
            }

            public static string ItemByName(string name, bool paginated = false)
            {
                return paginated
                    ? $"api/v1/catalog/items/withname/{name}" + Paginated(PageIndex, PageCount)
                    : $"api/v1/catalog/items/withname/{name}";
            }

            public static string Types = "api/v1/catalog/catalogtypes";

            public static string Brands = "api/v1/catalog/catalogbrands";

            public static string Filtered(int catalogTypeId, int catalogBrandId, bool paginated = false)
            {
                return paginated
                    ? $"api/v1/catalog/items/type/{catalogTypeId}/brand/{catalogBrandId}" + Paginated(PageIndex, PageCount)
                    : $"api/v1/catalog/items/type/{catalogTypeId}/brand/{catalogBrandId}";
            }

            private static string Paginated(int pageIndex, int pageCount)
            {
                return $"?pageIndex={pageIndex}&pageSize={pageCount}";
            }
        }
    }
}
