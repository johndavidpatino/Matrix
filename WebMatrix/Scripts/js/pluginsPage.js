function getDepartamentoPorPais(idControlPais, idControlDepartamentos) {
    $('#' + idControlDepartamentos).get(0).options.length = 0;
    $('#' + idControlDepartamentos).get(0).options[0] = new Option('[ Cargando... ]', '-2');

    $.ajax({
        type: 'POST',
        url: '../WServices/wsSeleccion.asmx/ObtenerDepartamentosPorPaises',
        data: '{idPais: ' + $("#" + idControlPais).val() + '}',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (msg) {
            $("#" + idControlDepartamentos).get(0).options.length = 0;
            $("#" + idControlDepartamentos).get(0).options[0] = new Option("-- Seleccione --", "-2");

            $.each(msg.d, function (index, item) {
                $('#' + idControlDepartamentos).get(0).options[$('#' + idControlDepartamentos).get(0).options.length] = new Option(item.nombre, item.valor);
            });
        },
        error: function (xhr, status, error) {
            $("#" + idControlDepartamentos).get(0).options.length = 0;
            $("#" + idControlDepartamentos).get(0).options[0] = new Option('[ Error al cargar los datos ]', '-2');
        }
    });

}

function getCiudadesDepartamento(idControlDepartamento, idControlCiudad) {

    $('#' + idControlCiudad).get(0).options.length = 0;
    $('#' + idControlCiudad).get(0).options[0] = new Option('[ Cargando... ]', '-2');

    $.ajax({
        type: 'POST',
        url: '../WServices/wsSeleccion.asmx/ObtenerCiudadesPorDepartamento',
        data: '{idDepartamento: ' + $("#" + idControlDepartamento).val() + '}',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (msg) {
            $('#' + idControlCiudad).get(0).options.length = 0;
            $('#' + idControlCiudad).get(0).options[0] = new Option("-- Seleccione --", "-2");

            $.each(msg.d, function (index, item) {
                $('#' + idControlCiudad).get(0).options[$('#' + idControlCiudad).get(0).options.length] = new Option(item.nombre, item.valor);
            });
        },
        error: function (xhr, status, error) {
            $('#' + idControlCiudad).get(0).options.length = 0;
            $('#' + idControlCiudad).get(0).options[0] = new Option('[ Error al cargar los datos ]', '-2');
        }
    });
}



function validarSelect(idControl, textoNotificacion, evt) {

    //    var idValidacion = $('#' + idControl).val();
    //    if (idValidacion == -2) {
    $('#lbltextError').text(textoNotificacion);
    $('#' + idControl).effect('highlight', 3000);

    var position = $('#' + idControl).position();
    $('html, body').animate({ scrollTop: position.top }, 'slow');
    $('#' + idControl).focus();

    ShowErrorNotificationClient();
    evt.preventDefault();
    //}

}

function ApplyCalendarPlugin(controlId) {
    $("#" + controlId).mask("9999/99/99");
    $("#" + controlId).datepicker({
        dateFormat: 'yy/mm/dd',
        changeMonth: true,
        changeYear: true,
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
    });
}


function ApplyCalendarPluginMinAndMaxDate(controlId) {
    $("#" + controlId).mask("9999/99/99");
    $("#" + controlId).datepicker({
        dateFormat: 'yy/mm/dd',
        minDate: '+0D',
        maxDate: '+1Y',
        changeMonth: true,
        changeYear: true,
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
    });
}

function ClearTextBoxOnFocus(ControlIdFocus, ControlIdClear) {
    $('#' + ControlIdFocus).focus(function () {
        $('#' + ControlIdClear).val('');
    });
}

function ClearDropDownListOnFocus(ControlIdFocus, ControlIdClear) {
    $('#' + ControlIdFocus).focus(function () {
        $('#' + ControlIdClear).val(-2);
    });
}


String.format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }

    return s;
}

function fRight(str, n) {
    if (n <= 0)
        return "";
    else if (n > String(str).length)
        return str;
    else {
        var iLen = String(str).length;
        return String(str).substring(iLen, iLen - n);
    }
}

//Notifications
function runEffect(notificationType) {
    var $notification;

    if (notificationType == "error") {
        $notification = $("#error");
    }

    else if (notificationType == "info") {
        $notification = $("#info");
    }

    // most effect types need no options passed by default
    var options = {};

    // run the effect
    $notification.toggle("blind", options, 500, callback(notificationType));

};

