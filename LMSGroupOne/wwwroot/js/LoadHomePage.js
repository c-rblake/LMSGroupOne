window.addEventListener("load", (event) => {
    LoadPersonalHomeView(event)
});

function LoadPersonalHomeView(event) {


    $.ajax({
        type: "GET",
        url: "/MainNavigation/OnHome",
        data: { type: "load" },
        cache: false,
        success: result => {
            document.getElementById("contentDivId").innerHTML = result;
        }
    });

};