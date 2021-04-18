using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Foundation.CQRS.Core
{
    public static class StringExtentions
    {
        public static bool IsNullOrEmpty(this string @this) => string.IsNullOrEmpty(@this);

        public static string ToSnakeCase(this string @this) {
            if (string.IsNullOrEmpty(@this)) {
                return @this;
            }
            var underScores = Regex.Match(@this, @"^_+");
            return underScores + Regex.Replace(@this, @"([a-z0-9])([A-Z])", "@$1_$2").ToLower();
        }
    }
}
