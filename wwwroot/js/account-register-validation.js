document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector('[data-register-form]');

    if (!form) {
        return;
    }

    const fields = ['Username', 'DisplayName', 'Email', 'OIB', 'JMBG', 'Password', 'ConfirmPassword'];
    const messages = {
        Username: {
            required: 'Username is required.',
            invalid: 'Username must be at least 3 characters and can contain only letters, numbers, dots, underscores, and dashes.'
        },
        DisplayName: {
            required: 'Display name is required.',
            invalid: 'Display name must be at least 3 characters long.'
        },
        Email: {
            required: 'Email is required.',
            invalid: 'Enter a valid email address.'
        },
        OIB: {
            required: 'OIB is required.',
            invalid: 'OIB must contain exactly 11 numbers.'
        },
        JMBG: {
            required: 'JMBG is required.',
            invalid: 'JMBG must contain exactly 13 numbers.'
        },
        Password: {
            required: 'Password is required.',
            invalid: 'Password must be at least 8 characters and include uppercase, lowercase, number, and special character.'
        },
        ConfirmPassword: {
            required: 'Confirm password is required.',
            invalid: 'Passwords do not match.'
        }
    };

    const getInput = fieldName => form.querySelector(`[data-register-validate-field="${fieldName}"]`);
    const getField = fieldName => form.querySelector(`[data-register-field="${fieldName}"]`);
    const getMessage = fieldName => form.querySelector(`[data-register-validation-message-for="${fieldName}"]`);

    const setFieldError = (fieldName, message) => {
        const field = getField(fieldName);
        const input = getInput(fieldName);
        const validationMessage = getMessage(fieldName);

        field?.classList.add('has-error');
        field?.classList.remove('has-valid');
        input?.setAttribute('aria-invalid', 'true');

        if (validationMessage) {
            validationMessage.textContent = message;
            validationMessage.hidden = false;
        }
    };

    const clearFieldError = fieldName => {
        const field = getField(fieldName);
        const input = getInput(fieldName);
        const validationMessage = getMessage(fieldName);
        const hasValue = Boolean(input?.value.trim());

        field?.classList.remove('has-error');
        field?.classList.toggle('has-valid', hasValue);
        input?.setAttribute('aria-invalid', 'false');

        if (validationMessage) {
            validationMessage.textContent = '';
            validationMessage.hidden = true;
        }
    };

    const isValidEmail = value => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
    const isStrongPassword = value =>
        value.length >= 8 &&
        /[a-z]/.test(value) &&
        /[A-Z]/.test(value) &&
        /\d/.test(value) &&
        /[^A-Za-z0-9]/.test(value);

    const validateField = fieldName => {
        const input = getInput(fieldName);
        const value = input?.value.trim() ?? '';

        if (!value) {
            setFieldError(fieldName, messages[fieldName].required);
            return false;
        }

        if (fieldName === 'DisplayName' && value.length < 3) {
            setFieldError(fieldName, messages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'Username' && (value.length < 3 || !/^[A-Za-z0-9._-]+$/.test(value))) {
            setFieldError(fieldName, messages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'Email' && !isValidEmail(value)) {
            setFieldError(fieldName, messages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'OIB' && !/^[0-9]{11}$/.test(value)) {
            setFieldError(fieldName, messages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'JMBG' && !/^[0-9]{13}$/.test(value)) {
            setFieldError(fieldName, messages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'Password' && !isStrongPassword(value)) {
            setFieldError(fieldName, messages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'ConfirmPassword' && value !== (getInput('Password')?.value ?? '')) {
            setFieldError(fieldName, messages[fieldName].invalid);
            return false;
        }

        clearFieldError(fieldName);
        return true;
    };

    const validateForm = () => fields.map(validateField).every(Boolean);

    form.addEventListener('submit', event => {
        if (validateForm()) {
            return;
        }

        event.preventDefault();
        const firstErrorField = form.querySelector('.has-error');
        firstErrorField?.querySelector('[data-register-validate-field]')?.focus();
    });

    fields.forEach(fieldName => {
        const input = getInput(fieldName);

        input?.addEventListener('blur', () => validateField(fieldName));
        input?.addEventListener('input', () => {
            if (getField(fieldName)?.classList.contains('has-error')) {
                validateField(fieldName);
            }

            if (fieldName === 'Password' && getInput('ConfirmPassword')?.value) {
                validateField('ConfirmPassword');
            }
        });
    });
});
