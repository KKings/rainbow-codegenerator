namespace Rainbow.CodeGenerator
{
    using System.Collections.Generic;
    using System.Linq;

    public static class CodeGenTaskRunner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templates"></param>
        /// <returns></returns>
        public static IList<TemplateModel> Generate(IList<TemplateItem> templates)
        {
            var models = new List<TemplateModel>();

            foreach (var template in templates)
            {
                var templateModel = CodeGenTaskRunner.CreateModel(template);

                foreach (var section in template.Sections)
                {
                    var sectionModel = new SectionModel
                    {
                        Item = section,
                        Fields = section.Fields.Select(field => new FieldModel
                        {
                            Item = field,
                            PropertyName = Helpers.GetPropertyName(field),
                            PropertyType = Helpers.GetGlassFieldType(field)
                        }).ToList()
                    };

                    templateModel.Sections.Add(sectionModel);
                }

                models.Add(templateModel);
            }


            var modelLookup = models.ToDictionary(t => t.Item.Id, t => t);

            foreach (var template in templates.Where(m => m.BaseTemplates.Any()))
            {
                var range = template.BaseTemplates
                                .Where(m => modelLookup.ContainsKey(m.Id)).Select(m => modelLookup[m.Id]);

                modelLookup[template.Id].BaseTemplates.AddRange(range);
            }

            return models;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        static TemplateModel CreateModel(TemplateItem template)
        {
            return new TemplateModel
            {
                Item = template,
                ClassName = template.Name.AsClassName(),
                InterfaceName = template.Name.AsInterfaceName(),
                Namespace = Helpers.GetNamespace(template.Path, template.Name)
            };
        }
    }
}
