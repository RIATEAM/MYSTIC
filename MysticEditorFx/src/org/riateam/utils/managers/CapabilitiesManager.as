package org.riateam.utils.managers
{
	import flash.events.EventDispatcher;
	import flash.external.ExternalInterface;
	import flash.system.Capabilities;

	/* [Event(name="displayStateChanged", type="flash.events.Event")] */
	/**
	 *
	 * @author gonte
	 *
	 */
	public class CapabilitiesManager extends EventDispatcher
	{
		/* public static var DISPLAYSTATE_CHANGED:String="displayStateChanged"; */


		private static var _instance:CapabilitiesManager;

		public static function get instance():CapabilitiesManager
		{
			if (!_instance)
			{
				_instance=new CapabilitiesManager();
			}
			return _instance;
		}


		public function get screenResolutionX():Number
		{
			return Capabilities.screenResolutionX;
		}

		public function get screenResolutionY():Number
		{
			return Capabilities.screenResolutionY;
		}

		public function get isDebugger():Boolean
		{
			return Capabilities.isDebugger;
		}

		public function get manufacturer():String
		{
			return Capabilities.manufacturer;
		}

		public function get language():String
		{
			return Capabilities.language;
		}

		public function get os():String
		{
			return Capabilities.os;
		}

		public function get screenColor():String
		{
			return Capabilities.screenColor;
		}

		public function get screenDPI():Number
		{
			return Capabilities.screenDPI;
		}

		public function get version():String
		{
			return Capabilities.version;
		}
		public function get externalInterfaceAvailable():Boolean{
			return ExternalInterface.available
		}


	}
}