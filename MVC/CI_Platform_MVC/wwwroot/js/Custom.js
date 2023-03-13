////const { event } = require("jquery");

//const { event } = require("jquery");

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
let rating_btn = document.getElementsByClassName("rating-btn");
//let AddFav = document.getElementById("add-fav");
//let like_btn = document.getElementById("like-btn");
//like_btn.addEventListener("click", AddToFav);

let filterList = new Set([]);
let filterTheme = new Set([]);
let filterSkill = new Set([]);
let citylist = new Array();
let skilllist = new Array();
let themelist = new Array();
let themecitylist = new Array();
let hiddencity = new Array();
//input_text.addEventListener("keyup", search_mission);

//sortBy

for (i = 0; i < sortBy.length; i++) {
    sortBy[i].addEventListener("click" , SortByFilter)
}

//pagination

for (i = 0; i < pagination.length; i++) {
    pagination[i].addEventListener("click" , AddPagination)
}

//rating

for (i = 0; i < rating_btn.length; i++) {
    rating_btn[i].addEventListener("click", missionRating)
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
var pageNo = 0 ;
function AddPagination() {
    pageNo = event.target.innerHTML;
    pageNo = Number.parseInt(pageNo);
    sendInfo();
}


//nextpointer

function NextPointer() {
    pageNo = Number.parseInt(pageNo);
    pageNo = pageNo + 1;
    sendInfo();
}

//prevpointer

function PrevPointer() {
    pageNo = Number.parseInt(pageNo);
    pageNo = pageNo - 1;
    sendInfo();
}

//sortBy function
var sortname; 
function SortByFilter() {
    sortname = event.target.innerHTML;
    sendInfo();
}




let inputData = "";
function search_mission() {
    let count = 0;
    inputData = input_text.value
    inputData = inputData.toLowerCase();

    sendInfo();

  

}


//function AddToFav() {
//    if (like_btn.classList.contains("bg-white")) {
//        add_fav.setAttribute("src", "/images/heart.png");
//        like_btn.classList.remove("bg-white");
//        like_btn.classList.add("bg-red");
//    }
//    else {
//        add_fav.setAttribute("src", "/images/heart1.png");
//        like_btn.classList.add("bg-white");
//        like_btn.classList.remove("bg-red");
//    }
//    var Data = event.target.getAttribute("value");
//    var favArr = Data.split(" ");
//    let favObj = {
//        missionId: favArr[0],
//        userId: favArr[1]
//    }
//    var url = "/Mission/AddToFav?favObj=" + JSON.stringify(favObj);
//    $.ajax({
//        url: url,
//        success: function (data) {
//            //var url = "/Mission/Volunteering_Page";
//            window.location.reload();
//        },
//        error: function (err) {
//            console.error(err);
//        }
//    })

//}


function missionRating() {
    var rating = event.target.getAttribute("value");
    var ratingarr = rating.split(" ");
    let ratingObj = {
        ratingval: ratingarr[0],
        missionId: ratingarr[1],
        userId: ratingarr[2]
    }
    var url = "/Mission/AddRating?rating=" + JSON.stringify(ratingObj);
    $.ajax({
        url: url,
        success: function (data) {
            //var url = "/Mission/Volunteering_Page";
            window.location.reload();
        },
        error: function (err) {
            console.error(err);
        }
    })
}

function sendInfo() {
    filtercitystr = "";
    filterthemestr = "";
    filterSkillstr = "";
    let sort = sortname;
    let searchInput = inputData;
    



    if (filterList.size == 0 && filterTheme.size == 0 && filterSkill.size == 0 && searchInput == "" && sort == null && pageNo == 0 ) {

        var url = "/Mission/Home/";
        window.location.reload();
    }


    else if (filterList.size == 0 && filterTheme.size == 0 && filterSkill.size == 0 && searchInput == "" && sort == null)
    {
        var url = "/Mission/Home?page=" + pageNo ;
    }





    else if (filterList.size == 0 && filterTheme.size == 0 && filterSkill.size == 0 && searchInput == "") {

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
        let obj = { city: filtercitystr, theme: filterthemestr, skill: filterSkillstr, searchItem : searchInput }

        var url = "/Mission/Home?filter=" + JSON.stringify(obj) + "&page=" + pageNo ;
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
        let obj = { city: filtercitystr, theme: filterthemestr, skill: filterSkillstr, searchItem: searchInput}

        var url = "/Mission/Home?filter=" + JSON.stringify(obj) + "&sort=" + sort + "&page=" + pageNo ;
    }

    
       

        $.ajax({
            url: url,
            success: function (data) {
                if (data.length == 0) {
                    notfound.classList.remove('hide');
                }
                else {
                    notfound.classList.add('hide');
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
                                <p>${data[i].value.organizationName}</p>
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

   
}





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
