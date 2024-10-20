document.addEventListener('DOMContentLoaded', function () {
    const calendarEl = document.getElementById('calendar');
    const eventDetails = document.getElementById("eventDetails");
    var sidebar = document.getElementById('sidebar');
    var overlay = document.getElementById('overlay');

    const calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        locale:"pt-br",
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
        },
        events: function (fetchInfo, successCallback, failureCallback) {
            fetch('/Agendamentos/GetAllAsJSON')
                .then(response => response.json())
                .then(data => successCallback(data.result))
                .catch(error => failureCallback(error));
        },
        eventClick: function (info) {
            fetch(`/Agendamentos/Details/${info.event.id}`)
                .then(response => response.text())
                .then(data => {
                    eventDetails.innerHTML = data
                })
                .catch(error => console.log(error));

            // Previna que o link padrão seja acionado, caso exista
            info.jsEvent.preventDefault();

            sidebar.classList.add('open');
            overlay.classList.add('active');
        }
    })

    calendar.render();

    document.getElementById('closeSidebar').addEventListener('click', function () {
        sidebar.classList.remove('open');
        overlay.classList.remove('active');
    });

    overlay.addEventListener('click', function () {
        sidebar.classList.remove('open');
        overlay.classList.remove('active');
    });
})
