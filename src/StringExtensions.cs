namespace Rainbow.CodeGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {

        public static string TitleCase(this string word)
        {
            var newWord = Regex.Replace(word, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1+");
            newWord = System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(newWord);
            newWord = newWord.Replace("+", "");
            return newWord;
        }

        /// <summary>
        /// <para>
        /// Something -> something
        /// ISomething -> iSomething
        /// SomethingElse -> somethingElse
        /// ISomethingElse -> iSomethingElse
        /// </para>
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string CamelCase(this string word)
        {
            if (word == null)
            {
                return "";
            }

            var titleCase = TitleCase(word);
            return titleCase.Substring(0, 1).ToLower() + titleCase.Substring(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsInterfaceWord(this string word)
        {
            // looks like an interface if... I[A-Z]xxxx
            // proper definition is http://msdn.microsoft.com/en-us/library/8bc1fexb(v=VS.71).aspx
            return word.Length > 2 && !word.Contains(" ") && word[0] == 'I' && char.IsUpper(word, 1) && char.IsLower(word, 2);
        }

        /// <summary>
        /// <para>
        /// return I[TitleCaseWord]
        /// something -> ISomething
        /// Something -> ISomething
        /// ISomething -> ISomething
        /// </para>
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string AsInterfaceName(this string word)
        {
            var interfaceWord = GetFormattedWord(word, TitleCase);

            // Only prefix the word with a 'I' if we don't have a word that already looks like an interface.
            if (!IsInterfaceWord(word))
            {
                interfaceWord = string.Concat("I", interfaceWord);
            }
            return interfaceWord.Replace("_", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string AsClassName(this string word)
        {
            return GetFormattedWord(word, TitleCase).Replace("_", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <param name="pluralize"></param>
        /// <returns></returns>
        public static string AsPropertyName(this string word, bool pluralize = false)
        {
            // TitleCase the word and pluralize it
            return pluralize ? GetFormattedWord(word, TitleCase, Inflector.Pluralize) : GetFormattedWord(word, TitleCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string AsFieldName(this string word)
        {
            // return _someParam. 
            // Note, this isn't MS guideline, but it easier to deal with than using this. everywhere to avoid name collisions
            return GetFormattedWord(word, CamelCase, s => "_" + s);
        }

        /// <summary>
        /// Tests whether the words conflicts with reserved or language keywords, and if so, attempts to return 
        /// valid words that do not conflict. Usually the returned words are only slightly modified to differentiate 
        /// the identifier from the keyword; for example, the word might be preceded by the underscore ("_") character.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <returns></returns>
        public static IEnumerable<string> AsValidWords(this IEnumerable<string> words)
        {
            return words.Select(AsValidWord);
        }

        /// <summary>
        /// Tests whether the word conflicts with reserved or language keywords, and if so, attempts to return a 
        /// valid word that does not conflict. Usually the returned word is only slightly modified to differentiate 
        /// the identifier from the keyword; for example, the word might be preceded by the underscore ("_") character.
        /// <para>
        /// Valid identifiers in C# are defined in the C# Language Specification, item 2.4.2. The rules are very simple:
        /// - An identifier must start with a letter or an underscore
        /// - After the first character, it may contain numbers, letters, connectors, etc
        /// - If the identifier is a keyword, it must be prepended with “@”
        /// </para>
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>A valid word for the specified word.</returns>
        public static string AsValidWord(this string word)
        {
            var identifier = word;

            if (identifier == "*")
            {
                identifier = "Wildcard";
            }

            //identifier = RemoveDiacritics(identifier);

            // C# Identifiers - http://msdn.microsoft.com/en-us/library/aa664670(VS.71).aspx
            // replace all illegal chars with an '_'
            var regex = new Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]");
            identifier = regex.Replace(identifier, "_");

            //The identifier must start with a character or '_'
            if (!(char.IsLetter(identifier, 0) || identifier[0] == '_'))
            {
                identifier = string.Concat("_", identifier);
            }

            // fix language specific reserved words
            identifier = FixReservedWord(identifier);

            // Let's make sure we have a valid name
            System.Diagnostics.Debug.Assert(System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(identifier), string.Format("'{0}' is an invalid name for a Template or Field", word));
            return identifier;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string AsNamespace(this string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return String.Empty;
            }

            return String.Join(".",
                path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(s => !String.IsNullOrEmpty(s))
                    .Select(word => word.AsValidWord())
                    .ToArray());
        }

        /// <summary>
        /// Tests whether the word conflicts with reserved or language keywords, and if so, attempts to return a 
        /// valid word that does not conflict. Usually the returned word is only slightly modified to differentiate 
        /// the identifier from the keyword; for example, the word might be preceded by the underscore ("_") character.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        private static string FixReservedWord(this string word)
        {
            // turns keywords into usable words.
            // i.e. class -> _class
            var codeProvider = new Microsoft.CSharp.CSharpCodeProvider();
            return codeProvider.CreateValidIdentifier(word);
        }

        private static string GetFormattedWord(this string word, params Func<string, string>[] transformations)
        {
            var newWord = transformations.Where(item => item != null).Aggregate(word, (current, item) => item(current));
            
            newWord = AsValidWord(newWord);
            return newWord;
        }
    }
}
