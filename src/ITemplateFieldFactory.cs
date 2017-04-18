namespace Rainbow.CodeGenerator
{
    using Model;

    internal interface ITemplateFieldFactory
    {
        TemplateField Create(IItemData field);
    }
}
