namespace jQueryTmpl.Templates
{
    public class Token
    {
        public int Index { get; set; }
        public int Length { get; set; }
        public string Value { get; set; }
        public bool Closing { get; set; }
        public string Name { get; set; }
        public string[] Parameters { get; set; }
        public string Expression { get; set; }
    }
}