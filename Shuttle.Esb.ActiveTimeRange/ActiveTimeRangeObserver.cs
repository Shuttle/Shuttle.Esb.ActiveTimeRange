using System;
using System.Threading;
using System.Threading.Tasks;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;

namespace Shuttle.Esb.ActiveTimeRange;

internal class ActiveTimeRangeObserver : IPipelineObserver<OnPipelineStarting>
{
    private readonly ActiveTimeRange _activeTimeRange;
    private readonly CancellationToken _cancellationToken;

    public ActiveTimeRangeObserver(ActiveTimeRange activeTimeRange, CancellationToken cancellationToken)
    {
        _activeTimeRange = Guard.AgainstNull(activeTimeRange);
        _cancellationToken = cancellationToken;
    }

    public async Task ExecuteAsync(IPipelineContext<OnPipelineStarting> pipelineContext)
    {
        const int sleep = 15000;

        if (_activeTimeRange.Active())
        {
            return;
        }

        pipelineContext.Pipeline.Abort();

        try
        {
            await Task.Delay(sleep, _cancellationToken);
        }
        catch (OperationCanceledException)
        {
        }
    }
}