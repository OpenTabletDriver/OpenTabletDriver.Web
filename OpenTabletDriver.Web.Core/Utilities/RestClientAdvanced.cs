using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenTabletDriver.Web.Core.Utilities
{
    public class RestClientAdvanced
    {
        public object Get(string endpointString, object values)
        {
            string str = endpointString;
            var matches = Regex.Matches(endpointString, "{(.+?)}") as IEnumerable<Match>;
            var matchingProperties = GetMatchingProperties(matches, values);
            foreach (var match in matches)
            {
                var targetValue = match.Groups[1].Value;
                var value = GetValue(match, values);
                str = str.Replace($"{{{targetValue}}}", value.ToString());
            }

            throw new NotImplementedException();
        }

        private IEnumerable<PropertyInfo> GetMatchingProperties(IEnumerable<Match> matches, object values)
        {
            return matches.Select(match => GetMatchingProperty(match, values));
        }

        private PropertyInfo GetMatchingProperty(Match match, object values)
        {
            string replaceValue = match.Groups[1].Value;
            return values.GetType().GetProperty(replaceValue, BindingFlags.IgnoreCase);
        }

        private object GetValue(Match match, object values)
        {
            var property = GetMatchingProperty(match, values);
            return property.GetValue(values);
        }
    }
}