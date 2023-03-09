////const { event } = require("jquery");

localStorage.setItem("test2", "6969");
grid_btn=document.getElementById("grid-btn");
list_btn=document.getElementById("list-btn");
grid_view=document.getElementById("grid-view");
list_view = document.getElementById("list-view");
let country_name = document.getElementsByClassName("country-name");
let city_name = document.getElementsByClassName("city-name");
let skill_name = document.getElementsByClassName("skill-name");
let theme_name = document.getElementsByClassName("theme-name");
let input_text = document.getElementById("searchbar");
let x = document.getElementsByClassName('card-title');
let y = document.getElementsByClassName('mission-list');
let notfound = document.getElementById('notfound');
input_text.addEventListener("keyup", search_mission);

let navbadge_city = document.getElementById("nav-badge-city");
let navbadge_skill = document.getElementById("nav-badge-skill");
let navbadge_theme = document.getElementById("nav-badge-theme");

let badgeclose = document.getElementsByClassName("badge-close");

let removebadge = document.getElementsByClassName("remove-badge");

let navbadge_country = document.getElementById("nav-badge-country");

let mission_city_list = document.getElementsByClassName("mission-location-city");
let mission_theme_list = document.getElementsByClassName("mission-theme-list");
//let mission_skill_list = document.getElementsByClassName("mission-skill-list");

let checkboxInput = document.getElementsByClassName("form-check-input");
let sortBy = document.getElementsByClassName("sortby");
let pagination = document.getElementsByClassName("pagination");

let filterList = new Set([]);
let filterTheme = new Set([]);
let filterSkill = new Set([]);
let citylist = new Array();
let skilllist = new Array();
let themelist = new Array();
let themecitylist = new Array();
let hiddencity = new Array();

//sortBy

for (i = 0; i < sortBy.length; i++) {
    sortBy[i].addEventListener("click" , SortByFilter)
}

//pagination

for (i = 0; i < pagination.length; i++) {
    pagination[i].addEventListener("click" , AddPagination)
}

for (i = 0; i < mission_city_list.length; i++) {
    citylist.push(mission_city_list[i].innerHTML)
}



for (i = 0; i < mission_theme_list.length; i++) {
    themelist.push(mission_theme_list[i].innerHTML)
}

for (i = 0; i < mission_theme_list.length; i++) {
    themecitylist.push({ city: mission_city_list[i].innerHTML, theme: mission_theme_list[i].innerHTML })
    console.log(themecitylist[i].city)
}



for (let i = 0; i < city_name.length; i++) {
    city_name[i].addEventListener('click', filterMissions);
    
   
}

for (let i = 0; i < skill_name.length; i++) {
    skill_name[i].addEventListener('click', filterSkillMissions);
    
   
}

for (let i = 0; i < theme_name.length; i++) {
    theme_name[i].addEventListener('click', filterThemeMissions);
    
   
}

//pagination function
var pageNo = "1";
function AddPagination() {
    pageNo = event.target.innerHTML;
    //pageNo = Number(pageNo)
    //pageNo = pageNo
    sendInfo();
}


//sortBy function
var sortname; 
function SortByFilter() {
    sortname = event.target.innerHTML;
    sendInfo();
}



var obj = {
    city: citylist,
    theme: themelist
}

for (const item in obj) {
    console.log(obj[item]);
}




function search_mission() {
    let count=0;
    let input = input_text.value
    input = input.toLowerCase();


    for (i = 0; i < y.length; i++) {
        if (!x[i].innerHTML.toLowerCase().includes(input)) {
            y[i].classList.add("hide");
            

        }
        else {
            y[i].classList.remove("hide");
            

        }
    }


    for (i = 0; i < y.length; i++) {
        if (y[i].classList.contains("hide")) {
            count++;
        }
    }

    console.log(count, y.length)

    if (count == y.length) {
        notfound.classList.remove('hide');
    }
    else {
        notfound.classList.add('hide');
    }

}

