export { FormValidator }

class FormValidator {
    #Container
    #Fields

    constructor() {
    }

    Initialize(containerId) {
        this.#Container = document.getElementById(containerId);
        if (!this.#Container) {
            throw new Error(`Container with ID ${containerId} not found.`);
        }
        this.#Fields = Array.from(this.#Container.querySelectorAll('input, textarea, select'));
        this.#Fields.forEach(field => {
            if (field.classList.contains('number')) {
                field.addEventListener('input', this.#ValidateNumber.bind(this));
            }
        });
    }

    IsFormValid() {
        let isValid = true;

        this.#Fields.forEach(field => {
            if (field.classList.contains('required') && !field.value.trim()) {
                isValid = false;
                this.#ShowError(field, 'El campo es requerido.');
            } else {
                this.#ClearError(field);
            }

            if (field.classList.contains('number') && !this.#IsNumber(field.value)) {
                isValid = false;
                this.#ShowError(field, 'Solo se permiten numeros.');
            } else if (field.classList.contains('number')) {
                this.#ClearError(field);
            }
        });

        return isValid;
    }

    #ValidateNumber(event) {
        const field = event.target;
        if (!this.#IsNumber(field.value)) {
            this.#ShowError(field, 'Solo se permiten numeros.');
        } else {
            this.#ClearError(field);
        }
    }

    #IsNumber(value) {
        return /^\d+$/.test(value);
    }

    #ShowError(field, message) {
        field.classList.add('error');
        let errorElement = field.nextElementSibling;
        if (!errorElement || !errorElement.classList.contains('error-message')) {
            errorElement = document.createElement('span');
            errorElement.classList.add('error-message');
            field.parentNode.insertBefore(errorElement, field.nextSibling);
        }
        errorElement.textContent = message;
    }

    #ClearError(field) {
        field.classList.remove('error');
        const errorElement = field.nextElementSibling;
        if (errorElement && errorElement.classList.contains('error-message')) {
            errorElement.remove();
        }
    }
}

