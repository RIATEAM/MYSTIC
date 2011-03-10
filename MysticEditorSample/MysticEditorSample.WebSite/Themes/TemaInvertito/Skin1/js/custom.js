//var oldClicked;
//var oldBack;
//var oldFont;

function onMouseOver(o){
    var oldClicked = document.getElementById(document.scrollpos.currentSelected.value);
	if ( o != oldClicked ){
		document.scrollpos.oldBack.value = o.style.background;
		document.scrollpos.oldFont.value = o.style.color;

		o.style.background = "#C2BEAC";
		o.style.color = "#0078e8";
	}
}

function onMouseOut(o){
    var oldClicked = document.getElementById(document.scrollpos.currentSelected.value);
	if ( o != oldClicked ){
		o.style.background = document.scrollpos.oldBack.value;
		o.style.color = document.scrollpos.oldFont.value;
	}
}

function onMouseDown(o){
    var oldClicked = document.getElementById(document.scrollpos.currentSelected.value);
    if (oldClicked && oldClicked  != o ){
	    oldClicked.style.background = document.scrollpos.oldBack.value;
	    oldClicked.style.color = document.scrollpos.oldFont.value;
    }
	o.style.background = "#C2BEAC";
	//oldClicked  = o;
	document.scrollpos.currentSelected.value = o.id;
}


function expOnMouseOver(o){
	o.style.textDecoration = 'underline';
}


function expOnMouseOut(o){
	o.style.textDecoration = '';
}