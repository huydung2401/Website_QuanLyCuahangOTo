document.addEventListener("DOMContentLoaded", () => {
    const popup = document.getElementById("loginPopup");

    if (!popup) return;

    const loginButtons = document.querySelectorAll(".open-login-modal");

    loginButtons.forEach(btn => {
        btn.addEventListener("click", function (e) {
            e.preventDefault();
            popup.classList.add("active");
        });
    });

    popup.addEventListener("click", function (e) {
        if (e.target === popup) {
            popup.classList.remove("active");
        }
    });
});