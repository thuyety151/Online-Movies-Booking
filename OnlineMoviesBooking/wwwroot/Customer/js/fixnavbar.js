const toggleBtn = document.querySelector(".navbar-toggler");
const sideBar = document.querySelector(".navbar-nav");

toggleBtn.addEventListener("click", function () {
    sideBar.classList.toggle("show-links");
    console.log("done");
});

//---------fixed navbar-------
const navBar = document.getElementById("nav");

window.addEventListener("scroll", function () {
    const scrollHeight = window.pageYOffset;
    const navHeight = navBar.getBoundingClientRect().height;
    if (scrollHeight > 1) {
        navBar.classList.add("fixed-nav");
    } else {
        navBar.classList.remove("fixed-nav");
    }
});
