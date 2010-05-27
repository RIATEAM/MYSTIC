package scripts.Classes
{

	[Bindable]
	[RemoteClass(alias="Editor.DTO.PageDTO")]
	public class PageDTO
	{
		public function PageDTO()
		{
		}

		public var Contentid:Number;


		public var Pageid:Number;


		public var Parentpageid:Number;


		public var Position:Number;


		public var Level:Number;


		public var Title:String;


		public var Publictitle:String;


		public var Skinid:Number;


		public var State:Number;


		public var IsPersisted:Boolean;


		public var IsNew:Boolean;


		public var Dirty:Boolean;


		public var Deleted:Boolean;


		public var HasChanged:Boolean;

//ADDED
		public var thumb:String;

		public static function createPageFromXML(item:XML):PageDTO
		{
			var page:PageDTO=new PageDTO();
			page.Pageid=item.@Pageid;
			page.Title=item.@Publictitle;
			page.State=item.@State;
			page.Position=item.@Position;
			page.Parentpageid=item.@Parentpageid;
			return page;
		}

		public static function getMockPageDTO(title:String="mock"):PageDTO
		{
			var page:PageDTO=new PageDTO();
			page.Contentid=99;
			page.Pageid=99;
			page.Parentpageid=99;
			page.Title=title;
			page.Publictitle="Public " + title;
			page.thumb="assets/samplePage001.jpg";
			return page;

		}

	}
}