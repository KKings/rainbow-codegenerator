namespace Rainbow.CodeGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using Sitecore;
    using Sitecore.Data;

    internal class TemplateFactory : ITemplateFactory
    {
        private readonly ITemplateSectionFactory _templateSectionFactory;

        public TemplateFactory() : this(new TemplateSectionFactory()) { }

        public TemplateFactory(ITemplateSectionFactory templateSectionFactory)
        {
            this._templateSectionFactory = templateSectionFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pool"></param>
        /// <returns></returns>
        public virtual IList<TemplateItem> Create(IReadOnlyCollection<IItemData> pool)
        {
            if (pool == null || !pool.Any())
            {
                return new TemplateItem[0];
            }

            var templates = pool.Select(item => this.Create(item, pool))
                .ToList();
            
            this.AddBaseTemplates(templates);

            return this.ExcludeTemplates(templates);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemData"></param>
        /// <param name="pool"></param>
        /// <returns></returns>
        public virtual TemplateItem Create(IItemData itemData, IReadOnlyCollection<IItemData> pool)
        {
            var name = itemData.Name;
            var templateId = new ID(itemData.TemplateId).ToString();
            var id = new ID(itemData.Id).ToString();
            var path = itemData.Path;
            var parentId = new ID(itemData.ParentId).ToString();
            var sections = this.SelectSections(itemData, pool);

            return new TemplateItem(itemData, id, name, path, parentId, templateId, sections);
        }

        public virtual IList<TemplateItem> ExcludeTemplates(IEnumerable<TemplateItem> templates)
        {
            return templates.Where(template =>
                template.TemplateId != TemplateIDs.BranchTemplateFolder.ToString() &&
                template.TemplateId != TemplateIDs.TemplateFolder.ToString() &&
                template.TemplateId != TemplateIDs.TemplateSection.ToString() &&
                template.TemplateId.ToUpper() != TemplateIDs.TemplateField.ToString()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemData"></param>
        /// <param name="pool"></param>
        /// <returns></returns>
        protected virtual IList<TemplateSection> SelectSections(IItemData itemData, IReadOnlyCollection<IItemData> pool)
        {
            return pool
                .Where(template => template.TemplateId == TemplateIDs.TemplateSection.ToGuid() && template.ParentId == itemData.Id)
                .Select(template => this._templateSectionFactory.Create(template, pool))
                .ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templates"></param>
        protected virtual void AddBaseTemplates(IList<TemplateItem> templates)
        {
            var templateLookup = templates.ToDictionary(t => t.Id, t => t);

            foreach (var template in templates)
            {
                var field = template.Item
                    .SharedFields
                    .FirstOrDefault(f => f.FieldId == FieldIDs.BaseTemplate.ToGuid());

                if (!String.IsNullOrWhiteSpace(field?.Value))
                {
                    var baseTemplateIds = field.Value
                        .Split(new[] { '|', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(ID.IsID)
                        .Select(ID.Parse)
                        .ToArray();

                    var baseTemplates = baseTemplateIds
                        .Where(id => templateLookup.Keys.Contains(id.ToString()))
                        .Select(id => templateLookup[id.ToString()])
                        .ToList();
                   var da= baseTemplates.Any();

                    template.BaseTemplates.AddRange(baseTemplates);
                }
            }
        }
    }
}