function sendInfo() {
    filtercitystr = "";
    filterthemestr = "";
    filterSkillstr = "";
    let sort = sortname;
    
    if (filterList.size == 0 && filterTheme.size == 0 && filterSkill.size == 0 && sort == null) {
        var url = "/Mission/Home?page=" + pageNo;
        window.location.reload();
    }



    else if (filterList.size == 0 && filterTheme.size == 0 && filterSkill.size == 0) {

        var url = "/Mission/Home?sort=" + sort + "&page=" + pageNo;
    }

    else if (sort == null)
    {
        for (const item of filterList) {
            filtercitystr += item + ",";
        }
        for (const item of filterTheme) {
            filterthemestr += item + ",";
        }
        for (const item of filterSkill) {
            filterSkillstr += item + ",";
        }
        let obj = { city: filtercitystr, theme: filterthemestr, skill: filterSkillstr }

        var url = "/Mission/Home?filter=" + JSON.stringify(obj) + "&page=" + pageNo;
    }
    else {
        for (const item of filterList) {
            filtercitystr += item + ",";
        }
        for (const item of filterTheme) {
            filterthemestr += item + ",";
        }
        for (const item of filterSkill) {
            filterSkillstr += item + ",";
        }
        let obj = { city: filtercitystr, theme: filterthemestr, skill: filterSkillstr }

        var url = "/Mission/Home?filter=" + JSON.stringify(obj) + "&sort=" + sort + "&page=" + pageNo;
    }

    
       

        $.ajax({
            url: url,
            success: function (data) {
                if (data.length == 0) {
                    notfound.classList.remove('hide');
                }
                console.log(data);
                var temp = "";
                for (let i = 0; i < data.length; i++) {
                    var card = `<div class="col mission-list">
                <div class="card h-100">
                    <div class="d-flex justify-content-end" style="background: url('/images/Grow-Trees-On-the-path-to-environment-sustainability.png') center center no-repeat content-box; background-size: cover; height: 200px">

                        <div class="mission-location" >
                            <img src="/images/pin.png" alt="" style="margin-right: 5px;"><span class="mission-location-city">${data[i].value.name}</span>
                        </div>
                        <div class="btn-group mission-buttons-List flex-column">
                            <div class="mission-like">

                                <button class="like-btn-card"><img src="/images/heart.png" alt="" style="height:20px;"></button>
                            </div>
                            <div class="mission-seat">

                                <button data-bs-toggle="modal"
                                data-bs-target="#exampleModal"><img src="/images/user.png" alt=""></button>
                            </div>
                        </div>

                    </div>
                    <div class="mission-theme">
                            <p class="mission-theme-list">${data[i].value.theme}</p>
                    </div>
                    <div class="card-body border-bottom">
                        <h5 class="card-title">${data[i].value.title}</h5>
                        <p class="card-text">${data[i].value.shortDescription}</p>
                        <div class="mission-organization">
                            <div>
                                <p>${data[i].value.OrganizationName}</p>
                            </div>
                            <div>
                                <img src="/images/selected-star.png" alt="">
                                <img src="/images/selected-star.png" alt="">
                                <img src="/images/selected-star.png" alt="">
                                <img src="/images/selected-star.png" alt="">
                                <img src="/images/star.png" alt="">

                            </div>
                        </div>
                    </div>

                    
                        <div class="mission-theme">
                            <span>
                                From ${data[i].value.startDate}until ${data[i].value.endDate}
                            </span>
                        </div>
                    
                    


                    <div class="timestamp row card-body mt-4">
                        <div class="seats-left col d-inline-flex align-items-center">
                            <img src="/images/Seats-left.png" alt="">
                            <div class="p-1 ">
                                <p class="mb-0" style="font-size: 20px;"><strong>10</strong></p>
                                <p class="mb-0">Seats-left</p>
                            </div>
                        </div>
                        <div class="seats-left col d-inline-flex align-items-center">
                            <img src="/images/deadline.png" alt="">
                            <div class="p-1">
                                <p class="mb-0" style="font-size: 20px;"><strong> ${data[i].value.deadLine} </strong></p>
                                <p class="mb-0">Deadline</p>
                            </div>
                        </div>

                    </div>
                    <div class="card-footer text-center">
                        <form>
                            <button asp-controller="Mission" asp-action="Volunteering_Page">Apply <span><img src="/images/right-arrow.png" alt=""></span></button>
                        </form>

                    </div>
                </div>
            </div>`;
                    temp += card;
                }
                var view = `<div class="row row-cols-1 row-cols-lg-2 row-cols-xl-3  g-4" id="list">
                ${temp}
            </div>`
                $("#grid-view").html(view);
            },
            error: function (err) {
                console.error(err);
            }
        })

    
        
    
}




