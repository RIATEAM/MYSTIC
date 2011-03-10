$(document).ready(function(){
$("#nav-one li").hover(
function(){ $("ul", this).fadeIn("fast"); },
function() { }
);
if (document.all) {
$("#nav-one li").hoverClass ("sfHover");
}
});

$.fn.hoverClass = function(c) {
return this.each(function(){
$(this).hover(
function() { $(this).addClass(c);  },
function() { $(this).removeClass(c); }
);
});
};

function changeMenu(obj){
    var o = parent.left.document.getElementById(obj);
    var oldClicked = parent.left.document.getElementById(parent.left.scrollpos.currentSelected.value);
    if (oldClicked && oldClicked  != o ){
        oldClicked.style.background = parent.left.scrollpos.oldBack.value;
        oldClicked.style.color = parent.left.scrollpos.oldFont.value;
    }
    o.style.background = "#E6E2CC";
	o.style.color = "#292824";
    parent.left.scrollpos.currentSelected.value = o.id;
}