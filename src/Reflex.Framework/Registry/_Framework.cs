using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Reflex.Framework.Registry
{
	public class Framework
	{
		public void Register(IServiceCollection services, IConfiguration configuration)
		{
			new CoreRegistry().Register(services, configuration);
			new CommandRegistry().Register(services, configuration);
		}
	}
}
