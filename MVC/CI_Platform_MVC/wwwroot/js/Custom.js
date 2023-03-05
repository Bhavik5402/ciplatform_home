//localStorage.setItem("test2", "6969");
grid_btn=document.getElementById("grid-btn");
list_btn=document.getElementById("list-btn");
grid_view=document.getElementById("grid-view");
list_view = document.getElementById("list-view");
let country_name = document.getElementsByClassName("country-name");
let city_name = document.getElementsByClassName("city-name");
let theme_name = document.getElementsByClassName("theme-name");
let input_text = document.getElementById("searchbar");
let x = document.getElementsByClassName('card-title');
let y = document.getElementsByClassName('mission-list');
let notfound = document.getElementById('notfound');
input_text.addEventListener("keyup", search_mission);

let navbadge_city = document.getElementById("nav-badge-city");
let navbadge_theme = document.getElementById("nav-badge-theme");

let badgeclose = document.getElementsByClassName("badge-close");

let removebadge = document.getElementsByClassName("remove-badge");

let navbadge_country = document.getElementById("nav-badge-country");

let mission_city_list = document.getElementsByClassName("mission-location-city");
let mission_theme_list = document.getElementsByClassName("mission-theme-list");

let checkboxInput = document.getElementsByClassName("form-check-input");

let filterList = new Set([]);
let filterTheme = new Set([]);
let citylist = new Array();
let themelist = new Array();
let themecitylist = new Array();

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

for (let i = 0; i < theme_name.length; i++) {
    theme_name[i].addEventListener('click', filterThemeMissions);
    
   
}




//var obj = {
//    city: citylist,
//    theme: themelist
//}

//for (const item in obj) {
//    console.log(obj[item]);
//}




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

let countCheck = -1;
function filterMissions() {
    text = event.target.value;
    for (let i = 0; i < city_name.length; i++) {
        if (city_name[i].value == text) {
            countCheck = i;
            break;
        }
    }
    addFilterTag(text);
    
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

    if (filterList.size != 0) {
        if (filterTheme.size == 0) {
            for (const item of citylist) {
                if (!filterList.has(item)) {
                    y[citylist.indexOf(item)].classList.add("hide");
                }
                else {
                    y[citylist.indexOf(item)].classList.remove("hide");
                }
            }
        }
        else
        {
            for (const item of filterTheme)
            {
               
                ////city = themecitylist["city"]
                console.log(themecitylist[themelist.indexOf(item)].city, themecitylist[themelist.indexOf(item)].theme )
                //console.log(item)
            }

        }
        
    }
    else {
        for (let i = 0; i < y.length; i++)
            y[i].classList.remove("hide");
    }
   
}

let countCheckTheme = -1;
function filterThemeMissions() {
    text = event.target.value;
    for (let i = 0; i < theme_name.length; i++) {
        if (theme_name[i].value == text) {
            countCheckTheme = i;
            break;
        }
    }
    addFilterThemeTag(text);

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

    if (filterTheme.size != 0) {
        for (const theme of themelist) {

            if (!filterTheme.has(theme) ) {
                y[themelist.indexOf(theme)].classList.add("hide");
            }
            else {
                y[themelist.indexOf(theme)].classList.remove("hide");
            }
        }
    }
    else {
        for (let i = 0; i < y.length; i++)
            y[i].classList.remove("hide");
    }

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

        //navbadge.innerHTML = "";
    });


}



