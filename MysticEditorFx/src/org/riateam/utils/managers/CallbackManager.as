package org.riateam.utils.managers
{
	import flash.events.EventDispatcher;
	import flash.external.ExternalInterface;

	import mx.controls.Alert;
	import mx.logging.Log;

	/**
	 *
	 * @author gonte
	 *
	 */
	public class CallbackManager extends EventDispatcher
	{
		public static function RegisterCallbacks():void
		{

			if (!ExternalInterface.available)
			{
				try
				{
					//  ExternalInterface.addCallback("Flex_playerUnloaded", playerUnloaded);



				}
				catch (error:SecurityError)
				{
					Alert.show("A SecurityError occurred: " + error.message + "\n", "Warning");
				}
				catch (error:Error)
				{
					Alert.show("An Error occurred: " + error.message + "\n", "Warning");
				}
			}
		/* else
		   {
		   Alert.show("External interface is not available for this container.", "Warning");
		 } */

		}

		/**
		 *
		 * @param url
		 * @param title
		 *
		 */
		public static function showFancyBox(url:String, title:String):void
		{
			Log.getLogger("CallbackManager").debug("showFancyBox url = " + url + " title =" + title);
			ExternalInterface.call('showFancyBox', url, title);
		}

		/**
		 *
		 * @param measuredHeight
		 *
		 */
		public static function setFlashHeight(measuredHeight:Number):void
		{

			ExternalInterface.call('setFlashHeight', measuredHeight);
		}

		private function callExternalInterface(jsfunction:String, params:Array):void
		{

		}

	}
}