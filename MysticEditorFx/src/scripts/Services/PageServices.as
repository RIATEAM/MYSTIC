package scripts.Services
{
	import flash.events.EventDispatcher;
	
	import mx.controls.Alert;
	import mx.core.mx_internal;
	import mx.rpc.AsyncResponder;
	import mx.rpc.AsyncToken;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;
	
	import scripts.Classes.PageDTO;
	import scripts.Events.PageViewEvent;

	use namespace mx_internal;

	/**
	 *
	 * @author gonte
	 *
	 */
	[ManagedEvents("getPage")]
	[ManagedEvents("deletePage")]
	[ManagedEvents("savePage")]
	[ManagedEvents("publishPage")]
	[ManagedEvents("movePage")]
	[ManagedEvents("copyPage")]
	public class PageServices extends EventDispatcher implements IPageServices
	{
		[Inject(id="pageRO")]
		public var pageService:RemoteObject;
		
		private var currentEvent:String;

		public function PageServices():void
		{

		}

		public function getPage(page:PageDTO):AsyncToken {
			//var fakeResult:PageDTO=PageDTO.getMockPageDTO(model.MockPageTitle);

			var token:AsyncToken=pageService.GetPage(page.Pageid);
		//	page.thumb=MockUtility.getRandomThumb(page.Pageid);
//			var token:AsyncToken=MockUtility.createToken(page, applyResult);
			token.addResponder(new AsyncResponder(getPageResult, faultHandler, page));
			return token;
		}

		/******************************** COPY ****************************/
		public function copyPage(page:PageDTO):AsyncToken {
			currentEvent = PageViewEvent.COPY;
			var token:AsyncToken=pageService.SavePage(page);
			token.addResponder(new AsyncResponder(copyPageResult, faultHandler, page));
			return token;
		}
		private function copyPageResult(event:ResultEvent, page:PageDTO):void {
			if (event.result != null) {
				page.IsPersisted=event.result;
				dispatchEvent(new PageViewEvent(PageViewEvent.COPY, page));
			}
		}
		/******************************** DELETE ****************************/
		public function deletePage(page:PageDTO):AsyncToken {
			
			currentEvent = PageViewEvent.DELETE;
			var token:AsyncToken=pageService.DeletePage(page);
			token.addResponder(new AsyncResponder(deletePageResult, faultHandler, page));
			return token;
		}

		private function deletePageResult(event:ResultEvent, page:PageDTO):void {
			if (event.result != null) {
				page.IsPersisted=event.result;
				dispatchEvent(new PageViewEvent(PageViewEvent.DELETE, page));
			}
		}
		/******************************** SAVE ****************************/
		public function savePage(page:PageDTO):AsyncToken {
			currentEvent = PageViewEvent.SAVE;
			var token:AsyncToken=pageService.SavePage(page);
			token.addResponder(new AsyncResponder(savePageResult, faultHandler, page));
			return token;
		}
		private function savePageResult(event:ResultEvent, page:PageDTO):void {
			if (event.result != null) {
				page.IsPersisted=event.result;
				dispatchEvent(new PageViewEvent(PageViewEvent.SAVE, page));
			}
		}
		/******************************** MOVE ****************************/
		public function movePage(page:PageDTO):AsyncToken {
			currentEvent = PageViewEvent.MOVE;
			var token:AsyncToken=pageService.MovePage(page);
			token.addResponder(new AsyncResponder(movePageResult, faultHandler, page));
			return token;
		}
		
		private function movePageResult(event:ResultEvent, page:PageDTO):void {
			if (event.result != null) {
				page.IsPersisted=event.result;
				dispatchEvent(new PageViewEvent(PageViewEvent.MOVE, page));
			}
		}
		/******************************************************************/
		
		private function faultHandler(event:FaultEvent, a:Object):void
		{
			if ( currentEvent == PageViewEvent.MOVE ){
				dispatchEvent(new PageViewEvent(PageViewEvent.MOVE, a as PageDTO));
			}
			Alert.show(event.fault.faultString);
		}

		/**
		 *
		 * @param token
		 * @param result
		 *
		 */
		private function applyResult(token:AsyncToken, result:Object):void
		{
			mx_internal: token.setResult(result);
			var event:ResultEvent=new ResultEvent(ResultEvent.RESULT, false, true, result, token);
			mx_internal: token.applyResult(event);

		}

		/******************** RESULT HANDLER ********************/
		private function getPageResult(event:ResultEvent, page:PageDTO):void
		{
			if (event.result != null)
			{

				dispatchEvent(new PageViewEvent(PageViewEvent.GET, page));
			}
			else
			{
				Alert.show("Pagina non trovata");
			}
		}
	}
}