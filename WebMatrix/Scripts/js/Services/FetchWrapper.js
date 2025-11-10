import { API_URL } from './settings.js'

export const FetchWrapper = {
    get,
    post,
    PostFormData,
    postReturnBlob,
    Delete,
    put,
    getReturnBlob,
}

function get({ URLPart }) {
    let requestOptions = {
        method: 'GET',
        headers: {
            "access-control-allow-origin": "*",
            'Content-type': 'application/json; charset=utf-8',
        }
    }
    return fetch(
        `${API_URL}/${URLPart}`,
        requestOptions
    ).then(res => res.json());
}
function post({ URLPart, params }) {
    const requestOptions = {
        method: 'POST',
        body: JSON.stringify(params),
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
        }
    }
    return fetch(
        `${API_URL}/${URLPart}`,
        requestOptions
    ).then(res => res.json())
        .then(res => res.d);
}

function postReturnBlob({ URLPart, params }) {
    const requestOptions = {
        method: 'POST',
        body: JSON.stringify(params),
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
        }
    }
    return fetch(
        `${API_URL}/${URLPart}`,
        requestOptions
    )
        .then(res => { return res.blob() })
}
function Delete({ URLPart, params }) {
    const requestOptions = {
        method: 'DELETE',
        body: JSON.stringify(params),
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
        }

    }
    return fetch(
        `${API_URL}/${URLPart}`,
        requestOptions
    )
        .then(res => res.json())
}
function put({ URLPart, params }) {
    const requestOptions = {
        method: 'PUT',
        body: JSON.stringify(params),
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
        }

    }
    return fetch(
        `${API_URL}/${URLPart}`,
        requestOptions
    )
        .then(res => res.json())
}
function getReturnBlob({ URLPart }) {
    const requestOptions = {
        method: 'GET',
        headers: {
        }
    }
    return fetch(
        `${API_URL}/${URLPart}`,
        requestOptions
    )
        .then(res => res.blob())
}
function PostFormData(URLPart, body) {
    const myHeaders = new Headers();
    myHeaders.append('Accept', 'application/json')
    myHeaders.delete('Content-Type')

    var formData = toFormData(body)

    const requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: formData,
    };

    return fetch(
        `${API_URL}/${URLPart}`,
        requestOptions
    ).then(res => res.json())
}
function toFormData(obj, form, namespace) {
    let fd = form || new FormData();
    let formKey;
    for (let property in obj) {
        if (obj.hasOwnProperty(property) && obj[property]) {
            if (namespace) {
                formKey = namespace + '[' + property + ']';
            } else {
                formKey = property;
            }

            // if the property is an object, but not a File, use recursivity.
            if (obj[property] instanceof Date) {
                fd.append(formKey, obj[property].toISOString());
            }
            else if (typeof obj[property] === 'object' && !(obj[property] instanceof File)) {
                toFormData(obj[property], fd, formKey);
            } else { // if it's a string or a File object
                fd.append(formKey, obj[property]);
            }
        }
    }
    return fd
}
