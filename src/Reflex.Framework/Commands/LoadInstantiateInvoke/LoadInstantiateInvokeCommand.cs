using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Reflex.Framework.Commands.LoadInstantiateInvoke
{
	public class LoadInstantiateInvokeCommand
	{
		private readonly ILogger<LoadInstantiateInvokeCommand> _logger;
		private readonly LoadInstantiateInvokeOptions _options;

		public LoadInstantiateInvokeCommand(IOptions<LoadInstantiateInvokeOptions> options, ILogger<LoadInstantiateInvokeCommand> logger)
		{
			_logger = logger;
			_options = options?.Value ?? new LoadInstantiateInvokeOptions();
		}

		public void Run()
		{

		}
	}
}
