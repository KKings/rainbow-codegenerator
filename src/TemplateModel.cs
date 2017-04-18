namespace Rainbow.CodeGenerator
{
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("{ClassName} - {Namespace}")]
    public class TemplateModel
    {
        public string Namespace { get; set; }
        
        public TemplateItem Item { get; set; }

        public string ClassName { get; set; }

        public string InterfaceName { get; set; }

        public string FullNamespace  => $"{this.Namespace}.{this.ClassName}";

        public string GlobalFullNamesapce => $"global::{this.FullNamespace}";

        /// <summary>
        /// A list of all templates this template inherits from.
        /// </summary>
        public List<SectionModel> Sections { get; set; } = new List<SectionModel>();

        /// <summary>
        /// A list of all templates this template inherits from.
        /// </summary>
        public List<TemplateModel> BaseTemplates { get; set; } = new List<TemplateModel>();
    }

    public class SectionModel
    {
        public TemplateSection Item { get; set; }

        public List<FieldModel> Fields { get; set; } = new List<FieldModel>();
    }

    public class FieldModel
    {
        public TemplateField Item { get; set; }
        
        public string PropertyName { get; set; }

        public string PropertyType { get; set; }
    }
}
