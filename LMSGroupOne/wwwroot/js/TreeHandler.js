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

        if (this.#draging) {
            if (e.dataset.itemType == "trash") {
                clearTimeout(this.#pressTimer);
                this.#longPress = false;
                this.#initialMove = false;
                this.#draging = false;


                this.#OnDelete(event);
                //return;
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

            //console.log("distance:" + dist);

        }
        if (this.#draging) {
            this.#dragElement.style.top = event.pageY + "px";
            this.#dragElement.style.left = event.pageX + "px";

            let e = document.elementFromPoint(event.clientX, event.clientY);
            //console.log("mosemove-" + e.id + ": " + e.dataset.itemType);

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



        //let node = event.target.parentNode;
        //if (this.#currentElement != node) {
        //    console.log("new current------jjj-jj-jj-j");
        //    this.#currentElement = node;
        //}




        if (event.target.dataset.itemType != TreeFactory.NodeTypes.FOLDER)
        {
            $.ajax({
                type: "GET",
                url: "/MainNavigation/OnTreeClick",
                data: { id: event.target.id, type: event.target.dataset.itemType },
                cache: false,
                success: result => {
                    console.log(result);
                    //this.#contentDiv.innerHTML = result;
                    document.getElementById("contentDivId").innerHTML = result;
                }
            });

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
            console.log("new current");
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
        console.log("on down:" + event.clientY);

        this.#longPress = false;
        this.#pressTimer = window.setTimeout(() => {
            // your code here
            console.log("timer");
            this.#dragElement.innerHTML = event.target.parentNode.innerHTML;
            this.#dragElement.id = event.target.id;
            this.#dragElement.dataset.itemType = event.target.dataset.itemType;

            this.#movedElement = event.target.parentNode.parentNode.parentNode;

            this.#longPress = true; //if run hold function, longpress is true


        }, 100);
    }


    #OnDelete() {
        console.log("delete-----------------")
        console.log("item id" + this.#dragElement.id);
        console.log("item type" + this.#dragElement.dataset.itemType);

        $.ajax({
            type: "GET",
            url: "/MainNavigation/OnDelete",
            data: { id: this.#dragElement.id, type: this.#dragElement.dataset.itemType },
            cache: false,
            success: result => {
                let obj = JSON.parse(result);
                console.log(obj.success);
                if (obj.success) {

                    this.#removeElement(this.#movedElement);
                }
            }
        });


    }

    #removeElement(element) {
        let pNode = element.parentNode;
        element.remove();
        this.#SelectionOutline(pNode.parentNode.childNodes[0]);

    }

    
    #OnNew(event) {        
        let modal = document.getElementById("centerModalId");
        let button = document.getElementById("centerModalButton");
        button.innerHTML = "New";
        button.style.display = "block"; 
        let url = "";
        let type = event.target.dataset.itemCreates;        
        switch (parseInt(type))
        {
            case TreeFactory.NodeTypes.COURSE:
                url = "/Course/Create";                
                modal.style.display = "block";
                break;
        }

        

        $.ajax({
            type: "GET",
            url: url,
            data: {},
            cache: false,
            success: result => {
                let modalContent = document.getElementById("centerModalBodyId");
                modalContent.innerHTML = result;                                
                fixvalidation();               
            }
        });

        




        //$.ajax({
        //    type: "GET",
        //    url: "/AddNavigation/OnNew",
        //    data: { path: event.target.id, id: "hello_id", type: event.target.dataset.itemCreates, name: "hello world"},
        //    cache: false,
        //    success: result => {
        //        let obj = JSON.parse(result);                
        //        if (obj.success) {                    
        //            this.AddSubTree(obj.subTree, obj.type, obj.path);                    
        //        } 
        //    }
        //});
    }

    AddSubTree(subTree, type, path) {

        let elmer = this.#FindListGroup(subTree.Type, path);
        let item = TreeFactory.GenerateSubTree(subTree, (item) => this.#AddEventListener(item));
        elmer.appendChild(item);
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




}