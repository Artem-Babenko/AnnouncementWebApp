
function countChars(obj, max, elementName) {
    document.getElementById(elementName).innerHTML = obj.value.length + "/" + max;
}

async function upload(){
    const data = new FormData();
    data.append('FileName', document.getElementById('photo').value);
    data.append('Image', document.getElementById('photo').files[0]);
    
    const response = await fetch("/create/upload-photo", {
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

function deleteTemporaryPhoto(){
    document.getElementById('photo').style.display = "none";
    document.getElementById('dropcontainer').style.display = "flex";
    document.getElementById('photoImg').setAttribute('src', "");
    document.getElementById('photoLink').value = "";
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

async function getUser() {
    const response = await fetch("/create/user", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    if (response.ok) {
        const user = await response.json();
        document.getElementById("owner").value = user.name + " " + user.surname;
    }
}

async function getTags() {
    const response = await fetch("/create/tags", {
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

getUser();
getTags();
setNowDate();
dragAndDropEvents();
