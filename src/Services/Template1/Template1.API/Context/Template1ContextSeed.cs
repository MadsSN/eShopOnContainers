using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.eShopOnContainers.Services.Template1.API;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using Template1.API.Infrastructure;

namespace Template1.API.Context
{
    using Model;
    public class Template1ContextSeed
    {
        public async Task SeedAsync(Template1Context context, IWebHostEnvironment env, IOptions<Template1Settings> settings, ILogger<Template1ContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(Template1ContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!context.Template1s.Any())
                {
                    await context.Template1s.AddRangeAsync(GetPreconfiguredTemplate1());
                    await context.SaveChangesAsync();
                }

            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<Template1ContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }

        private IEnumerable<Template1> GetPreconfiguredTemplate1()
        {
            return new List<Template1>()
            {
                new Template1() { Name = "Mads Skytte Nielsen"},
                new Template1() { Name = "Jesper Strøm" },
                new Template1() { Name = "Simon Wessmann" },
            };
        }

    }
}
