export const HelperService = {
    FormatExcelFileToBase64Async,
    Base64ToBytes,
    FromDateASPNetJsonFormatToDate
}

function FormatExcelFileToBase64Async(file) {
    const temporaryFileReader = new FileReader();

    return new Promise((resolve, reject) => {
        temporaryFileReader.onerror = () => {
            temporaryFileReader.abort();
            reject(new DOMException("Problem parsing input file."));
        };

        temporaryFileReader.onload = () => {
            resolve(temporaryFileReader.result.replace('data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,', ''));
        };
        temporaryFileReader.readAsDataURL(file);
    });
}
function Base64ToBytes(base64) {
    var s = window.atob(base64);
    var bytes = new Uint8Array(s.length);
    for (var i = 0; i < s.length; i++) {
        bytes[i] = s.charCodeAt(i);
    }
    return bytes;
};
function FromDateASPNetJsonFormatToDate(dateASPNetJsonFormat) {
    if (!dateASPNetJsonFormat.startsWith("/Date(")) {
        throw new Error("La cadena no comienza con '/Date('.");
    }

    if (!dateASPNetJsonFormat.endsWith(")/")) {
        throw new Error("La cadena no termina con ')/'.");
    }

    const timestamp = dateASPNetJsonFormat.slice(6, -2);
    const timestampNumber = parseInt(timestamp);
    if (isNaN(timestampNumber)) {
        throw new Error("El timestamp no es un número válido.");
    }

    return new Date(timestampNumber);
}