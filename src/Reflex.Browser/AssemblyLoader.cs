using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Reflex.Browser
{
	public class AssemblyFileLoader : IAssemblyLoader
	{
		public Assembly Load(string filePath)
		{
			var assembly = Assembly.LoadFile(filePath);
			return assembly;
		}
	}
}
