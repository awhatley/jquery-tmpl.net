namespace jQueryTmpl.Templates
{
    public class BinaryEvaluator
    {
        public object Evaluate(object a, object b, string operation)
        {
            switch(operation.Trim())
            {
                case "==":
                    return Equals(a, b);

                case "!=":
                    return !Equals(a, b);

                case ">":
                    return GreaterThan(a, b);

                case ">=":
                    return GreaterThanEqual(a, b);

                case "<":
                    return LessThan(a, b);

                case "<=":
                    return LessThanEqual(a, b);
            }

            return null;
        }

        private object GreaterThan(dynamic a, dynamic b)
        {
            return a > b;
        }

        private object GreaterThanEqual(dynamic a, dynamic b)
        {
            return a >= b;
        }

        private object LessThan(dynamic a, dynamic b)
        {
            return a < b;
        }

        private object LessThanEqual(dynamic a, dynamic b)
        {
            return a <= b;
        }
    }
}