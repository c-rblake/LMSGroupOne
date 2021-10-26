
// an example class to handle a search view in the "contentDivId"
class SearchHandler
{

    // triggered by the submit
    static OnSearch(event) {
        //first index is called by TreeHandler

        let form = document.getElementById("searchFormId");

        





        $.ajax({
            type: "GET",
            url: "/Authors/List",
            data: { Name:form.elements["Name"].value},      //reposts values, numbers etc might need some manipulation
            cache: false,
            success: result => {
                console.log(result);

                // dynamically adds the loaded the return from /Search/Search
                document.getElementById("authorsListId").innerHTML = result;

                ResizeMainView(event);  // function in windowResizer
               


                // overrides the submit and calls this function...
                //let searchForm = document.getElementById("searchFormId");
                //if (searchForm) {
                //    searchForm.onsubmit = function (event) {
                //        SearchHandler.OnSearch(event);
                //        return false;
                //    };
                //}

            }
        });

       





    }

    static OnCreateAuthor()
    {

        let modal = document.getElementById("centerModalId");
        let button = document.getElementById("centerModalButton");
        let title = document.getElementById("centerModalTitleId");
        let data = "";
        button.innerHTML = "New";
        button.style.display = "block";
        let url = "";
        let type = parseInt(TreeFactory.NodeTypes.AUTHOR);
        //modal.dataset.itemType = type;
        //modal.dataset.itemParentId = event.target.id;
        modal.dataset.itemOperation = "new";

        modal.dataset.itemType = type;
        modal.dataset.itemParentId = "1";
        modal.dataset.itemOperation = "new";

               
            
        url = "/Authors/Create";
        //data = event.target.dataset.itemParentId;
        modal.style.display = "block";
        title.innerHTML = "Create Author";
        



        $.ajax({
            type: "GET",
            url: url,
            data: {},
            cache: false,
            success: result => {
                let modalContent = document.getElementById("centerModalBodyId");
                modalContent.innerHTML = result;
                ModalHandler.FixValidation();
            }
        });








    }




}