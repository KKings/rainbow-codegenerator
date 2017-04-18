namespace Rainbow.CodeGenerator
{
    using System.Collections.Generic;
    using System.IO;
    using Model;

    internal interface IUnicornReader
    {
        IEnumerable<IItemData> ReadDirectory(DirectoryInfo info);

        bool FilterExclusions(IItemData itemData);
    }
}
