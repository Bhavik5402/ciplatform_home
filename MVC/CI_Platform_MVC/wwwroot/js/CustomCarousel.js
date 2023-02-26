$(document).ready(function () {
    var itemsMainDiv = ('.MultiCarousel');
    var itemsDiv = ('.MultiCarousel-inner');
    var itemWidth = "";

    $('.leftLst, .rightLst').click(function () {
        var condition = $(this).hasClass("leftLst");
        if (condition)
            click(0, this);
        else
            click(1, this)
    });

    ResCarouselSize();




    $(window).resize(function () {
        ResCarouselSize();
    });

    //this function define the size of the items
    function ResCarouselSize() {
        var incno = 0;
        var dataItems = ("data-items");
        var itemClass = ('.item');
        var id = 0;
        var btnParentSb = '';
        var itemsSplit = '';
        var sampwidth = $(itemsMainDiv).width();
        var bodyWidth = $('body').width();
        $(itemsDiv).each(function () {
            id = id + 1;
            var itemNumbers = $(this).find(itemClass).length;
            btnParentSb = $(this).parent().attr(dataItems);
            itemsSplit = btnParentSb.split(',');
            $(this).parent().attr("id", "MultiCarousel" + id);

            incno = itemsSplit[1];
            itemWidth = sampwidth / incno;

            $(this).css({ 'transform': 'translateX(0px)', 'width': itemWidth * itemNumbers });
            $(this).find(itemClass).each(function () {
                $(this).outerWidth(itemWidth);
            });

            $(".leftLst").addClass("over");
            $(".rightLst").removeClass("over");

        });
    }


    //this function used to move the items
    function ResCarousel(e, el, s) {
        var leftBtn = ('.leftLst');
        var rightBtn = ('.rightLst');
        var translateXval = '';
        var divStyle = $(el + ' ' + itemsDiv).css('transform');
        var values = divStyle.match(/-?[\d\.]+/g);
        var xds = Math.abs(values[4]);
        if (e == 0) {
            translateXval = parseInt(xds) - parseInt(itemWidth * s);
            $(el + ' ' + rightBtn).removeClass("over");

            if (translateXval <= itemWidth / 2) {
                translateXval = 0;
                $(el + ' ' + leftBtn).addClass("over");
            }
        }
        else if (e == 1) {
            var itemsCondition = $(el).find(itemsDiv).width() - $(el).width();
            translateXval = parseInt(xds) + parseInt(itemWidth * s);
            $(el + ' ' + leftBtn).removeClass("over");

            if (translateXval >= itemsCondition - itemWidth / 2) {
                translateXval = itemsCondition;
                $(el + ' ' + rightBtn).addClass("over");
            }
        }
        $(el + ' ' + itemsDiv).css('transform', 'translateX(' + -translateXval + 'px)');
    }

    //It is used to get some elements from btn
    function click(ell, ee) {
        var Parent = "#" + $(ee).parent().attr("id");
        var slide = $(Parent).attr("data-slide");
        ResCarousel(ell, Parent, slide);
    }

});

let like_btn = document.getElementById("like-btn");

let add_fav = document.getElementById("add-fav");

let main_img = document.getElementById("main-img");

let infovol_img = document.getElementsByClassName("infovol-img");

let rec_btn = document.getElementsByClassName("rec-btn");

let like_btn_card = document.getElementsByClassName("like-btn-card");

let comments = document.getElementById("comments");

let mission_intro = document.getElementById("mission-intro");

tab_1 = document.getElementById("tab-1");
tab_2 = document.getElementById("tab-2");
tab_3 = document.getElementById("tab-3");






for (i = 0; i < rec_btn.length; i++) {
    rec_btn[i].addEventListener("click", (e) => {
        if (e.target.classList.contains("btn-primary")) {
            e.target.classList.remove("btn-primary");
            e.target.classList.add("btn-outline-success");
            e.target.innerHTML = `<img src="/Images/right.png" alt="">Recommended`;
        }
        else {
            e.target.classList.remove("btn-outline-success");
            e.target.classList.add("btn-primary");
            e.target.innerHTML = `<img src="/Images/user.png" alt="">Recommend`;
        }

    })
}

for (i = 0; i < like_btn_card.length; i++) {
    like_btn_card[i].addEventListener("click", (e) => {

        if (e.target.classList.contains("liked")) {
            e.target.setAttribute("src", "/images/heart.png");
            e.target.classList.remove("liked")
        }
        else {
            e.target.setAttribute("src", "/images/heartred.png");
            e.target.classList.add("liked");
        }

        
    })
}


like_btn.addEventListener("click", function () {
    if (like_btn.classList.contains("bg-white")) {
        add_fav.setAttribute("src", "/images/heart.png");
        like_btn.classList.remove("bg-white");
        like_btn.classList.add("bg-red");
    }
    else {
        add_fav.setAttribute("src", "/images/heart1.png");
        like_btn.classList.add("bg-white");
        like_btn.classList.remove("bg-red");
    }

})

for (i = 0; i < infovol_img.length; i++) {
    
    infovol_img[i].addEventListener("click", (e) => {
        console.log(e.target.getAttribute("src"));
        main_img.setAttribute("src", e.target.getAttribute("src"));
    })

}






tab_1.addEventListener("click", function () {
    this.classList.add('active-link');
    tab_2.classList.remove('active-link');
    tab_3.classList.remove('active-link');
    mission_intro.classList.remove('hide');
    comments.classList.add('hide');

});
tab_2.addEventListener("click", function () {
    this.classList.add('active-link');
    tab_1.classList.remove('active-link');
    tab_3.classList.remove('active-link');
    
});
tab_3.addEventListener("click", function () {
    this.classList.add('active-link');
    tab_1.classList.remove('active-link');
    tab_2.classList.remove('active-link');
    mission_intro.classList.add('hide');
    comments.classList.remove('hide');
   
});