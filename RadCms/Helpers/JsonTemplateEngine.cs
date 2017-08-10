using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RadCms.Helpers
{
    public class JsonTemplateEngine
    {
        /// <summary>
        ///  var replacements = new Dictionary<string, object> {
        ///     { "networkid", "WHEEE!!" } // etc.
        ///  };
        ///  
        /// <h1 mytag="title">{title}</h1>
        /// </summary>
        /// <param name="src"></param>
        /// <param name="replacements"></param>
        /// <returns></returns>
        /// 
        private static Regex functionRegex = new Regex(@"{(\w+)}");

        public static string BuildPageContentFromTemplate(string template, IDictionary<string, string> replacements)
        {
            return functionRegex.Replace(template, (m) =>
            {
                string replacement;
                var key = m.Groups[1].Value;
                if (replacements.TryGetValue(key, out replacement))
                {
                    return Convert.ToString(replacement);
                }
                else
                {
                    return "";
                    //return m.Groups[0].Value;
                }
            });
        }
    }
}
