namespace Rainbow.CodeGenerator
{
    using System.Collections.Generic;
    using Model;

    /// <summary>
    /// Represents a deserialized template section.
    /// </summary>
    public class TemplateSection : ItemBase
    {
        /// <summary>
        /// Gets or sets the Section Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the fields within a section
        /// </summary>
        public IEnumerable<TemplateField> Fields { get; }

        public TemplateSection(IItemData item, string name, IEnumerable<TemplateField> fields) : base(item)
        {
            this.Name = name;
            this.Fields = fields;
        }
    }
}