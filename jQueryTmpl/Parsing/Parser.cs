using System.Collections.Generic;

using jQueryTmpl.Tokenization;

namespace jQueryTmpl.Parsing
{
    public class Parser
    {
        public Template Parse(IEnumerable<Token> tokens)
        {
            var stack = new Stack<Template>();
            stack.Push(new Template());

            foreach(var token in tokens)
            {
                if(token.IsStartTag)
                {
                    var template = token.CreateTemplate();
                    stack.Push(template);
                }

                else if(token.IsEndTag)
                {
                    while(token.CanClose(stack.Peek()))
                    {
                        var template = stack.Pop();
                        template.Close(token);
                        stack.Peek().AddChild(template);

                        if(token.IsFinalCloseFor(template))
                            break;
                    }
                }

                else
                {
                    var template = token.CreateTemplate();
                    stack.Peek().AddChild(template);
                }
            }

            if(stack.Count > 1)
                throw new TagMismatchException("The tag " + stack.Peek().Value + " was not properly closed.");

            var parent = stack.Pop();
            return parent.Children.Count == 1 ? parent.Children[0] : parent;
        }
    }
}