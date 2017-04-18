namespace Rainbow.CodeGenerator
{
    using System;
    using System.Linq;
    using Model;
    using Sitecore.Data;

    internal class TemplateFieldFactory : ITemplateFieldFactory
    {
        public virtual TemplateField Create(IItemData field)
        {
            var name = field.Name;
            var templateId = new ID(field.TemplateId).ToString();
            var path = field.Path;
            var id = field.Id.ToString();
            var type = this.GetSharedFieldValue(field, "Type", "(unknown)");

            var title = this.GetFieldValue(field, "Title");

            return new TemplateField(field, id, name, templateId, path, type, title);
        }


        /// <summary>
        /// Returns the shared field value, based on the field name.
        /// </summary>
        /// <param name="syncItem"></param>
        /// <param name="name">Name of the field</param>
        /// <param name="default">If provided, will fallback to this value if no field value is available</param>
        /// <returns>The field value</returns>
        protected virtual string GetSharedFieldValue(IItemData syncItem, string name, string @default = "")
        {
            if (syncItem == null)
            {
                return String.Empty;
            }

            var typeField = syncItem.SharedFields.FirstOrDefault(f => name.Equals(f.NameHint));

            return !String.IsNullOrWhiteSpace(typeField?.NameHint)
                       ? typeField.Value
                       : @default;
        }

        /// <summary>
        /// Returns the field value in the first version it can find, based on the field name.
        /// </summary>
        /// <param name="syncItem"></param>
        /// <param name="name">Name of the field</param>
        /// <param name="default">If provided, will fallback to this value if no field value is available</param>
        /// <returns>The field value</returns>
        protected virtual string GetFieldValue(IItemData syncItem, string name, string @default = "")
        {
            if (syncItem == null)
            {
                return String.Empty;
            }

            var typeField = syncItem.Versions.SelectMany(v => v.Fields).FirstOrDefault(f => name.Equals(f.NameHint));

            return !String.IsNullOrWhiteSpace(typeField?.Value)
                       ? typeField.Value
                       : @default;
        }
    }
}
