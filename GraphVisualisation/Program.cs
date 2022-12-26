namespace GraphVisualisation
{
	internal static class Program
	{
		[STAThread]
		static void Main()
		{
			ApplicationConfiguration.Initialize();
			Application.Run(new GraphVisul(new EdgeEditBox()));
		}
	}
}