function ShowInfoNotifications() {

    if ($("#notificationHide").is(":hidden")) {

        //Cambiar Imagen de Fondo
        $('#Notification-Error').css('background-image', 'url(../images/bgNotification-Up_50.png)');

        //Animar expansion del div contenedor
        $("#Notification-Error").animate({
            height: "50px"
        }, 500);
    }
    else if ($("#notificationHide").is(":visible")) {

        //Cambiar Imagen de Fondo
        $('#Notification-Error').css('background-image', 'url(../images/bgNotification-Down_50.png)');

        //Animar expansion del div contenedor
        $("#Notification-Error").animate({
            height: "25px"
        }, 300);

    }

    //Mostrar div con iconos de informacion y de error
    $("#notificationHide").slideToggle(250);

}

function ShowErrorNotificationClient() {
    $('#error').show('blind', {}, 500, function () {
        setTimeout(function () {
            $('#error:visible').removeAttr('style').hide('blind');
        }, 3000);
    });
}

//callback function to bring a hidden box back
function callback(notificationType) {

    if (notificationType == "error") {
        setTimeout(function () {
            $("#error:visible").removeAttr("style").hide("blind");
        }, 30000);
    }
    else if (notificationType == "info") {
        setTimeout(function () {
            $("#info:visible").removeAttr("style").hide("blind");
        }, 30000);
    }
};
//End Notificaciones


//Validation Group
function Validate(evt) {
    try {
        var $group = $(this).parents('.validationGroup');
        var isValid = true;

        $group.find(':input').each(function (i, item) {
            if (!$(item).valid()) {
                isValid = false;
            }
        });

        if (!isValid) {
            evt.preventDefault();
        }
    }
    catch (err) {

    }


}


function validationForm() {
    formValidation = $('#<%= Master.FindControl("formMasterPage").ClientID %>');
    formValidation.validate({ onsubmit: false });

    $('.validationGroup .causesValidation').click(Validate);
    $('.validationGroup :text').keydown(function (evt) {
        if (evt.keyCode == 13) {
            var $nextInput = $(this).nextAll(':input:first');
            if ($nextInput.is(':submit')) {
                Validate(evt);
            }
            else {
                evt.preventDefault();
                $nextInput.focus();
            }
        }
    });
}


function Validate2(evt) {
    var $group = $(this).parents('.validationGroup2');
    var isValid = true;

    $group.find(':input').each(function (i, item) {
        if (!$(item).valid()) {
            isValid = false;
        }
    });

    if (!isValid) {
        evt.preventDefault();
    }

}
function validationForm2() {
    formValidation = $('#<%= Master.FindControl("formMasterPage").ClientID %>');
    formValidation.validate({ onsubmit: false });

    $('.validationGroup2 .causesValidation2').click(Validate2);
    $('.validationGroup2 :text').keydown(function (evt) {
        if (evt.keyCode == 13) {
            var $nextInput = $(this).nextAll(':input:first');
            if ($nextInput.is(':submit')) {
                Validate2(evt);
            }
            else {
                evt.preventDefault();
                $nextInput.focus();
            }
        }
    });
}

//Change values on PostBack
function InitializeRequest(sender, args) {
    try {
        if ((args._postBackElement != undefined) && ($get(args._postBackElement.id) != null)) {
            $get(args._postBackElement.id).disabled = true;
        }
    }
    catch (err) {

    }

    //$get(args._postBackElement.id).innerText = "Guardando...";
    //    var buttonSubmit = $get(args._postBackElement.id)
    //    if (buttonSubmit.innerText == "Buscar") {
    //        buttonSubmit.innerText = "Buscando...";
    //    }
    //    else if (buttonSubmit.innerText == "Guardar") {
    //        buttonSubmit.innerText = "Gurdando...";
    //    }
}
function EndRequest(sender, args) {
    try {
        loadPlugins();
        if ((sender._postBackSettings != undefined) && ($get(sender._postBackSettings.sourceElement.id) != null)) {
            $get(sender._postBackSettings.sourceElement.id).disabled = false;
        }

        //        if (args.get_error() != undefined) {
        //            args.set_errorHandled(true);
        //        }
    }
    catch (err) {
        //alert('aqui es el error');
    }

}

//    Loading....
//    var prm = Sys.WebForms.PageRequestManager.getInstance();
//    prm.add_initializeRequest(InitializeRequest);
//    prm.add_endRequest(EndRequest);
//    function InitializeRequest(sender, args) {
//        $get(args._postBackElement.id).disabled = true;
//        $get(args._postBackElement.id).innerText = "Guardando...";
//    }
//    function EndRequest(sender, args) {
//        $get(sender._postBackSettings.sourceElement.id).disabled = false;
//    }
//    End Loading...

