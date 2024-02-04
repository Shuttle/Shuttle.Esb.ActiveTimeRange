using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Threading;

namespace Shuttle.Esb.ActiveTimeRange
{
    public class ActiveTimeRangeHostedService : IHostedService
    {
        private readonly ICancellationTokenSource _cancellationTokenSource;
        private readonly IPipelineFactory _pipelineFactory;
        private readonly ActiveTimeRange _activeTimeRange;
        private readonly Type _shutdownPipelineType = typeof(ShutdownPipeline);
        private readonly Type _startupPipelineType = typeof(StartupPipeline);

        public ActiveTimeRangeHostedService(IOptions<ActiveTimeRangeOptions> activeTimeRangeOptions, IPipelineFactory pipelineFactory, ICancellationTokenSource cancellationTokenSource)
        {
            Guard.AgainstNull(activeTimeRangeOptions, nameof(activeTimeRangeOptions));
            Guard.AgainstNull(activeTimeRangeOptions.Value, nameof(activeTimeRangeOptions.Value));

            _pipelineFactory = Guard.AgainstNull(pipelineFactory, nameof(pipelineFactory));
            _cancellationTokenSource = Guard.AgainstNull(cancellationTokenSource, nameof(cancellationTokenSource));

            _activeTimeRange = new ActiveTimeRange(activeTimeRangeOptions.Value.ActiveFromTime, activeTimeRangeOptions.Value.ActiveToTime);

            pipelineFactory.PipelineCreated += OnPipelineCreated;
        }

        private void OnPipelineCreated(object sender, PipelineEventArgs e)
        {
            var pipelineType = e.Pipeline.GetType();

            if (pipelineType == _startupPipelineType
                ||
                pipelineType == _shutdownPipelineType)
            {
                return;
            }

            e.Pipeline.RegisterObserver(new ActiveTimeRangeObserver(_activeTimeRange, _cancellationTokenSource.Get().Token));
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
    }
}