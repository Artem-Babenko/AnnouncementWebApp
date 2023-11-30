
// Отримуємо посилання на розділ div з класом ".sort-by-tag"
const sortByTag = document.querySelector(".sort-by-tag");
let selectedInput = sortByTag.querySelector('input[type="radio"]:checked');
let selectedTag = Cookies.get('selectedTag') || selectedInput.value;
// Отримуємо посилання на розділ div з класом "sort"
const divElement = document.querySelector(".sort");
// Отримуємо посилання на кнопку з класом "active" всередині розділу div
let activeButton = divElement.querySelector(".active");
// Отримуємо текст з кнопки з класом "active"
let sortMethod = Cookies.get('sortMethod') || activeButton.textContent;

let currentPage = parseInt(Cookies.get("currentPage")) || 1;
const announcementPerPage = 25;
const pagesPerGroup = 3;
let totalPages = 1;

const pageList = document.querySelector('ul.pages');
document.querySelector('.fa-chevron-left').style.display = "none";
// Кнопка повернення вгору
let scrollBackButton = document.getElementById("go-top-button");
// Отримуємо всі кнопки сортування
const sortButtons = document.querySelectorAll('.sort button');

async function getTagAndSetFilters() {
    const response = await fetch("/main/tags", {
        method: "GET",
        headers: {"Accept" : "application/json"}
    });
    if (response.ok) {
        const tags = await response.json();
        const sortByTag = document.querySelector(".sort-by-tag");
        tags.forEach(tag => sortByTag.append(tagLabel(tag)));
    }

    // Отримуємо значення з кук
    let sortMethod = Cookies.get("sortMethod") || "За датою";
    let selectedTag = Cookies.get("selectedTag");

    // Встановлюємо клас "active" для фільтрів на основі значень з кук
    const sortButtons = document.querySelectorAll(".sort button");
    for (const button of sortButtons) {
        if (button.textContent === sortMethod) {
            button.classList.add("active");
        } else {
            button.classList.remove("active");
        }
    }

    const tagInputs = document.querySelectorAll('input[name="tag"]');
    for (const input of tagInputs) {
        if (input.value === selectedTag) {
            input.checked = true;
        }
    }
}

function tagLabel(tag) {
    const labelContainer = document.createElement("label");

    const radioInput = document.createElement("input");
    radioInput.setAttribute("type", "radio");
    radioInput.setAttribute("name", "tag");
    radioInput.setAttribute("value", tag.name);
    labelContainer.append(radioInput);

    const inputSpan = document.createElement("span");
    inputSpan.setAttribute("class", "checkmark");
    labelContainer.append(inputSpan);

    labelContainer.append(tag.name);

    return labelContainer;
}

async function getUserInfo() {
    const response = await fetch("/main/user", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok) {
        const mes = await response.json();
        if (mes.message == "null") {
            document.getElementById("login").style.display = "block";
            document.getElementById("registration").style.display = "block";
            document.getElementById("profile").style.display = "none";
        }
        else {
            document.getElementById("login").style.display = "none";
            document.getElementById("registration").style.display = "none";
            document.getElementById("profile").style.display = "block";
            document.getElementById("login-li").style.borderRight = "none";
        }
    }
}

