function ScrollLeftToo() {
    if (document.scrollpos) {
        y_pos = document.all ? document.body.scrollTop : window.pageYOffset;
        
        document.scrollpos.y.value = y_pos;
    }
}

$(document).ready(function() {

    if (parent.left) {
        if (parent.left.document.scrollpos) {
            $('#divsize').height(parent.left.document.scrollpos.menuheight.value);
        }
    }else if (parent.myAlternativeContent) {
        if (parent.myAlternativeContent.document.scrollpos) {
            $('#divsize').height(parent.myAlternativeContent.document.scrollpos.menuheight.value);
        }
	}else{
		window.document.body.scroll = 'no';
		
		//setFlashHeightNO($(document).height());
	}
});