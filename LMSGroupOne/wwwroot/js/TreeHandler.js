
// a class to handle the tree navigations
class TreeHandler {
    #dragElement;
    #pressPositionX;
    #pressPositionY;
    #initialMove;
    #draging;
    #longPress;
    #pressTimer;

    #selectBar;
    #selectSelection;

    #menuDiv;

    #currentElement;
    #editTimer;
    #editCancel;

    #thrashCan;
    #movedElement;

    #contentDiv;

    #currentDisplayId;
    #currentDisplayType;

    constructor(menuDivId, contentDivId, thrashCanId) {
        this.#pressPositionX = 0;
        this.#pressPositionY = 0;
        this.#initialMove = false;
        this.#draging = false;
        this.#longPress = false;
        this.#editCancel = false;


        window.addEventListener("mouseup", (event) => this.#OnMouseUp(event));
        window.addEventListener("mousemove", (event) => this.#OnMouseMove(event));
                
        this.#menuDiv = document.getElementById(menuDivId);

        // the selection
        this.#selectBar = document.createElement("div");
        this.#selectSelection = document.createElement("div");
        let tempdoc = document.getElementById("treeDivId");
        tempdoc.appendChild(this.#selectBar);
        tempdoc.appendChild(this.#selectSelection);
        this.#selectBar.style = "top:-4096px; left:0px; width:100%; height:32px; position:absolute; background-color:#ffff0055; z-index: -1;";
        this.#selectSelection.style = "top:-4096px;left:0px;width:100%;height:32px;position:absolute;background-color:#ff000033; z-index: -1;";

        // the draggable element
        this.#dragElement = document.createElement("nobr");
        document.body.append(this.#dragElement);
        this.#dragElement.style = "top:-4096px; left:0px; width:00px; height:0px; position:absolute; background-color:#ffff0055; z-index:100;";


        this.#thrashCan = document.getElementById(thrashCanId);

        this.#contentDiv = document.getElementById(contentDivId);
    }

    #OnMouseUp(event) {


        this.#dragElement.style.top = -4096 + "px";
        let e = document.elementFromPoint(event.clientX, event.clientY);
        console.log("dragged to>>" + e.id + ": " + e.dataset.itemType);
        console.log("belongs to parent" + e.dataset.itemParentId + ": " + e.dataset.itemParentType);
        

        // dragto

        if (this.#draging) {            
            switch (e.dataset.itemType)
            {
                case "trash":
                    this.#OnDelete(event);
                    break;
                case String(TreeFactory.NodeTypes.COURSE):
                    console.log("dragged to course");
                    break;
                case String(TreeFactory.NodeTypes.MODULE):
                    console.log("dragged to module");
                    break;
                case String(TreeFactory.NodeTypes.ACTIVITY):
                    console.log("dragged to activity");
                    break;
            }
        }

        clearTimeout(this.#pressTimer);
        this.#longPress = false;
        this.#initialMove = false;
        this.#draging = false;        
    }

    #OnMouseMove(event) {
        if (this.#longPress) {

            if (!this.#initialMove) {
                let dist = (event.clientX - this.#pressPositionX) * (event.clientX - this.#pressPositionX) + (event.clientY - this.#pressPositionY) * (event.clientY - this.#pressPositionY);
                if (dist < 50) {
                    //longpress = false;
                    this.#draging = true;
                }
                this.#initialMove = true;
            }           

        }
        if (this.#draging) {
            this.#dragElement.style.top = event.pageY + "px";
            this.#dragElement.style.left = event.pageX + "px";
            let e = document.elementFromPoint(event.clientX, event.clientY);            

        }

    }

    #OnClick(event) {
        
        if (this.#longPress) {
            return;
        }

        console.log("enkel click");
        console.log("id:" + event.target.id);
        console.log("type:" + event.target.dataset.itemType);
        console.log("type:" + event.target.dataset.itemExtra);
        console.log("ypos:" + event.clientY);

        if (event.target.dataset.itemType != TreeFactory.NodeTypes.FOLDER)
        {
            this.#LoadMainContent(event.target.id, event.target.dataset.itemType);                       
        }         

        if (event.target.dataset.itemExtra == "caret") {

            this.#ToggleExpand(event.target.parentNode);
            this.#SelectionOutline(event.target.parentNode);
            return;
        }

        if (event.target.dataset.itemExtra == "text") {
            this.#SelectionOutline(event.target.parentNode);
            return;
        }

        if (event.target.dataset.itemExtra == "new") {                        
            this.#OnNew(event);
        }        

    }

    #LoadMainContent(id, type)
    {        
        $.ajax({
            type: "GET",
            url: "/MainNavigation/OnTreeClick",
            data: { id: id, type: type },
            cache: false,
            success: result => {                

                document.getElementById("contentDivId").innerHTML = result;

                let editButton = document.getElementById("editButtonId");
                if (editButton) {
                    editButton.addEventListener("click", (event) => { this.#OnEdit(event) });
                }

                let searchForm = document.getElementById("searchFormId");
                if (searchForm)
                {                    
                    searchForm.onsubmit = function(event)
                    {                        
                        SearchHandler.OnSearch(event);
                        return false;
                    };
                }

                let createAuthor = document.getElementById("createAuthorId");
                if (createAuthor)
                {
                    createAuthor.addEventListener("click", (event) => {
                        SearchHandler.OnCreateAuthor();
                    });
                }

                this.#currentDisplayId = id;
                this.#currentDisplayType = type;
            }
        }); 
    }   

    #OnEdit(event)
    {        
        let modal = document.getElementById("centerModalId");
        let button = document.getElementById("centerModalButton");
        let title = document.getElementById("centerModalTitleId");
        let data = "";
        button.innerHTML = "Edit";
        button.style.display = "block";
        let url = "";
        let type = event.target.dataset.itemType;
        let id = event.target.dataset.itemId;
        modal.dataset.itemType = type;        
        modal.dataset.itemOperation = "edit";
        modal.dataset.itemId = id;       

                
        switch (parseInt(type)) {
            case TreeFactory.NodeTypes.COURSE:
                url = "/Course/Edit";  
                data = { id:id };
                modal.style.display = "block";
                title.innerHTML = "Edit Course";
                break;
            case TreeFactory.NodeTypes.MODULE:
                url = "/Module/EditModule";  
                data = { id: id };
                modal.style.display = "block";
                title.innerHTML = "Edit Module";
                break;
            case TreeFactory.NodeTypes.ACTIVITY:
                url = "/Activity/Edit";  
                data = { id :id };
                modal.style.display = "block";
                title.innerHTML = "Edit Activity";
                break;            
        }

        $.ajax({
            type: "GET",
            url: url,
            data: data,
            cache: false,
            success: result => {
                let modalContent = document.getElementById("centerModalBodyId");
                modalContent.innerHTML = result;
                ModalHandler.FixValidation();
            }
        });
    }

    #OnDblClick(event) {
        this.#editCancel = true;
                
        if (event.target.parentNode.parentNode.childElementCount > 1) {
            this.#ToggleExpand(event.target.parentNode);
            this.#SelectionOutline(event.target.parentNode);
        }
    }

    #ToggleExpand(node) {

        if (node.parentNode.childNodes[1].childElementCount) {
            if (node.dataset.itemOpen == "true") {
                this.#CloseNode(node);
            }
            else {
                this.#OpenNode(node);
            }
        }

    }

    #OpenNode(node) {
        node.dataset.itemOpen = true;
        node.nextSibling.hidden = false;

        let item = TreeFactory.GetIconClass(parseInt(node.childNodes[1].dataset.itemType), true);
        let icon = node.childNodes[1];
        icon.classList = item.classList;
        icon.style = item.style;

        let caret = node.childNodes[0];
        if (caret.classList.contains("carretIcon")) {
            caret.classList.remove("fa-caret-right");
            caret.classList.add("fa-caret-down");
        }
    }

    #CloseNode(node) {
        node.dataset.itemOpen = false;
        node.nextSibling.hidden = true;

        let item = TreeFactory.GetIconClass(parseInt(node.childNodes[1].dataset.itemType), false);
        let icon = node.childNodes[1];
        icon.classList = item.classList;
        icon.style = item.style;


        let caret = node.childNodes[0];
        if (caret.classList.contains("carretIcon")) {
            caret.classList.add("fa-caret-right");
            caret.classList.remove("fa-caret-down");
        }

    }


    #SelectionOutline(node) {
        let tRect = document.getElementById("toolBarId").getBoundingClientRect();
        let tOffset = tRect.height;

        // new current
        if (this.#currentElement != node) {            
            this.#currentElement = node;
        }


        let rect = node.getBoundingClientRect();
        let rect2 = node.parentNode.getBoundingClientRect();
        let mr = this.#menuDiv.getBoundingClientRect();

        let marginal = rect.top - rect2.top;

        this.#selectBar.style.top = (rect.top - mr.top) + tOffset + "px";
        this.#selectBar.style.height = (rect.height) + "px";

        this.#selectSelection.style.top = (rect.top - mr.top) + tOffset + "px";
        this.#selectSelection.style.height = (rect2.height - marginal * 2) + "px";
    }

    #OnDown(event) {
        this.#pressPositionX = event.clientX;
        this.#pressPositionY = event.clientY;
        
        this.#longPress = false;
        this.#pressTimer = window.setTimeout(() => {
                        
            this.#dragElement.innerHTML = event.target.parentNode.innerHTML;
            this.#dragElement.id = event.target.id;
            this.#dragElement.dataset.itemType = event.target.dataset.itemType;
            this.#movedElement = event.target.parentNode.parentNode.parentNode;
            this.#longPress = true; 

        }, 100);
    }


    #OnDelete() {
        
        let modal = document.getElementById("centerModalId");
        let button = document.getElementById("centerModalButton");
        let title = document.getElementById("centerModalTitleId");
        let data = "";
        button.innerHTML = "Delete";
        button.style.display = "block";
        let url = "";
        let id = this.#dragElement.id;
        let type = this.#dragElement.dataset.itemType;
        modal.dataset.itemType = type;        
        modal.dataset.itemOperation = "delete";
        modal.dataset.itemId = id;
         
        switch (parseInt(type)) {
            case TreeFactory.NodeTypes.COURSE:
                url = "/Delete/DeleteCourse";  
                data = { Id:id };
                modal.style.display = "block";
                title.innerHTML = "Delete Course";
                break;
            case TreeFactory.NodeTypes.MODULE:
                url = "/Delete/DeleteModule";  
                data = { Id:id };
                modal.style.display = "block";
                title.innerHTML = "Delete Module";
                break;
            case TreeFactory.NodeTypes.ACTIVITY:
                url = "/Delete/DeleteActivity";  
                data = { Id:id };
                modal.style.display = "block";
                title.innerHTML = "Delete Activity";
                break;
            case TreeFactory.NodeTypes.FILE:
                url = "/Delete/DeleteDocument";
                data = { Id: id };
                modal.style.display = "block";
                title.innerHTML = "Delete Document";
                break;
        }

        $.ajax({
            type: "GET",
            url: url,
            data: data,
            cache: false,
            success: result => {
                let modalContent = document.getElementById("centerModalBodyId");
                modalContent.innerHTML = result;
                ModalHandler.FixValidation();
            }
        });
    }

    UpdateAfterDelete(id, type)
    {
        let item = this.#FindItem(type, id).parentNode.parentNode;
        let pNode = item.parentNode;

        item.remove();
        
        if (pNode.childElementCount===0)
        {            
            this.#CloseNode(pNode.previousSibling);
            pNode.previousSibling.childNodes[0].hidden = true;
           
        }
        this.#SelectionOutline(pNode.parentNode.childNodes[0]);

        if (this.#currentDisplayId == id && this.#currentDisplayType == type)
        {
            document.getElementById("contentDivId").innerHTML = "";            
        }

    }
        
    #OnNew(event) {        

        let modal = document.getElementById("centerModalId");        
        let button = document.getElementById("centerModalButton");
        let title = document.getElementById("centerModalTitleId");
        let data = "";
        button.innerHTML = "New";
        button.style.display = "block"; 
        let url = "";
        let type = event.target.dataset.itemCreates;
        modal.dataset.itemType = type;
        modal.dataset.itemParentId = event.target.id;
        modal.dataset.itemOperation = "new";

        switch (parseInt(type))
        {
            case TreeFactory.NodeTypes.COURSE:
                url = "/Course/Create";                
                modal.style.display = "block";
                title.innerHTML = "Create Course";
                break;
            case TreeFactory.NodeTypes.MODULE:
                url = "/Module/CreateModule";
                data = { id : event.target.dataset.itemParentId };
                modal.style.display = "block";
                title.innerHTML = "Create Module";
                break;
            case TreeFactory.NodeTypes.ACTIVITY:
                url = "/Activity/Create";
                data = { id : event.target.dataset.itemParentId };
                modal.style.display = "block";
                title.innerHTML = "Create Activity";
                break;
            case TreeFactory.NodeTypes.FILE:
                url = "/Course/Create";
                data = event.target.dataset.itemParentId;
                modal.style.display = "block";
                title.innerHTML = "Create Document";
                break;
            case TreeFactory.NodeTypes.STUDENT:
                url = "/Account/CreateTeacher";
                data = event.target.dataset.itemParentId;
                modal.style.display = "block";
                title.innerHTML = "Create Student";
                break;
            case TreeFactory.NodeTypes.TEACHER:
                url = "/Account/CreateTeacher";
                data = {},
                modal.style.display = "block";
                title.innerHTML = "Create Teacher";
                break;

            default:
                return;
        }        

        $.ajax({
            type: "GET",
            url: url,
            data: data,
            cache: false,
            success: result => {
                let modalContent = document.getElementById("centerModalBodyId");
                modalContent.innerHTML = result;                                
                ModalHandler.FixValidation();               
            }
        });             

    }

    UpdateAfterEdit(id, type, name)
    {
        let item = this.#FindItem(type, id);
        
        // rename the treenode
        item.childNodes[2].innerHTML = name;

        // relaod page if current
        if (this.#currentDisplayId == id && this.#currentDisplayType == type) {            
            this.#LoadMainContent(id, type);
        }
    }

    AddSubTree(subTree, type, path) {

        let elmer = this.#FindListGroup(subTree.Type, path);
        let item = TreeFactory.GenerateSubTree(subTree, (item) => this.#AddEventListener(item));
        elmer.appendChild(item);
        
        let caret = elmer.previousSibling.childNodes[0].hidden = false;
        this.#SelectionOutline(elmer.previousSibling);
    }

    #AddEventListener(item) {
        item.addEventListener("click", (event) => this.#OnClick(event));
        item.addEventListener("dblclick", (event) => this.#OnDblClick(event));
        item.addEventListener("mousedown", (event) => this.#OnDown(event));        
    }

    GenerateTree(node)
    {
        this.#menuDiv.appendChild(TreeFactory.GenerateSubTree(node, (item) => this.#AddEventListener(item)));        
    }   

    // finds the place to put created items in  
    #FindListGroup(type, id) {
                        
        let gstr = "[id='"+id+"']";
        let bstr = '[data-item-creates="' + type + '"]';        
        let q = document.querySelectorAll(gstr + bstr);       
        return q[0].parentNode.nextSibling;                
    }

    #FindItem(type, id)
    {
        let gstr = "[id='" + id + "']";
        let bstr = '[data-item-type="' + type + '"]';
        let q = document.querySelectorAll(gstr + bstr);
        
        return q[0].parentNode;
    }




}