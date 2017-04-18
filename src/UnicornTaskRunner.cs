namespace Rainbow.CodeGenerator
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Storage.Yaml;

    internal static class UnicornTaskRunner
    {
        public static IList<TemplateItem> Run(AppSettings settings)
        {
            var formatter = new YamlSerializationFormatter(null, null);

            var folder = new DirectoryInfo(settings.Folder);

            var unicornData = new UnicornReader(formatter, settings.Debug, settings.IncludeSitecorePaths).ReadDirectory(folder).ToList();

            var factory = new TemplateFactory();

           return factory.Create(unicornData);
        }
    }
}
