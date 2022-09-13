using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Core.Templating.Parsers
{
    /// <summary>
    /// The main entry point for this library. Use the static "Parse" methods to create template functions.
    /// Functions are safe for reuse, so you may parse and cache the resulting function.
    /// </summary>
    public class TemplateParser
    {
        public static string Parse<TModel>(string template, TModel model, bool disableContentEscaping = false)
        {
            var tokens = new Queue<TokenPair>(Tokenizer.Tokenize(template));
            var internalTemplate = Parse(tokens, new ParsingOptions { DisableContentSafety = disableContentEscaping });
            var retval = new StringBuilder();
            var context = new ContextObject
            {
                Value = model,
                Key = ""
            };
            internalTemplate(retval, context);
            return retval.ToString();
        }

        private static Action<StringBuilder, ContextObject> Parse(Queue<TokenPair> tokens, ParsingOptions options)
        {
            var buildArray = new List<Action<StringBuilder, ContextObject>>();

            while (tokens.Any())
            {
                var currentToken = tokens.Dequeue();
                switch (currentToken.Type)
                {
                    case TokenType.Comment:
                        break;
                    case TokenType.Content:
                        buildArray.Add(HandleContent(currentToken.Value));
                        break;
                    case TokenType.CollectionOpen:
                        buildArray.Add(HandleCollectionOpen(currentToken, tokens, options));
                        break;
                    case TokenType.ElementOpen:
                        buildArray.Add(HandleElementOpen(currentToken, tokens, options));
                        break;
                    case TokenType.InvertedElementOpen:
                        buildArray.Add(HandleInvertedElementOpen(currentToken, tokens, options));
                        break;
                    case TokenType.CollectionClose:
                    case TokenType.ElementClose:
                        // This should immediately return if we're in the element scope, 
                        // and if we're not, this should have been detected by the tokenizer!
                        return (builder, context) =>
                        {
                            foreach (var a in buildArray)
                            {
                                a(builder, context);
                            }
                        };
                    case TokenType.EscapedSingleValue:
                    case TokenType.UnescapedSingleValue:
                        buildArray.Add(HandleSingleValue(currentToken, options));
                        break;
                }
            }

            return (builder, context) =>
            {
                foreach (var a in buildArray)
                {
                    a(builder, context);
                }
            };
        }

        private static string HtmlEncodeString(string context)
        {
            return HttpUtility.HtmlEncode(context);
        }

        private static Action<StringBuilder, ContextObject> HandleSingleValue(TokenPair token, ParsingOptions options)
        {
            return (builder, context) =>
            {
                if (context != null)
                {
                    //try to locate the value in the context, if it exists, append it.
                    var c = context.GetContextForPath(token.Value);
                    if (c.Value != null)
                    {
                        if (token.Type == TokenType.EscapedSingleValue && !options.DisableContentSafety)
                        {
                            builder.Append(HtmlEncodeString(c.ToString()));
                        }
                        else
                        {
                            builder.Append(c);
                        }
                    }
                }
            };
        }

        private static Action<StringBuilder, ContextObject> HandleContent(string token)
        {
            return (builder, context) => builder.Append(token);
        }

        private static Action<StringBuilder, ContextObject> HandleInvertedElementOpen(TokenPair token, Queue<TokenPair> remainder,
            ParsingOptions options)
        {
            var innerTemplate = Parse(remainder, options);

            return (builder, context) =>
            {
                var c = context.GetContextForPath(token.Value);
                //"falsey" values by Javascript standards...
                if (!c.Exists())
                {
                    innerTemplate(builder, c);
                }
            };
        }

        private static Action<StringBuilder, ContextObject> HandleCollectionOpen(TokenPair token, Queue<TokenPair> remainder, ParsingOptions options)
        {
            var innerTemplate = Parse(remainder, options);

            return (builder, context) =>
            {
                //if we're in the same scope, just negating, then we want to use the same object
                var c = context.GetContextForPath(token.Value);

                //"falsey" values by Javascript standards...
                if (!c.Exists()) return;

                if (c.Value is IEnumerable && !(c.Value is String) && !(c.Value is IDictionary<string, object>))
                {
                    var index = 0;
                    foreach (object i in c.Value as IEnumerable)
                    {
                        var innerContext = new ContextObject()
                        {
                            Value = i,
                            Key = String.Format("[{0}]", index),
                            Parent = c
                        };
                        innerTemplate(builder, innerContext);
                        index++;
                    }
                }
                else
                {
                    throw new IndexedParseException("'{0}' is used like an array by the template, but is a scalar value or object in your model.", token.Value);
                }
            };
        }

        private static Action<StringBuilder, ContextObject> HandleElementOpen(TokenPair token, Queue<TokenPair> remainder, ParsingOptions options)
        {
            var innerTemplate = Parse(remainder, options);

            return (builder, context) =>
            {
                var c = context.GetContextForPath(token.Value);
                //"falsey" values by Javascript standards...
                if (c.Exists())
                {
                    innerTemplate(builder, c);
                }
            };
        }
    }

}

