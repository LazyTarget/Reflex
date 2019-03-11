namespace Reflex.Framework.Commands.LoadInstantiateInvoke
{
	public class LoadInstantiateInvokeOptions
	{
		public LoadInstantiateInvokeOptions()
		{

		}

		public string FilePath { get; set; } = "";
		public string TypeName { get; set; }
		public string[] Methods { get; set; }
	}
}
