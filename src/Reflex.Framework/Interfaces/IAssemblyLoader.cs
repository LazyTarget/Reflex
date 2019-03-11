using System.Reflection;

namespace Reflex.Framework.Interfaces
{
	public interface IAssemblyLoader
	{
		Assembly Load(string filePath);
	}
}