grid_btn.addEventListener("click", function(){
    grid_view.classList.remove("hide");
    list_view.classList.add("hide");
    grid_btn.classList.add("selected");
    grid_btn.classList.remove("not-selected");
    list_btn.classList.add("not-selected");
    list_btn.classList.remove("selected");

});
list_btn.addEventListener("click", function(){
    grid_view.classList.add("hide");
    list_view.classList.remove("hide");
    list_btn.classList.add("selected");
    list_btn.classList.remove("not-selected");
    grid_btn.classList.add("not-selected");
    grid_btn.classList.remove("selected");

});

function listToGrid() {
    grid_view.classList.remove("hide");
    list_view.classList.add("hide");
}

let k = window.matchMedia("(max-width: 991px)");
k.addListener(mediaWidthCheck);

function mediaWidthCheck() {
    if (k.matches) {
        console.log("matched");
        listToGrid();
    }

}

let countCheckskill = -1;
function filterSkillMissions() {
    let count = 0;
    text = event.target.value;
    for (let i = 0; i < skill_name.length; i++) {
        if (skill_name[i].value == text) {
            countCheckskill = i;
            break;
        }
    }
    addFilterSkillTag(text);
    sendInfo();

    for (i = 0; i < y.length; i++) {
        if (y[i].classList.contains("hide")) {
            count++;
        }
    }

    console.log(count, y.length)

    if (count == y.length) {
        notfound.classList.remove('hide');
    }
    else {
        notfound.classList.add('hide');
    }

}

function addFilterSkillTag(text) {
    let temp = "";
    if (!filterSkill.has(text))
        filterSkill.add(text);
    else {
        filterSkill.delete(text);
        if (countCheckskill != -1)
            skill_name[countCheckskill].checked = false;
    }





    for (const item of filterSkill) {
        temp = temp + `<h1 class="badge bg-light text-dark mx-1">  ${item} <button class="not-selected badge-close" value="${item}" aria-label="Close" onclick="filterSkillMissions()">X</button> </h1>`;

    }



    navbadge_skill.innerHTML = temp;

}

let countCheck = -1;
function filterMissions() {
    let count = 0;
    text = event.target.value;
    for (let i = 0; i < city_name.length; i++) {
        if (city_name[i].value == text) {
            countCheck = i;
            break;
        }
    }
    addFilterTag(text);
    sendInfo();

    //for (i = 0; i < y.length; i++) {
    //    if (y[i].classList.contains("hide")) {
    //        count++;
    //    }
    //}

    //console.log(count, y.length)

    //if (count == y.leng) {
    //    notfound.classList.remove('hide');
    //}
    //else {
    //    notfound.classList.add('hide');
    //}
    //if (grid_view)
    //if (y.length == 0) {
        //notfound.classList.remove('hide');
    //}
    //else {
        //notfound.classList.add('hide');
    //}
    
}

