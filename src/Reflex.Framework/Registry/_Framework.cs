using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Reflex.Framework.Registry
{
	public class Framework
	{
		public void Register(IServiceCollection services)
		{
			new CoreRegistry().Register(services);
			new CommandRegistry().Register(services);
		}
	}
}
