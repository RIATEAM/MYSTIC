package scripts.Model
{
	import scripts.Classes.PageDTO;

	[Bindable]
	public class Model
	{

		public var SelectedPageIndex:int;
		public var selectedPage:PageDTO;
		public var treeXML:XML;
		public var selectedNode:XML;
	}
}