using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Threading;

namespace Shuttle.Esb.ActiveTimeRange;

public class ActiveTimeRangeHostedService : IHostedService
{
    private readonly ActiveTimeRange _activeTimeRange;
    private readonly ICancellationTokenSource _cancellationTokenSource;
    private readonly IPipelineFactory _pipelineFactory;
    private readonly Type _shutdownPipelineType = typeof(ShutdownPipeline);
    private readonly Type _startupPipelineType = typeof(StartupPipeline);

    public ActiveTimeRangeHostedService(IOptions<ActiveTimeRangeOptions> activeTimeRangeOptions, IPipelineFactory pipelineFactory, ICancellationTokenSource cancellationTokenSource)
    {
        Guard.AgainstNull(Guard.AgainstNull(activeTimeRangeOptions).Value);

        _pipelineFactory = Guard.AgainstNull(pipelineFactory);
        _cancellationTokenSource = Guard.AgainstNull(cancellationTokenSource);

        _activeTimeRange = new(activeTimeRangeOptions.Value.ActiveFromTime, activeTimeRangeOptions.Value.ActiveToTime);

        pipelineFactory.PipelineCreated += OnPipelineCreated;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _pipelineFactory.PipelineCreated -= OnPipelineCreated;

        await Task.CompletedTask;
    }

    private void OnPipelineCreated(object? sender, PipelineEventArgs e)
    {
        var pipelineType = e.Pipeline.GetType();

        if (pipelineType == _startupPipelineType
            ||
            pipelineType == _shutdownPipelineType)
        {
            return;
        }

        e.Pipeline.AddObserver(new ActiveTimeRangeObserver(_activeTimeRange, _cancellationTokenSource.Get().Token));
    }
}