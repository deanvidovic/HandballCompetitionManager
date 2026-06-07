document.addEventListener('DOMContentLoaded', function () {
    const modal = document.querySelector('[data-player-modal]');
    const openButton = document.querySelector('[data-player-modal-open]');
    const closeButtons = document.querySelectorAll('[data-player-modal-close]');
    const form = document.querySelector('[data-player-create-form]');
    const status = document.querySelector('[data-player-form-status]');
    const teamInput = document.querySelector('[data-player-team-input]');
    const teamIdInput = document.querySelector('[data-player-team-id]');
    const teamResults = document.querySelector('[data-player-team-results]');
    const teamAutocomplete = document.querySelector('[data-player-team-autocomplete]');
    const teamClearButton = document.querySelector('[data-player-team-clear]');
    let lastFocusedElement = null;
    let teamSearchTimer = null;
    let activeTeamIndex = -1;

    const validationMessages = {
        FirstName: {
            required: 'First name is required.',
            invalid: 'First name must be at least 2 characters long and cannot contain numbers.'
        },
        LastName: {
            required: 'Last name is required.',
            invalid: 'Last name must be at least 2 characters long and cannot contain numbers.'
        },
        BirthDate: {
            required: 'Birth date is required.',
            invalid: 'Birth date cannot be after today.'
        },
        JerseyNumber: {
            required: 'Jersey number is required.',
            invalid: 'Jersey number must be between 1 and 99.'
        },
        Position: {
            required: 'Select a position.',
            invalid: 'Select a position.'
        },
        TeamId: {
            required: 'Select an existing team from the suggestions.',
            invalid: 'Select an existing team from the suggestions.'
        },
        GoalsScored: {
            required: 'Goals scored is required.',
            invalid: 'Goals scored must be zero or a positive number.'
        },
        Assists: {
            required: 'Assists are required.',
            invalid: 'Assists must be zero or a positive number.'
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

    const fields = ['FirstName', 'LastName', 'BirthDate', 'JerseyNumber', 'Position', 'TeamId', 'GoalsScored', 'Assists'];
    const getFocusableElements = () => Array.from(modal.querySelectorAll(focusableSelector));
    const getValidationInput = fieldName => form.querySelector(`[data-player-validate-field="${fieldName}"]`);
    const getValidationMessage = fieldName => form.querySelector(`[data-player-validation-message-for="${fieldName}"]`);
    const getValidationField = fieldName => form.querySelector(`[data-player-field="${fieldName}"]`);

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
        const hasValue = fieldName === 'TeamId'
            ? Boolean(teamIdInput?.value)
            : Boolean(input?.value.trim());

        field?.classList.remove('has-error');
        field?.classList.toggle('has-valid', hasValue);
        input?.setAttribute('aria-invalid', 'false');

        if (validationMessage) {
            validationMessage.textContent = '';
            validationMessage.hidden = true;
        }
    };

    const isWholeNumber = value => /^\d+$/.test(value);

    const validateField = fieldName => {
        const input = getValidationInput(fieldName);
        const value = input?.value.trim() ?? '';

        if (fieldName === 'TeamId') {
            if (!teamIdInput?.value) {
                setFieldError(fieldName, validationMessages[fieldName].required);
                return false;
            }

            clearFieldError(fieldName);
            return true;
        }

        if (!value) {
            setFieldError(fieldName, validationMessages[fieldName].required);
            return false;
        }

        if ((fieldName === 'FirstName' || fieldName === 'LastName') && (value.length < 2 || /\d/.test(value))) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'BirthDate' && new Date(value) > new Date(new Date().toDateString())) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'JerseyNumber' && (!isWholeNumber(value) || Number(value) < 1 || Number(value) > 99)) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        if ((fieldName === 'GoalsScored' || fieldName === 'Assists') && !isWholeNumber(value)) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        clearFieldError(fieldName);
        return true;
    };

    const validateForm = () => fields.map(validateField).every(Boolean);

    const clearValidationState = () => {
        form.querySelectorAll('[data-player-validation-message-for]').forEach(message => {
            message.textContent = '';
            message.hidden = true;
        });

        form.querySelectorAll('[data-player-field]').forEach(field => {
            field.classList.remove('has-error', 'has-valid');
        });

        form.querySelectorAll('[data-player-validate-field]').forEach(input => {
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
        document.body.classList.add('player-modal-open');
        openButton.setAttribute('aria-expanded', 'true');
        clearValidationState();

        window.requestAnimationFrame(() => {
            modal.querySelector('input, select, textarea')?.focus();
        });
    };

    const hideTeamResults = () => {
        if (!teamInput || !teamResults) {
            return;
        }

        teamResults.hidden = true;
        teamResults.innerHTML = '';
        teamInput.setAttribute('aria-expanded', 'false');
        teamInput.removeAttribute('aria-activedescendant');
        activeTeamIndex = -1;
    };

    const clearSelectedTeam = () => {
        if (!teamInput || !teamAutocomplete || !teamIdInput) {
            return;
        }

        teamInput.value = '';
        teamIdInput.value = '';
        teamInput.readOnly = false;
        teamAutocomplete.classList.remove('is-selected');
        if (teamClearButton) {
            teamClearButton.hidden = true;
        }
        hideTeamResults();
        clearFieldError('TeamId');
        teamInput.focus();
    };

    const closeModal = () => {
        modal.hidden = true;
        document.body.classList.remove('player-modal-open');
        openButton.setAttribute('aria-expanded', 'false');
        status.hidden = true;
        clearValidationState();
        form.reset();
        clearSelectedTeam();
        form.querySelector('[name="GoalsScored"]').value = '0';
        form.querySelector('[name="Assists"]').value = '0';
        lastFocusedElement?.focus();
    };

    const setTeamResultsMessage = message => {
        if (!teamResults || !teamInput) {
            return;
        }

        teamResults.innerHTML = `<div class="player-team-empty">${message}</div>`;
        teamResults.hidden = false;
        teamInput.setAttribute('aria-expanded', 'true');
    };

    const escapeHtml = value => String(value ?? '')
        .replaceAll('&', '&amp;')
        .replaceAll('<', '&lt;')
        .replaceAll('>', '&gt;')
        .replaceAll('"', '&quot;')
        .replaceAll("'", '&#039;');

    const renderTeamResults = teams => {
        if (!teamResults || !teamInput) {
            return;
        }

        if (teams.length === 0) {
            setTeamResultsMessage('No teams found.');
            return;
        }

        teamResults.innerHTML = teams.map((team, index) => `
            <button type="button" class="player-team-option" id="player-team-option-${index}" role="option" data-player-team-option data-player-team-id="${team.id}" data-player-team-value="${escapeHtml(team.value)}">
                <span class="player-team-option-label">${escapeHtml(team.label)}</span>
                <span class="player-team-option-meta">${escapeHtml(team.meta)}</span>
            </button>
        `).join('');

        teamResults.hidden = false;
        teamInput.setAttribute('aria-expanded', 'true');
        activeTeamIndex = -1;
    };

    const fetchTeamSuggestions = () => {
        if (!teamInput || !teamResults) {
            return;
        }

        if (teamInput.readOnly) {
            hideTeamResults();
            return;
        }

        const url = teamInput.dataset.playerTeamAutocompleteUrl;
        const query = teamInput.value.trim();

        if (!url) {
            return;
        }

        window.clearTimeout(teamSearchTimer);
        teamSearchTimer = window.setTimeout(async () => {
            setTeamResultsMessage('Searching teams...');

            try {
                const response = await fetch(`${url}?query=${encodeURIComponent(query)}`, {
                    headers: {
                        'Accept': 'application/json'
                    }
                });

                if (!response.ok) {
                    throw new Error('Team autocomplete request failed.');
                }

                const teams = await response.json();
                renderTeamResults(teams);
            } catch {
                setTeamResultsMessage('Team suggestions are unavailable.');
            }
        }, 220);
    };

    const searchTeamAgain = () => {
        if (teamInput?.readOnly) {
            hideTeamResults();
            return;
        }

        if (teamIdInput) {
            teamIdInput.value = '';
        }
        clearFieldError('TeamId');
        fetchTeamSuggestions();
    };

    const setActiveTeamOption = index => {
        const options = Array.from(teamResults?.querySelectorAll('[data-player-team-option]') ?? []);

        if (options.length === 0) {
            activeTeamIndex = -1;
            return;
        }

        activeTeamIndex = (index + options.length) % options.length;

        options.forEach((option, optionIndex) => {
            const isActive = optionIndex === activeTeamIndex;
            option.classList.toggle('is-active', isActive);
            option.setAttribute('aria-selected', isActive.toString());
        });

        teamInput?.setAttribute('aria-activedescendant', options[activeTeamIndex].id);
    };

    const selectTeam = option => {
        if (!teamInput || !teamAutocomplete || !teamIdInput) {
            return;
        }

        teamInput.value = option.dataset.playerTeamValue;
        teamIdInput.value = option.dataset.playerTeamId;
        teamInput.readOnly = true;
        teamAutocomplete.classList.add('is-selected');
        if (teamClearButton) {
            teamClearButton.hidden = false;
        }
        hideTeamResults();
        clearFieldError('TeamId');
        teamInput.focus();
    };

    openButton.addEventListener('click', openModal);

    closeButtons.forEach(button => {
        button.addEventListener('click', closeModal);
    });

    modal.addEventListener('keydown', event => {
        if (event.key === 'Escape') {
            if (teamResults && !teamResults.hidden) {
                hideTeamResults();
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

    form.addEventListener('submit', async event => {
        event.preventDefault();
        const submitButton = form.querySelector('.player-modal-submit');
        const originalSubmitText = submitButton?.textContent;

        if (!validateForm()) {
            status.textContent = 'Please fix the highlighted fields.';
            status.classList.remove('is-success');
            status.classList.add('is-error');
            status.hidden = false;
            const firstErrorField = form.querySelector('.has-error');
            firstErrorField?.querySelector('[data-date-picker-trigger], [data-player-validate-field]')?.focus();
            return;
        }

        if (submitButton) {
            submitButton.disabled = true;
            submitButton.textContent = 'Saving...';
        }

        status.textContent = 'Creating player...';
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
                    : 'Player could not be created.';

                status.textContent = errors;
                status.classList.add('is-error');
                applyServerFieldErrors(result.fieldErrors);
                window.AppNotifications?.error(errors, { duration: 3000 });
                return;
            }

            closeModal();
            if (window.AppNotifications) {
                window.AppNotifications.success(result.message ?? 'Player created successfully.', {
                    onClose: () => window.location.reload()
                });
            } else {
                window.location.reload();
            }
        } catch {
            status.textContent = 'Player could not be created. Please try again.';
            status.classList.add('is-error');
            window.AppNotifications?.error('Player could not be created. Please try again.', { duration: 3000 });
        } finally {
            if (submitButton) {
                submitButton.disabled = false;
                submitButton.textContent = originalSubmitText;
            }
        }
    });

    form.querySelectorAll('[data-player-validate-field]').forEach(input => {
        input.addEventListener('blur', () => {
            validateField(input.dataset.playerValidateField);
        });

        input.addEventListener('input', () => {
            const fieldName = input.dataset.playerValidateField;
            const field = getValidationField(fieldName);

            if (field?.classList.contains('has-error')) {
                validateField(fieldName);
            }
        });
    });

    teamInput?.addEventListener('input', searchTeamAgain);
    teamInput?.addEventListener('focus', searchTeamAgain);

    teamInput?.addEventListener('keydown', event => {
        const options = Array.from(teamResults?.querySelectorAll('[data-player-team-option]') ?? []);

        if (event.key === 'ArrowDown') {
            event.preventDefault();
            setActiveTeamOption(activeTeamIndex + 1);
        } else if (event.key === 'ArrowUp') {
            event.preventDefault();
            setActiveTeamOption(activeTeamIndex - 1);
        } else if (event.key === 'Enter' && activeTeamIndex >= 0 && options[activeTeamIndex]) {
            event.preventDefault();
            selectTeam(options[activeTeamIndex]);
        }
    });

    teamResults?.addEventListener('click', event => {
        const option = event.target.closest('[data-player-team-option]');

        if (!option) {
            return;
        }

        selectTeam(option);
    });

    teamClearButton?.addEventListener('click', clearSelectedTeam);

    document.addEventListener('click', event => {
        if (!teamAutocomplete?.contains(event.target)) {
            hideTeamResults();
        }
    });
});
