using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;

namespace Shuttle.Esb.ActiveTimeRange
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddActiveTimeRangeModule(this IServiceCollection services,
            Action<ActiveTimeRangeBuilder> builder = null)
        {
            Guard.AgainstNull(services, nameof(services));

            var activeTimeRangeBuilder = new ActiveTimeRangeBuilder(services);

            builder?.Invoke(activeTimeRangeBuilder);

            services.TryAddSingleton<ActiveTimeRangeObserver>();
            
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