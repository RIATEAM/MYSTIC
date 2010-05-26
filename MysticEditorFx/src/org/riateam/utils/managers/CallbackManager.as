package org.riateam.utils.managers
{
	import flash.events.EventDispatcher;
	import flash.external.ExternalInterface;

	import mx.controls.Alert;

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
			else
			{
				Alert.show("External interface is not available for this container.", "Warning");
			}

		}

	}
}