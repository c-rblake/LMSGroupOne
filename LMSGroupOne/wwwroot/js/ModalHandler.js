class ModalHandler {

    constructor() {
        document.getElementById("centerModalClose").addEventListener("click", this.OnCloseButton);
        document.getElementById("centerModalButton").addEventListener("click", this.OnButton);
    }

    static FixValidation() {
        const form = document.getElementById("formId");
        $.validator.unobtrusive.parse(form);
    }

    OnCloseButton() {
        let modal = document.getElementById("centerModalId");
        modal.style.display = "none";
    }

    OnButton() {
        console.log("clicked on the modal button");

        var tform = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', tform).val();


        let type = document.getElementById("centerModalId").dataset.itemType;
        console.log("receiving type");
        console.log("----------------------");
        console.log(type);

        let form = document.getElementById("formId");
        //console.log(form);

        let url = "";
        let data = "";
        switch (parseInt(type)) {
            case TreeFactory.NodeTypes.COURSE:
                url = "/Course/Create";
                data =
                {
                    __RequestVerificationToken: token,
                    Name: form.elements["Name"].value,
                    Description: form.elements["Description"].value,
                    StartDate: form.elements["StartDate"].value,
                    EndDate: form.elements["EndDate"].value,
                };
                break;
            case TreeFactory.NodeTypes.MODULE:
                url = "/Module/CreateModule";
                //todo data
                break;
            case TreeFactory.NodeTypes.ACTIVITY:
                url = "/Activity/Create";
                // todo data
                break;
        }


        // todo check type and switch
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            cache: false,
            success: result => {
                let modalContent = document.getElementById("centerModalBodyId");
                modalContent.innerHTML = result;
                ModalHandler.FixValidation();



                form = document.getElementById("formId");
                let success = form.elements["Success"].value;
                let returnId = form.elements["ReturnId"].value;
                let name = form.elements["Name"].value;


                console.log(success);
                if (success == "True") {
                    document.getElementById("centerModalButton").style.display = "none";
                    let path = document.getElementById("centerModalId").dataset.itemParentId;

                    $.ajax({
                        type: "GET",
                        url: "/AddNavigation/OnNew",
                        data: { path: path, id: returnId, type: type, name: name },
                        cache: false,
                        success: result => {
                            let obj = JSON.parse(result);
                            if (obj.success) {
                                treeHandler.AddSubTree(obj.subTree, obj.type, obj.path);
                            }
                        }
                    });


                }

            }

        });



    }




}