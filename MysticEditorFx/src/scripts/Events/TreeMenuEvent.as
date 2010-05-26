package scripts.Events
{
	import flash.events.Event;

	public class TreeMenuEvent extends Event
	{
		public var success:Boolean;

		// Public constructor.
		public function TreeMenuEvent(type:String="getTree", success:Boolean=false, bubbles:Boolean=false, cancelable:Boolean=false)
		{
			// Call the constructor of the superclass.
			super(type, bubbles, cancelable);

			// Set the new property.
			this.success=success;

		}

		// Define static constant.
		public static const GET:String="getTree";


	}
}