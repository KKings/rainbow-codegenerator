namespace Rainbow.CodeGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fclp;
    using Newtonsoft.Json;

    class Program
    {
        static void Main(string[] args)
        {
            var settings = Program.Setup(args);

            if (settings == null)
            {
                return;
            }

            var templates = UnicornTaskRunner.Run(settings);

            var models = CodeGenTaskRunner.Generate(templates);

            if (!settings.Debug)
            {
                Console.WriteLine(JsonConvert.SerializeObject(models));
                return;
            }

            ListTemplates(templates);
            Console.ReadLine();
        }

        /// <summary>
        /// Initialize the Command Line and process the arguments
        /// </summary>
        /// <param name="args">The Args</param>
        /// <returns><c>AppSettings</c> from the args</returns>
        static AppSettings Setup(string[] args)
        {
            // create a generic parser for the ApplicationArguments type
            var parser = new FluentCommandLineParser<AppSettings>();

            parser.SetupHelp("?", "help")
                  .Callback(text => Console.WriteLine(text));

            parser.Setup(arg => arg.Folder)
                  .As('f', "folder")
                  .WithDescription("Folder to serialize unicorn files.")
                  .Required();

            parser.Setup(arg => arg.Debug)
                  .As('d', "debug")
                  .WithDescription("Application will output debug messages");

            parser.Setup(arg => arg.IncludeSitecorePaths)
                  .As('p', "include-sitecore-paths")
                  .SetDefault(new List<string>())
                  .WithDescription("Include these sitecore paths to generate models.");

            var result = parser.Parse(args);

            if (result.HasErrors)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Missing or invalid argument, please pass in a {error.Option.SetupType} using {error.Option.LongName} or {error.Option.ShortName}");
                }

                Console.ReadLine();

                return null;
            }

            var settings = parser.Object;

            if (settings.IncludeSitecorePaths.Any())
            {
                var temp = settings.IncludeSitecorePaths.Select(path => path.Replace(@"\", "/")).ToList();

                settings.IncludeSitecorePaths = temp;
            }

            return settings;
        }

        static void ListTemplates(IList<TemplateItem> templates)
        {
            foreach (var template in templates)
            {
                Console.WriteLine($"{template.Name} {String.Join(" ", template.BaseTemplates.Select(m => m.Name))}");

                foreach (var section in template.Sections)
                {
                    Console.WriteLine($"\t{section.Name}");

                    foreach (var templateField in section.Fields)
                    {
                        Console.WriteLine($"\t\t{templateField.Name} - {templateField.FieldType}");
                    }
                }
            }
        }
    }
}