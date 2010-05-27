function setFlashHeight(newHeight){
        //assuming flashDiv is the name of the div contains flex app.
        var flashContentHolderDiv = document.getElementById('flashDiv'); 
        flashContentHolderDiv.style.height = newHeight;
		alert("newHeight =" +newHeight)
}

function showFancyBox(url,title) {
 
		var frameUrl =  url;
		 alert(frameUrl);
		$.fancybox({
			'padding'		: 0,
			'margin'		: 0,
			'transitionIn'	: 'elastic',
			'transitionOut'	: 'elastic',
			'title'			: title,
			'width'			: '75%',
			'height'		: '90%',
			'href'			: frameUrl,
			'type'			: 'iframe'
		});
		return false;
}