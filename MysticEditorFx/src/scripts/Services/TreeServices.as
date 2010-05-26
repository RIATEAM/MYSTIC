package scripts.Services
{
	import flash.events.EventDispatcher;

	import mx.controls.Alert;
	import mx.rpc.events.FaultEvent;
	import mx.rpc.events.ResultEvent;
	import mx.rpc.http.HTTPService;

	import scripts.Events.TreeMenuEvent;
	import scripts.Model.Model;

	[ManagedEvents("getTree")]
	public class TreeServices extends EventDispatcher
	{
		[Inject]
		public var model:Model;

		[Inject(id="treeHS")]
		public var treeHTTPService:HTTPService;

		public function TreeServices()
		{

		}

		[Init]
		public function getTree():void
		{
			treeHTTPService.addEventListener(ResultEvent.RESULT, result_handler);
			treeHTTPService.addEventListener(FaultEvent.FAULT, fault_handler);
			treeHTTPService.send();
		}

		private function result_handler(event:ResultEvent):void
		{
			model.treeXML=event.result as XML;
			dispatchEvent(new TreeMenuEvent(TreeMenuEvent.GET));
		}

		private function fault_handler(event:FaultEvent):void
		{
			Alert.show(event.fault.message);
		}
	}
}