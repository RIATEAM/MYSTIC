package views.menu

{

	[Event(name="change", type="mx.events.MenuEvent")]
	[Event(name="itemClick", type="mx.events.MenuEvent")]
	[Event(name="menuHide", type="mx.events.MenuEvent")]
	[Event(name="menuShow", type="mx.events.MenuEvent")]
	[Event(name="itemRollOut", type="mx.events.MenuEvent")]
	[Event(name="itemRollOver", type="mx.events.MenuEvent")]

	[DefaultProperty("children")]
	public class MenuItem extends EventDispatcher
	{
		public function MenuItem()
		{
		}

		[Bindable]
		public var enabled:Boolean=true;

		[Bindable]
		public var toggled:Boolean;

		public var name:String=null;

		public var group:String=null;

		public var parent:MenuItem=null;

		public var label:String=null;

		[Inspectable(category="General", enumeration="check,radio,separator")]
		public var type:String=null;

		public var icon:Object=null;

		private var _children:Array=null;

		[Inspectable(category="General", arrayType="MenuItem")]
		[ArrayElementType("MenuItem")]
		public function set children(c:Array):void
		{
			children=c;
			if (c)
				for (var i:int=0; i < c.length; i++)
					c[i].parent=this;
		}

		public function get children():Array
		{
			return _children;
		}

		// functions for manipulating children:
	}
}