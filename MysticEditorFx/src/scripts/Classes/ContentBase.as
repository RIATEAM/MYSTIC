package scripts.Classes
{

	[Bindable]
	public class ContentBase
	{


		[RemoteClass(alias="Editor.BE.Model.ContentBase")]
		public function ContentBase()
		{
		}

		public var Contentid:Number;


		public var Parentcontentid:Number;


		public var Title:String;


		public var Skinid:Number;


		public var IsPersisted:Boolean;


		public var IsNew:Boolean;


		public var Dirty:Boolean;

		public var Deleted:Boolean;


		public var HasChanged:Boolean;
	}
}