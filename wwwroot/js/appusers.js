document.addEventListener('DOMContentLoaded', function () {
    const modal = document.querySelector('[data-appuser-modal]');
    const openButton = document.querySelector('[data-appuser-modal-open]');
    const closeButtons = document.querySelectorAll('[data-appuser-modal-close]');
    const form = document.querySelector('[data-appuser-create-form]');
    const status = document.querySelector('[data-appuser-form-status]');
    let lastFocusedElement = null;

    const validationMessages = {
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
        Role: {
            required: 'Select a role.',
            invalid: 'Select a role.'
        },
        DateOfBirth: {
            required: 'Date of birth is required.',
            invalid: 'Date of birth cannot be after today.'
        }
    };

    if (!modal || !openButton || !form) {
        return;
    }

    const focusableSelector = [
        'button:not([disabled])',
        'input:not([disabled])',
        'select:not([disabled])',
        'textarea:not([disabled])',
        'a[href]'
    ].join(',');

    const fields = ['Username', 'DisplayName', 'Email', 'Role', 'DateOfBirth'];
    const getFocusableElements = () => Array.from(modal.querySelectorAll(focusableSelector));
    const getValidationInput = fieldName => form.querySelector(`[data-appuser-validate-field="${fieldName}"]`);
    const getValidationMessage = fieldName => form.querySelector(`[data-appuser-validation-message-for="${fieldName}"]`);
    const getValidationField = fieldName => form.querySelector(`[data-appuser-field="${fieldName}"]`);

    const setFieldError = (fieldName, message) => {
        const field = getValidationField(fieldName);
        const input = getValidationInput(fieldName);
        const validationMessage = getValidationMessage(fieldName);

        field?.classList.add('has-error');
        field?.classList.remove('has-valid');
        input?.setAttribute('aria-invalid', 'true');

        if (validationMessage) {
            validationMessage.textContent = message;
            validationMessage.hidden = false;
        }
    };

    const clearFieldError = fieldName => {
        const field = getValidationField(fieldName);
        const input = getValidationInput(fieldName);
        const validationMessage = getValidationMessage(fieldName);
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
    const isAfterToday = value => {
        const date = new Date(value);
        const today = new Date(new Date().toDateString());
        return date.toString() !== 'Invalid Date' && date > today;
    };

    const validateField = fieldName => {
        const input = getValidationInput(fieldName);
        const value = input?.value.trim() ?? '';

        if (!value) {
            setFieldError(fieldName, validationMessages[fieldName].required);
            return false;
        }

        if (fieldName === 'Username' && (value.length < 3 || !/^[A-Za-z0-9._-]+$/.test(value))) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'DisplayName' && value.length < 3) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'Email' && !isValidEmail(value)) {
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

    const validateForm = () => fields.map(validateField).every(Boolean);

    const clearValidationState = () => {
        form.querySelectorAll('[data-appuser-validation-message-for]').forEach(message => {
            message.textContent = '';
            message.hidden = true;
        });

        form.querySelectorAll('[data-appuser-field]').forEach(field => {
            field.classList.remove('has-error', 'has-valid');
        });

        form.querySelectorAll('[data-appuser-validate-field]').forEach(input => {
            input.setAttribute('aria-invalid', 'false');
        });
    };

    const applyServerFieldErrors = fieldErrors => {
        if (!fieldErrors || typeof fieldErrors !== 'object') {
            return;
        }

        Object.entries(fieldErrors).forEach(([fieldName, messages]) => {
            if (!Array.isArray(messages) || messages.length === 0) {
                return;
            }

            setFieldError(fieldName, messages.join(' '));
        });
    };

    const openModal = () => {
        lastFocusedElement = document.activeElement;
        modal.hidden = false;
        document.body.classList.add('appuser-modal-open');
        openButton.setAttribute('aria-expanded', 'true');
        clearValidationState();

        window.requestAnimationFrame(() => {
            modal.querySelector('input, select, textarea')?.focus();
        });
    };

    const closeModal = () => {
        modal.hidden = true;
        document.body.classList.remove('appuser-modal-open');
        openButton.setAttribute('aria-expanded', 'false');
        status.hidden = true;
        clearValidationState();
        form.reset();
        lastFocusedElement?.focus();
    };

    openButton.addEventListener('click', openModal);

    closeButtons.forEach(button => {
        button.addEventListener('click', closeModal);
    });

    modal.addEventListener('keydown', event => {
        if (event.key === 'Escape') {
            closeModal();
            return;
        }

        if (event.key !== 'Tab') {
            return;
        }

        const focusableElements = getFocusableElements();
        const firstElement = focusableElements[0];
        const lastElement = focusableElements[focusableElements.length - 1];

        if (!firstElement || !lastElement) {
            return;
        }

        if (event.shiftKey && document.activeElement === firstElement) {
            event.preventDefault();
            lastElement.focus();
        } else if (!event.shiftKey && document.activeElement === lastElement) {
            event.preventDefault();
            firstElement.focus();
        }
    });

    form.addEventListener('submit', async event => {
        event.preventDefault();
        const submitButton = form.querySelector('.appuser-modal-submit');
        const originalSubmitText = submitButton?.textContent;

        if (!validateForm()) {
            status.textContent = 'Please fix the highlighted fields.';
            status.classList.remove('is-success');
            status.classList.add('is-error');
            status.hidden = false;
            const firstErrorField = form.querySelector('.has-error');
            firstErrorField?.querySelector('[data-date-picker-trigger], [data-appuser-validate-field]')?.focus();
            return;
        }

        if (submitButton) {
            submitButton.disabled = true;
            submitButton.textContent = 'Saving...';
        }

        status.textContent = 'Creating app user...';
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
                    : 'App user could not be created.';

                status.textContent = errors;
                status.classList.add('is-error');
                applyServerFieldErrors(result.fieldErrors);
                window.AppNotifications?.error(errors, { duration: 3000 });
                return;
            }

            closeModal();
            if (window.AppNotifications) {
                window.AppNotifications.success(result.message ?? 'App user created successfully.', {
                    onClose: () => window.location.reload()
                });
            } else {
                window.location.reload();
            }
        } catch {
            status.textContent = 'App user could not be created. Please try again.';
            status.classList.add('is-error');
            window.AppNotifications?.error('App user could not be created. Please try again.', { duration: 3000 });
        } finally {
            if (submitButton) {
                submitButton.disabled = false;
                submitButton.textContent = originalSubmitText;
            }
        }
    });

    form.querySelectorAll('[data-appuser-validate-field]').forEach(input => {
        input.addEventListener('blur', () => {
            validateField(input.dataset.appuserValidateField);
        });

        input.addEventListener('input', () => {
            const fieldName = input.dataset.appuserValidateField;
            const field = getValidationField(fieldName);

            if (field?.classList.contains('has-error')) {
                validateField(fieldName);
            }
        });
    });
});
