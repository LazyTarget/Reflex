using System;
using System.Collections.Generic;
using System.Text;

namespace Reflex.Framework.Abstractions.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class CommandAttribute : Attribute
	{
		public CommandAttribute(string alias)
		{

		}

		public string Alias { get; set; }
	}
}
