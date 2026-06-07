// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.AppNotifications = (() => {
    let notificationTimer = null;

    const show = (message, type = 'success', options = {}) => {
        const notification = document.querySelector('[data-app-notification]');
        const text = document.querySelector('[data-app-notification-text]');
        const icon = document.querySelector('[data-app-notification-icon]');

        if (!notification || !text || !icon) {
            return;
        }

        const normalizedType = type === 'error' ? 'error' : 'success';
        const duration = Number.isInteger(options.duration) ? options.duration : 3000;
        const onClose = typeof options.onClose === 'function' ? options.onClose : null;

        window.clearTimeout(notificationTimer);
        text.textContent = message;
        icon.className = normalizedType === 'error' ? 'fas fa-xmark' : 'fas fa-check';

        notification.hidden = false;
        notification.classList.remove('is-visible', 'is-leaving', 'is-success', 'is-error');
        notification.classList.add(`is-${normalizedType}`);

        window.requestAnimationFrame(() => {
            notification.classList.add('is-visible');
        });

        notificationTimer = window.setTimeout(() => {
            notification.classList.add('is-leaving');
            notification.classList.remove('is-visible');

            window.setTimeout(() => {
                notification.hidden = true;
                notification.classList.remove('is-leaving');
                onClose?.();
            }, 480);
        }, duration);
    };

    return {
        show,
        success: (message, options) => show(message, 'success', options),
        error: (message, options) => show(message, 'error', options)
    };
})();

