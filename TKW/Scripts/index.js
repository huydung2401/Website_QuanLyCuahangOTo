$(document).ready(function () {
    $('.color-btn').click(function () {
        $(this).siblings().removeClass('active');
        $(this).addClass('active');
        var imgUrl = $(this).data('img');
        $(this).closest('.card').find('.product-img').attr('src', imgUrl);
    });
});

const track = document.getElementById("reviewTrack");

let isDown = false;
let startX;
let scrollLeft;

track.addEventListener("mousedown", (e) => {
    isDown = true;
    startX = e.pageX - track.offsetLeft;
    scrollLeft = track.scrollLeft;
    track.style.cursor = "grabbing";
});

track.addEventListener("mouseleave", () => {
    isDown = false;
});
track.addEventListener("mouseup", () => {
    isDown = false;
    track.style.cursor = "grab";
});
track.addEventListener("mousemove", (e) => {
    if (!isDown) return;
    e.preventDefault();
    const x = e.pageX - track.offsetLeft;
    const walk = (x - startX) * 1.5;
    track.scrollLeft = scrollLeft - walk;
});