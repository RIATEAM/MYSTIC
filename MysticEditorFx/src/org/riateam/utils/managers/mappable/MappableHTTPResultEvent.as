package org.riateam.utils.managers.mappable
{
	import flash.events.Event;


	/**
	 *
	 * @author gonte
	 *
	 */
	public class MappableHTTPResultEvent extends Event
	{
		public function MappableHTTPResultEvent(result:Object, type:String="mappableResult", bubbles:Boolean=false, cancelable:Boolean=false)
		{
			this.result=result;
			super(type, bubbles, cancelable);
		}

		public var result:Object;
		public static const MAPPABLE_RESULT:String="mappableResult";

		override public function clone():Event
		{

			return super.clone();
		}

	}
}