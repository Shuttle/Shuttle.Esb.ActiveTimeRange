using Microsoft.Extensions.DependencyInjection;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.ActiveTimeRange;

public class ActiveTimeRangeBuilder
{
    private ActiveTimeRangeOptions _activeTimeRangeOptions = new();

    public ActiveTimeRangeBuilder(IServiceCollection services)
    {
        Services = Guard.AgainstNull(services);
    }

    public ActiveTimeRangeOptions Options
    {
        get => _activeTimeRangeOptions;
        set => _activeTimeRangeOptions = Guard.AgainstNull(value);
    }

    public IServiceCollection Services { get; }
}