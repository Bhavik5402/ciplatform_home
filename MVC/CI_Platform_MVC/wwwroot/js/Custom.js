grid_btn=document.getElementById("grid-btn");
list_btn=document.getElementById("list-btn");
grid_view=document.getElementById("grid-view");
list_view = document.getElementById("list-view");
let country_name = document.getElementsByClassName("country-name");
let city_name = document.getElementsByClassName("city-name");
let input_text = document.getElementById("searchbar");
let x = document.getElementsByClassName('card-title');
let y = document.getElementsByClassName('mission-list');
let notfound = document.getElementById('notfound');
input_text.addEventListener("keyup", search_mission);

let navbadge = document.getElementById("nav-badge");

let badgeclose = document.getElementsByClassName("badge-close");

let removebadge = document.getElementsByClassName("remove-badge");

let navbadge_country = document.getElementById("nav-badge-country");





//let filterList = new Set([]);


// Filtering feature variables
//let dropdownItems = document.getElementsByClassName("dropdown-item");
let checkboxInput = document.getElementsByClassName("form-check-input");
//let filtersDiv = document.getElementById("nav-badge");
let filterList = new Set([]);



// Event Listener for filtering items
//for (let i = 0; i < dropdownItems.length; i++) {
//    dropdownItems[i].addEventListener('click', filterMissionsCountry);
//}

for (let i = 0; i < checkboxInput.length; i++) {
    checkboxInput[i].addEventListener('click', filterMissions);
   
}





function search_mission() {
    let count=0;
    let input = input_text.value
    input = input.toLowerCase();


    for (i = 0; i < y.length; i++) {
        if (!x[i].innerHTML.toLowerCase().includes(input)) {
            //x[i].style.display = "none";
            //y[i].style.display = "none";
            y[i].classList.add("hide");
            

        }
        else {
            //y[i].style.display = "block";
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



//function filterMissionsCountry() {
//    let text = event.target.innerHTML;
//    addFilterTag(text);
//    //console.log(filterList);
//}

function filterMissions() {
     text = event.target.value;
    addFilterTag(text);
    //console.log(filterList);
}

function addFilterTag(text) {
    let temp = "";
    if (!filterList.has(text))
        filterList.add(text);
    else
        filterList.delete(text);
        
    
       

    for (const item of filterList) {
        temp = temp + `<h1 class="badge bg-light text-dark mx-1">  ${item} <button class="not-selected badge-close" value="${item}" aria-label="Close" onclick="filterMissions()">X</button> </h1>`;
        
    }

  

    navbadge.innerHTML = temp;
   
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

        //navbadge.innerHTML = "";
    });


}


//for (i = 0; i < city_name.length; i++) {
//    city_name[i].addEventListener("click", (e) => {
//        let text = e.target.value;
//        console.log(e.target.value);
//        if (!navbadge.innerHTML.includes(text)) {
//            navbadge.innerHTML += `<h1 class="badge bg-danger remove-badge mx-1">  ${e.target.value} <button class="not-selected badge-close">X</button> </h1>`
//        }
      
//    });


//}




//for (i = 0; i < removebadge.length; i++) {
//    badgeclose.addEventListener("click", () => {
//        removebadge[i].classList.add("hide");
//        //navbadge.remove(badgeclose)
//    });
//}