window.AppDatePickers = (() => {
    const pad = value => String(value).padStart(2, '0');
    const toIsoDate = date => `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}`;
    const toIsoDateTime = (date, time) => `${toIsoDate(date)}T${time || '00:00'}`;
    const locale = navigator.language || 'en-US';
    const startsOnMonday = locale.toLowerCase().startsWith('hr') || locale.toLowerCase().startsWith('en-gb');

    const parseIsoDate = value => {
        if (!value) {
            return null;
        }

        const datePart = value.split('T')[0];
        const parts = datePart.split('-').map(Number);

        if (parts.length !== 3 || parts.some(Number.isNaN)) {
            return null;
        }

        return new Date(parts[0], parts[1] - 1, parts[2]);
    };

    const formatDisplay = (date, includeTime, time) => {
        if (!date) {
            return '';
        }

        const formattedDate = new Intl.DateTimeFormat(locale, {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric'
        }).format(date);

        return includeTime ? `${formattedDate} ${time || '00:00'}` : formattedDate;
    };

    const getTimeValue = picker => {
        const hour = picker.querySelector('[data-date-picker-hour]')?.value ?? '00';
        const minute = picker.querySelector('[data-date-picker-minute]')?.value ?? '00';
        return `${hour}:${minute}`;
    };

    const setHiddenValue = (picker, date, time) => {
        const hiddenInput = picker.querySelector('[data-date-picker-value]');
        const display = picker.querySelector('[data-date-picker-display]');
        const includeTime = picker.dataset.datePickerMode === 'datetime';

        if (!hiddenInput || !display || !date) {
            return;
        }

        hiddenInput.value = includeTime ? toIsoDateTime(date, time) : toIsoDate(date);
        display.textContent = formatDisplay(date, includeTime, time);
        hiddenInput.dispatchEvent(new Event('input', { bubbles: true }));
        hiddenInput.dispatchEvent(new Event('change', { bubbles: true }));
    };

    const getWeekdayLabels = () => {
        const base = startsOnMonday
            ? [1, 2, 3, 4, 5, 6, 0]
            : [0, 1, 2, 3, 4, 5, 6];

        return base.map(day => {
            const date = new Date(2026, 5, 7 + day);
            return new Intl.DateTimeFormat(locale, { weekday: 'short' }).format(date);
        });
    };

    const getMonthLabels = () => Array.from({ length: 12 }, (_, month) =>
        new Intl.DateTimeFormat(locale, { month: 'long' }).format(new Date(2026, month, 1)));

    const getYearRange = selectedYear => {
        const currentYear = new Date().getFullYear();
        const middleYear = selectedYear || currentYear;
        const minYear = Math.min(currentYear - 120, middleYear - 10);
        const maxYear = Math.max(currentYear + 20, middleYear + 10);
        const years = [];

        for (let year = maxYear; year >= minYear; year -= 1) {
            years.push(year);
        }

        return years;
    };

    const render = picker => {
        const state = picker._datePickerState;
        const heading = picker.querySelector('[data-date-picker-heading]');
        const monthSelect = picker.querySelector('[data-date-picker-month]');
        const yearSelect = picker.querySelector('[data-date-picker-year]');
        const weekdays = picker.querySelector('[data-date-picker-weekdays]');
        const days = picker.querySelector('[data-date-picker-days]');

        if (!state || !heading || !monthSelect || !yearSelect || !weekdays || !days) {
            return;
        }

        if (monthSelect.options.length === 0) {
            monthSelect.innerHTML = getMonthLabels()
                .map((label, month) => `<option value="${month}">${label}</option>`)
                .join('');
        }

        const yearOptions = getYearRange(state.viewYear);
        yearSelect.innerHTML = yearOptions
            .map(year => `<option value="${year}">${year}</option>`)
            .join('');
        monthSelect.value = String(state.viewMonth);
        yearSelect.value = String(state.viewYear);

        weekdays.innerHTML = getWeekdayLabels()
            .map(label => `<span>${label}</span>`)
            .join('');

        const firstDay = new Date(state.viewYear, state.viewMonth, 1);
        const firstWeekday = startsOnMonday ? (firstDay.getDay() + 6) % 7 : firstDay.getDay();
        const daysInMonth = new Date(state.viewYear, state.viewMonth + 1, 0).getDate();
        const todayIso = toIsoDate(new Date());
        const selectedIso = state.selectedDate ? toIsoDate(state.selectedDate) : '';
        const cells = [];

        for (let index = 0; index < firstWeekday; index += 1) {
            cells.push('<span class="app-date-picker-day is-empty"></span>');
        }

        for (let day = 1; day <= daysInMonth; day += 1) {
            const date = new Date(state.viewYear, state.viewMonth, day);
            const iso = toIsoDate(date);
            const classes = [
                'app-date-picker-day',
                iso === todayIso ? 'is-today' : '',
                iso === selectedIso ? 'is-selected' : ''
            ].filter(Boolean).join(' ');

            cells.push(`<button type="button" class="${classes}" data-date-picker-day="${iso}">${day}</button>`);
        }

        days.innerHTML = cells.join('');
    };

    const close = picker => {
        const trigger = picker.querySelector('[data-date-picker-trigger]');
        const panel = picker.querySelector('[data-date-picker-panel]');

        panel.hidden = true;
        trigger.setAttribute('aria-expanded', 'false');
    };

    const open = picker => {
        const trigger = picker.querySelector('[data-date-picker-trigger]');
        const panel = picker.querySelector('[data-date-picker-panel]');

        panel.hidden = false;
        trigger.setAttribute('aria-expanded', 'true');
        render(picker);
    };

    const init = root => {
        const pickers = root.querySelectorAll('[data-app-date-picker]');

        pickers.forEach(picker => {
            const hiddenInput = picker.querySelector('[data-date-picker-value]');
            const trigger = picker.querySelector('[data-date-picker-trigger]');
            const panel = picker.querySelector('[data-date-picker-panel]');
            const hourSelect = picker.querySelector('[data-date-picker-hour]');
            const minuteSelect = picker.querySelector('[data-date-picker-minute]');
            const initialDate = parseIsoDate(hiddenInput?.value);
            const initialTime = hiddenInput?.value?.includes('T') ? hiddenInput.value.split('T')[1].slice(0, 5) : '00:00';
            const baseDate = initialDate ?? new Date();

            picker._datePickerState = {
                selectedDate: initialDate,
                viewMonth: baseDate.getMonth(),
                viewYear: baseDate.getFullYear()
            };

            if (hourSelect && minuteSelect) {
                const [hour, minute] = initialTime.split(':');
                hourSelect.value = hour ?? '00';
                minuteSelect.value = minute ?? '00';
            }

            if (initialDate) {
                setHiddenValue(picker, initialDate, getTimeValue(picker));
            }

            trigger?.addEventListener('click', () => {
                if (panel.hidden) {
                    open(picker);
                } else {
                    close(picker);
                }
            });

            picker.querySelector('[data-date-picker-prev]')?.addEventListener('click', () => {
                const state = picker._datePickerState;
                state.viewMonth -= 1;
                if (state.viewMonth < 0) {
                    state.viewMonth = 11;
                    state.viewYear -= 1;
                }
                render(picker);
            });

            picker.querySelector('[data-date-picker-next]')?.addEventListener('click', () => {
                const state = picker._datePickerState;
                state.viewMonth += 1;
                if (state.viewMonth > 11) {
                    state.viewMonth = 0;
                    state.viewYear += 1;
                }
                render(picker);
            });

            picker.querySelector('[data-date-picker-month]')?.addEventListener('change', event => {
                picker._datePickerState.viewMonth = Number(event.target.value);
                render(picker);
            });

            picker.querySelector('[data-date-picker-year]')?.addEventListener('change', event => {
                picker._datePickerState.viewYear = Number(event.target.value);
                render(picker);
            });

            picker.querySelector('[data-date-picker-days]')?.addEventListener('click', event => {
                const dayButton = event.target.closest('[data-date-picker-day]');

                if (!dayButton) {
                    return;
                }

                const selectedDate = parseIsoDate(dayButton.dataset.datePickerDay);
                picker._datePickerState.selectedDate = selectedDate;
                setHiddenValue(picker, selectedDate, getTimeValue(picker));
                render(picker);
            });

            picker.querySelector('[data-date-picker-today]')?.addEventListener('click', () => {
                const today = new Date();
                picker._datePickerState.selectedDate = today;
                picker._datePickerState.viewMonth = today.getMonth();
                picker._datePickerState.viewYear = today.getFullYear();
                setHiddenValue(picker, today, getTimeValue(picker));
                render(picker);
            });

            picker.querySelector('[data-date-picker-done]')?.addEventListener('click', () => {
                close(picker);
                trigger?.focus();
            });

            picker.querySelectorAll('[data-date-picker-hour], [data-date-picker-minute]').forEach(select => {
                select.addEventListener('change', () => {
                    if (picker._datePickerState.selectedDate) {
                        setHiddenValue(picker, picker._datePickerState.selectedDate, getTimeValue(picker));
                    }
                });
            });

            picker.querySelector('[data-date-picker-time]')?.addEventListener('change', () => {
                if (picker._datePickerState.selectedDate) {
                    setHiddenValue(picker, picker._datePickerState.selectedDate, getTimeValue(picker));
                }
            });
        });

        document.addEventListener('click', event => {
            document.querySelectorAll('[data-app-date-picker]').forEach(picker => {
                if (!picker.contains(event.target)) {
                    close(picker);
                }
            });
        });

        document.addEventListener('keydown', event => {
            if (event.key !== 'Escape') {
                return;
            }

            document.querySelectorAll('[data-app-date-picker]').forEach(close);
        });
    };

    return { init };
})();

