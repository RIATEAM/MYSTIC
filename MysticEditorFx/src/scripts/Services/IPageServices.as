package scripts.Services
{
	import mx.rpc.AsyncToken;

	import scripts.Classes.PageDTO;
	import scripts.Model.Model;

	/**
	 * Interface allow manage MockServices
	 * @author gonte
	 *
	 */
	public interface IPageServices
	{

		function getPage(page:PageDTO):AsyncToken;
		function deletePage(page:PageDTO):AsyncToken;
		function movePage(page:PageDTO):AsyncToken;
		function savePage(page:PageDTO):AsyncToken;
		function copyPage(page:PageDTO):AsyncToken;

	}
}