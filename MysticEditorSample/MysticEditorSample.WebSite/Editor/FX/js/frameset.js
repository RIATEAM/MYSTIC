function ScrollThis() {
    if (parent.contentFrame) {
        if (parent.contentFrame.document.scrollpos) {
			var y_pos = 0;
			if ( parent.contentFrame.document.scrollpos.y ){
				y_pos = parent.contentFrame.document.scrollpos.y.value;
			}else if ( parent.contentFrame.document.scrollpos.children["y"] ){
				y_pos = parent.contentFrame.document.scrollpos.children["y"].value;
			}
            self.scrollTo(0, y_pos);
        }
    }
}
function setSize() {
    if (parent.contentFrame) {
        if (document.scrollpos.menuheight) {
            setInterval("intSetSize()", 100);
        //    $(parent.contentFrame.document.getElementById('divsize')).height(document.scrollpos.menuheight.value);
        }
    }
}

function intSetSize(){
            document.scrollpos.menuheight.value = $(document).height();
}
$(document).ready(function() {
    if (parent.contentFrame) {
        if (document.scrollpos.menuheight) {
            document.scrollpos.menuheight.value = $(document).height();
        //    $(parent.contentFrame.document.getElementById('divsize')).height(document.scrollpos.menuheight.value);
        }
    }
});

setInterval("ScrollThis()", 50);
        