document.addEventListener('DOMContentLoaded', function () {
    window.AppDatePickers.init(document);

    const formatDateDisplay = value => {
        if (!value) {
            return '';
        }

        const date = new Date(value);
        if (Number.isNaN(date.getTime())) {
            return value;
        }

        return new Intl.DateTimeFormat(navigator.language || 'en-US', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric'
        }).format(date);
    };

    const setDatePickerValue = (form, fieldName, value) => {
        const input = form.querySelector(`[name="${fieldName}"]`);
        const picker = input?.closest('[data-app-date-picker]');
        const display = picker?.querySelector('[data-date-picker-display]');

        if (input) {
            input.value = value || '';
            input.dispatchEvent(new Event('input', { bubbles: true }));
            input.dispatchEvent(new Event('change', { bubbles: true }));
        }

        if (display) {
            display.textContent = value ? formatDateDisplay(value) : 'Select date';
        }

        if (picker && value) {
            const date = new Date(value);
            if (!Number.isNaN(date.getTime())) {
                picker._datePickerState = {
                    selectedDate: date,
                    viewMonth: date.getMonth(),
                    viewYear: date.getFullYear()
                };
            }
        }
    };

    const setInputValue = (form, name, value) => {
        const input = form.querySelector(`[name="${name}"]`);
        if (input) {
            input.value = value ?? '';
            input.dispatchEvent(new Event('input', { bubbles: true }));
        }
    };

    const setModalMode = (modal, form, options) => {
        const title = modal.querySelector('h2[id]');
        const eyebrow = modal.querySelector('[class$="-modal-eyebrow"]');
        const description = modal.querySelector('[class$="-modal-description"]');
        const submit = form.querySelector('button[type="submit"]');

        form.action = options.action;
        setInputValue(form, 'Id', options.id);

        if (title) {
            title.textContent = options.title;
        }

        if (eyebrow) {
            eyebrow.textContent = options.eyebrow;
        }

        if (description) {
            description.textContent = options.description;
        }

        if (submit) {
            submit.textContent = options.submitText;
        }
    };

    const openModal = selector => {
        const opener = document.querySelector(selector);
        opener?.click();
    };

    const resetCreateMode = (buttonSelector, formSelector, options) => {
        const button = document.querySelector(buttonSelector);
        const form = document.querySelector(formSelector);
        const modal = form?.closest('[role="dialog"]');

        button?.addEventListener('click', () => {
            if (!form || !modal) {
                return;
            }

            setModalMode(modal, form, options);
        }, true);
    };

    resetCreateMode('[data-team-modal-open]', '[data-team-create-form]', {
        action: '/Teams/Create',
        id: 0,
        title: 'Create Team',
        eyebrow: 'New team',
        description: 'Add the main team details now. Saving will be connected when the backend CRUD flow is added.',
        submitText: 'Save Team'
    });

    resetCreateMode('[data-player-modal-open]', '[data-player-create-form]', {
        action: '/Players/Create',
        id: 0,
        title: 'Create Player',
        eyebrow: 'New player',
        description: 'Add player identity, team assignment, position, and starting statistics.',
        submitText: 'Save Player'
    });

    resetCreateMode('[data-competition-modal-open]', '[data-competition-create-form]', {
        action: '/Competitions/Create',
        id: 0,
        title: 'Create Competition',
        eyebrow: 'New competition',
        description: 'Add competition identity, season, location, and schedule dates.',
        submitText: 'Save Competition'
    });

    resetCreateMode('[data-appuser-modal-open]', '[data-appuser-create-form]', {
        action: '/AppUsers/Create',
        id: 0,
        title: 'Create AppUser',
        eyebrow: 'New app user',
        description: 'Add a new account profile with identity, role, and date of birth.',
        submitText: 'Save AppUser'
    });

    document.querySelectorAll('[data-edit-team]').forEach(button => {
        button.addEventListener('click', () => {
            const form = document.querySelector('[data-team-create-form]');
            const modal = document.querySelector('[data-team-modal]');
            if (!form || !modal) return;

            openModal('[data-team-modal-open]');
            setModalMode(modal, form, {
                action: button.dataset.editUrl,
                id: button.dataset.id,
                title: 'Edit Team',
                eyebrow: 'Team details',
                description: 'Update team identity, coach, city, arena, and founded year.',
                submitText: 'Save Changes'
            });
            const coach = form.querySelector('[name="CoachName"]');
            const clear = form.querySelector('[data-coach-clear]');
            const wrapper = form.querySelector('[data-coach-autocomplete]');
            if (coach) coach.readOnly = true;
            if (clear) clear.hidden = false;
            wrapper?.classList.add('is-selected');
            setInputValue(form, 'Name', button.dataset.name);
            setInputValue(form, 'HomeCity', button.dataset.homeCity);
            setInputValue(form, 'HomeArena', button.dataset.homeArena);
            setInputValue(form, 'CoachName', button.dataset.coachName);
            setInputValue(form, 'FoundedYear', button.dataset.foundedYear);
        });
    });

    document.querySelectorAll('[data-edit-player]').forEach(button => {
        button.addEventListener('click', () => {
            const form = document.querySelector('[data-player-create-form]');
            const modal = document.querySelector('[data-player-modal]');
            if (!form || !modal) return;

            openModal('[data-player-modal-open]');
            setModalMode(modal, form, {
                action: button.dataset.editUrl,
                id: button.dataset.id,
                title: 'Edit Player',
                eyebrow: 'Player details',
                description: 'Update player identity, team assignment, position, and statistics.',
                submitText: 'Save Changes'
            });
            setInputValue(form, 'FirstName', button.dataset.firstName);
            setInputValue(form, 'LastName', button.dataset.lastName);
            setDatePickerValue(form, 'BirthDate', button.dataset.birthDate);
            setInputValue(form, 'JerseyNumber', button.dataset.jerseyNumber);
            setInputValue(form, 'Position', button.dataset.position);
            setInputValue(form, 'GoalsScored', button.dataset.goalsScored);
            setInputValue(form, 'Assists', button.dataset.assists);
            setInputValue(form, 'TeamId', button.dataset.teamId);
            const teamInput = form.querySelector('[data-player-team-input]');
            const clear = form.querySelector('[data-player-team-clear]');
            const wrapper = form.querySelector('[data-player-team-autocomplete]');
            if (teamInput) {
                teamInput.value = button.dataset.teamName || '';
                teamInput.readOnly = true;
            }
            if (clear) clear.hidden = false;
            wrapper?.classList.add('is-selected');
        });
    });

    document.querySelectorAll('[data-edit-competition]').forEach(button => {
        button.addEventListener('click', () => {
            const form = document.querySelector('[data-competition-create-form]');
            const modal = document.querySelector('[data-competition-modal]');
            if (!form || !modal) return;

            openModal('[data-competition-modal-open]');
            setModalMode(modal, form, {
                action: button.dataset.editUrl,
                id: button.dataset.id,
                title: 'Edit Competition',
                eyebrow: 'Competition details',
                description: 'Update competition identity, season, location, and schedule dates.',
                submitText: 'Save Changes'
            });
            setInputValue(form, 'Name', button.dataset.name);
            setInputValue(form, 'Season', button.dataset.season);
            setInputValue(form, 'City', button.dataset.city);
            setDatePickerValue(form, 'StartDate', button.dataset.startDate);
            setDatePickerValue(form, 'EndDate', button.dataset.endDate);
        });
    });

    document.querySelectorAll('[data-edit-appuser]').forEach(button => {
        button.addEventListener('click', () => {
            const form = document.querySelector('[data-appuser-create-form]');
            const modal = document.querySelector('[data-appuser-modal]');
            if (!form || !modal) return;

            openModal('[data-appuser-modal-open]');
            setModalMode(modal, form, {
                action: button.dataset.editUrl,
                id: button.dataset.id,
                title: 'Edit AppUser',
                eyebrow: 'App user details',
                description: 'Update account identity, role, and date of birth.',
                submitText: 'Save Changes'
            });
            setInputValue(form, 'Username', button.dataset.username);
            setInputValue(form, 'DisplayName', button.dataset.displayName);
            setInputValue(form, 'Email', button.dataset.email);
            setInputValue(form, 'Role', button.dataset.role);
            setDatePickerValue(form, 'DateOfBirth', button.dataset.dateOfBirth);
        });
    });

    const deleteConfirmModal = document.querySelector('[data-delete-confirm-modal]');
    const deleteConfirmMessage = document.querySelector('[data-delete-confirm-message]');
    const deleteConfirmSubmit = document.querySelector('[data-delete-confirm-submit]');
    const deleteConfirmCancelButtons = document.querySelectorAll('[data-delete-confirm-cancel]');
    let pendingDeleteForm = null;
    let lastDeleteTrigger = null;

    const closeDeleteConfirm = () => {
        if (!deleteConfirmModal) {
            return;
        }

        deleteConfirmModal.hidden = true;
        document.body.classList.remove('app-confirm-open');
        pendingDeleteForm = null;
        lastDeleteTrigger?.focus();
    };

    const openDeleteConfirm = form => {
        if (!deleteConfirmModal || !deleteConfirmMessage) {
            form.submit();
            return;
        }

        const name = form.dataset.deleteName || 'this item';
        pendingDeleteForm = form;
        lastDeleteTrigger = document.activeElement;
        deleteConfirmMessage.textContent = `Are you sure you want to delete ${name}? This action cannot be undone.`;
        deleteConfirmModal.hidden = false;
        document.body.classList.add('app-confirm-open');

        window.requestAnimationFrame(() => {
            deleteConfirmSubmit?.focus();
        });
    };

    document.querySelectorAll('[data-delete-form]').forEach(form => {
        form.addEventListener('submit', event => {
            if (form.dataset.deleteConfirmed === 'true') {
                return;
            }

            event.preventDefault();
            openDeleteConfirm(form);
        });
    });

    deleteConfirmSubmit?.addEventListener('click', () => {
        if (!pendingDeleteForm) {
            closeDeleteConfirm();
            return;
        }

        pendingDeleteForm.dataset.deleteConfirmed = 'true';
        pendingDeleteForm.requestSubmit();
    });

    deleteConfirmCancelButtons.forEach(button => {
        button.addEventListener('click', closeDeleteConfirm);
    });

    deleteConfirmModal?.addEventListener('keydown', event => {
        if (event.key === 'Escape') {
            closeDeleteConfirm();
        }
    });

    const searchInputs = document.querySelectorAll('[data-autocomplete-url]');
    const itemSelectors = [
        '.competition-list-item',
        '.team-list-item',
        '.player-list-item',
        '.match-card',
        '.appuser-list-item'
    ];
    const listSelectors = [
        '.competitions-list',
        '.teams-list',
        '.players-list',
        '.match-sections',
        '.appusers-list'
    ];

    searchInputs.forEach(input => {
        const form = input.closest('form');
        const status = document.createElement('div');
        let loadingTimer;

        status.className = 'search-results-status';
        status.hidden = true;
        status.innerHTML = `
            <div class="search-results-loader" aria-hidden="true">
                <span></span>
                <span></span>
                <span></span>
            </div>
            <p class="search-results-message"></p>
        `;

        const getListContainer = () => {
            for (const selector of listSelectors) {
                const element = document.querySelector(selector);
                if (element) {
                    return element;
                }
            }

            return null;
        };

        const ensureStatusLocation = () => {
            const listContainer = getListContainer();

            if (listContainer && status.parentElement !== listContainer.parentElement) {
                listContainer.parentElement.insertBefore(status, listContainer);
            }
        };

        const setStatus = (mode, message = '') => {
            ensureStatusLocation();
            const messageElement = status.querySelector('.search-results-message');

            status.hidden = mode === 'hidden';
            status.classList.toggle('is-loading', mode === 'loading');
            status.classList.toggle('is-empty', mode === 'empty');
            messageElement.textContent = message;
        };

        const setListVisibility = isVisible => {
            listSelectors.forEach(selector => {
                const element = document.querySelector(selector);
                if (element) {
                    element.hidden = !isVisible;
                }
            });
        };

        const filterCurrentList = query => {
            const normalizedQuery = query.trim().toLowerCase();
            const items = document.querySelectorAll(itemSelectors.join(','));
            let visibleCount = 0;

            window.clearTimeout(loadingTimer);
            setStatus('loading', 'Searching list...');
            setListVisibility(false);

            items.forEach(item => {
                const itemText = item.textContent.toLowerCase();
                const isVisible = normalizedQuery.length === 0 || itemText.includes(normalizedQuery);

                item.hidden = !isVisible;
                if (isVisible) {
                    visibleCount += 1;
                }
            });

            document.querySelectorAll('.match-section').forEach(section => {
                const visibleCards = section.querySelectorAll('.match-card:not([hidden])');
                section.hidden = normalizedQuery.length > 0 && visibleCards.length === 0;
            });

            loadingTimer = window.setTimeout(() => {
                if (normalizedQuery.length > 0 && visibleCount === 0) {
                    setStatus('empty', 'No results match your search.');
                    setListVisibility(false);
                    return;
                }

                setStatus('hidden');
                setListVisibility(true);
            }, 350);
        };

        input.addEventListener('input', function () {
            filterCurrentList(input.value);
        });

        input.addEventListener('keydown', function (event) {
            if (event.key === 'Enter') {
                event.preventDefault();
                filterCurrentList(input.value);
            }
        });

        form?.addEventListener('submit', function (event) {
            event.preventDefault();
            filterCurrentList(input.value);
        });
    });
});
