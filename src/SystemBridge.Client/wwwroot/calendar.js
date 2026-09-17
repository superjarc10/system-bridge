window.systemBridgeCalendar = {
    instances: {},
    init: function (element, events, dotNetReference) {
        if (this.instances[element.id]) {
            this.instances[element.id].destroy();
        }

        const calendar = new FullCalendar.Calendar(element, {
            initialView: 'dayGridMonth',
            height: 'auto',
            firstDay: 1,
            fixedWeekCount: false,
            dayMaxEvents: 3,
            events: events,
            eventContent: function (arg) {
                const content = document.createElement('span');
                const prefix = document.createElement('span');
                const customer = document.createElement('strong');
                prefix.className = 'calendar-event-prefix';
                customer.className = 'calendar-event-customer';
                prefix.textContent = arg.event.extendedProps.prefix;
                customer.textContent = arg.event.extendedProps.customer;
                content.append(prefix, customer);
                return { domNodes: [content] };
            },
            eventClick: function (info) {
                info.jsEvent.preventDefault();
                document.querySelectorAll('.fc-event.calendar-event-selected').forEach(function (event) {
                    event.classList.remove('calendar-event-selected');
                });
                info.el.classList.add('calendar-event-selected');
                if (dotNetReference) {
                    dotNetReference.invokeMethodAsync('ShowEventDetails', info.event.id);
                }
            }
        });

        calendar.render();
        this.instances[element.id] = calendar;
    },
    updateSize: function (element) {
        if (element && element.id && this.instances[element.id]) {
            this.instances[element.id].updateSize();
        }
    },
    clearSelection: function () {
        document.querySelectorAll('.fc-event.calendar-event-selected').forEach(function (event) {
            event.classList.remove('calendar-event-selected');
        });
    },
    dispose: function (element) {
        const calendar = this.instances[element.id];
        if (calendar) {
            calendar.destroy();
            delete this.instances[element.id];
        }
    }
};