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






        // dragged to


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


        // todo filtera bort folder och dubbelaktivering
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





        if (event.target.dataset.itemExtra == "caret") {

            console.log("current element " + this.#currentElement.dataset.itemType);

            this.#ToggleExpand(event.target.parentNode);
            this.#SelectionOutline(event.target.parentNode);
            return;
        }

        if (event.target.dataset.itemExtra == "text") {
            this.#SelectionOutline(event.target.parentNode);
            return;
        }

        if (event.target.dataset.itemExtra == "new") {

            console.log("adding something");
            this.#OnNew(event);
        }




    }


    #OnDblClick(event) {
        this.#editCancel = true;

        console.log("dubbeltrubbel");
        console.log("childCount" + event.target.parentNode.parentNode.childElementCount);
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


        //if (pNode.childElementCount <= 1)
        //{
        //    this.#ToggleExpand(pNode.childNodes[1])
        //}



        // selections todo refactor
        //let rect = pNode.childNodes[1].getBoundingClientRect();
        //let rect2 = pNode.parentNode.childNodes[1].getBoundingClientRect();
        //let mr = this.#menuDiv.getBoundingClientRect();

        //let marginal = rect.top - rect2.top;

        //this.#selectBar.style.top = (rect.top - mr.top) + "px";
        //this.#selectBar.style.height = (rect.height) + "px";

        //this.#selectSelection.style.top = (rect.top - mr.top) + "px";
        //this.#selectSelection.style.height = (rect2.height - marginal * 2) + "px";


        this.#SelectionOutline(pNode.parentNode.childNodes[0]);


    }

    #GetParent(node)
    {

    }

    #OnNew(event) {
        //console.log("new---------------------------")
        //console.log("new entity id:" + event.target.id + "can create " + event.target.dataset.itemCreates);
        //console.log("belongs to id:" + event.target.dataset.itemParentId + " of type " + event.target.dataset.itemParentType);
        //console.log(event.target.parentNode.parentNode.parentNode.parentNode.parentNode.firstChild);

        $.ajax({
            type: "GET",
            url: "/AddNavigation/OnNew",
            data: { id: event.target.id, type: event.target.dataset.itemCreates, name: "hello dolly"},
            cache: false,
            success: result => {
                let obj = JSON.parse(result);
                console.log("result forn new " + obj.type);
                if (obj.success) {

                    console.log("--return fron controller---");
                    console.log(obj.subTree.Id);
                    console.log(obj.subTree.Type);
                    this.AddSubTree(obj.subTree, obj.type, obj.parentId);
                    
                } 
            }
        });
    }

    AddSubTree(subTree, type, parentId) {


        let elmer = this.#FindListGroup(subTree.Type, subTree.Id);
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
        //TreeFactory.GenerateTree(node, node);
    }


   

    // finds the place to put created items in  !bug file!
    #FindListGroup(type, id) {

        console.log("from find listgroup--");
        console.log(id);

        let element = document.getElementById(id);
        console.log(element);
        //let gstr = '[data-item-type="' + type + '"]';
        let gstr = "[id='"+id+"']";
        let bstr = '[data-item-creates="' + type + '"]';
        
        let q = document.querySelectorAll(gstr + bstr);
        console.log(q[0].parentNode.nextSibling);

        return q[0].parentNode.nextSibling;

        //console.log("----quer-----");
        //console.log(q);
        //console.log(parentId);

        //let elmer = document.getElementById(id);

        //let children = elmer.parentNode.nextSibling.childNodes;
        //console.log("-----------------------------------------------------------");
        //console.log(type);

        //for (let i = 0; i < children.length; i++) {

        //    if (children[i].firstChild.firstChild.firstChild.dataset.itemCreates == type) {

        //        return children[i].firstChild.firstChild.nextSibling;
        //    }
        //}
    }




}