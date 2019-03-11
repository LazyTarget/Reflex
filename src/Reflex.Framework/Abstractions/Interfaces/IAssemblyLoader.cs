using System.Reflection;

namespace Reflex.Framework.Abstractions.Interfaces
{
	public interface IAssemblyLoader
	{
		Assembly Load(string filePath);
	}
}