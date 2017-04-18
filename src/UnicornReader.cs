namespace Rainbow.CodeGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Formatting;
    using Model;
    using Storage.Yaml;

    internal class UnicornReader : IUnicornReader
    {
        private readonly ISerializationFormatter _formatter;
        private readonly bool _debug;
        private readonly IList<string> _pathExclusions;

        public UnicornReader(ISerializationFormatter formatter, bool debug, IList<string> pathExclusions)
        {
            this._formatter = formatter;
            this._debug = debug;
            this._pathExclusions = pathExclusions;

        }

        public virtual IEnumerable<IItemData> ReadDirectory(DirectoryInfo folder)
        {
            var serializedItems = new List<IItemData>();

            foreach (var itemFile in folder.GetFiles("*.yml", SearchOption.AllDirectories))
            {
                using (var sr = new StreamReader(itemFile.FullName))
                {
                    try
                    {
                        var item = this._formatter.ReadSerializedItem(sr.BaseStream, itemFile.Name);

                        if (this.FilterExclusions(item))
                        {
                            continue;
                        }

                        serializedItems.Add(item);

                    }
                    catch (YamlFormatException ex)
                    {
                        if (this._debug)
                        {
                            Console.WriteLine($"Skipping {itemFile.FullName} as it does not appear to be represent an item.");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Unable to deserialize '{itemFile.FullName}'", ex);
                    }
                }
            }

            return serializedItems;
        }

        public virtual bool FilterExclusions(IItemData itemData)
        {
            return itemData == null
                   || itemData.Name.Contains("__Standard Values")
                   || !String.Equals(itemData.DatabaseName, "master", StringComparison.InvariantCultureIgnoreCase)
                   || !this._pathExclusions.Any(p => itemData.Path.StartsWith(p));
        }
    }
}
