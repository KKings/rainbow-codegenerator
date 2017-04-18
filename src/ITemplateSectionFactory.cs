namespace Rainbow.CodeGenerator
{
    using System.Collections.Generic;
    using Model;

    internal interface ITemplateSectionFactory
    {
        TemplateSection Create(IItemData syncItem, IEnumerable<IItemData> syncItems);
    }
}
