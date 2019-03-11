using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reflex.Framework.Abstractions.Attributes;

namespace Reflex.Framework.Commands.LoadInstantiateInvoke
{
	[Command("lii")]
	[Command("LoadInstantiateInvoke")]
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