function addFilterTag(text) {
    let temp = "";
    if (!filterList.has(text))
        filterList.add(text);
    else {
        filterList.delete(text);
        if (countCheck != -1)
            city_name[countCheck].checked = false;
    }
        
        
    
       

    for (const item of filterList) {
        temp = temp + `<h1 class="badge bg-light text-dark mx-1">  ${item} <button class="not-selected badge-close" value="${item}" aria-label="Close" onclick="filterMissions()">X</button> </h1>`;
        
    }

  

    navbadge_city.innerHTML = temp;

    //if (filterList.size != 0) {
    //    if (filterTheme.size == 0) {
    //        //hiddencity = []
    //        for (const item of citylist) {
    //            if (!filterList.has(item)) {
    //                //let nextIndex = citylist.indexOf(item);
    //                //for (const cityobj of hiddencity) {
    //                //    if (cityobj["city"] == item) {
    //                //        nextIndex = citylist.indexOf(item, cityobj["index"] + 1);
    //                //    }
                        
    //                //}
    //                //y[nextIndex].classList.add("hide");
    //                //y[nextIndex + (y.length / 2)].classList.add("hide");
    //                //hiddencity.push({ index: nextIndex, city: item });
    //                hideCities(item)

    //            }
    //            else {
    //                //var mulCity = 
    //                //y[citylist.indexOf(item)].classList.remove("hide");
    //                //y[citylist.indexOf(item) + (y.length / 2)].classList.remove("hide");
    //                displayCities(item)
    //            }
    //        }
    //    }
    //    else {
    //        for (const item of filterTheme) {
    //            console.log(themecitylist[themelist.indexOf(item)].city, themecitylist[themelist.indexOf(item)].theme);
    //            if (!filterList.has(themecitylist[themelist.indexOf(item)].city)) {
    //                hideCities(themecitylist[themelist.indexOf(item)].city)
    //                //y[themelist.indexOf(item)].classList.add("hide");
    //                //y[themelist.indexOf(item) + (y.length / 2)].classList.add("hide");
    //            }
    //            else {
    //                displayCities(themecitylist[themelist.indexOf(item)].city)
    //                //y[themelist.indexOf(item)].classList.remove("hide");
    //                //y[themelist.indexOf(item) + (y.length / 2)].classList.remove("hide");
    //            }
    //        }

    //    }

    //}
    //else {
    //    for (let i = 0; i < y.length; i++) {
    //        y[i].classList.remove("hide");
    //    }

    //    if (filterTheme.size != 0) {
    //        for (const item of themelist) {
    //            if (!filterTheme.has(item)) {
    //                hideThemes(item);
    //            }
    //            else {
    //                displayThemes(item);
    //            }
    //        }
    //    }
    //}
}
   

let countCheckTheme = -1;
function filterThemeMissions() {
    let count = 0;
    text = event.target.value;
    for (let i = 0; i < theme_name.length; i++) {
        if (theme_name[i].value == text) {
            countCheckTheme = i;
            break;
        }
    }
    addFilterThemeTag(text);
    sendInfo();
    for (i = 0; i < y.length; i++) {
        if (y[i].classList.contains("hide")) {
            count++;
        }
    }

    console.log(count, y.length)

    if (count == y.length) {
        notfound.classList.remove('hide');
    }
    else {
        notfound.classList.add('hide');
    }

}

function addFilterThemeTag(text) {
    let temp = "";
    if (!filterTheme.has(text))
        filterTheme.add(text);
    else {
        filterTheme.delete(text);
        if (countCheckTheme != -1)
            theme_name[countCheckTheme].checked = false;
    }





    for (const item of filterTheme) {
        temp = temp + `<h1 class="badge bg-light text-dark mx-1">  ${item} <button class="not-selected badge-close" value="${item}" aria-label="Close" onclick="filterThemeMissions()">X</button> </h1>`;

    }



    navbadge_theme.innerHTML = temp;

    //if (filterTheme.size != 0) {
    //    if (filterList.size == 0) {
    //        for (const item of themelist) {
    //            if (!filterTheme.has(item)) {
    //                hideThemes(item)
    //                //y[themelist.indexOf(item)].classList.add("hide");
    //                //y[themelist.indexOf(item) + (y.length / 2)].classList.add("hide");
    //            }
    //            else {
    //                //y[themelist.indexOf(item)].classList.remove("hide");
    //                //y[themelist.indexOf(item) + (y.length / 2)].classList.remove("hide");
    //                displayThemes(item)
    //            }
    //        }
    //    }
    //    else {
    //        for (const item of filterList) {
    //            if (!filterTheme.has(themecitylist[citylist.indexOf(item)].theme)) {
    //                hideThemes(themecitylist[citylist.indexOf(item)].theme)
    //                //y[citylist.indexOf(item)].classList.add("hide");
    //                //y[citylist.indexOf(item) + (y.length / 2)].classList.add("hide");
    //            }
    //            else {
    //                displayThemes(themecitylist[citylist.indexOf(item)].theme)
    //                //y[citylist.indexOf(item)].classList.remove("hide");
    //                //y[citylist.indexOf(item) + (y.length / 2)].classList.remove("hide");
    //            }
    //        }

    //    }

    //}
    //else {
    //    for (let i = 0; i < y.length; i++) {
    //        y[i].classList.remove("hide");
    //    }
            
    //    if (filterList.size != 0) {
    //        for (const item of citylist) {
    //            if (!filterList.has(item)) {
    //                hideCities(item);
    //            }
    //            else {
    //                displayCities(item);
    //            }
    //        }
    //    }
    //}

}


