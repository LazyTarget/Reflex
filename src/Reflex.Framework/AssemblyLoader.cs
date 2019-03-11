using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Reflex.Framework.Abstractions.Interfaces;

namespace Reflex.Framework
{
	public class AssemblyFileLoader : IAssemblyLoader
	{
		private readonly AppDomain _domain;

		public AssemblyFileLoader(AppDomain domain)
		{
			_domain = domain;
		}


		public Assembly Load(string filePath)
		{
			Assembly assembly;
			if (_domain == null)
			{
				//var assembly = Assembly.LoadFile(filePath);
				assembly = Assembly.LoadFrom(filePath);
			}
			else
			{
				var assemblyName = AssemblyName.GetAssemblyName(filePath);
				assembly = _domain.Load(assemblyName);
			}
			return assembly;
		}
	}
}
