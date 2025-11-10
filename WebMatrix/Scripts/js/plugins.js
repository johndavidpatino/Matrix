function popup(url, Ventana, w, h) {
    var largo = w;
    var altura = h;
    var top = (screen.height - altura) / 2;
    var izq = (screen.width - largo) / 2;
    var opciones = "top=" + top + ",left=" + izq + ",toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=" + w + ",height=" + h + "";
    window.open(url, Ventana, opciones);
}

$(document).ready(function () {

    $('.ui-datepicker').live("mousewheel", function (event, delta) {
        if (delta < 0) {
            $(this).find('.ui-datepicker-next').click();
        } else {
            $(this).find('.ui-datepicker-prev').click();
        }
        event.preventDefault();
        event.stopPropagation();
    });
    
    //$(".formulario").validate();
    //    $("#commentForm").validate();
    $("#accordionMaster").accordion({
        header: "h3",
        autoHeight: false
    });
  
    $( ".send_button" ).button();
   
    $('.tabs').tabs();
   
    $('.scrollbar1').tinyscrollbar();	
   
    $('.datepicker').datepicker();
   
    // Dialog			
    $('#dialog').dialog({
        autoOpen: false,
        width: 600,
        buttons: {
            "Ok": function() { 
                $(this).dialog("close"); 
            }, 
            "Cancel": function() { 
                $(this).dialog("close"); 
            } 
        }
    });
    
    // Dialog Link
    $('#dialog_link').click(function(){
        $('#dialog').dialog('open');
        return false;
    });
    
    var bar_status = false;
    var w = "82%";
    $(".show_hide_bar").click(function(){
        $(".sidebar").toggle();
        
        if(!bar_status){
            $(".content").css("width","100%");
            $(".content").css("padding-left","10px");
            $(".show_hide_bar").css("left",0);
        }else{
            $(".content").css("width",w);
            $(".show_hide_bar").css("left","-15px");
            $(".content").css("padding-left","0");
        }     
        bar_status = !bar_status;
    });
    
   
    //FOR TABLES
    if($( "#table_id" ).length){
        oTable = $('#table_id').dataTable({
            "bJQueryUI": true,
            "sPaginationType": "full_numbers",
            "aoColumns": [
            null,
            null,
            null,
            {
                "bSortable": false
            },
            {
                "bSortable": false
            },
            ]
        });

    }

    $("#infoMaster").hide();

	
});