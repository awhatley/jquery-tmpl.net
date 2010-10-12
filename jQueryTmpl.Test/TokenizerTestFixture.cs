using System.Linq;

using jQueryTmpl.Tags;
using jQueryTmpl.Tokenization;

using NUnit.Framework;

namespace jQueryTmpl.Test
{
    [TestFixture]
    public class TokenizerTestFixture
    {
        [Test]
        public void ParseNoTokenTemplate()
        {
            var templateString = @"<ul><li>Item $1</li><li>{Item 2</li><li>{}{}{}</li></ul>";
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(templateString);

            Assert.That(tokens.Count(), Is.EqualTo(1));
            Assert.That(tokens.ElementAt(0), Is.Not.Null);
            Assert.That(tokens.ElementAt(0).Value, Is.EqualTo(templateString));
            Assert.That(tokens.ElementAt(0).Descriptor, Is.TypeOf<LiteralTagDescriptor>());
        }

        [Test]
        public void ParseFalseTokenTemplate()
        {
            var templateString = @"<ul><li>Item $1</li><li>${Item 2</li><li>{}{}{}</li></ul>";
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(templateString);

            Assert.That(tokens.Count(), Is.EqualTo(1));
            Assert.That(tokens.ElementAt(0), Is.Not.Null);
            Assert.That(tokens.ElementAt(0).Value, Is.EqualTo(templateString));
            Assert.That(tokens.ElementAt(0).Descriptor, Is.TypeOf<LiteralTagDescriptor>());
        }

        [Test]
        public void ParseSingleToken()
        {
            var templateString = @"<li>${firstName}</li>";
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(templateString);

            Assert.That(tokens.Count(), Is.EqualTo(3));

            Assert.That(tokens.ElementAt(0), Is.Not.Null);
            Assert.That(tokens.ElementAt(0).Value, Is.EqualTo(@"<li>"));
            Assert.That(tokens.ElementAt(0).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(1), Is.Not.Null);
            Assert.That(tokens.ElementAt(1).Value, Is.EqualTo(@"${firstName}"));
            Assert.That(tokens.ElementAt(1).Descriptor, Is.TypeOf<PrintTagDescriptor>());

            Assert.That(tokens.ElementAt(2), Is.Not.Null);
            Assert.That(tokens.ElementAt(2).Value, Is.EqualTo(@"</li>"));
            Assert.That(tokens.ElementAt(2).Descriptor, Is.TypeOf<LiteralTagDescriptor>());
        }

        [Test]
        public void ParseLongPrintToken()
        {
            var templateString = @"<li><b>{{= Name}}</b> was released in {{= ReleaseYear}}.</li>";
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(templateString);

            Assert.That(tokens.Count(), Is.EqualTo(5));
        
            Assert.That(tokens.ElementAt(0), Is.Not.Null);
            Assert.That(tokens.ElementAt(0).Value, Is.EqualTo(@"<li><b>"));
            Assert.That(tokens.ElementAt(0).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(1), Is.Not.Null);
            Assert.That(tokens.ElementAt(1).Value, Is.EqualTo(@"{{= Name}}"));
            Assert.That(tokens.ElementAt(1).Descriptor, Is.TypeOf<PrintTagDescriptor>());

            Assert.That(tokens.ElementAt(2), Is.Not.Null);
            Assert.That(tokens.ElementAt(2).Value, Is.EqualTo(@"</b> was released in "));
            Assert.That(tokens.ElementAt(2).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(3), Is.Not.Null);
            Assert.That(tokens.ElementAt(3).Value, Is.EqualTo(@"{{= ReleaseYear}}"));
            Assert.That(tokens.ElementAt(3).Descriptor, Is.TypeOf<PrintTagDescriptor>());

            Assert.That(tokens.ElementAt(4), Is.Not.Null);
            Assert.That(tokens.ElementAt(4).Value, Is.EqualTo(@".</li>"));
            Assert.That(tokens.ElementAt(4).Descriptor, Is.TypeOf<LiteralTagDescriptor>());
        }

