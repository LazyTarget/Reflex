﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reflex.Framework.Commands.LoadInstantiateInvoke;

namespace Reflex.Framework.Registry
{
	public class CommandRegistry
	{
		public CommandRegistry()
		{
			
		}

		public void Register(IServiceCollection services, IConfiguration configuration)
		{
			services.AddTransient<LoadInstantiateInvokeCommand>();
			services.Configure<LoadInstantiateInvokeOptions>(configuration);
		}
	}
}
