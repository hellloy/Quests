using System.Text;
using System.Text.RegularExpressions;

namespace Quests.Client
{
    public class Helper
    {
        public static string GetFirstCharacters(string htmlString, int length)
        {
            if (htmlString == null)
                return string.Empty;

            if(htmlString.Length < length)
                return htmlString;

            // regex to separate string on parts: tags, texts
            var separateRegex = new Regex("([^>][^<>]*[^<])|[\\S]{1}");
            // regex to identify tags
            var tagsRegex = new Regex("^<[^>]+>$");

            // separate string on tags and texts
            var matches = separateRegex.Matches(htmlString);

            // looping by mathes
            // if it's a tag then just append it to resuls,
            // if it's a text then append substing of it (considering the number of characters)
            var counter = 0;
            var sb = new StringBuilder();
            for (var i = 0; i < matches.Count; i++)
            {
                var m = matches[i].Value;

                // check if it's a tag
                if (tagsRegex.IsMatch(m))
                {
                    sb.Append(m);
                }
                else
                {
                    var lengthToCut = length - counter;

                    var sub = lengthToCut >= m.Length
                        ? m
                        : m.Substring(0, lengthToCut);

                    counter += sub.Length;
                    sb.Append(sub);
                }
            }

            return sb.ToString();
        }
    }
}