        [Test]
        public void ParseMultipleTokens()
        {
            var templateString = @"<li>${firstName} ${lastName} ${suffix}</li>";
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(templateString);

            Assert.That(tokens.Count(), Is.EqualTo(7));

            Assert.That(tokens.ElementAt(0), Is.Not.Null);
            Assert.That(tokens.ElementAt(0).Value, Is.EqualTo(@"<li>"));
            Assert.That(tokens.ElementAt(0).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(1), Is.Not.Null);
            Assert.That(tokens.ElementAt(1).Value, Is.EqualTo(@"${firstName}"));
            Assert.That(tokens.ElementAt(1).Descriptor, Is.TypeOf<PrintTagDescriptor>());

            Assert.That(tokens.ElementAt(2), Is.Not.Null);
            Assert.That(tokens.ElementAt(2).Value, Is.EqualTo(@" "));
            Assert.That(tokens.ElementAt(2).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(3), Is.Not.Null);
            Assert.That(tokens.ElementAt(3).Value, Is.EqualTo(@"${lastName}"));
            Assert.That(tokens.ElementAt(3).Descriptor, Is.TypeOf<PrintTagDescriptor>());

            Assert.That(tokens.ElementAt(4), Is.Not.Null);
            Assert.That(tokens.ElementAt(4).Value, Is.EqualTo(@" "));
            Assert.That(tokens.ElementAt(4).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(5), Is.Not.Null);
            Assert.That(tokens.ElementAt(5).Value, Is.EqualTo(@"${suffix}"));
            Assert.That(tokens.ElementAt(5).Descriptor, Is.TypeOf<PrintTagDescriptor>());

            Assert.That(tokens.ElementAt(6), Is.Not.Null);
            Assert.That(tokens.ElementAt(6).Value, Is.EqualTo(@"</li>"));
            Assert.That(tokens.ElementAt(6).Descriptor, Is.TypeOf<LiteralTagDescriptor>());
        }

        [Test]
        public void ParseSimpleIf()
        {
            var templateString = @"<li class='something{{if Blah}} blah{{/if}}'>Test</li>";
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(templateString);

            Assert.That(tokens.Count(), Is.EqualTo(5));

            Assert.That(tokens.ElementAt(0), Is.Not.Null);
            Assert.That(tokens.ElementAt(0).Value, Is.EqualTo(@"<li class='something"));
            Assert.That(tokens.ElementAt(0).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(1), Is.Not.Null);
            Assert.That(tokens.ElementAt(1).Value, Is.EqualTo(@"{{if Blah}}"));
            Assert.That(tokens.ElementAt(1).Descriptor, Is.TypeOf<IfTagDescriptor>());

            Assert.That(tokens.ElementAt(2), Is.Not.Null);
            Assert.That(tokens.ElementAt(2).Value, Is.EqualTo(@" blah"));
            Assert.That(tokens.ElementAt(2).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(3), Is.Not.Null);
            Assert.That(tokens.ElementAt(3).Value, Is.EqualTo(@"{{/if}}"));
            Assert.That(tokens.ElementAt(3).Descriptor, Is.TypeOf<EndIfTagDescriptor>());

            Assert.That(tokens.ElementAt(4), Is.Not.Null);
            Assert.That(tokens.ElementAt(4).Value, Is.EqualTo(@"'>Test</li>"));
            Assert.That(tokens.ElementAt(4).Descriptor, Is.TypeOf<LiteralTagDescriptor>());
        }

