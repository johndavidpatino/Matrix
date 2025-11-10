$(document).ready(
    function() {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(onRequestStart)
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onRequestEnd)
    }
);

function onRequestStart() {
    $.blockUI({ message: "Procesando ...." });
}

function onRequestEnd() {
    $.unblockUI();
} 
