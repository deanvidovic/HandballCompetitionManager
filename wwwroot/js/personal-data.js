document.addEventListener('DOMContentLoaded', function () {
    const modal = document.querySelector('[data-personal-data-modal]');
    const openButton = document.querySelector('[data-personal-data-modal-open]');
    const closeButtons = document.querySelectorAll('[data-personal-data-modal-close]');
    const form = document.querySelector('[data-personal-data-form]');
    const status = document.querySelector('[data-personal-data-form-status]');
    let lastFocusedElement = null;

    if (!modal || !openButton || !form || !status) {
        return;
    }

    const validationMessages = {
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
            invalid: 'OIB must contain exactly 11 digits.'
        },
        JMBG: {
            required: 'JMBG is required.',
            invalid: 'JMBG must contain exactly 13 digits.'
        },
        DateOfBirth: {
            invalid: 'Date of birth cannot be after today.'
        }
    };

    const fields = ['DisplayName', 'Email', 'OIB', 'JMBG', 'DateOfBirth'];
    const focusableSelector = 'button:not([disabled]), input:not([disabled]), select:not([disabled]), textarea:not([disabled]), a[href]';
    const getInput = fieldName => form.querySelector(`[data-personal-data-validate-field="${fieldName}"]`);
    const getField = fieldName => form.querySelector(`[data-personal-data-field="${fieldName}"]`);
    const getMessage = fieldName => form.querySelector(`[data-personal-data-validation-message-for="${fieldName}"]`);

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

        field?.classList.remove('has-error');
        field?.classList.toggle('has-valid', Boolean(input?.value.trim()));
        input?.setAttribute('aria-invalid', 'false');

        if (validationMessage) {
            validationMessage.textContent = '';
            validationMessage.hidden = true;
        }
    };

    const isValidEmail = value => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
    const isAfterToday = value => {
        const date = new Date(value);
        const today = new Date(new Date().toDateString());
        return date.toString() !== 'Invalid Date' && date > today;
    };

    const validateField = fieldName => {
        const value = getInput(fieldName)?.value.trim() ?? '';

        if (!value && fieldName !== 'DateOfBirth') {
            setFieldError(fieldName, validationMessages[fieldName].required);
            return false;
        }

        if (!value) {
            clearFieldError(fieldName);
            return true;
        }

        if (fieldName === 'DisplayName' && value.length < 3) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'Email' && !isValidEmail(value)) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'OIB' && !/^[0-9]{11}$/.test(value)) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'JMBG' && !/^[0-9]{13}$/.test(value)) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'DateOfBirth' && isAfterToday(value)) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        clearFieldError(fieldName);
        return true;
    };

    const clearValidation = () => {
        fields.forEach(clearFieldError);
        status.hidden = true;
        status.textContent = '';
        status.classList.remove('is-error', 'is-success');
    };

    const openModal = () => {
        lastFocusedElement = document.activeElement;
        modal.hidden = false;
        document.body.classList.add('appuser-modal-open');
        clearValidation();
        window.requestAnimationFrame(() => form.querySelector('input')?.focus());
    };

    const closeModal = () => {
        modal.hidden = true;
        document.body.classList.remove('appuser-modal-open');
        clearValidation();
        lastFocusedElement?.focus();
    };

    const applyServerFieldErrors = fieldErrors => {
        if (!fieldErrors || typeof fieldErrors !== 'object') {
            return;
        }

        Object.entries(fieldErrors).forEach(([fieldName, messages]) => {
            if (Array.isArray(messages) && messages.length > 0) {
                setFieldError(fieldName, messages.join(' '));
            }
        });
    };

    openButton.addEventListener('click', openModal);
    closeButtons.forEach(button => button.addEventListener('click', closeModal));

    modal.addEventListener('keydown', event => {
        if (event.key === 'Escape') {
            closeModal();
            return;
        }

        if (event.key !== 'Tab') {
            return;
        }

        const focusableElements = Array.from(modal.querySelectorAll(focusableSelector));
        const firstElement = focusableElements[0];
        const lastElement = focusableElements[focusableElements.length - 1];

        if (event.shiftKey && document.activeElement === firstElement) {
            event.preventDefault();
            lastElement?.focus();
        } else if (!event.shiftKey && document.activeElement === lastElement) {
            event.preventDefault();
            firstElement?.focus();
        }
    });

    fields.forEach(fieldName => {
        const input = getInput(fieldName);
        input?.addEventListener('blur', () => validateField(fieldName));
        input?.addEventListener('input', () => {
            if (getField(fieldName)?.classList.contains('has-error')) {
                validateField(fieldName);
            }
        });
    });

    form.addEventListener('submit', async event => {
        event.preventDefault();
        const submitButton = form.querySelector('.appuser-modal-submit');
        const originalSubmitText = submitButton?.textContent;

        if (!fields.map(validateField).every(Boolean)) {
            status.textContent = 'Please fix the highlighted fields.';
            status.classList.add('is-error');
            status.hidden = false;
            form.querySelector('.has-error [data-date-picker-trigger], .has-error [data-personal-data-validate-field]')?.focus();
            return;
        }

        if (submitButton) {
            submitButton.disabled = true;
            submitButton.textContent = 'Saving...';
        }

        status.textContent = 'Saving personal data...';
        status.classList.remove('is-error', 'is-success');
        status.hidden = false;

        try {
            const response = await fetch(form.action, {
                method: 'POST',
                body: new FormData(form),
                headers: {
                    'Accept': 'application/json'
                }
            });
            const result = await response.json();

            if (!response.ok || !result.success) {
                const errors = Array.isArray(result.errors) && result.errors.length > 0
                    ? result.errors.join(' ')
                    : 'Personal data could not be updated.';

                status.textContent = errors;
                status.classList.add('is-error');
                applyServerFieldErrors(result.fieldErrors);
                window.AppNotifications?.error(errors, { duration: 3000 });
                return;
            }

            closeModal();
            if (window.AppNotifications) {
                window.AppNotifications.success(result.message ?? 'Personal data updated successfully.', {
                    onClose: () => window.location.reload()
                });
            } else {
                window.location.reload();
            }
        } catch {
            status.textContent = 'Personal data could not be updated. Please try again.';
            status.classList.add('is-error');
            window.AppNotifications?.error('Personal data could not be updated. Please try again.', { duration: 3000 });
        } finally {
            if (submitButton) {
                submitButton.disabled = false;
                submitButton.textContent = originalSubmitText;
            }
        }
    });
});
