function uploadError(sender, args) {
    $('#html_lblUploadResult').html(String.format('Error al cargar el archivo: {0}', args.get_errorMessage()));
}

function uploadComplete(sender, args) {
    var contentType = args.get_contentType();
    var sizeFile = parseInt(args.get_length()) / 1024;
    var maxSizeFile = 10000;
    var fileNameUpload = args.get_fileName();
    var extFile = fRight(fileNameUpload, 4);
    var strErrorUpload = "<span style='color:red;'>No se pudo cargar el archivo por los siguientes motivos:</span><br/>";
    var errorUpload = false;

    if (contentType.length > 0){
        if (sizeFile > maxSizeFile) {
            errorUpload = true;
            strErrorUpload += String.format("El tamaño del archivo es {0} MB, el cual supera el valor maximo permitido que es {1} MB<br/>", (sizeFile / 1024).toFixed(2), (maxSizeFile / 1024).toFixed(2)) //Tamaños en MB
        }
        if (!errorUpload) {
                $('#html_lblUploadResult').html(String.format('Nombre Archivo: {3}{0}{4}<br/>Tamaño Archivo: {3}{1} MB{4}<br/>Tipo Archivo: {3}{2}{4}', fileNameUpload, (sizeFile / 1024).toFixed(2), contentType, "<span style='font-weight:normal;'>", '</span>'));

        }
    }

    if (errorUpload) { 
        $('#html_lblUploadResult').html(strErrorUpload); 
    }
}
