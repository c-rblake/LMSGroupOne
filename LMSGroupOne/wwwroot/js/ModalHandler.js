class ModalHandler {

    constructor() {
        document.getElementById("centerModalClose").addEventListener("click", this.OnCloseButton);
        document.getElementById("centerModalButton").addEventListener("click", this.#OnButton);
    }

    static FixValidation() {
        const form = document.getElementById("formId");
        $.validator.unobtrusive.parse(form);
    }

    OnCloseButton() {
        let modal = document.getElementById("centerModalId");
        modal.style.display = "none";
    }

    #OnButton() {
        console.log("clicked on the modal button");

        



        let type = document.getElementById("centerModalId").dataset.itemType;
        let operation = document.getElementById("centerModalId").dataset.itemOperation;
        console.log("receiving type");
        console.log("----------------------");
        console.log(type);
        console.log(operation);
        

        //let form = document.getElementById("formId");

        
        let repostData=ModalHandler.GetRepostData(type, operation);
        let url = repostData.url;
        let data = repostData.data;

        // todo check type and switch
        $.ajax({
            type: "POST",
            url: url,
            data:  data,
            cache: false,
            success: result => {
                let modalContent = document.getElementById("centerModalBodyId");
                modalContent.innerHTML = result;
                ModalHandler.FixValidation();

                let form = document.getElementById("formId");
                let success = JSON.parse((form.elements["Success"].value).toLowerCase());

                console.log(form.elements);
                console.log("returning id " + (form.elements["ReturnId"].value));
                console.log(form);

                console.log(success);
                if (success) {
                    console.log("success--from afterpost");
                    ModalHandler.OnSuccess(form, type, operation);
                }
            }

        });

    }

    static OnSuccess(form, type, operation)
    {
        console.log("success--from afterpost-onsuccwess op:"+operation);
        document.getElementById("centerModalButton").style.display = "none";
        let path = document.getElementById("centerModalId").dataset.itemParentId;

        let returnId = form.elements["ReturnId"].value;
        //let name = form.elements["Name"].value;
        
        switch (operation)
        {
            case "new":                
                $.ajax({
                    type: "GET",
                    url: "/AddNavigation/OnNew",
                    data: { path: path, id: returnId, type: type, name: form.elements["Name"].value },
                    cache: false,
                    success: result => {
                        let obj = JSON.parse(result);
                        if (obj.success) {
                            treeHandler.AddSubTree(obj.subTree, obj.type, obj.path);
                        }
                    }
                });
                break;
            case "edit":                
                let id = document.getElementById("centerModalId").dataset.itemId;
                
                treeHandler.UpdateAfterEdit(id, type, form.elements["Name"].value);
                // todo metod to update node name, reload page

                break;
            case "delete":
                console.log("deleted");
                treeHandler.UpdateAfterDelete(parseInt(returnId), type);                
                break;

        }


        
    }


    static GetRepostData(type, operation)
    {
        let form = document.getElementById("formId");

        var tform = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', tform).val();

        let data = {};
        for (let i = 0; i < form.elements.length; i++) {
            if ((form.elements[i].name != "Success") && (form.elements[i].name != "ReturnId") ) {
                data[form.elements[i].name] = form.elements[i].value;
            }
        }
        data["__RequestVerificationToken"] = token;               

        // add url and modify data
        switch (parseInt(type)) {
            case TreeFactory.NodeTypes.COURSE:
                return ModalHandler.GetCourseRepostData(operation, token, data);
            case TreeFactory.NodeTypes.MODULE:
                return ModalHandler.GetModuleRepostData(operation, token, data);
            case TreeFactory.NodeTypes.ACTIVITY:
                return ModalHandler.GetActivityRepostData(operation, token, data);
        }

    }

    

  



    static GetCourseRepostData(operation, token, data)
    {

        let form = document.getElementById("formId");
        let url = "";
        
        switch (operation)
        {
            case "new":
                url = "/Course/Create";
                // modify data if needed
                break;
            case "edit":
                url = "/Course/Create";                
                break;
            case "delete":
                url = "/Delete/DeleteCourse";
                data["Id"] = parseInt(form.elements["Id"].value);
                break;
        }
        return { url: url, data: data };
    }


    static GetActivityRepostData(operation, token, data) {

        let form = document.getElementById("formId");
        let url = "";        

        switch (operation) {
            case "new":
                url = "/Course/Create";                
                break;
            case "edit":
                url = "/Course/Create";                
                break;
            case "delete":
                url = "/Delete/DeleteActivity";
                data["Id"] = parseInt(form.elements["Id"].value);
                break;
        }
        return { url: url, data: data };
    }


    static GetModuleRepostData(operation, token, data) {

        let form = document.getElementById("formId");
        let url = "";        

        switch (operation) {
            case "new":
                url = "/Course/Create";                
                break;
            case "edit":
                url = "/Course/Create";               
                break;
            case "delete":
                url = "/Delete/DeleteModule";                
                data["Id"] = parseInt(form.elements["Id"].value);
                break;
        }
        return { url: url, data: data };
    }


}