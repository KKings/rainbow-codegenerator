namespace Rainbow.CodeGenerator
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using Sitecore;

    internal class TemplateSectionFactory : ITemplateSectionFactory
    {
        /// <summary>
        /// Field Factory for Creating Fields
        /// </summary>
        private readonly ITemplateFieldFactory _fieldFactory;

        public TemplateSectionFactory() : this(new TemplateFieldFactory()) { }

        public TemplateSectionFactory(ITemplateFieldFactory fieldFactory)
        {
            this._fieldFactory = fieldFactory;
        }

        public virtual TemplateSection Create(IItemData section, IEnumerable<IItemData> pool)
        {
            var name = section.Name;

            var fields = pool
                .Where(field => field.TemplateId == TemplateIDs.TemplateField.ToGuid() && field.ParentId == section.Id)
                .Select(field => this._fieldFactory.Create(field));

            return new TemplateSection(section, name, fields);
        }
    }
}
