grid_btn=document.getElementById("grid-btn");
list_btn=document.getElementById("list-btn");
grid_view=document.getElementById("grid-view");
list_view=document.getElementById("list-view");

grid_btn.addEventListener("click", function(){
    grid_view.classList.add("show");
    list_view.classList.remove("show");

});
list_btn.addEventListener("click", function(){
    grid_view.classList.remove("show");
    list_view.classList.add("show");

});

