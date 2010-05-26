package scripts.Events
{
	import flash.events.Event;

	import scripts.Classes.PageDTO;
	/**
	 * 
	 * @author gonte
	 * 
	 */
	public class PageViewEvent extends Event
	{
		public var page:PageDTO;


		// Public constructor.
		public function PageViewEvent(type:String, page:PageDTO, bubbles:Boolean=false, cancelable:Boolean=false)
		{
			// Call the constructor of the superclass.
			super(type, bubbles, cancelable);

			// Set the new property.
			this.page=page;
		}

		// Define static constant.
		public static const GET:String="getPage";
		public static const DELETE:String="deletePage";
		public static const SAVE:String="savePage";
		public static const MOVE:String="movePage";
		public static const PUBLISH:String="publishPage";



	}
}