        [Test]
        public void ParseNestedIf()
        {
            var templateString = @"<li class='something{{if Blah}} blah{{if Foo}} foo{{/if}} bar{{/if}}'>Test</li>";
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(templateString);

            Assert.That(tokens.Count(), Is.EqualTo(9));

            Assert.That(tokens.ElementAt(0), Is.Not.Null);
            Assert.That(tokens.ElementAt(0).Value, Is.EqualTo(@"<li class='something"));
            Assert.That(tokens.ElementAt(0).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(1), Is.Not.Null);
            Assert.That(tokens.ElementAt(1).Value, Is.EqualTo(@"{{if Blah}}"));
            Assert.That(tokens.ElementAt(1).Descriptor, Is.TypeOf<IfTagDescriptor>());

            Assert.That(tokens.ElementAt(2), Is.Not.Null);
            Assert.That(tokens.ElementAt(2).Value, Is.EqualTo(@" blah"));
            Assert.That(tokens.ElementAt(2).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(3), Is.Not.Null);
            Assert.That(tokens.ElementAt(3).Value, Is.EqualTo(@"{{if Foo}}"));
            Assert.That(tokens.ElementAt(3).Descriptor, Is.TypeOf<IfTagDescriptor>());

            Assert.That(tokens.ElementAt(4), Is.Not.Null);
            Assert.That(tokens.ElementAt(4).Value, Is.EqualTo(@" foo"));
            Assert.That(tokens.ElementAt(4).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(5), Is.Not.Null);
            Assert.That(tokens.ElementAt(5).Value, Is.EqualTo(@"{{/if}}"));
            Assert.That(tokens.ElementAt(5).Descriptor, Is.TypeOf<EndIfTagDescriptor>());

            Assert.That(tokens.ElementAt(6), Is.Not.Null);
            Assert.That(tokens.ElementAt(6).Value, Is.EqualTo(@" bar"));
            Assert.That(tokens.ElementAt(6).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(7), Is.Not.Null);
            Assert.That(tokens.ElementAt(7).Value, Is.EqualTo(@"{{/if}}"));
            Assert.That(tokens.ElementAt(7).Descriptor, Is.TypeOf<EndIfTagDescriptor>());

            Assert.That(tokens.ElementAt(8), Is.Not.Null);
            Assert.That(tokens.ElementAt(8).Value, Is.EqualTo(@"'>Test</li>"));
            Assert.That(tokens.ElementAt(8).Descriptor, Is.TypeOf<LiteralTagDescriptor>());
        }

        [Test]
        public void ParseNestedIfElse()
        {
            var templateString = @"<li class='something{{if Blah}} blah{{else Foo}} foo{{if Foobar}} foobar{{/if}}{{else}} bar{{/if}}'>Test</li>";
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(templateString);

            Assert.That(tokens.Count(), Is.EqualTo(12));

            Assert.That(tokens.ElementAt(0), Is.Not.Null);
            Assert.That(tokens.ElementAt(0).Value, Is.EqualTo(@"<li class='something"));
            Assert.That(tokens.ElementAt(0).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(1), Is.Not.Null);
            Assert.That(tokens.ElementAt(1).Value, Is.EqualTo(@"{{if Blah}}"));
            Assert.That(tokens.ElementAt(1).Descriptor, Is.TypeOf<IfTagDescriptor>());

            Assert.That(tokens.ElementAt(2), Is.Not.Null);
            Assert.That(tokens.ElementAt(2).Value, Is.EqualTo(@" blah"));
            Assert.That(tokens.ElementAt(2).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(3), Is.Not.Null);
            Assert.That(tokens.ElementAt(3).Value, Is.EqualTo(@"{{else Foo}}"));
            Assert.That(tokens.ElementAt(3).Descriptor, Is.TypeOf<ElseTagDescriptor>());

            Assert.That(tokens.ElementAt(4), Is.Not.Null);
            Assert.That(tokens.ElementAt(4).Value, Is.EqualTo(@" foo"));
            Assert.That(tokens.ElementAt(4).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(5), Is.Not.Null);
            Assert.That(tokens.ElementAt(5).Value, Is.EqualTo(@"{{if Foobar}}"));
            Assert.That(tokens.ElementAt(5).Descriptor, Is.TypeOf<IfTagDescriptor>());

            Assert.That(tokens.ElementAt(6), Is.Not.Null);
            Assert.That(tokens.ElementAt(6).Value, Is.EqualTo(@" foobar"));
            Assert.That(tokens.ElementAt(6).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(7), Is.Not.Null);
            Assert.That(tokens.ElementAt(7).Value, Is.EqualTo(@"{{/if}}"));
            Assert.That(tokens.ElementAt(7).Descriptor, Is.TypeOf<EndIfTagDescriptor>());

            Assert.That(tokens.ElementAt(8), Is.Not.Null);
            Assert.That(tokens.ElementAt(8).Value, Is.EqualTo(@"{{else}}"));
            Assert.That(tokens.ElementAt(8).Descriptor, Is.TypeOf<ElseTagDescriptor>());

            Assert.That(tokens.ElementAt(9), Is.Not.Null);
            Assert.That(tokens.ElementAt(9).Value, Is.EqualTo(@" bar"));
            Assert.That(tokens.ElementAt(9).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(10), Is.Not.Null);
            Assert.That(tokens.ElementAt(10).Value, Is.EqualTo(@"{{/if}}"));
            Assert.That(tokens.ElementAt(10).Descriptor, Is.TypeOf<EndIfTagDescriptor>());

            Assert.That(tokens.ElementAt(11), Is.Not.Null);
            Assert.That(tokens.ElementAt(11).Value, Is.EqualTo(@"'>Test</li>"));
            Assert.That(tokens.ElementAt(11).Descriptor, Is.TypeOf<LiteralTagDescriptor>());
        }
    
