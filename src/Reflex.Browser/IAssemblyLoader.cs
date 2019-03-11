using System.Reflection;

namespace Reflex.Browser
{
	public interface IAssemblyLoader
	{
		Assembly Load(string filePath);
	}
}