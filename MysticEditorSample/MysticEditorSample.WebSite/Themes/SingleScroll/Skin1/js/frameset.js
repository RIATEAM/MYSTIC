function ScrollThis() {
    if (parent.mainframe) {
        if (parent.mainframe.document.scrollpos) {
            y_pos = parent.mainframe.document.scrollpos.y.value;
            self.scrollTo(0, y_pos);
        }
    }
}
function setSize() {
    if (parent.mainframe) {
        if (document.scrollpos.menuheight) {
            document.scrollpos.menuheight.value = $(document).height();
            document.scrollpos.menuheight.value = $(document).height();
            $(parent.mainframe.document.getElementById('divsize')).height(document.scrollpos.menuheight.value);
        }
    }
}
$(document).ready(function() {
    if (parent.mainframe) {
        if (document.scrollpos.menuheight) {
            document.scrollpos.menuheight.value = $(document).height();
            $(parent.mainframe.document.getElementById('divsize')).height(document.scrollpos.menuheight.value);
        }
    }
});

setInterval("ScrollThis()", 50);
        