        [Test]
        public void ParseIfElseEach()
        {
            var templateString = @"{{each Thing}}<li class='something{{if Blah}} blah{{else Foo}} foo{{each Foobar}} <span>${value}</span>{{/each}}{{/if}}{{else}} bar{{/if}}'>Test</li>{{/each}}";
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(templateString);
     
            Assert.That(tokens.Count(), Is.EqualTo(17));

            Assert.That(tokens.ElementAt(0), Is.Not.Null);
            Assert.That(tokens.ElementAt(0).Value, Is.EqualTo(@"{{each Thing}}"));
            Assert.That(tokens.ElementAt(0).Descriptor, Is.TypeOf<EachTagDescriptor>());

            Assert.That(tokens.ElementAt(1), Is.Not.Null);
            Assert.That(tokens.ElementAt(1).Value, Is.EqualTo(@"<li class='something"));
            Assert.That(tokens.ElementAt(1).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(2), Is.Not.Null);
            Assert.That(tokens.ElementAt(2).Value, Is.EqualTo(@"{{if Blah}}"));
            Assert.That(tokens.ElementAt(2).Descriptor, Is.TypeOf<IfTagDescriptor>());

            Assert.That(tokens.ElementAt(3), Is.Not.Null);
            Assert.That(tokens.ElementAt(3).Value, Is.EqualTo(@" blah"));
            Assert.That(tokens.ElementAt(3).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(4), Is.Not.Null);
            Assert.That(tokens.ElementAt(4).Value, Is.EqualTo(@"{{else Foo}}"));
            Assert.That(tokens.ElementAt(4).Descriptor, Is.TypeOf<ElseTagDescriptor>());

            Assert.That(tokens.ElementAt(5), Is.Not.Null);
            Assert.That(tokens.ElementAt(5).Value, Is.EqualTo(@" foo"));
            Assert.That(tokens.ElementAt(5).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(6), Is.Not.Null);
            Assert.That(tokens.ElementAt(6).Value, Is.EqualTo(@"{{each Foobar}}"));
            Assert.That(tokens.ElementAt(6).Descriptor, Is.TypeOf<EachTagDescriptor>());

            Assert.That(tokens.ElementAt(7), Is.Not.Null);
            Assert.That(tokens.ElementAt(7).Value, Is.EqualTo(@" <span>"));
            Assert.That(tokens.ElementAt(7).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(8), Is.Not.Null);
            Assert.That(tokens.ElementAt(8).Value, Is.EqualTo(@"${value}"));
            Assert.That(tokens.ElementAt(8).Descriptor, Is.TypeOf<PrintTagDescriptor>());

            Assert.That(tokens.ElementAt(9), Is.Not.Null);
            Assert.That(tokens.ElementAt(9).Value, Is.EqualTo(@"</span>"));
            Assert.That(tokens.ElementAt(9).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(10), Is.Not.Null);
            Assert.That(tokens.ElementAt(10).Value, Is.EqualTo(@"{{/each}}"));
            Assert.That(tokens.ElementAt(10).Descriptor, Is.TypeOf<EndEachTagDescriptor>());

            Assert.That(tokens.ElementAt(11), Is.Not.Null);
            Assert.That(tokens.ElementAt(11).Value, Is.EqualTo(@"{{/if}}"));
            Assert.That(tokens.ElementAt(11).Descriptor, Is.TypeOf<EndIfTagDescriptor>());

            Assert.That(tokens.ElementAt(12), Is.Not.Null);
            Assert.That(tokens.ElementAt(12).Value, Is.EqualTo(@"{{else}}"));
            Assert.That(tokens.ElementAt(12).Descriptor, Is.TypeOf<ElseTagDescriptor>());

            Assert.That(tokens.ElementAt(13), Is.Not.Null);
            Assert.That(tokens.ElementAt(13).Value, Is.EqualTo(@" bar"));
            Assert.That(tokens.ElementAt(13).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(14), Is.Not.Null);
            Assert.That(tokens.ElementAt(14).Value, Is.EqualTo(@"{{/if}}"));
            Assert.That(tokens.ElementAt(14).Descriptor, Is.TypeOf<EndIfTagDescriptor>());

            Assert.That(tokens.ElementAt(15), Is.Not.Null);
            Assert.That(tokens.ElementAt(15).Value, Is.EqualTo(@"'>Test</li>"));
            Assert.That(tokens.ElementAt(15).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(16), Is.Not.Null);
            Assert.That(tokens.ElementAt(16).Value, Is.EqualTo(@"{{/each}}"));
            Assert.That(tokens.ElementAt(16).Descriptor, Is.TypeOf<EndEachTagDescriptor>());
        }

