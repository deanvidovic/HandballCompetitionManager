document.addEventListener('DOMContentLoaded', function () {
    const notification = document.querySelector('[data-competition-notification]');
    const errorNotification = document.querySelector('[data-competition-error-notification]');
    const teamInput = document.querySelector('[data-competition-team-input]');
    const teamResults = document.querySelector('[data-competition-team-results]');
    const teamAutocomplete = document.querySelector('[data-competition-team-autocomplete]');
    const selectedTeams = document.querySelector('[data-competition-selected-teams]');
    const selectedTeamsEmpty = document.querySelector('[data-competition-selected-teams-empty]');
    const selectedTeamIds = new Set();
    let teamSearchTimer = null;
    let activeTeamIndex = -1;
    let suppressTeamFocusSearch = false;

    if (notification?.dataset.competitionNotification) {
        window.AppNotifications?.success(notification.dataset.competitionNotification);
    }

    if (errorNotification?.dataset.competitionErrorNotification) {
        window.AppNotifications?.error(errorNotification.dataset.competitionErrorNotification);
    }

    const escapeHtml = value => String(value ?? '')
        .replaceAll('&', '&amp;')
        .replaceAll('<', '&lt;')
        .replaceAll('>', '&gt;')
        .replaceAll('"', '&quot;')
        .replaceAll("'", '&#039;');

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

    const setTeamResultsMessage = message => {
        if (!teamResults || !teamInput) {
            return;
        }

        teamResults.innerHTML = `<div class="competition-team-empty">${escapeHtml(message)}</div>`;
        teamResults.hidden = false;
        teamInput.setAttribute('aria-expanded', 'true');
    };

    const renderTeamResults = teams => {
        if (!teamResults || !teamInput) {
            return;
        }

        const availableTeams = teams.filter(team => !selectedTeamIds.has(String(team.id)));

        if (availableTeams.length === 0) {
            setTeamResultsMessage('No available teams found.');
            return;
        }

        teamResults.innerHTML = availableTeams.map((team, index) => `
            <button type="button" class="competition-team-option" id="competition-team-option-${index}" role="option" data-competition-team-option data-team-id="${team.id}" data-team-value="${escapeHtml(team.value)}" data-team-meta="${escapeHtml(team.meta)}">
                <span class="competition-team-option-label">${escapeHtml(team.label)}</span>
                <span class="competition-team-option-meta">${escapeHtml(team.meta)} - ${escapeHtml(team.players)} players</span>
            </button>
        `).join('');

        teamResults.hidden = false;
        teamInput.setAttribute('aria-expanded', 'true');
        activeTeamIndex = -1;
    };

    const fetchTeamSuggestions = () => {
        if (!teamInput || !teamResults) {
            hideTeamResults();
            return;
        }

        const url = teamInput.dataset.competitionTeamAutocompleteUrl;
        const query = teamInput.value.trim();

        if (!url) {
            return;
        }

        window.clearTimeout(teamSearchTimer);
        teamSearchTimer = window.setTimeout(async () => {
            setTeamResultsMessage('Searching available teams...');

            try {
                const response = await fetch(`${url}&query=${encodeURIComponent(query)}`, {
                    headers: {
                        'Accept': 'application/json'
                    }
                });

                if (!response.ok) {
                    throw new Error('Available teams request failed.');
                }

                const teams = await response.json();
                renderTeamResults(teams);
            } catch {
                setTeamResultsMessage('Team suggestions are unavailable.');
            }
        }, 220);
    };

    const setActiveTeamOption = index => {
        const options = Array.from(teamResults?.querySelectorAll('[data-competition-team-option]') ?? []);

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
        if (!teamInput || !selectedTeams) {
            return;
        }

        const teamId = option.dataset.teamId;

        if (!teamId || selectedTeamIds.has(teamId)) {
            hideTeamResults();
            return;
        }

        selectedTeamIds.add(teamId);
        selectedTeamsEmpty?.setAttribute('hidden', '');

        const item = document.createElement('div');
        const content = document.createElement('div');
        const name = document.createElement('span');
        const meta = document.createElement('span');
        const hiddenInput = document.createElement('input');
        const removeButton = document.createElement('button');
        const removeIcon = document.createElement('i');

        item.className = 'competition-selected-team';
        item.dataset.selectedTeamId = teamId;
        name.className = 'competition-selected-team-label';
        meta.className = 'competition-selected-team-meta';
        hiddenInput.type = 'hidden';
        hiddenInput.name = 'AddTeamIds';
        hiddenInput.value = teamId;
        removeButton.type = 'button';
        removeButton.className = 'competition-selected-team-remove';
        removeButton.setAttribute('aria-label', `Remove ${option.dataset.teamValue}`);
        removeButton.dataset.removeSelectedTeam = teamId;
        removeIcon.className = 'fas fa-xmark';
        removeIcon.setAttribute('aria-hidden', 'true');

        name.textContent = option.dataset.teamValue;
        meta.textContent = option.dataset.teamMeta;
        removeButton.appendChild(removeIcon);
        content.append(name, meta);
        item.append(content, hiddenInput, removeButton);
        selectedTeams.appendChild(item);

        teamInput.value = '';
        teamAutocomplete?.classList.add('has-selection');
        hideTeamResults();
        suppressTeamFocusSearch = true;
        teamInput.blur();
    };

    teamInput?.addEventListener('input', () => {
        suppressTeamFocusSearch = false;
        fetchTeamSuggestions();
    });

    teamInput?.addEventListener('focus', () => {
        if (suppressTeamFocusSearch) {
            suppressTeamFocusSearch = false;
            return;
        }

        fetchTeamSuggestions();
    });

    teamInput?.addEventListener('keydown', event => {
        const options = Array.from(teamResults?.querySelectorAll('[data-competition-team-option]') ?? []);

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
        const option = event.target.closest('[data-competition-team-option]');

        if (!option) {
            return;
        }

        selectTeam(option);
    });

    selectedTeams?.addEventListener('click', event => {
        const removeButton = event.target.closest('[data-remove-selected-team]');

        if (!removeButton) {
            return;
        }

        selectedTeamIds.delete(removeButton.dataset.removeSelectedTeam);
        removeButton.closest('[data-selected-team-id]')?.remove();

        if (selectedTeamIds.size === 0) {
            selectedTeamsEmpty?.removeAttribute('hidden');
            teamAutocomplete?.classList.remove('has-selection');
        }
    });

    document.addEventListener('click', event => {
        if (!teamAutocomplete?.contains(event.target)) {
            hideTeamResults();
        }
    });

    document.querySelectorAll('[data-bracket-group-toggle]').forEach(button => {
        button.addEventListener('click', () => {
            const panel = document.getElementById(button.getAttribute('aria-controls'));
            const isExpanded = button.getAttribute('aria-expanded') === 'true';

            button.setAttribute('aria-expanded', (!isExpanded).toString());
            if (panel) {
                panel.hidden = isExpanded;
            }
        });
    });

    const modal = document.querySelector('[data-competition-modal]');
    const openButton = document.querySelector('[data-competition-modal-open]');
    const closeButtons = document.querySelectorAll('[data-competition-modal-close]');
    const form = document.querySelector('[data-competition-create-form]');
    const status = document.querySelector('[data-competition-form-status]');
    let lastFocusedElement = null;

    const validationMessages = {
        Name: {
            required: 'Competition name is required.',
            invalid: 'Competition name must be at least 3 characters long.'
        },
        Season: {
            required: 'Season is required.',
            invalid: 'Season must be in format 2025/2026.',
            order: 'First season year cannot be greater than second season year.',
            future: `Second season year cannot be greater than ${new Date().getFullYear()}.`
        },
        City: {
            required: 'City is required.',
            invalid: 'City must be longer than 2 characters and cannot contain numbers.'
        },
        StartDate: {
            required: 'Start date is required.',
            invalid: 'Start date cannot be after end date.'
        },
        EndDate: {
            required: 'End date is required.',
            invalid: 'End date cannot be before start date.'
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

    const fields = ['Name', 'Season', 'City', 'StartDate', 'EndDate'];
    const getFocusableElements = () => Array.from(modal.querySelectorAll(focusableSelector));
    const getValidationInput = fieldName => form.querySelector(`[data-competition-validate-field="${fieldName}"]`);
    const getValidationMessage = fieldName => form.querySelector(`[data-competition-validation-message-for="${fieldName}"]`);
    const getValidationField = fieldName => form.querySelector(`[data-competition-field="${fieldName}"]`);

    const parseDate = value => {
        if (!value) {
            return null;
        }

        const date = new Date(value);
        return Number.isNaN(date.getTime()) ? null : date;
    };

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
        field?.classList.toggle('has-valid', Boolean(input?.value.trim()));
        input?.setAttribute('aria-invalid', 'false');

        if (validationMessage) {
            validationMessage.textContent = '';
            validationMessage.hidden = true;
        }
    };

    const validateField = fieldName => {
        const input = getValidationInput(fieldName);
        const value = input?.value.trim() ?? '';

        if (!value) {
            setFieldError(fieldName, validationMessages[fieldName].required);
            return false;
        }

        if (fieldName === 'Name' && value.length < 3) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'Season') {
            const seasonMatch = value.match(/^(\d{4})\/(\d{4})$/);

            if (!seasonMatch) {
                setFieldError(fieldName, validationMessages[fieldName].invalid);
                return false;
            }

            const firstYear = Number(seasonMatch[1]);
            const secondYear = Number(seasonMatch[2]);

            if (firstYear > secondYear) {
                setFieldError(fieldName, validationMessages[fieldName].order);
                return false;
            }

            if (secondYear > new Date().getFullYear()) {
                setFieldError(fieldName, validationMessages[fieldName].future);
                return false;
            }
        }

        if (fieldName === 'City' && (value.length < 3 || /\d/.test(value))) {
            setFieldError(fieldName, validationMessages[fieldName].invalid);
            return false;
        }

        if (fieldName === 'StartDate') {
            const startDate = parseDate(value);
            const endDate = parseDate(getValidationInput('EndDate')?.value);

            if (startDate && endDate && startDate > endDate) {
                setFieldError(fieldName, validationMessages[fieldName].invalid);
                return false;
            }
        }

        if (fieldName === 'EndDate') {
            const startDate = parseDate(getValidationInput('StartDate')?.value);
            const endDate = parseDate(value);

            if (startDate && endDate && endDate < startDate) {
                setFieldError(fieldName, validationMessages[fieldName].invalid);
                return false;
            }
        }

        clearFieldError(fieldName);
        return true;
    };

    const validateForm = () => fields.map(validateField).every(Boolean);

    const clearValidationState = () => {
        form.querySelectorAll('[data-competition-validation-message-for]').forEach(message => {
            message.textContent = '';
            message.hidden = true;
        });

        form.querySelectorAll('[data-competition-field]').forEach(field => {
            field.classList.remove('has-error', 'has-valid');
        });

        form.querySelectorAll('[data-competition-validate-field]').forEach(input => {
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
        document.body.classList.add('competition-modal-open');
        openButton.setAttribute('aria-expanded', 'true');
        clearValidationState();

        window.requestAnimationFrame(() => {
            modal.querySelector('input, select, textarea')?.focus();
        });
    };

    const closeModal = () => {
        modal.hidden = true;
        document.body.classList.remove('competition-modal-open');
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
        const submitButton = form.querySelector('.competition-modal-submit');
        const originalSubmitText = submitButton?.textContent;

        if (!validateForm()) {
            status.textContent = 'Please fix the highlighted fields.';
            status.classList.remove('is-success');
            status.classList.add('is-error');
            status.hidden = false;
            const firstErrorField = form.querySelector('.has-error');
            firstErrorField?.querySelector('[data-date-picker-trigger], [data-competition-validate-field]')?.focus();
            return;
        }

        if (submitButton) {
            submitButton.disabled = true;
            submitButton.textContent = 'Saving...';
        }

        status.textContent = 'Creating competition...';
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
                    : 'Competition could not be created.';

                status.textContent = errors;
                status.classList.add('is-error');
                applyServerFieldErrors(result.fieldErrors);
                window.AppNotifications?.error(errors, { duration: 3000 });
                return;
            }

            closeModal();
            if (window.AppNotifications) {
                window.AppNotifications.success(result.message ?? 'Competition created successfully.', {
                    onClose: () => window.location.reload()
                });
            } else {
                window.location.reload();
            }
        } catch {
            status.textContent = 'Competition could not be created. Please try again.';
            status.classList.add('is-error');
            window.AppNotifications?.error('Competition could not be created. Please try again.', { duration: 3000 });
        } finally {
            if (submitButton) {
                submitButton.disabled = false;
                submitButton.textContent = originalSubmitText;
            }
        }
    });

    form.querySelectorAll('[data-competition-validate-field]').forEach(input => {
        input.addEventListener('blur', () => {
            validateField(input.dataset.competitionValidateField);
        });

        input.addEventListener('input', () => {
            const fieldName = input.dataset.competitionValidateField;
            const field = getValidationField(fieldName);

            if (field?.classList.contains('has-error')) {
                validateField(fieldName);
            }

            if (fieldName === 'StartDate' && getValidationField('EndDate')?.classList.contains('has-error')) {
                validateField('EndDate');
            }

            if (fieldName === 'EndDate' && getValidationField('StartDate')?.classList.contains('has-error')) {
                validateField('StartDate');
            }
        });
    });
});
