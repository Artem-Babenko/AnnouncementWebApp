
function countChars(obj, max, elementName, newValue) {
    if (newValue !== undefined) {
        obj.value = newValue;
    }
    document.getElementById(elementName).innerHTML = obj.value.length + "/" + max;
}

async function getAnnouncemetAndSetValue() {
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get('id');

    const response = await fetch(`/update/get?id=${id}`, {
        method: "GET",
        headers: {"Accept" : "application/json"}
    });

    if (response.ok) {
        const announcement = await response.json();
        // Фото
        document.getElementById('photoImg').setAttribute('src', announcement.photoLink);
        document.getElementById("photoLink").value = announcement.photoLink;
        // Назва
        const nameInput = document.querySelector(".name input");
        nameInput.value = announcement.name;
        countChars(nameInput, 40, 'max-length-name', announcement.name);
        // Опис
        const shortDescription = document.querySelector(".short-descritpion textarea");
        shortDescription.value = announcement.shortDescription;
        countChars(shortDescription, 300, 'max-length-short', announcement.shortDescription);
        const fullDescritpion = document.querySelector(".full-descritpion textarea");
        fullDescritpion.value = announcement.fullDescription;
        countChars(shortDescription, 1500, 'max-length-full', announcement.fullDescription);
        // Контакти
        document.querySelector(".contacts .telegram input").value = announcement.telegram;
        document.querySelector(".contacts .email input").value = announcement.email;
        document.querySelector(".contacts .address input").value = announcement.address;
        document.querySelector(".contacts .phone input").value = announcement.phone;
        // Власник
        document.getElementById("owner").value = announcement.owner;
        // Дата
        const startDate = document.getElementById("startDate");
        startDate.valueAsDate = new Date(announcement.startDate);
        const endDate = document.getElementById("endDate");
        endDate.valueAsDate = new Date(announcement.endDate);
        // Тег
        const select = document.querySelector('.tag-select');
        const tagValue = announcement.tag;
        const options = select.options;

        for (let i = 0; i < options.length; i++) {
            if (options[i].value === tagValue) {
                select.selectedIndex = i;
                break; // Exit the loop once the matching option is found
            }
        }
    }
}

async function upload(){
    const data = new FormData();
    data.append('FileName', document.getElementById('photo').value);
    data.append('Image', document.getElementById('photo').files[0]);
    
    const response = await fetch("/update/upload-photo", {
        method: "POST",
        body: data
    });

    if(response.ok){
        const photo = await response.json();
        console.log(`success fileName: ${photo.fileName}`);
        document.getElementById('photo').style.display = "none";
        document.getElementById('dropcontainer').style.display = "none";
        document.getElementById('photoLink').value = photo.fileName;
        document.getElementById('photoImg').setAttribute('src', photo.fileName);
        document.getElementById("double-click").style.display = "flex";
    }
}

async function deleteTemporaryPhoto() {
    const photoLinkElement = document.getElementById('photoLink');
    const response = await fetch(`/update/add-temporary-photo?path=${photoLinkElement.value}`, {
        method: "GET",
        headers: {"Accept" : "application/json"}
    });
    document.getElementById('photo').style.display = "none";
    document.getElementById('dropcontainer').style.display = "flex";
    document.getElementById('photoImg').setAttribute('src', "");
    photoLinkElement.value = "";
    document.getElementById("double-click").style.display = "none";
}

function setNowDate() {
    let currentDate = new Date();
    const startDate = document.getElementById("startDate");
    startDate.value = currentDate.toJSON().slice(0, 10);
    startDate.setAttribute("min", currentDate.toJSON().slice(0, 10));
    currentDate.setMonth(currentDate.getMonth() + 1);
    startDate.setAttribute("max", currentDate.toJSON().slice(0, 10));

    currentDate = new Date();
    const endDate = document.getElementById("endDate");
    currentDate.setDate(currentDate.getDate() + 1);
    endDate.value = currentDate.toJSON().slice(0, 10);
    endDate.setAttribute("min", currentDate.toJSON().slice(0, 10));
    currentDate.setMonth(currentDate.getMonth() + 1);
    endDate.setAttribute("max", currentDate.toJSON().slice(0, 10));
}

async function getTags() {
    const response = await fetch("/update/tags", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    if (response.ok) {
        const tags = await response.json();
        const select = document.querySelector('select');
        tags.forEach(tag => select.append(option(tag)));
    }
}

function option(tag) {
    const optionTag = document.createElement("option");
    optionTag.setAttribute("value", tag.name);
    optionTag.append(tag.name);
    return optionTag;
}

function dragAndDropEvents() {
    const dropContainer = document.getElementById("dropcontainer");
    const fileInput = document.getElementById("photo");

    dropContainer.addEventListener("dragover", (e) => {
        // prevent default to allow drop
        e.preventDefault();
    }, false);

    dropContainer.addEventListener("dragenter", () => {
        dropContainer.classList.add("drag-active");
    });

    dropContainer.addEventListener("dragleave", () => {
        dropContainer.classList.remove("drag-active");
    });

    dropContainer.addEventListener("drop", (e) => {
        e.preventDefault();
        dropContainer.classList.remove("drag-active");
        fileInput.files = e.dataTransfer.files;
        upload();
    });
}

setNowDate();
getTags();
dragAndDropEvents();
getAnnouncemetAndSetValue();

document.getElementById('dropcontainer').style.display = "none";
document.getElementById("double-click").style.display = "flex";