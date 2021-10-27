// a function to resize the screen on windows rezize event

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


    let authorsHead = document.getElementById("authorSearchHeadId");
    let authorsList = document.getElementById("authorsListId");
    if (authorsList)
    {
        console.log("resize the searcharo");
        let aHeight = (content.clientHeight - authorsHead.clientHeight - footer.clientHeight);
        authorsList.style.height = aHeight + "px";
    }

}

