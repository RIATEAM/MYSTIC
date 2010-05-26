package scripts.Services.mock
{
	import flash.utils.setTimeout;

	import mx.rpc.AsyncToken;


	public class MockUtility
	{
		public static function getRandomThumb(id:int):String
		{
			var thumb:String;
			var factor:Number=id % 3;
			switch (factor)
			{
				case 0:
					return "assets/samplePage001.jpg";
				case 1:
					return "assets/samplePage002.jpg";
				case 2:
					return "assets/samplePage003.jpg";

			}
			return "assets/samplePage003.jpg";
		}

		public static function createToken(result:Object, resultHanlder:Function):AsyncToken
		{
			var token:AsyncToken=new AsyncToken(null);
			setTimeout(resultHanlder, Math.random() * 500, token, result);

			return token;
		}
	}
}