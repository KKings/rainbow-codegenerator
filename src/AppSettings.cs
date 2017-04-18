namespace Rainbow.CodeGenerator
{
    using System.Collections.Generic;

    internal class AppSettings
    {
        public bool Debug { get; set; }

        public string Output { get; set; }

        public string Folder { get; set; }

        public List<string> IncludeSitecorePaths { get; set; } = new List<string>();
    }
}
