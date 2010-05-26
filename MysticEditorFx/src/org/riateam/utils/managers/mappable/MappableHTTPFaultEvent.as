package org.riateam.utils.managers.mappable
{
	import flash.events.Event;

	import mx.rpc.Fault;


	/**
	 *
	 * @author gonte
	 *
	 */
	public class MappableHTTPFaultEvent extends Event
	{
		public function MappableHTTPFaultEvent(fault:Fault, type:String="mappableFault", bubbles:Boolean=false, cancelable:Boolean=false)
		{
			this.fault=fault;
			super(type, bubbles, cancelable);
		}

		public var fault:Fault;
		public static const MAPPABLE_FAULT:String="mappableFault";

		override public function clone():Event
		{

			return super.clone();
		}

	}
}