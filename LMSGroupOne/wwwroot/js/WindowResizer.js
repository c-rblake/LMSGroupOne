window.addEventListener("load", (event) =>
{
    window.addEventListener("resize", ResizeMainView);
    ResizeMainView(this);
});


function ResizeMainView(event)
{
    let menu = document.getElementById("menuDivId");
    let content = document.getElementById("contentDivId");
    let header = document.getElementById("headerBarId");
    let footer = document.getElementById("footerBarId");
    let height = (window.innerHeight - header.clientHeight - footer.clientHeight);
    menu.style.height = height + "px";
    content.style.height = height + "px";
}

