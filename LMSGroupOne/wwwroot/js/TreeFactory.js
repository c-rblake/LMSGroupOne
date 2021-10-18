

//root = 0,    enum from c#
//folder = 1,
//student = 2,
//teacher = 3,
//course = 4,
//module = 5,
//activity = 6,
//file = 7,
//search = 8,
//none = 9,
//trash = 10,



//let season = seasons.SPRING


class TreeFactory {
    static NodeTypes =
        {
            ROOT: 0,
            FOLDER: 1,
            STUDENT: 2,
            TEACHER: 3,
            COURSE: 4,
            MODULE: 5,
            ACTIVITY: 6,
            FILE: 7,
            SEARCH: 8,
            NONE: 9,
            TRASH: 10
        }

    static GetIconClass(type, isOpen)
    {
        let classList = "";
        let style = "";

        switch (type) {
            case this.NodeTypes.TEACHER:
                classList = "fas fa-user-graduate";
                style = "color:black;";
                break;
            case this.NodeTypes.COURSE:
                classList = "fas fa-chalkboard-teacher";
                style = "color:darkblue;";
                break;
            case this.NodeTypes.MODULE:
                classList = "fas fa-book-open";
                style = "color:green;";
                break;
            case this.NodeTypes.ACTIVITY:
                classList = "fas fa-flask";
                style = "color:dodgerblue;";
                break;
            case this.NodeTypes.FILE:
                classList = "fas fa-file";
                style = "color:dimgray;";
                break;
            case this.NodeTypes.STUDENT:
                classList = "fas fa-user";
                style = "color:black;";
                break;
            case this.NodeTypes.FOLDER:
                if (isOpen) {
                    classList = "folderIcon fas fa-folder-open";
                    style = "color:yellow;";
                }
                else {
                    classList = "folderIcon fas fa-folder";
                    style = "color:yellow;";
                }
                break;
            case this.NodeTypes.ROOT:
                classList = "fas fa-globe";
                style = "color:black;";
                break;
            case this.NodeTypes.SEARCH:
                classList = "fas fa-search";
                style = "color:black;";
                break;
        };

        return { classList, style };
    }


    static GenerateItem(id, type, parentId, parentType, name, hasChildren, canCreate, canEdit, isOpen, showCaret) {
        let item = document.createElement("nobr");
        item.classList = "treeItem";        

        let temp = this.GetIconClass(type, isOpen);
        
        let classList = temp.classList;
        let style = temp.style;


        if (hasChildren)
        {
            let caret = this.#GenerateCaret(id, type, isOpen, showCaret, canCreate);
            item.innerHTML += caret.outerHTML;
        }
        let icon = this.#GenerateIcon(id, type, classList, style, isOpen);
        item.innerHTML += icon.outerHTML;
        let text = this.#GenerateText(id, name, type, parentId, parentType, canEdit, isOpen);
        item.innerHTML += text.outerHTML;

        
        if (canCreate != this.NodeTypes.NONE) {
            let add = this.#GenerateAddIcon(id, type, parentId, parentType, isOpen, canCreate);
            item.innerHTML += add.outerHTML;
        }

        return item;
    }

    static GenerateTree(node, parentNode) {        
        let mainList = document.createElement("ul");
        mainList.classList = "list-group";
        mainList.style = "user-select:none;";

        mainList.appendChild(this.Recurse(node, parentNode));

        document.write(mainList.outerHTML);
    }

    static GenerateSubTree(node, parentNode, AddEventListener) {
        //console.log(AddEventListener("sdf"));
        let mainList = document.createElement("ul");
        mainList.classList = "list-group";
        mainList.style = "user-select:none;";

        mainList.appendChild(this.Recurse(node, parentNode, (item)=>AddEventListener(item)));

        return mainList;
    }





    static Recurse(node, parentNode, AddEventListener) {
        let list = document.createElement("ul");
        list.classList = "list-group";
        list.hidden = false;

        let listItem = document.createElement("li");
        listItem.classList = "list-group-item";
        listItem.style = "background:none;";

        let hasChildren = node.Nodes != null;
        let showCaret = hasChildren && node.Nodes.length == 0;
        let item = this.GenerateItem(node.Id, node.Type, parentNode.Id, parentNode.Type, node.Name, hasChildren, node.CanCreate, node.Editable, node.Open, showCaret);
        listItem.appendChild(item);
        if (AddEventListener)
        {
            AddEventListener(item);
        }
        //console.log(AddEventListenerCallback(item));

        let childList = document.createElement("ul");
        childList.classList = "list-group";
        childList.hidden = !node.Open;

        if (hasChildren) {
            node.Nodes.forEach(c => childList.appendChild(this.Recurse(c, node)));
        }
        listItem.appendChild(childList);

        list.appendChild(listItem);
        return list;
    }

       

    static #GenerateAddIcon(id, type, parentId, parentType, isOpen, canCreate) {
        
        let add = document.createElement("i");
        add.id = id;
        add.dataset.itemType = type;
        add.dataset.itemCreates = canCreate;
        add.dataset.itemParentId = parentId;
        add.dataset.itemParentType = parentType;
        add.dataset.itemExtra = "new";
        add.dataset.itemOpen = isOpen;
        add.classList = "fas fa-plus-circle";
        add.style = "color:green";
        return add;
    }

    static #GenerateCaret(id, type, isOpen, showCaret, canCreate) {
        let caret = document.createElement("i");
        if (isOpen) {
            caret.classList = "carretIcon fas fa-caret-down";
        }
        else {
            caret.classList = "carretIcon fas fa-caret-right";
        }
        caret.id = id;
        caret.dataset.itemType = type;
        caret.dataset.itemExtra = "caret";
        caret.dataset.itemOpen = isOpen;
        caret.dataset.itemCreates = canCreate;
        caret.hidden = showCaret;
        return caret;
    }

    static #GenerateIcon(id, type, classList, style, isOpen) {
        let icon = document.createElement("i");
        icon.id = id;
        icon.dataset.itemType = type;
        icon.dataset.itemExtra = "icon";
        icon.dataset.itemOpen = isOpen;
        icon.classList = classList;
        icon.style = style;
        return icon;
    }

    static #GenerateText(id, name, type, parentId, parentType, canEdit, isOpen) {
        let text = document.createElement("span");
        text.innerHTML = name;
        text.id = id;
        text.contentEditable = false;
        text.dataset.itemType = type;
        text.dataset.itemExtra = "text";
        text.dataset.itemParentId = parentId;
        text.dataset.itemParentType = parentType;
        text.dataset.itemOpen = isOpen;
        text.dataset.itemEdit = canEdit;
        return text;
    }
        

}

