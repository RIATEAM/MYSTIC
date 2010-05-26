package org.riateam.utils.managers
{
	import flash.display.DisplayObject;
	import flash.events.EventDispatcher;
	
	import mx.core.Application;
	import mx.managers.PopUpManager;
	
	import org.riateam.utils.managers.com.InspectorPopup;


	/**
	 *
	 * @author gonte
	 *
	 */
	public class DebugManager extends EventDispatcher
	{



		private static var _instance:DebugManager;

		public static function get instance():DebugManager
		{
			if (!_instance)
			{
				_instance=new DebugManager();
			}
			return _instance;
		}

		public function setInspectorPopUp(message:String, title:String, parent:DisplayObject, modal:Boolean=false):void
		{
			if (!Application.application.DEBUG)
				return;
			var popup:InspectorPopup=new InspectorPopup();
			popup.title=title;
			popup.Text=message;
			PopUpManager.addPopUp(popup, parent, modal);
		}

	}
}