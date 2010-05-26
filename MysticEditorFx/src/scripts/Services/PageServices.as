package scripts.Services
{
	import flash.events.EventDispatcher;

	import mx.controls.Alert;
	import mx.rpc.AsyncResponder;
	import mx.rpc.AsyncToken;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.remoting.mxml.RemoteObject;

	import scripts.Classes.PageDTO;
	import scripts.Events.PageViewEvent;

	
	[ManagedEvents("deletePage")]
	[ManagedEvents("savePage")]
	[ManagedEvents("movePage")]
	[ManagedEvents("publishPage")]
	public class PageServices extends EventDispatcher implements IPageServices
	{
		[Inject(id="pageRO")]
		public var pageService:RemoteObject;

		public function PageServices()
		{

		}

		public function getPage(page:PageDTO):AsyncToken
		{
			var token:AsyncToken=pageService.GetPage(page.Pageid);
			token.addResponder(new AsyncResponder(getPageResult, faultHandler, page));
			return token;
		}

		private function getPageResult(event:ResultEvent, page:PageDTO):void
		{
			if (event.result != null)
			{
				//model.Page=event.result as PageDTO;
				dispatchEvent(new PageViewEvent(PageViewEvent.CHANGE, page));
			}
			else
			{
				Alert.show("Pagina non trovata");
			}
		}

		private function faultHandler(event:FaultEvent, a:Object):void
		{
			Alert.show(event.fault.faultString);
		}
	}
}