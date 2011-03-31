using System;

using jQueryTmpl.Templates;

using NUnit.Framework;

namespace jQueryTmpl.Test
{
    [TestFixture]
    public class ExpressionParserTestFixture
    {
        [Test]
        public void ParseSimpleBinaryExpression()
        {
            TestParse("$blah == $foo");
            TestParse("$blah != $foo");
        }
        
        [Test]
        public void ParseComplexLeftSideBinaryExpression()
        {
            TestParse("$blah.array['bar'] <= $foo");
        }
        
        [Test]
        public void ParseComplexRightSideBinaryExpression()
        {
            TestParse("$blah >= $item.html('*', $foo[0])");
        }
        
        [Test]
        public void ParseNestedBinaryExpression()
        {
            TestParse("$blah < $item.html('*', $foo[0] > 'bar')");
        }
        
        [Test]
        public void ParseComplexExpression()
        {
            TestParse("$item.html('*', $data.array['foo'])[0,5].blah");
        }

        private static void TestParse(string text)
        {
            var expression = new ExpressionParser().Parse(text);

            Console.WriteLine(text);
            Console.WriteLine(expression);

            Assert.That(expression.ToString(), Is.EqualTo(text));
        }
    }
}