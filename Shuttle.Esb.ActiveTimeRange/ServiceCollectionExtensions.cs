using System;
using Microsoft.Extensions.DependencyInjection;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.ActiveTimeRange
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddActiveTimeRange(this IServiceCollection services,
            Action<ActiveTimeRangeBuilder> builder = null)
        {
            Guard.AgainstNull(services, nameof(services));

            var activeTimeRangeBuilder = new ActiveTimeRangeBuilder(services);

            builder?.Invoke(activeTimeRangeBuilder);

            services.AddOptions<ActiveTimeRangeOptions>().Configure(options =>
            {
                options.ActiveFromTime = activeTimeRangeBuilder.Options.ActiveFromTime;
                options.ActiveToTime = activeTimeRangeBuilder.Options.ActiveToTime;
            });

            services.AddHostedService<ActiveTimeRangeHostedService>();

            return services;
        }
    }
}