package org.riateam.utils.managers
{
	import flash.display.StageDisplayState;
	import flash.events.Event;
	import flash.events.EventDispatcher;
	import flash.events.FullScreenEvent;
	import flash.system.Capabilities;

	import mx.core.Application;

	[Event(name="displayStateChanged", type="flash.events.Event")]
	/**
	 *
	 * @author gonte
	 *
	 */
	public class FullscreenManager extends EventDispatcher
	{
		public static var DISPLAYSTATE_CHANGED:String="displayStateChanged";
		[Bindable]
		public var DisplayState:String=StageDisplayState.FULL_SCREEN;
		[Bindable]
		public var MinWidth:Number=538;
		[Bindable]
		public var MinHeight:Number=500;
		[Bindable]
		public var BindableWidth:Number;
		[Bindable]
		public var BindableHeight:Number;

		public function get IsFullScreen():Boolean
		{
			return DisplayState == StageDisplayState.FULL_SCREEN;
		}

		private static var _instance:FullscreenManager;

		public static function get instance():FullscreenManager
		{
			if (!_instance)
			{
				_instance=new FullscreenManager();
			}
			return _instance;
		}

		public function fullScreenHandler(evt:FullScreenEvent):void
		{
			/* dispState=Application.application.stage.displayState; */

			if (evt.fullScreen)
			{
				DisplayState=StageDisplayState.NORMAL;
				BindableWidth=MaxWidth;
				BindableHeight=MaxHeight;
			}
			else
			{
				DisplayState=StageDisplayState.FULL_SCREEN;
				BindableWidth=MinWidth;
				BindableHeight=MinHeight;
			}
			dispatchEvent(new Event(DISPLAYSTATE_CHANGED, true));
		}

		public function get MaxWidth():Number
		{
			return Capabilities.screenResolutionX;
		}

		public function get MaxHeight():Number
		{
			return Capabilities.screenResolutionY;
		}

		public function toggleFullScreen():void
		{
			try
			{
				switch (Application.application.stage.displayState)
				{
					case StageDisplayState.FULL_SCREEN:
						/* If already in full screen mode, switch to normal mode. */
						setFullScreenModeOFF();
						break;
					default:
						/* If not in full screen mode, switch to full screen mode. */
						setFullScreenModeON();
						break;
				}
			}
			catch (err:SecurityError)
			{
				// ignore
			}
		}

		public function setFullScreenModeON():void
		{
			Application.application.stage.displayState=StageDisplayState.FULL_SCREEN;
		}

		public function setFullScreenModeOFF():void
		{

			Application.application.stage.displayState=StageDisplayState.NORMAL;
		}

		public function get ScreenResolutionX():Number
		{
			return Capabilities.screenResolutionX;
		}

		public function get ScreenResolutionY():Number
		{
			return Capabilities.screenResolutionY;
		}

	}
}