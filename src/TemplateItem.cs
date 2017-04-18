namespace Rainbow.CodeGenerator
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Model;

    /// <summary>
    /// Represents a deserialized template item.
    /// </summary>
    [DebuggerDisplay("{Name} {TemplateId}")]
    public class TemplateItem : ItemBase
    {
        /// <summary>
        /// All sections within the template, excluding inherited ones.
        /// </summary>
        public IEnumerable<TemplateSection> Sections { get; }

        /// <summary>
        /// Direct base templates for the current template.
        /// </summary>
        public List<TemplateItem> BaseTemplates { get; set; } = new List<TemplateItem>();

        public string Name { get; }

        public string TemplateId { get; }

        public string Id { get; }

        public string Path { get; }

        public string ParentId { get; }

        public TemplateItem(IItemData item,
            string id, 
            string name, 
            string path,
            string parentId,
            string templateId, 
            IEnumerable<TemplateSection> sections) : base(item)
        {
            this.Id = id;
            this.Name = name;
            this.Path = path;
            this.ParentId = parentId;
            this.TemplateId = templateId;
            
            this.Sections = sections;
        }
    }
}