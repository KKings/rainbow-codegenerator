let getType = (sitecoreType, fieldId) => {

    var mappedType = getFieldMapping(fieldId.toLowerCase())

    if (mappedType) {
        return mappedType
    }

    switch(sitecoreType.toLowerCase()) {
        case "tristate":
            return "TriState";
        case "checkbox":
        case "readonlycheckbox":
            return "bool";

        case "date":
        case "datetime":
            return "DateTime";

        case "number":
            return "float";

        case "integer":
            return "int";

        case "treelist with search":
        case "treelist":
        case "treelistex":
        case "treelist descriptive":
        case "checklist":
        case "multilist with search":
        case "multilist":
        case "product list":
        case "brightcove multilist with search":
        case "queryable treelist":
            return "IEnumerable<Item>";
        case "grouped droplink":
        case "droplink":
        case "lookup":
        case "droptree":
        case "internal link":
        case "reference":
        case "tree":
            return "Item";

        case "file":
            return "File";

        case "image":
            return "Image";

        case "general link":
        case "general link with search":
            return "Link";

        case "readonlytext":
        case "password":
        case "icon":
        case "rich text":
        case "html":
        case "single-line text":
        case "multi-line text":
        case "frame":
        case "text":
        case "memo":
        case "droplist":
        case "grouped droplist":
        case "valuelookup":
        case "imagepreview":
            return "string";
        case "attachment":
        case "word document":
            return "System.IO.Stream";
        case "name lookup value list":
        case "name value list":
            return "NameValueCollection";
        case "link list":
            return "IList<Link>";
        default:
            return "object";
    }
}

let mapping = {
    "021aa3f7-206f-4acc-9538-f6d7fe86b168": "Folder"
}

let getFieldMapping = (fieldId) => {
    return  (mapping[fieldId]) ? mapping[fieldId] : false
}

module.exports = {
    getType: getType, 
    getFieldMapping: getFieldMapping
}