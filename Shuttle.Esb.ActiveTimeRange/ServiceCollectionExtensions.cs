using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.ActiveTimeRange;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddActiveTimeRange(this IServiceCollection services, Action<ActiveTimeRangeBuilder>? builder = null)
    {
        var activeTimeRangeBuilder = new ActiveTimeRangeBuilder(Guard.AgainstNull(services));

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