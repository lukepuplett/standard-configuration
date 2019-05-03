namespace Evoq.Configuration
{
    public sealed class ModelMapping
    {
        public bool IsRequired { get; set; }
        public string SourceKeyName { get; set; }
        public string ModelPropertyName { get; set; }
        public System.Reflection.PropertyInfo PropertyInfo { get; set; }
    }
}
