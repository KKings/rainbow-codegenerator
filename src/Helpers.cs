namespace Rainbow.CodeGenerator
{
    using System;
    using System.Linq;

    public static class Helpers
    {
        /// <summary>
        /// Gets the calculated namespace for the template
        /// </summary>
        public static string GetNamespace(string path, string name, bool includeGlobal = false)
        {
            var temp = path;

            if (path.EndsWith(name))
            {
                temp = path.Replace($"/{name}", "");
            }

            var @namespace = temp.AsNamespace();

            return (includeGlobal ? String.Concat("global::", @namespace) : @namespace).Replace("sitecore.templates.", "").Replace("_", "");
        }
        
        /// <summary>
        /// Given a sitecore field, returns the name of the property to use.
        /// <para>If the field is plural, it returns a plural property name</para>
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>A string to use for a property representing the field</returns>
        public static string GetPropertyName(TemplateField field)
        {
            var isFieldPlural = IsFieldPlural(field);

            return field.Name.AsPropertyName(isFieldPlural).Replace("_", "");
        }

        /// <summary>
        /// Determines whether the Sitecore Field holds multiple values.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>
        ///   <c>true</c> if the field holds multiple values; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFieldPlural(TemplateField field)
        {
            var multipleValueFields = new[]
            {
                "treelist",
                "treelistex",
                "treelist descriptive",
                "checklist",
                "multilist"
            };

            return multipleValueFields.Contains(field.FieldType.ToLower());
        }

        public static string GetGlassFieldType(TemplateField field)
        {
            if (field?.FieldType == null)
            {
                throw new Exception("There is no 'Type' field on the " + field?.Name + " field.");
            }

            switch (field.FieldType.ToLower())
            {
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
                case "multilist with search ecommerce":
                case "multilist with search fix":
                case "product multilist with search":
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
                case "product smudge images":
                case "product hero images":
                case "jsontext":

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
                case "product more tab":
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
    }
}
