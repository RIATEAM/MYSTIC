
/*
"Click to Activate" workaround. Works only for Internet Explorer only for ActiveX!!!

Usage:  Include this file at the end of your html document using the following
<script type="text/javascript" src="ActivateActiveControlWorkaround.js"></script>

Note. To automatically activate ActiveX controls, Internet Explorer must be using versions of vbscript.dll and jscript.dll newer than September 30, 2003. Earlier versions of this DLL will require all controls to be activated. 

Relayted links:
Microsoft official workaround
http://msdn2.microsoft.com/en-us/library/ms537508.aspx

*/

if (navigator.appName == "Microsoft Internet Explorer")
{
	var searchElements = new Array("object");
	for (i = 0; i < searchElements.length; i++)
	{
		neededObj = document.getElementsByTagName(searchElements[i]);
		for (i2 = 0; i2 < neededObj.length; i2++ )
		{
			neededObj[i2].outerHTML = neededObj[i2].outerHTML;
		}
	}
}