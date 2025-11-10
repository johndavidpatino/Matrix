/*movimiento logo*/
$(document).ready(function() {
		$(".logo").hover(function() {
		    $(this).stop().animate({ marginLeft: "20px" }, 200);
		    $(this).parent().find("span").stop().animate({ marginTop: "18px", opacity: 0.25 }, 200);
		},function(){
		    $(this).stop().animate({ marginLeft: "0px" }, 300);
		    $(this).parent().find("span").stop().animate({ marginTop: "1px", opacity: 1 }, 300);
		});
});
/*fin movimiento logo*/

/*movimiento header*/

$(document).ready(function(){
	$("header2").animate({ 'background-position-y': "-500px" }, {duration:1000});
});
/*fin movimiento header*/

/*altura div*/
/*$(document).ready(function($){
    var div_alto = $(".submenu").height();
});

$(document).ready(function($){
    var div_alto = 5;
});*/
/*fin altura div*/