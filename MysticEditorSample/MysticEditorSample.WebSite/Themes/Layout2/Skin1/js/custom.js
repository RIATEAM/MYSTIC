//var oldClicked;
//var oldBack;
//var oldFont;

function onMouseOver(o){
    var oldClicked = document.getElementById(document.scrollpos.currentSelected.value);
	if ( o != oldClicked ){
		document.scrollpos.oldBack.value = o.style.background;
		document.scrollpos.oldFont.value = o.style.color;

		o.style.background = "#EDE7D1";
		o.style.color = "#000000";
		
	}
}

function onMouseOut(o){
	try{
		var oldClicked = document.getElementById(document.scrollpos.currentSelected.value);
		if ( o != oldClicked ){
			o.style.background = document.scrollpos.oldBack.value;
			o.style.color = document.scrollpos.oldFont.value;
			
		}
	}catch(err){
	}
}

function onMouseDown(o){
	if ( o.id != "" ){
		var oldClicked = document.getElementById(document.scrollpos.currentSelected.value);
		if (oldClicked && oldClicked  != o ){
			oldClicked.style.background = document.scrollpos.oldBack.value;
			oldClicked.style.color = document.scrollpos.oldFont.value;
			try{
                oldClicked.style.font = 'normal';
            }catch(err){                      
	        }
		}
		
		o.style.background = "#F9FAFB";
		o.style.color = "#333333";	
        try{
            o.style.font = 'bold';
        }catch(err){           
	    }
		//oldClicked  = o;
		document.scrollpos.currentSelected.value = o.id;
	}
}


function expOnMouseOver(o){
	o.style.textDecoration = 'underline';
}


function expOnMouseOut(o){
	o.style.textDecoration = '';
}