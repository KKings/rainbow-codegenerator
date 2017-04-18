namespace Rainbow.CodeGenerator
{
    using System.Diagnostics;
    using Model;

    /// <summary>
    /// Represents a deserialized template field.
    /// </summary>
    [DebuggerDisplay("{Name} {FieldType}")]
    public class TemplateField : ItemBase
    {
        /// <summary>
        /// Gets or sets the Name of the Field
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the Template Id of the Field
        /// </summary>
        public string TemplateId { get; }

        /// <summary>
        /// Gets or sets the Id of the Field
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets or sets the Field Title
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets or sets the Field Type
        /// </summary>
        public string FieldType { get; }

        /// <summary>
        /// Gets or sets the Field Path
        /// </summary>
        public string Path { get; }

        public TemplateField(IItemData item,
            string id,
            string name, 
            string templateId, 
            string path, 
            string type, 
            string title) : base(item)
        {
            this.Id = id;
            this.Name = name;
            this.TemplateId = templateId;
            this.Path = path;
            this.FieldType = type;
            this.Title = title;
        }
    }
}