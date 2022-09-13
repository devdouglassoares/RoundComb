using System;

namespace Core.Templating.Parsers
{
    public class ParseException : Exception
    {
        public ParseException(string message, params object[] replacements) : base(String.Format(message, replacements)) { }
    }
}
