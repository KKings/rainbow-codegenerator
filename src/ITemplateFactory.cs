namespace Rainbow.CodeGenerator
{
    using System.Collections.Generic;
    using Model;

    internal interface ITemplateFactory
    {
        IList<TemplateItem> Create(IReadOnlyCollection<IItemData> pool);

        TemplateItem Create(IItemData syncItem, IReadOnlyCollection<IItemData> pool);
    }
}
