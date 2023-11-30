async function getUser() {
    const response = await fetch("/profile/user", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    if (response.ok) {
        const user = await response.json();
        document.getElementById("userName").innerText = "Ім'я: " + user.name;
        document.getElementById("userSurname").innerText = "Прізвище: " + user.surname;
        document.getElementById("userEmail").innerText = "Пошта: " + user.email;
        document.getElementById("dateRegistration").innerText = "Дата реєстрації: " + user.registrationDate;
        document.getElementById("user-views").innerText = "Переглянуто оголошень: " + user.views;
    }
}

async function getUserAnnouncements() {
    const response = await fetch("/profile/user/announcements", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok) {
        const announcements = await response.json();
        const table = document.querySelector('tbody');
        let allViews = 0;
        let activeAnnouncemets = 0;
        let number = 1;
        announcements.forEach(announcement => {
            table.append(row(announcement, number));
            number++;
            allViews += announcement.views;
            activeAnnouncemets += announcement.active ? 1 : 0;
            });
        
        document.getElementById("all-announcements").innerText = "Всього оголошень: " + announcements.length;
        document.getElementById("active-announcements").innerText = "Активних оголошень: " + activeAnnouncemets;
        document.getElementById("all-views").innerText = "Переглядів: " + allViews;
    }
}

async function deleteAnnouncement(id) {
    const response = await fetch("/profile/user/announcements", {
        method: "DELETE",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            id: id
        })
    });

    if (response.ok) {
        const announcementId = await response.json();
        document.querySelector(`tr[row-id='${announcementId}']`).remove();
    }
}


function createActionPanel(announcement) {
    const actionPanel = document.createElement('div');
    actionPanel.setAttribute("id", "action-panel");

    //<i class="fa-solid fa-xmark"></i>
    const closeButton = document.createElement('i');
    closeButton.addEventListener("click", function () {
        const trs = document.querySelectorAll('tr');
        trs.forEach(function (tr) {
            tr.style.boxShadow = "";
        });
        actionPanel.remove();
    });
    closeButton.setAttribute("class", "fa-solid fa-xmark close-button");
    actionPanel.append(closeButton);

    const label = document.createElement("h3");
    label.append(announcement.name);
    actionPanel.append(label);

    const openButton = document.createElement('div');
    openButton.setAttribute("id", "open");
    openButton.innerText = "Відкрити";
    openButton.addEventListener("click", function () {
        window.location.href = `/announcement?id=${announcement.id}`;
    });
    actionPanel.append(openButton);

    const editButton = document.createElement('div');
    editButton.addEventListener("click", function () {
        window.location.href = `/update?id=${announcement.id}`;
    });
    editButton.setAttribute("id", "edit");
    editButton.innerText = "Редагувати";
    actionPanel.append(editButton);

    const deleteButton = document.createElement('div');
    deleteButton.addEventListener('click', async () => {
        await deleteAnnouncement(announcementId);
        const trs = document.querySelectorAll('tr');
        trs.forEach(function (tr) {
            tr.style.boxShadow = "";
        });
        actionPanel.remove();  
    });
    deleteButton.setAttribute("id", "delete");
    deleteButton.innerText = "Видалити";
    actionPanel.append(deleteButton);

    actionPanel.addEventListener("mouseleave", function () {
        const trs = document.querySelectorAll('tr');
        trs.forEach(function (tr) {
            tr.style.boxShadow = "";
        });
        actionPanel.remove();
    });

    return actionPanel;
}

function row(announcement, number) {
    const tr = document.createElement('tr');
    tr.setAttribute("row-id", announcement.id);

    const num = document.createElement('td');
    num.style.textAlign = "center";
    num.append(number);
    tr.append(num);

    const name = document.createElement('td');
    name.append(announcement.name);
    tr.append(name);
    
    const startDate = document.createElement('td');
    startDate.append(announcement.startDateString);
    tr.append(startDate);

    const endDate = document.createElement('td');
    endDate.append(announcement.endDateString);
    tr.append(endDate);

    const tag = document.createElement('td');
    tag.append(announcement.tag);
    tr.append(tag);

    const active = document.createElement('td');
    active.style.fontWeight = "500";

    // Отримуємо сьогоднішню дату в форматі "dd.mm.yy"
    const today = new Date().toLocaleDateString('uk-UA', {
        year: '2-digit',
        month: '2-digit',
        day: '2-digit'
    }).replace(/\./g, '-');

    // Перетворюємо endDateString у формат "dd.mm.yy"
    const endDateStringParts = announcement.endDateString.split('.');
    const endDateFormatted = `${endDateStringParts[0]}-${endDateStringParts[1]}-${endDateStringParts[2]}`;

    if (endDateFormatted === today) {
        active.style.color = "#d7bf04";
        active.append("Сьогодні");
    } else if(announcement.active){
        active.style.color = "green";
        active.append("Так");
    } else {
        active.style.color = "red";
        active.append("Ні");
    }
    tr.append(active);

    const views = document.createElement('td');
    views.append(announcement.views);
    tr.append(views);

    tr.addEventListener("click", function () {
        
        tr.style.boxShadow = "3px 1px 10px rgba(0,0,0,0.3)";

        const actionPanel = createActionPanel(announcement);
        document.querySelector('body').append(actionPanel);

        actionPanel.style.display = "block";
        
    });

    return tr;
}

const helpButton = document.querySelector(".help-button");
const helpContainer = document.querySelector(".help-container");

helpButton.addEventListener("click", function () {
    if (helpContainer.style.transform === "scale(1)") {
        helpContainer.style.transform = "scale(0)";
    } else {
        helpContainer.style.transform = "scale(1)";
    }
});

getUserAnnouncements();
getUser();