async function getAnnouncements(page, sortBy, sortByTag, search = "") {
    const response = await fetch(`/main/announcements?page=${page}&sort=${sortBy}&tag=${sortByTag}&search=${search}`, {
        method: "GET",
        headers: {"Accept" : "application/json"}
    });
    if (response.ok) {
        let responseObj = await response.json();
        const main = document.querySelector('main');
        const pager = document.querySelector('.pager');

        if (responseObj.announcements.length > 0) {
            pager.querySelector(".pages").style.display = "flex";
            pager.querySelector(".null-message").style.display = "none";
            currentPage = page;
            //Очистити оголошення
            main.innerText = '';
            //Заповнити новими
            responseObj.announcements.forEach(announcement => main.append(container(announcement)));
            totalPages = responseObj.totalPages;
            updatePageList();

            /*const computedStyle = window.getComputedStyle(document.getElementById('main'));
            const columnCount = parseInt(computedStyle.getPropertyValue('column-count'));

            let newAnnouncements = responseObj.announcements;

            if (columnCount === 2) {
                const even = [];
                const odd = [];
                responseObj.announcements.forEach((announcement, index) => {
                    if (index % 2 === 0) {
                        even.push(announcement);
                    } else {
                        odd.push(announcement);
                    }
                });
                newAnnouncements = even.concat(odd);
            } else if (columnCount === 3) {
                const columns = [];
                for (let i = 0; i < columnCount; i++) {
                    columns.push([]);
                }
                responseObj.announcements.forEach((announcement, index) => {
                    const columnIndex = index % columnCount;
                    columns[columnIndex].push(announcement);
                });
                newAnnouncements = columns.flat();
            }

            newAnnouncements.forEach(announcement => main.appendChild(container(announcement)));*/
        }
        else {
            currentPage = 1;
            totalPages = 1;
            main.innerText = '';
            pager.querySelector(".pages").style.display = "none";
            pager.querySelector(".null-message").style.display = "block";
            updatePageList();
        }

        Cookies.set("currentPage", currentPage);
    }
}

// Функція для оновлення списку сторінок
function updatePageList() {
    pageList.innerHTML = "";

    // Розрахунок діапазону сторінок для відображення
    const startPage = Math.max(1, currentPage - pagesPerGroup);
    const endPage = Math.min(totalPages, currentPage + pagesPerGroup);

    for (let i = startPage; i <= endPage; i++) {
        const isActive = i === currentPage;
        const pageElement = createPageElement(i, isActive);
        pageList.appendChild(pageElement);
    }

    if (currentPage === totalPages) {
        document.querySelector('.fa-chevron-right').style.display = "none";
    }
    else {
        document.querySelector('.fa-chevron-right').style.display = "flex";
    }

    if (currentPage === 1) {
        document.querySelector('.fa-chevron-left').style.display = "none";
    }
    else {
        document.querySelector('.fa-chevron-left').style.display = "flex";
    }
}

function createPageElement(pageNumber, isActive) {
    const li = document.createElement("li");
    li.textContent = pageNumber;
    if (isActive) {
        li.classList.add("active");
    }
    li.addEventListener("click", () => {
        getAnnouncements(pageNumber, sortMethod, selectedTag);
        scrollToTop(250);
    });
    return li;
}

// Обробка кліку на пейджері
document.querySelector('.fa-chevron-left').addEventListener('click', () => {
    if (currentPage > 1) {
        getAnnouncements(currentPage - 1, sortMethod, selectedTag);
        scrollToTop(250);
    }
});

document.querySelector('.fa-chevron-right').addEventListener('click', () => {
    getAnnouncements(currentPage + 1, sortMethod, selectedTag);
    scrollToTop(250);
});

function container(announcement) {
    const container = document.createElement("div");
    container.setAttribute("class", "container");

    const img = document.createElement("img");
    img.setAttribute("src", announcement.photoLink);
    container.append(img);

    const name = document.createElement("div");
    name.setAttribute("class", "name");
    name.append(announcement.name);
    container.append(name);

    const shortDescription = document.createElement("div");
    shortDescription.setAttribute("class", "short-description");
    shortDescription.append(announcement.shortDescription);
    container.append(shortDescription);

    const meta = document.createElement("div");
    meta.setAttribute("class", "meta");

    const calendarIcon = document.createElement("i");
    calendarIcon.setAttribute("class", "fa-solid fa-calendar-days");
    meta.append(calendarIcon);

    const date = document.createElement("div");
    date.setAttribute("class", "date");
    date.append(announcement.startDateString);
    meta.append(date);

    const tagIcon = document.createElement("i");
    tagIcon.setAttribute("class", "fa-solid fa-tag");
    meta.append(tagIcon);

    const tag = document.createElement("div");
    tag.setAttribute("class", "date");
    tag.append(announcement.tag);
    meta.append(tag);

    const viewsIcon = document.createElement("i");
    viewsIcon.setAttribute("class", "fa-solid fa-eye");
    meta.append(viewsIcon);

    const views = document.createElement("div");
    views.setAttribute("class", "date");
    views.append(announcement.views);
    meta.append(views);

    container.append(meta);

    container.addEventListener("click", () => {
        window.location.href = `/announcement?id=${announcement.id}`;
    });
    return container;
}

