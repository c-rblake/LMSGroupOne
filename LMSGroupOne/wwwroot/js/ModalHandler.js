// a class to handle the modal views
class ModalHandler {

    constructor() {
        document.getElementById("centerModalClose").addEventListener("click", this.OnCloseButton);
        document.getElementById("centerModalButton").addEventListener("click", this.#OnButton);
    }

    static FixValidation() {
        const form = document.getElementById("formId");
        $.validator.unobtrusive.parse(form);
    }

    static FixValidation2() {
        $(function hideOnLoad() {
            if ($("#role").val() == "Teacher") { $("#course").hide(); }
        });
        $(function () {
            $("#role").change(function () {
                if ($(this).val() == "Student") { $("#course").toggle(); }
                else if ($(this).val() == "Teacher") { $("#course-select")[0].selectedIndex = -1; $("#course").hide('slow'); }
            });
        });
    }

    static FixValidation3() {
        function validateForm() {
            var x = document.forms["UploadCourseDocumentsForm"]["postedDocuments"].value;
            if (x == "") {
                alert("Please choose document(s) to upload");
                return false;
            }
        }
}

    OnCloseButton() {
        let modal = document.getElementById("centerModalId");
        modal.style.display = "none";
    }

    #OnButton() {
        
        let type = document.getElementById("centerModalId").dataset.itemType;
        let operation = document.getElementById("centerModalId").dataset.itemOperation;

        

        console.log("-----on button modalhandler-----------");
        console.log(type);
        console.log(operation);

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
                ModalHandler.FixValidation2();
                ModalHandler.FixValidation3();


                let form = document.getElementById("formId");
                let success = JSON.parse((form.elements["Success"].value).toLowerCase());
                               

                console.log(success);
                if (success) {                    
                    ModalHandler.OnSuccess(form, type, operation);
                }
            }

        });

    }

    static OnSuccess(form, type, operation)
    {
        form = document.getElementById("formId");
        document.getElementById("centerModalButton").style.display = "none";
        let path = document.getElementById("centerModalId").dataset.itemParentId;

        let returnId = "";
        if ((parseInt(type) == TreeFactory.NodeTypes.TEACHER) || (parseInt(type) == TreeFactory.NodeTypes.STUDENT) )
        {
            returnId = form.elements["PersonReturnId"].value;
        }
        else
        {
            returnId = parseInt(form.elements["ReturnId"].value);
        }
        
         

        
        switch (operation)
        {
            case "new":
                console.log("id in succsses:"+returnId)
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
                break;
            case "delete":
                console.log("deleted");
                treeHandler.UpdateAfterDelete(parseInt(returnId), type);                
                break;
            case "uploadcoursedocuments":
                console.log("id in succsses:" + returnId)
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
            case "uploadmoduledocuments":
                console.log("id in succsses:" + returnId)
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
            case "uploadactivitydocuments":
                console.log("id in succsses:" + returnId)
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
            case TreeFactory.NodeTypes.FILE:
                return ModalHandler.GetDocumentRepostData(operation, token, data);
            case TreeFactory.NodeTypes.AUTHOR:
                return ModalHandler.GetAuthorRepostData(operation, token, data);
            case TreeFactory.NodeTypes.TEACHER:
                return ModalHandler.GetTeacherRepostData(operation, token, data);
            case TreeFactory.NodeTypes.STUDENT:
                return ModalHandler.GetStudentRepostData(operation, token, data);
            case TreeFactory.NodeTypes.FILECOURSE:
                return ModalHandler.GetStudentRepostData(operation, token, data);
            case TreeFactory.NodeTypes.FILEMODULE:
                return ModalHandler.GetStudentRepostData(operation, token, data);
            case TreeFactory.NodeTypes.FILEACTIVITY:
                return ModalHandler.GetStudentRepostData(operation, token, data);
            
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
                url = "/Course/Edit";                
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
                url = "/Activity/Create";                
                break;
            case "edit":
                url = "/Activity/Edit";                
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
                url = "/Module/CreateModule";                
                break;
            case "edit":
                url = "/Module/EditModule";               
                break;
            case "delete":
                url = "/Delete/DeleteModule";                
                data["Id"] = parseInt(form.elements["Id"].value);
                break;
        }
        return { url: url, data: data };
    }


    static GetDocumentRepostData(operation, token, data) {

        let form = document.getElementById("formId");
        let url = "";

        switch (operation) {
            case "new":
                url = "/Course/Create";
                break;
            case "uploadcoursedocuments":
                url = "/Account/UploadCourseDocuments";
                data["Id"] = parseInt(form.elements["Id"].value);
                break;
            case "uploadmoduledocuments":
                url = "/Account/UploadModuleDocument";
                break;
            case "uploadactivitydocuments":
                url = "Account/ActivityDocument";
                break;
            case "edit":
                url = "/Course/Create";
                break;
            case "delete":
                url = "/Delete/DeleteDocument";
                data["Id"] = parseInt(form.elements["Id"].value);
                break;
        }
        return { url: url, data: data };
    }


    static GetAuthorRepostData(operation, token, data) {

        let form = document.getElementById("formId");
        let url = "";

        switch (operation) {
            case "new":
                url = "/Authors/Create";
                break;
            case "edit":
                url = "/Course/Create";
                break;
            case "delete":
                url = "/Delete/DeleteDocument";
                data["Id"] = parseInt(form.elements["Id"].value);
                break;
        }
        return { url: url, data: data };
    }
    static GetTeacherRepostData(operation, token, data) {

        let form = document.getElementById("formId");
        let url = "";

        switch (operation) {
            case "new":
                url = "/Account/CreateTeacher";
                break;
            case "edit":
                url = "/Account/Edit";
                break;
            case "delete":
                url = "/Delete/DeleteTeacher";
                //data["Id"] = parseInt(form.elements["Id"].value);
                break;
        }
        return { url: url, data: data };
    }
    static GetStudentRepostData(operation, token, data) {

        let form = document.getElementById("formId");
        let url = "";

        switch (operation) {
            case "new":
                url = "/Account/CreateStudent";
                break;
            case "edit":
                url = "/Account/Edit";
                break;
            case "delete":
                url = "/Delete/DeleteStudent";
                //data["Id"] = parseInt(form.elements["Id"].value);
                break;
        }
        return { url: url, data: data };
    }
}