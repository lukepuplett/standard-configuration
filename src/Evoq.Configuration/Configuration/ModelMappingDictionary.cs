using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Evoq.Configuration
{
    /// <summary>
    /// Contains <see cref="ModelMapping"/> instances for a particular object model type.
    /// </summary>
    internal class ModelMappingDictionary : Dictionary<string, ModelMapping>
    {
        private ModelMappingDictionary(Type objectModelType)
            : base()
        {
            this.ObjectModelType = objectModelType;
        }

        //

        public Type ObjectModelType { get; }

        //

        /// <summary>
        /// Creates a new <see cref="ModelMappingDictionary"/> for an object model type.
        /// </summary>
        /// <param name="modelType">The type of object model to read property mappings from.</param>
        /// <returns>A new, filled instance.</returns>
        public static ModelMappingDictionary CreateFrom(Type modelType)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            var modelMappingDictionary = new ModelMappingDictionary(modelType);

            foreach (var mapping in GetModelMappingsForPropertiesOfType(modelType))
            {
                modelMappingDictionary.Add(mapping.SourceKeyName, mapping);
            }

            return modelMappingDictionary;
        }

        private static IEnumerable<ModelMapping> GetModelMappingsForPropertiesOfType(Type modelType)
        {
            // Option to use DataAnnotationAttribute to read AppSettings name.

            return modelType
                .GetProperties()
                .Select(property => CreateModelMappingFrom(property))
                .ToArray();
        }

        private static ModelMapping CreateModelMappingFrom(System.Reflection.PropertyInfo propertyInfo)
        {
            var displayAttribute = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;

            return new ModelMapping()
            {
                IsRequired = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), true).Any(),
                PropertyInfo = propertyInfo,
                ModelPropertyName = propertyInfo.Name,
                SourceKeyName = displayAttribute?.Name ?? propertyInfo.Name,
            };
        }
    }
}
