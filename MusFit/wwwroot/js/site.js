// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$('.sub-btn').click(function () {
    $(this).next('.sub-menu').slideToggle();
    $(this).find('.dropdown').toggleClass('rotate');
});

$('.menu-btn').click(function () {
    $('.side-bar').css("width", "100%");
    $('.close-btn').css("display","block");
    $('.side-bar').css("left", "0%");
});

$('.close-btn').click(function () {
    $('.side-bar').css("left", "-100%");
    $('.content').css("marginLeft", "0");
    $('.logo-area').css("marginLeft", "0");
    $('.content').css("width", "100%");
    $('.logo-area').css("width", "100%");
});

var subbtn = document.getElementsByClassName("sub-btn");
var i;

for (i = 0; i < subbtn.length; i++) {
    subbtn[i].addEventListener("click", function () {
        this.classList.toggle("active");
    })
    }