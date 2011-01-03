using System;

using jQueryTmpl.Templates;

using NUnit.Framework;

namespace jQueryTmpl.Test
{
    [TestFixture]
    public class ExpressionParserTestFixture
    {
        [Test]
        public void TestParser()
        {
            const string text = "$item.html('*', $data.array['foo'])[0,5].blah";
            var expression = new ExpressionParser().Parse(text);

            Console.WriteLine(text);
            Console.WriteLine(expression);

            Assert.That(expression.ToString(), Is.EqualTo(text));
        }
    }
}