        [Test]
        public void ParseHtml()
        {
            var templateString = @"<h4>${Name}</h4><p>{{html Synopsis}}</p>";
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(templateString);

            Assert.That(tokens.Count(), Is.EqualTo(5));
        
            Assert.That(tokens.ElementAt(0), Is.Not.Null);
            Assert.That(tokens.ElementAt(0).Value, Is.EqualTo(@"<h4>"));
            Assert.That(tokens.ElementAt(0).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(1), Is.Not.Null);
            Assert.That(tokens.ElementAt(1).Value, Is.EqualTo(@"${Name}"));
            Assert.That(tokens.ElementAt(1).Descriptor, Is.TypeOf<PrintTagDescriptor>());

            Assert.That(tokens.ElementAt(2), Is.Not.Null);
            Assert.That(tokens.ElementAt(2).Value, Is.EqualTo(@"</h4><p>"));
            Assert.That(tokens.ElementAt(2).Descriptor, Is.TypeOf<LiteralTagDescriptor>());

            Assert.That(tokens.ElementAt(3), Is.Not.Null);
            Assert.That(tokens.ElementAt(3).Value, Is.EqualTo(@"{{html Synopsis}}"));
            Assert.That(tokens.ElementAt(3).Descriptor, Is.TypeOf<HtmlTagDescriptor>());

            Assert.That(tokens.ElementAt(4), Is.Not.Null);
            Assert.That(tokens.ElementAt(4).Value, Is.EqualTo(@"</p>"));
            Assert.That(tokens.ElementAt(4).Descriptor, Is.TypeOf<LiteralTagDescriptor>());
        }
    }
}