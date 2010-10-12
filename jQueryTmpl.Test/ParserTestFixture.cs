using System.Collections.Generic;
using System.Linq;

using jQueryTmpl.Parsing;
using jQueryTmpl.Tags;
using jQueryTmpl.Tokenization;

using NUnit.Framework;

namespace jQueryTmpl.Test
{
    [TestFixture]
    public class ParserTestFixture
    {
        [Test]
        public void ParseSingleLiteralTag()
        {
            var tokens = new List<Token> {
                new Token(@"<ul><li>Blah</li></ul>", new LiteralTagDescriptor())
            };

            var parser = new Parser();
            var template = parser.Parse(tokens);

            Assert.That(template, Is.TypeOf<LiteralTemplate>());
            Assert.That(template.Value, Is.EqualTo(tokens[0].Value));
        }

        [Test]
        public void ParseSingleToken()
        {
            var tokens = new List<Token> {
                new Token(@"<li>", new LiteralTagDescriptor()),
                new Token(@"${firstName}", new PrintTagDescriptor()),
                new Token(@"</li>", new LiteralTagDescriptor())
            };
            
            var parser = new Parser();
            var template = parser.Parse(tokens);

            Assert.That(template.Children.Count(), Is.EqualTo(3));
            Assert.That(template.Children.ElementAt(0), Is.TypeOf<LiteralTemplate>());
            Assert.That(template.Children.ElementAt(0).Value, Is.EqualTo(@"<li>"));
            Assert.That(template.Children.ElementAt(1), Is.TypeOf<PrintTemplate>());
            Assert.That(template.Children.ElementAt(1).Value, Is.EqualTo(@"${firstName}"));
            Assert.That(template.Children.ElementAt(2), Is.TypeOf<LiteralTemplate>());
            Assert.That(template.Children.ElementAt(2).Value, Is.EqualTo(@"</li>"));
        }

        [Test]
        public void ParseMultipleTokens()
        {
            var tokens = new List<Token> {
                new Token(@"<li>", new LiteralTagDescriptor()),
                new Token(@"${firstName}", new PrintTagDescriptor()),
                new Token(@" ", new LiteralTagDescriptor()),
                new Token(@"${lastName}", new PrintTagDescriptor()),
                new Token(@" ", new LiteralTagDescriptor()),
                new Token(@"${suffix}", new PrintTagDescriptor()),
                new Token(@"</li>", new LiteralTagDescriptor())
            };
            
            var parser = new Parser();
            var template = parser.Parse(tokens);

            Assert.That(template.Children.Count(), Is.EqualTo(7));
            Assert.That(template.Children.ElementAt(0), Is.TypeOf<LiteralTemplate>());
            Assert.That(template.Children.ElementAt(0).Value, Is.EqualTo(@"<li>"));
            Assert.That(template.Children.ElementAt(1), Is.TypeOf<PrintTemplate>());
            Assert.That(template.Children.ElementAt(1).Value, Is.EqualTo(@"${firstName}"));
            Assert.That(template.Children.ElementAt(2), Is.TypeOf<LiteralTemplate>());
            Assert.That(template.Children.ElementAt(2).Value, Is.EqualTo(@" "));
            Assert.That(template.Children.ElementAt(3), Is.TypeOf<PrintTemplate>());
            Assert.That(template.Children.ElementAt(3).Value, Is.EqualTo(@"${lastName}"));
            Assert.That(template.Children.ElementAt(4), Is.TypeOf<LiteralTemplate>());
            Assert.That(template.Children.ElementAt(4).Value, Is.EqualTo(@" "));
            Assert.That(template.Children.ElementAt(5), Is.TypeOf<PrintTemplate>());
            Assert.That(template.Children.ElementAt(5).Value, Is.EqualTo(@"${suffix}"));
            Assert.That(template.Children.ElementAt(6), Is.TypeOf<LiteralTemplate>());
            Assert.That(template.Children.ElementAt(6).Value, Is.EqualTo(@"</li>"));
        }

        [Test]
        public void ParseSimpleIf()
        {
            var tokens = new List<Token> {
                new Token(@"<li class='something", new LiteralTagDescriptor()),
                new Token(@"{{if Blah}}", new IfTagDescriptor()),
                new Token(@" blah", new LiteralTagDescriptor()),
                new Token(@"{{/if}}", new EndIfTagDescriptor()),
                new Token(@"'>Test</li>", new LiteralTagDescriptor()),
            };

            var parser = new Parser();
            var template = parser.Parse(tokens);

            Assert.That(template.Children.Count(), Is.EqualTo(3));
            Assert.That(template.Children.ElementAt(0), Is.TypeOf<LiteralTemplate>());
            Assert.That(template.Children.ElementAt(0).Value, Is.EqualTo(@"<li class='something"));
            Assert.That(template.Children.ElementAt(1), Is.TypeOf<IfTemplate>());
            Assert.That(template.Children.ElementAt(1).Value, Is.EqualTo(@"{{if Blah}} blah{{/if}}"));
            Assert.That(template.Children.ElementAt(2), Is.TypeOf<LiteralTemplate>());
            Assert.That(template.Children.ElementAt(2).Value, Is.EqualTo(@"'>Test</li>"));
        }

        [Test]
        public void ParseNestedIf()
        {
            var tokens = new List<Token> {
                new Token(@"<li class='something", new LiteralTagDescriptor()),
                new Token(@"{{if Blah}}", new IfTagDescriptor()),
                new Token(@" blah", new LiteralTagDescriptor()),
                new Token(@"{{if Foo}}", new IfTagDescriptor()),
                new Token(@" foo", new LiteralTagDescriptor()),
                new Token(@"{{if Foobar}}", new IfTagDescriptor()),
                new Token(@" foobar", new LiteralTagDescriptor()),
                new Token(@"{{/if}}", new EndIfTagDescriptor()),
                new Token(@"{{/if}}", new EndIfTagDescriptor()),
                new Token(@" bar", new LiteralTagDescriptor()),
                new Token(@"{{/if}}", new EndIfTagDescriptor()),
                new Token(@"'>Test</li>", new LiteralTagDescriptor()),
            };

            var parser = new Parser();
            var template = parser.Parse(tokens);

            Assert.That(template.Children.Count(), Is.EqualTo(3));
            Assert.That(template.Children.ElementAt(0), Is.TypeOf<LiteralTemplate>());
            Assert.That(template.Children.ElementAt(0).Value, Is.EqualTo(@"<li class='something"));
            Assert.That(template.Children.ElementAt(1), Is.TypeOf<IfTemplate>());
            Assert.That(template.Children.ElementAt(1).Value, Is.EqualTo(@"{{if Blah}} blah{{if Foo}} foo{{if Foobar}} foobar{{/if}}{{/if}} bar{{/if}}"));
            Assert.That(template.Children.ElementAt(2), Is.TypeOf<LiteralTemplate>());
            Assert.That(template.Children.ElementAt(2).Value, Is.EqualTo(@"'>Test</li>"));
        }
    }
}