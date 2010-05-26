package org.riateam.utils
{
	import flash.events.Event;
	import flash.events.IEventDispatcher;

	import mx.core.Application;
	import mx.events.ResourceEvent;

	/**
	 * This class extends the Application to provide some helpers in order to
	 * write localizable applications.
	 *
	 * An example of application structure is shown below:
	 *
	 * <core:LocalizableApplication
	 *     xmlns:mx="http://www.adobe.com/2006/mxml"
	 *     xmlns:core="com.webgriffe.core.*"
	 *     preinitialize="onPreinitialize(event);">
	 *   <mx:Metadata>
	 *     [ResourceBundle("resource")]
	 *   </mx:Metadata>
	 *   <mx:Script>
	 *   <![CDATA[
	 *     private function onPreinitialize(event:FlexEvent):void
	 *     {
	 *       setLocale("it_IT"); // to change from default locale
	 *     }
	 *   ]]>
	 *   </mx:Script>
	 * </core:LocalizableApplication>
	 *
	 * Remember:
	 *
	 * - use the following directives with mxmlc: -locale en_US it_IT -source-path=../locale/{locale}
	 *   (separate different locale names with a space)
	 *
	 * - create a folder named "locale" at the same level of "src" folder
	 *
	 * - create a sub-locale folder named "en_US" (and so on for each locale you
	 *   intend to use) under the "locale" folder
	 *
	 * - create a file named "resource.properties" under each sub-locale folder.
	 *   If you want to name this file other than "resource" remembre to set the
	 *   defaultBundle property in the preinitialize handler of your main
	 *   application.
	 */
	public class LocalizableApplication extends Application
	{

		// -------------------------------------------------------------------------
		//
		// Public properties
		//
		// -------------------------------------------------------------------------

		public var defaultBundle:String="resource";
		public var defaultLocale:String="en_US";

		// -------------------------------------------------------------------------
		//
		// Private properties
		//
		// -------------------------------------------------------------------------

		/**
		 * Used to store current locale string.
		 */
		private var _currLocale:String=defaultLocale;

		// -------------------------------------------------------------------------
		//
		// Constructor
		//
		// -------------------------------------------------------------------------

		public function LocalizableApplication()
		{
			super();
		}

		// -------------------------------------------------------------------------
		//
		// Public methods
		//
		// -------------------------------------------------------------------------

		public function setLocale(locale:String):void
		{
			_currLocale=locale;
			if (resourceManager.getLocales().indexOf(locale) != -1)
			{
				onResourceLoaded(null);
			}
			else
			{
				var resourceModuleURL:String="Resources_" + locale + ".swf";
				var eventDispatcher:IEventDispatcher=resourceManager.loadResourceModule(resourceModuleURL);
				eventDispatcher.addEventListener(ResourceEvent.COMPLETE, onResourceLoaded);
			}
		}

		/**
		 * Gets the value of a specified resource as a Boolean.
		 */
		[ChangeEvent("locale_changed")]
		public function __b(resourceName:String, bundleName:String=null, locale:String=null):Boolean
		{
			return resourceManager.getBoolean(bundleName == null ? defaultBundle : bundleName, resourceName, locale == null ? _currLocale : locale);
		}

		/**
		 * Gets the value of a specified resource as an int.
		 */
		[ChangeEvent("locale_changed")]
		public function __i(resourceName:String, bundleName:String=null, locale:String=null):int
		{
			return resourceManager.getInt(bundleName == null ? defaultBundle : bundleName, resourceName, locale == null ? _currLocale : locale);
		}

		/**
		 * Gets the value of a specified resource as a Number.
		 */
		[ChangeEvent("locale_changed")]
		public function __n(resourceName:String, bundleName:String=null, locale:String=null):Number
		{
			return resourceManager.getNumber(bundleName == null ? defaultBundle : bundleName, resourceName, locale == null ? _currLocale : locale);

		}

		/**
		 * Gets the value of a specified resource as an Object.
		 */
		[ChangeEvent("locale_changed")]
		public function __o(resourceName:String, bundleName:String=null, locale:String=null):*
		{
			return resourceManager.getObject(bundleName == null ? defaultBundle : bundleName, resourceName, locale == null ? _currLocale : locale);
		}

		/**
		 * Gets the value of a specified resource as a String, after substituting
		 * specified values for placeholders.
		 */
		[ChangeEvent("locale_changed")]
		public function __s(resourceName:String, bundleName:String=null, parameters:Array=null, locale:String=null):String
		{
			return resourceManager.getString(bundleName == null ? defaultBundle : bundleName, resourceName, parameters, locale == null ? _currLocale : locale);
		}

		/**
		 * Gets the value of a specified resource as an Array of Strings.
		 */
		[ChangeEvent("locale_changed")]
		public function __sa(resourceName:String, bundleName:String, locale:String=null):Array
		{
			return resourceManager.getStringArray(bundleName == null ? defaultBundle : bundleName, resourceName, locale == null ? _currLocale : locale);
		}

		/**
		 * Gets the value of a specified resource as a uint.
		 */
		[ChangeEvent("locale_changed")]
		public function __ui(resourceName:String, bundleName:String=null, locale:String=null):uint
		{
			return resourceManager.getUint(bundleName == null ? defaultBundle : bundleName, resourceName, locale == null ? _currLocale : locale);
		}

		// -------------------------------------------------------------------------
		//
		// Handlers
		//
		// -------------------------------------------------------------------------

		private function onResourceLoaded(event:ResourceEvent):void
		{
			resourceManager.localeChain=[_currLocale];
			dispatchEvent(new Event('locale_changed'));
		}

	}
}