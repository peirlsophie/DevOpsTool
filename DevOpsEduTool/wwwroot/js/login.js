function openLogin() {
    document.getElementById("loginModal").style.display = "block";
}

function closeLogin() {
    document.getElementById("loginModal").style.display = "none";
}

window.onclick = function (event) {
    let modal = document.getElementById("loginModal");
    if (event.target == modal) {
        closeLogin();
    }
}