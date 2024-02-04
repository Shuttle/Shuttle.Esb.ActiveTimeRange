using System;
using System.Threading;
using System.Threading.Tasks;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Threading;

namespace Shuttle.Esb.ActiveTimeRange
{
	internal class ActiveTimeRangeObserver : IPipelineObserver<OnPipelineStarting>
	{
		private readonly ActiveTimeRange _activeTimeRange;
		private readonly CancellationToken _cancellationToken;

		public ActiveTimeRangeObserver(ActiveTimeRange activeTimeRange, CancellationToken cancellationToken)
		{
			_activeTimeRange = Guard.AgainstNull(activeTimeRange, nameof(activeTimeRange));
			_cancellationToken = cancellationToken;
		}

		public void Execute(OnPipelineStarting pipelineEvent)
		{
			ExecuteAsync(pipelineEvent).GetAwaiter().GetResult();
		}

		public async Task ExecuteAsync(OnPipelineStarting pipelineEvent)
		{
			const int sleep = 15000;

			if (_activeTimeRange.Active())
			{
				return;
			}

			pipelineEvent.Pipeline.Abort();

			try
			{
				await Task.Delay(sleep, _cancellationToken);
			}
			catch (OperationCanceledException)
			{
			}
		}
    }
}