//arr = new Array(Object)


for (i = 0; i < country_name.length; i++) {
    country_name[i].addEventListener("click", (e) => {
        if (e.target.innerHTML == "All") {
            navbadge_country.innerHTML = "";
            navbadge.innerHTML = "";
        }
        else {
            console.log(e.target.innerHTML);
            navbadge_country.innerHTML = `<h1 class="badge bg-light text-dark mx-1">  ${e.target.innerHTML}  </h1>`
        }

    });


}


function hideCities(cityName) {
    var mulCity = citylist.filter(c => c == cityName);
    console.log(mulCity);

    if (mulCity.length > 1) {
        let nextItem = citylist.indexOf(cityName);
        for (const city of mulCity) {
            y[nextItem].classList.add("hide");
            y[nextItem + (y.length / 2)].classList.add("hide");
            nextItem = citylist.indexOf(cityName, nextItem + 1);
        }
    }
    else {
        y[citylist.indexOf(cityName)].classList.add("hide");
        y[citylist.indexOf(cityName) + (y.length / 2)].classList.add("hide");
    }
}

function displayCities(cityName) {
    var mulCity = citylist.filter(c => c == cityName);
    console.log(mulCity);

    if (mulCity.length > 1) {
        let nextItem = citylist.indexOf(cityName);
        for (const city of mulCity) {
            y[nextItem].classList.remove("hide");
            y[nextItem + (y.length / 2)].classList.remove("hide");
            nextItem = citylist.indexOf(cityName, nextItem + 1);
        }
    }
    else {
        y[citylist.indexOf(cityName)].classList.remove("hide");
        y[citylist.indexOf(cityName) + (y.length / 2)].classList.remove("hide");
    }
}

function hideThemes(themeTitle) {
    var mulTheme = themelist.filter(c => c == themeTitle);
    console.log(mulTheme);

    if (mulTheme.length > 1) {
        let nextItem = themelist.indexOf(themeTitle);
        for (const theme of mulTheme) {
            y[nextItem].classList.add("hide");
            y[nextItem + (y.length / 2)].classList.add("hide");
            nextItem = themelist.indexOf(themeTitle, nextItem + 1);
        }
    }
    else {
        y[themelist.indexOf(themeTitle)].classList.add("hide");
        y[themelist.indexOf(themeTitle) + (y.length / 2)].classList.add("hide");
    }
}

function displayThemes(themeTitle) {
    var mulTheme = themelist.filter(c => c == themeTitle);
    console.log(mulTheme);

    if (mulTheme.length > 1) {
        let nextItem = themelist.indexOf(themeTitle);
        for (const city of mulTheme) {
            y[nextItem].classList.remove("hide");
            y[nextItem + (y.length / 2)].classList.remove("hide");
            nextItem = themelist.indexOf(themeTitle, nextItem + 1);
        }
    }
    else {
        y[themelist.indexOf(themeTitle)].classList.remove("hide");
        y[themelist.indexOf(themeTitle) + (y.length / 2)].classList.remove("hide");
    }
}



