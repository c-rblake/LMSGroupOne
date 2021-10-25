class SearchHandler
{

    // triggered by the submit
    static OnSearch(event) {
        //first index is called by TreeHandler

        let form = document.getElementById("searchFormId");



        $.ajax({
            type: "GET",
            url: "/Search/Search",
            data: { Name:form.elements["Name"].value},      //reposts values, numbers etc might need some manipulation
            cache: false,
            success: result => {
                console.log(result);

                // dynamically adds the loaded the return from /Search/Search
                document.getElementById("contentDivId").innerHTML = result;


                // overrides the submit and calls this function...
                let searchForm = document.getElementById("searchFormId");
                if (searchForm) {
                    searchForm.onsubmit = function (event) {
                        SearchHandler.OnSearch(event);
                        return false;
                    };
                }

            }
        });





    }






}