scrollBackButton.addEventListener("click", function () {
    scrollToTop(250); // Викликаємо функцію для плавного повернення
});

// Плавне повернення вгору з прискоренням
function scrollToTop(scrollDuration) {
    const scrollStep = -window.scrollY / (scrollDuration / 15);
    let currentScrollPosition = window.scrollY;

    function scroll() {
        currentScrollPosition += scrollStep;
        window.scrollTo(0, currentScrollPosition);

        if (currentScrollPosition < 0) {
            clearInterval(scrollInterval);
        }
    }

    const scrollInterval = setInterval(scroll, 15);
}

// Відображення кнопки при прокрутці
window.onscroll = function () {
    scrollFunction();
};

function scrollFunction() {
    if (document.body.scrollTop > 500 || document.documentElement.scrollTop > 500) {
        scrollBackButton.style.opacity = 1;
        scrollBackButton.style.transform = "scale(1)"; // Масштабуємо кнопку для створення ефекту
    } else {
        scrollBackButton.style.opacity = 0;
        scrollBackButton.style.transform = "scale(0)"; // Зменшуємо масштаб кнопки для приховування
    }
}
// ---------------

// Код фільтрів
// Додаємо обробник події для кожної кнопки сортування
sortButtons.forEach(button => {
    button.addEventListener('click', () => {
        // Перевіряємо, чи кнопка має клас "active"
        if (!button.classList.contains('active')) {
            // Якщо кнопка не має класу "active", то додаємо його і видаляємо з інших кнопок
            sortButtons.forEach(otherButton => {
                otherButton.classList.remove('active');
            });
            button.classList.add('active');
        }
    });
});

document.getElementById("filters").addEventListener("click", function () {
    const filterContainer = document.querySelector(".filter-container");
    filterContainer.style.transform = "scale(1)";
});

document.getElementById("saveButton").addEventListener("click", function () {
    const filterContainer = document.querySelector(".filter-container");
    filterContainer.style.transform = "scale(0)";
    activeButton = divElement.querySelector(".active");
    sortMethod = activeButton.textContent;
    selectedInput = sortByTag.querySelector('input[type="radio"]:checked');
    selectedTag = selectedInput.value;
    currentPage = 1;
    getAnnouncements(currentPage, sortMethod, selectedTag);

    //збереження в куки
    //document.cookie = `sortMethod=${sortMethod}; selectedTag=${selectedTag}`;
    Cookies.set("sortMethod", sortMethod);
    Cookies.set("selectedTag", selectedTag);
});

document.getElementById("searchButton").addEventListener("click", function () {
    const searchInput = document.getElementById("searchInput");
    const searchValue = searchInput.value;
    currentPage = 1;
    getAnnouncements(currentPage, sortMethod, selectedTag, searchValue);
});

// Додаємо обробник події для кнопки "Скинути"
document.getElementById("resetButton").addEventListener("click", function () {
    // Знаходимо всі радіокнопки і знімаємо позначку з кожної
    const radioButtons = document.querySelectorAll('.sort-by-tag input[type="radio"]');
    radioButtons.forEach(radioButton => {
        radioButton.checked = false;
    });
    radioButtons[0].checked = true;
    // Знаходимо всі кнопки сортування і видаляємо клас "active"
    sortButtons.forEach(button => {
        button.classList.remove('active');
    });
    sortButtons[0].classList.add('active');
});

getUserInfo();
getAnnouncements(currentPage, sortMethod, selectedTag);
updatePageList();
getTagAndSetFilters();