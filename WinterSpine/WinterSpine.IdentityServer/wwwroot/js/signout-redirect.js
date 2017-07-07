window.addEventListener("load", function () {
    console.log("sign out");
    var a = document.querySelector("a.PostLogoutRedirectUri");
    if (a) {
        window.location = a.href;
    }
});
