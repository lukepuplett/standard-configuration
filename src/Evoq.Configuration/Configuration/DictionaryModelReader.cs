using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evoq.Configuration
{
    /// <summary>
    /// Reads a collection of key-value pairs into an object model.
    /// </summary>
    public sealed class DictionaryModelReader : ModelReader
    {
        /// <summary>
        /// Creates a new <see cref="DictionaryModelReader"/>.
        /// </summary>
        /// <param name="sourceValues">A dictionary or collection of keyed-values.</param>
        public DictionaryModelReader(IEnumerable<KeyValuePair<string, object>> sourceValues)
            : this(sourceValues, null) { }

        /// <summary>
        /// Creates a new <see cref="DictionaryModelReader"/>.
        /// </summary>
        /// <param name="sourceValues">A dictionary or collection of keyed-values.</param>
        /// <param name="modelValidator">The model validator.</param>
        public DictionaryModelReader(IEnumerable<KeyValuePair<string, object>> sourceValues, ConfigurationModelValidator modelValidator)
            : base(modelValidator)
        {
            this.SourceValues = sourceValues ?? throw new ArgumentNullException(nameof(sourceValues));

            if (!this.SourceValues.Any())
            {
                throw new ArgumentException("The collection of source values is empty.", nameof(sourceValues));
            }

            this.SourceValuesAsDictionary = ConvertToDictionary(this.SourceValues);
        }

        //

        /// <summary>
        /// Gets or sets the comparer to use for matching keys to model properties.
        /// </summary>
        public StringComparison KeyComparer { get; set; } = StringComparison.CurrentCultureIgnoreCase;

        /// <summary>
        /// Gets or sets the source of key-value pairs.
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> SourceValues { get; }

        private Dictionary<string, object> SourceValuesAsDictionary { get; }

        //

        protected override object GetSourceValue(ModelMapping mapping)
        {
            return this.SourceValuesAsDictionary.First(pair => pair.Key.Equals(mapping.SourceKeyName, this.KeyComparer)).Value;
        }

        private static Dictionary<string, object> ConvertToDictionary(IEnumerable<KeyValuePair<string, object>> sourceValues)
        {
            Dictionary<string, object> dictionary = sourceValues as Dictionary<string, object>;

            if (dictionary != null)
            {
                return dictionary;
            }
            else
            {
                return sourceValues.ToDictionary(pair => pair.Key, pair => pair.Value);
            }
        }
    }
}
