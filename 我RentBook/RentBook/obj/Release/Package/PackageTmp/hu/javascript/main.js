
//夜間日間模式
var ahover = document.querySelectorAll(".dropdown-item")

function changeStyle3() {
    for (var i = 0; i < ahover.length; i++) {
        ahover[i].setAttribute("class", "dropdown-item dark");
    }
}

//eventlistener
var darkMode = document.getElementById("darkMode")
var brightMode = document.getElementById("brightMode")
//style
var bodyStyle = document.getElementsByTagName("body")
var navStyle = document.getElementsByTagName("nav")
var navA = document.querySelectorAll("nav a")
var navDrop = document.getElementById("navbarNavDropdown")
var dpmenu = document.querySelector(".dropdown-menu")
var contentStyle = document.getElementById("content")

function dark() {
    for (var i = 0; i < navA.length; i++) {
        navA[i].style.color = '#EBEBEB'
    }
    bodyStyle[0].style.cssText = 'background-color:#373C3F;color:#EBEBEB';
    navStyle[0].style.cssText = 'background-color:#2F3437;color:#EBEBEB';
    contentStyle.style.cssText = 'background-color:#2F3437;color:#EBEBEB';
    dpmenu.style.cssText = 'background-color:#373C3F;color:#EBEBEB';
    localStorage.setItem('Mode', 'dark');

    changeStyle3()
}

function bright() {
    for (var i = 0; i < ahover.length; i++) {
        ahover[i].setAttribute("class", "dropdown-item");
    }
    for (var i = 0; i < navA.length; i++) {
        navA[i].style.color = ''
    }
    bodyStyle[0].style.cssText = '';
    navStyle[0].style.cssText = '';
    contentStyle.style.cssText = '';
    dpmenu.style.cssText = '';
    localStorage.setItem('Mode', 'bright');
}

darkMode.addEventListener("click", function (e) {
    dark();

})
brightMode.addEventListener("click", function (e) {
    bright();
})

window.addEventListener('load', function (e) {
    var setting = localStorage.getItem('Mode')
    //
    var localFontsize = localStorage.getItem('fontSize') //22px
    for (var i = 0; i < webFontSize.length; i++) {
        webFontSize[i].style.cssText = `font-size:${localFontsize};`
    }
    if (setting == 'dark') {
        dark();
    } else {
        //
    }
})



//點擊後菜單不收起
$('#setting').click(function (e) {
    e.stopPropagation();
});
//為什麼js不行
// var menu = document.querySelector(".dropdown-menu")
// menu.addEventListener("click", function(e) {
//     e.stopPropagation()
// })