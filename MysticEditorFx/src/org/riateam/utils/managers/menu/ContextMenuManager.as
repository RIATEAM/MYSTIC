package org.riateam.utils.managers.menu
{
	import flash.events.ContextMenuEvent;
	import flash.events.Event;
	import flash.events.EventDispatcher;
	import flash.ui.ContextMenu;
	import flash.ui.ContextMenuItem;


	[Event(name="onDelete", type="flash.events.Event")]
	[Event(name="onSelect", type="flash.events.Event")]
	[Event(name="onCopy", type="flash.events.Event")]
	[Event(name="onPreview", type="flash.events.Event")]
	/**
	 *
	 * @author gonte
	 *
	 */
	public class ContextMenuManager extends EventDispatcher
	{
		public static var DELETE:String="onDelete";
		public static var SELECT:String="onSelect";
		public static var COPY:String="onCopy";
		public static var PREVIEW:String="onPreview";

		public function ContextMenuManager()
		{
			initContextMenu();
		}

		[Bindable]
		public var cm:ContextMenu;

		private function initContextMenu():void
		{


			cm=new ContextMenu();
			cm.hideBuiltInItems();
			cm.customItems=[];
			cm.addEventListener(ContextMenuEvent.MENU_SELECT, function contextMenu_menuSelect(evt:ContextMenuEvent):void
				{
					dispatchEvent(new Event(SELECT));
				});

			/*********** ITEMS ***********/

			/****** DELETE ******/
			var deleteItem:ContextMenuItem=new ContextMenuItem("Elimina..", true);
			deleteItem.addEventListener(ContextMenuEvent.MENU_ITEM_SELECT, function onDelete(evt:ContextMenuEvent):void
				{
					dispatchEvent(new Event(DELETE));

				});

			cm.customItems.push(deleteItem);

			/****** COPY ******/
			var copyItem:ContextMenuItem=new ContextMenuItem("Copia..", false);
			copyItem.addEventListener(ContextMenuEvent.MENU_ITEM_SELECT, function onCopy(evt:ContextMenuEvent):void
				{
					dispatchEvent(new Event(COPY));

				});

			cm.customItems.push(copyItem);
			
			/****** PREVIEW ******/
			var previewItem:ContextMenuItem=new ContextMenuItem("Anteprima..",false);
			previewItem.addEventListener(ContextMenuEvent.MENU_ITEM_SELECT, function onPreview(evt:ContextMenuEvent):void
				{
					dispatchEvent(new Event(PREVIEW));

				});

			cm.customItems.push(previewItem);


		}






	}
}