package org.riateam.utils
{
	import Welsy3.BL.CMSServices.event.GetCrtUserItemsEvent;
	import Welsy3.BL.CMSServices.event.GetCrtUserItersEvent;
	import Welsy3.BL.Common.model.CommonModel;
	import Welsy3.BL.Home.event.GetUserNewsEvent;
	import Welsy3.BL.Home.model.HomeModel;

	import com.adobe.cairngorm.control.CairngormEventDispatcher;

	import flash.display.MovieClip;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.ProgressEvent;
	import flash.filters.DropShadowFilter;

	import mx.events.FlexEvent;
	import mx.managers.PopUpManager;
	import mx.preloaders.DownloadProgressBar;

	public class FlashPreloader extends DownloadProgressBar
	{
		[Embed(source='assets/css/skin/MysticEditorFx.swf#customPreloader')]
		public static const preloaderClass:Class;

		private var cp:MovieClip;
		private var bgMc:MovieClip;

		public function FlashPreloader()
		{

			cp=new preloaderClass() as MovieClip;
			//cp.filters=[new DropShadowFilter(4, 45, 0, 0.5)];
			addEventListener(Event.ADDED_TO_STAGE, onAdded);

			addChild(cp);
		}

		public override function set preloader(preloader:Sprite):void
		{
			preloader.addEventListener(ProgressEvent.PROGRESS, onProgress);
			preloader.addEventListener(FlexEvent.INIT_PROGRESS, FlexInitProgress);
			preloader.addEventListener(FlexEvent.INIT_COMPLETE, initComplete);
		}

		private function onProgress(e:ProgressEvent):void
		{
			/* 	cp.percent.text = Math.ceil(e.bytesLoaded / e.bytesTotal * 100).toString() + "%"; */
			cp.gotoAndStop(Math.ceil(e.bytesLoaded / e.bytesTotal * 100));
		}

		private function FlexInitProgress(event:Event):void
		{
		/* if (Application.application.DEBUG)
		 CallbackManager.login(); */
		}



		private function onAdded(e:Event):void
		{

			cp.stop();
			cp.x=stage.stageWidth * 0.5 - 200;
			cp.y=stage.stageHeight * 0.5 - 63;
		}

		private function initComplete(e:Event):void
		{

			/* 		getUserData(); */
			CommonModel.getInstance().isInitialize=true;
			dispatchEvent(new Event(Event.COMPLETE));
		}

		private function getUserData():void
		{
			//  richiamo  news associate a user 
			PopUpManager.removePopUp(CommonModel.getInstance().waitIterItem);
			var cEvt_news:GetUserNewsEvent=new GetUserNewsEvent(CommonModel.getInstance().crtUser.USERID)
			CairngormEventDispatcher.getInstance().dispatchEvent(cEvt_news);

			//var cEvt_iter:Get =  new getItemsEvent(CommonModel.getInstance().crtUser.USERID, HomeModel.getInstance().iter_side)

			CairngormEventDispatcher.getInstance().dispatchEvent(new GetCrtUserItersEvent(GetCrtUserItersEvent.GET_DA_FARE, CommonModel.getInstance().crtUser.USERID, HomeModel.getInstance().iter_side))

			/* CairngormEventDispatcher.getInstance().dispatchEvent(new GetCrtUserItersEvent(GetCrtUserItersEvent.GET_COMPLETATI, CommonModel.getInstance().crtUser.USERID, HomeModel.getInstance().iter_side)) */
			//

			CairngormEventDispatcher.getInstance().dispatchEvent(new GetCrtUserItemsEvent(GetCrtUserItemsEvent.GET_DA_FARE, CommonModel.getInstance().crtUser.USERID, HomeModel.getInstance().item_side))

		}
	}
}