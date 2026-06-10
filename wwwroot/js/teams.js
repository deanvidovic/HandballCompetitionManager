document.addEventListener('DOMContentLoaded', function () {
    const modal = document.querySelector('[data-team-modal]');
    const openButton = document.querySelector('[data-team-modal-open]');
    const editButtons = document.querySelectorAll('[data-edit-team]');
    const closeButtons = document.querySelectorAll('[data-team-modal-close]');
    const form = document.querySelector('[data-team-create-form]');
    const status = document.querySelector('[data-team-form-status]');
    const modalEyebrow = modal?.querySelector('.team-modal-eyebrow');
    const modalTitle = modal?.querySelector('.team-modal-title');
    const modalDescription = modal?.querySelector('.team-modal-description');
    const coachInput = document.querySelector('[data-coach-input]');
    const coachResults = document.querySelector('[data-coach-results]');
    const coachAutocomplete = document.querySelector('[data-coach-autocomplete]');
    const coachClearButton = document.querySelector('[data-coach-clear]');
    let lastFocusedElement = null;
    let coachSearchTimer = null;
    let activeCoachIndex = -1;
    const validationMessages = {
        Name: {
            required: 'Team name is required.',
            invalid: 'Team name must be at least 3 characters long.'
        },
        HomeCity: {
            required: 'Home city is required.',
            invalid: 'Home city must be longer than 2 characters and cannot contain numbers.'
        },
        HomeArena: {
            required: 'Home arena is required.',
            invalid: 'Home arena must be longer than 3 characters.'
        },
        CoachName: {
            required: 'Select an existing coach from the suggestions.',
            invalid: 'Select an existing coach from the suggestions.'
        },
        FoundedYear: {
            required: 'Founded year is required.',
            invalid: 'Founded year can contain only numbers and cannot be greater than 2026.'
        }
    };

    if (!modal || (!openButton && editButtons.length === 0)) {
        return;
    }

    const focusableSelector = [
        'button:not([disabled])',
        'input:not([disabled])',
        'textarea:not([disabled])',
        'select:not([disabled])',
        'a[href]'
    ].join(',');

    const getFocusableElements = () => Array.from(modal.querySelectorAll(focusableSelector));
    const getValidationInput = fieldName => form?.querySelector(`[data-validate-field="${fieldName}"]`);
    const getValidationMessage = fieldName => form?.querySelector(`[data-validation-message-for="${fieldName}"]`);
    const getValidationField = fieldName => form?.querySelector(`[data-team-field="${fieldName}"]`);

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

        field?.classList.remove('has-error');
        if (input?.value.trim()) {
            field?.classList.add('has-valid');
        } else {
            field?.classList.remove('has-valid');
        }
        input?.setAttribute('aria-invalid', 'false');

        if (validationMessage) {
            validationMessage.textContent = '';
            validationMessage.hidden = true;
        }
    };

    const validateField = fieldName => {
        const input = getValidationInput(fieldName);

        if (!input) {
            return true;
        }

        const value = input.value.trim();

        if (!value) {
            setFieldError(fieldName, validationMessages[fieldName].required);
            return false;
        }

        if (fieldName === 'Name' && value.length < 3) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'HomeCity' && (value.length < 3 || /\d/.test(value))) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'HomeArena' && value.length < 4) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'CoachName' && !input.readOnly) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'FoundedYear' && (!/^\d+$/.test(value) || Number(value) > 2026)) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        clearFieldError(fieldName);
        return true;
    };

    const validateForm = () => ['Name', 'HomeCity', 'HomeArena', 'CoachName', 'FoundedYear']
        .map(validateField)
        .every(Boolean);

    const clearValidationState = () => {
        form?.querySelectorAll('[data-validation-message-for]').forEach(message => {
            message.textContent = '';
            message.hidden = true;
        });

        form?.querySelectorAll('[data-team-field]').forEach(field => {
            field.classList.remove('has-error', 'has-valid');
        });

        form?.querySelectorAll('[data-validate-field]').forEach(input => {
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

    const setInputValue = (fieldName, value) => {
        const input = getValidationInput(fieldName);

        if (input) {
            input.value = value ?? '';
        }
    };

    const setCreateMode = () => {
        if (!form) {
            return;
        }

        form.action = openButton?.dataset.createUrl || form.action;
        form.querySelector('[data-edit-id]').value = '0';
        form.reset();
        clearSelectedCoach();
        modalEyebrow && (modalEyebrow.textContent = 'New team');
        modalTitle && (modalTitle.textContent = 'Create Team');
        modalDescription && (modalDescription.textContent = 'Add the main team details.');
        form.querySelector('.team-modal-submit').textContent = 'Save Team';
    };

    const setEditMode = button => {
        if (!form) {
            return;
        }

        form.action = button.dataset.editUrl;
        form.querySelector('[data-edit-id]').value = button.dataset.id ?? '0';
        setInputValue('Name', button.dataset.name);
        setInputValue('HomeCity', button.dataset.homeCity);
        setInputValue('HomeArena', button.dataset.homeArena);
        setInputValue('CoachName', button.dataset.coachName);
        setInputValue('FoundedYear', button.dataset.foundedYear);
        selectCoach(button.dataset.coachName ?? '');
        modalEyebrow && (modalEyebrow.textContent = 'Edit team');
        modalTitle && (modalTitle.textContent = 'Edit Team');
        modalDescription && (modalDescription.textContent = 'Update team details.');
        form.querySelector('.team-modal-submit').textContent = 'Save changes';
    };

    const openModal = () => {
        lastFocusedElement = document.activeElement;
        modal.hidden = false;
        document.body.classList.add('team-modal-open');
        openButton?.setAttribute('aria-expanded', 'true');
        clearValidationState();

        window.requestAnimationFrame(() => {
            const firstInput = modal.querySelector('input, textarea');
            firstInput?.focus();
        });
    };

    const closeModal = () => {
        modal.hidden = true;
        document.body.classList.remove('team-modal-open');
        openButton?.setAttribute('aria-expanded', 'false');
        status.hidden = true;
        clearValidationState();
        form?.reset();
        clearSelectedCoach();
        lastFocusedElement?.focus();
    };

    const hideCoachResults = () => {
        if (!coachInput || !coachResults) {
            return;
        }

        coachResults.hidden = true;
        coachResults.innerHTML = '';
        coachInput.setAttribute('aria-expanded', 'false');
        coachInput.removeAttribute('aria-activedescendant');
        activeCoachIndex = -1;
    };

    const setCoachResultsMessage = message => {
        if (!coachResults || !coachInput) {
            return;
        }

        coachResults.innerHTML = `<div class="team-coach-empty">${message}</div>`;
        coachResults.hidden = false;
        coachInput.setAttribute('aria-expanded', 'true');
    };

    const setActiveCoachOption = index => {
        const options = Array.from(coachResults?.querySelectorAll('[data-coach-option]') ?? []);

        if (options.length === 0) {
            activeCoachIndex = -1;
            return;
        }

        activeCoachIndex = (index + options.length) % options.length;

        options.forEach((option, optionIndex) => {
            const isActive = optionIndex === activeCoachIndex;
            option.classList.toggle('is-active', isActive);
            option.setAttribute('aria-selected', isActive.toString());
        });

        coachInput?.setAttribute('aria-activedescendant', options[activeCoachIndex].id);
    };

    const selectCoach = value => {
        if (!coachInput || !coachAutocomplete) {
            return;
        }

        coachInput.value = value;
        coachInput.readOnly = true;
        coachAutocomplete.classList.add('is-selected');
        if (coachClearButton) {
            coachClearButton.hidden = false;
        }
        hideCoachResults();
        clearFieldError('CoachName');
        coachInput.focus();
    };

    const clearSelectedCoach = () => {
        if (!coachInput || !coachAutocomplete) {
            return;
        }

        coachInput.value = '';
        coachInput.readOnly = false;
        coachAutocomplete.classList.remove('is-selected');
        if (coachClearButton) {
            coachClearButton.hidden = true;
        }
        hideCoachResults();
        clearFieldError('CoachName');
        coachInput.focus();
    };

    const escapeHtml = value => String(value ?? '')
        .replaceAll('&', '&amp;')
        .replaceAll('<', '&lt;')
        .replaceAll('>', '&gt;')
        .replaceAll('"', '&quot;')
        .replaceAll("'", '&#039;');

    const renderCoachResults = coaches => {
        if (!coachResults || !coachInput) {
            return;
        }

        if (coaches.length === 0) {
            setCoachResultsMessage('No coaches found.');
            return;
        }

        coachResults.innerHTML = coaches.map((coach, index) => `
            <button type="button" class="team-coach-option" id="coach-option-${index}" role="option" data-coach-option data-coach-value="${escapeHtml(coach.value)}">
                <span class="team-coach-option-label">${escapeHtml(coach.label)}</span>
                <span class="team-coach-option-meta">${escapeHtml(coach.meta)}</span>
            </button>
        `).join('');

        coachResults.hidden = false;
        coachInput.setAttribute('aria-expanded', 'true');
        activeCoachIndex = -1;
    };

    const fetchCoachSuggestions = () => {
        if (!coachInput || !coachResults) {
            return;
        }

        if (coachInput.readOnly) {
            hideCoachResults();
            return;
        }

        const url = coachInput.dataset.coachAutocompleteUrl;
        const query = coachInput.value.trim();

        if (!url) {
            return;
        }

        window.clearTimeout(coachSearchTimer);
        coachSearchTimer = window.setTimeout(async () => {
            setCoachResultsMessage('Searching coaches...');

            try {
                const response = await fetch(`${url}?query=${encodeURIComponent(query)}`, {
                    headers: {
                        'Accept': 'application/json'
                    }
                });

                if (!response.ok) {
                    throw new Error('Coach autocomplete request failed.');
                }

                const coaches = await response.json();
                renderCoachResults(coaches);
            } catch {
                setCoachResultsMessage('Coach suggestions are unavailable.');
            }
        }, 220);
    };

    const searchCoachAgain = () => {
        clearFieldError('CoachName');
        fetchCoachSuggestions();
    };

    openButton?.addEventListener('click', () => {
        setCreateMode();
        openModal();
    });

    editButtons.forEach(button => {
        button.addEventListener('click', () => {
            setEditMode(button);
            openModal();
        });
    });

    closeButtons.forEach(button => {
        button.addEventListener('click', closeModal);
    });

    modal.addEventListener('keydown', event => {
        if (event.key === 'Escape') {
            if (coachResults && !coachResults.hidden) {
                hideCoachResults();
                return;
            }

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

    form?.addEventListener('submit', async event => {
        event.preventDefault();
        const submitButton = form.querySelector('.team-modal-submit');
        const originalSubmitText = submitButton?.textContent;

        if (!validateForm()) {
            status.textContent = 'Please fix the highlighted fields.';
            status.classList.remove('is-success');
            status.classList.add('is-error');
            status.hidden = false;
            form.querySelector('.has-error [data-validate-field]')?.focus();
            return;
        }

        if (submitButton) {
            submitButton.disabled = true;
            submitButton.textContent = 'Saving...';
        }

        status.textContent = 'Creating team...';
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
                    : 'Team could not be created.';

                status.textContent = errors;
                status.classList.add('is-error');
                applyServerFieldErrors(result.fieldErrors);
                window.AppNotifications?.error(errors, { duration: 3000 });
                return;
            }

            closeModal();
            if (window.AppNotifications) {
                window.AppNotifications.success(result.message ?? 'Team created successfully.', {
                    onClose: () => window.location.reload()
                });
            } else {
                window.location.reload();
            }
        } catch {
            status.textContent = 'Team could not be created. Please try again.';
            status.classList.add('is-error');
            window.AppNotifications?.error('Team could not be created. Please try again.', { duration: 3000 });
        } finally {
            if (submitButton) {
                submitButton.disabled = false;
                submitButton.textContent = originalSubmitText;
            }
        }
    });

    form?.querySelectorAll('[data-validate-field]').forEach(input => {
        input.addEventListener('blur', () => {
            validateField(input.dataset.validateField);
        });

        input.addEventListener('input', () => {
            const field = getValidationField(input.dataset.validateField);

            if (field?.classList.contains('has-error')) {
                validateField(input.dataset.validateField);
            }
        });
    });

    coachInput?.addEventListener('input', searchCoachAgain);

    coachInput?.addEventListener('focus', searchCoachAgain);

    coachInput?.addEventListener('keydown', event => {
        const options = Array.from(coachResults?.querySelectorAll('[data-coach-option]') ?? []);

        if (event.key === 'ArrowDown') {
            event.preventDefault();
            setActiveCoachOption(activeCoachIndex + 1);
        } else if (event.key === 'ArrowUp') {
            event.preventDefault();
            setActiveCoachOption(activeCoachIndex - 1);
        } else if (event.key === 'Enter' && activeCoachIndex >= 0 && options[activeCoachIndex]) {
            event.preventDefault();
            selectCoach(options[activeCoachIndex].dataset.coachValue);
        }
    });

    coachResults?.addEventListener('click', event => {
        const option = event.target.closest('[data-coach-option]');

        if (!option) {
            return;
        }

        selectCoach(option.dataset.coachValue);
    });

    coachClearButton?.addEventListener('click', clearSelectedCoach);

    document.addEventListener('click', event => {
        if (!coachAutocomplete?.contains(event.target)) {
            hideCoachResults();
        }
    });
});
