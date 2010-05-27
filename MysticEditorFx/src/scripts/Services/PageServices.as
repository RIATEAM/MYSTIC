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
	
	import org.spicefactory.lib.reflect.types.Void;
	
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
	public class PageServices extends EventDispatcher implements IPageServices
	{
		[Inject(id="pageRO")]
		public var pageService:RemoteObject;

		public function PageServices():void
		{

		}

		public function getPage(page:PageDTO):AsyncToken
		{
			//var fakeResult:PageDTO=PageDTO.getMockPageDTO(model.MockPageTitle);

			var token:AsyncToken=pageService.GetPage(page.Pageid);
		//	page.thumb=MockUtility.getRandomThumb(page.Pageid);
//			var token:AsyncToken=MockUtility.createToken(page, applyResult);
			token.addResponder(new AsyncResponder(getPageResult, faultHandler, page));
			return token;
		}

		public function copyPage(page:PageDTO):AsyncToken
		{
//			var token:AsyncToken=pageService.DeletePage(page);
////			var token:AsyncToken=MockUtility.createToken(page, applyResult);
//			token.addResponder(new AsyncResponder(deletePageResult, faultHandler, page));
//			return token;
			return null;
		}

		public function deletePage(page:PageDTO):AsyncToken
		{
			var token:AsyncToken=pageService.DeletePage(page);
//			var token:AsyncToken=MockUtility.createToken(page, applyResult);
			token.addResponder(new AsyncResponder(deletePageResult, faultHandler, page));
			return token;
		}

		/**
		 * Fault Handler
		 * @param event
		 * @param a
		 *
		 */
		private function faultHandler(event:FaultEvent, a:Object):void
		{
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
		/**
		 * Result handler from getPage
		 * @param event
		 * @param page
		 *
		 */
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

		/**
		 * Result handler from deletePage
		 * @param event
		 * @param page
		 *
		 */
		private function deletePageResult(event:ResultEvent, page:PageDTO):void
		{
			if (event.result != null)
			{
				page.IsPersisted=event.result; //fake delete
				dispatchEvent(new PageViewEvent(PageViewEvent.DELETE, page));
			}

		}
	}
}