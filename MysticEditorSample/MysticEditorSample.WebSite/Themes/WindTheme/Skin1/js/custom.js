var oldClicked;
var oldBack;
var oldFont;

function onMouseOver(o){
	if ( o != oldClicked ){
		oldBack = o.style.background;
		oldFont = o.style.color;

		o.style.background = "#C2BEAC";
		o.style.color = "#292824";
	}
}

function onMouseOut(o){
	if ( o != oldClicked ){
		o.style.background = oldBack;
		o.style.color = oldFont;
	}
}

function onMouseDown(o){
	if (oldClicked && oldClicked  != o ){
		oldClicked.style.background = oldBack;
		oldClicked.style.color = oldFont;
	}
	o.style.background = "#E6E2CC";
	oldClicked  = o;
}


function expOnMouseOver(o){
	o.style.textDecoration = 'underline';
}


function expOnMouseOut(o){
	o.style.textDecoration = '';
}