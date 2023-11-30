async function getAnnouncement() {
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get('id');

    const response = await fetch(`/announcement/get?id=${id}`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
        
    if (response.ok) {
        const announcement = await response.json();
        document.getElementById("name").innerText = announcement.name;
        document.getElementById("main-img").setAttribute("src", announcement.photoLink);
        document.getElementById("full-description").innerText = announcement.fullDescription;
        document.getElementById("owner").innerText = announcement.owner;
        document.getElementById("end-date").innerText = "Дійсне до " +announcement.endDateString;
        document.getElementById("tag").innerText = announcement.tag;
        document.getElementById("views").innerText = announcement.views;

        const contactElements = contacts(announcement);
        const contactsContainer = document.getElementById("contacts");
        contactElements.forEach(element => {
            contactsContainer.appendChild(element);
        });
    }
}

function contacts(announcement) {
    let contactsElements = [];
    if (announcement.telegram && announcement.telegram.trim().length > 0) {
        const telegram = createContactElement("telegram", "fa-brands fa-telegram", announcement.telegram);
        contactsElements.push(telegram);
    }
    if (announcement.email && announcement.email.trim().length > 0) {
        const email = createContactElement("email", "fa-solid fa-envelope", announcement.email);
        contactsElements.push(email);
    }
    if (announcement.address && announcement.address.trim().length > 0) {
        const address = createContactElement("address", "fa-solid fa-map-location-dot", announcement.address);
        contactsElements.push(address);
    }
    if (announcement.phone && announcement.phone.trim().length > 0) {
        const phone = createContactElement("phone", "fa-solid fa-phone", announcement.phone);
        contactsElements.push(phone);
    }
    return contactsElements;
}

function createContactElement(className, iconClass, text) {
    const contactElement = document.createElement('div');
    contactElement.setAttribute("class", className);
    
    const icon = document.createElement('i');
    icon.setAttribute("class", iconClass);
    contactElement.appendChild(icon);

    const textElement = document.createElement('p');
    textElement.innerText = text;
    contactElement.appendChild(textElement);

    contactElement.addEventListener('click', function () {
        if (this.classList.contains('active')) {
            this.classList.remove('active');
        } else {
            const activeElements = document.querySelectorAll(".contacts div.active");
            if (activeElements.length > 0) {
                activeElements.forEach(element => {
                    element.classList.remove('active');
                });
            }
            this.classList.add('active');
        }
    });

    return contactElement;
}

getAnnouncement();