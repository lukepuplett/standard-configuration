using System;
using System.Collections.Generic;
using System.Linq;

namespace Evoq.Configuration
{
    /// <summary>
    /// Base class for reading from a source of keyed-values into an object model.
    /// </summary>
    public abstract class ModelReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelReader"/> class.
        /// </summary>
        public ModelReader()
            : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelReader"/> class.
        /// </summary>
        /// <param name="modelValidator">The model validator.</param>
        public ModelReader(ConfigurationModelValidator modelValidator)
        {
            this.ModelValidator = modelValidator ?? new ConfigurationModelValidator();
        }

        //

        /// <summary>
        /// Gets the model validator used by the reader.
        /// </summary>
        /// <value>
        /// The model validator.
        /// </value>
        public ConfigurationModelValidator ModelValidator { get; }

        //

        /// <summary>
        /// Reads the current appSettings section into a new instance of the model class type specified.
        /// </summary>
        /// <typeparam name="T">The type of model DTO to read into.</typeparam>
        /// <returns>An instance of the type specified.</returns>
        public T ReadInto<T>() where T : new()
        {
            return (T)ReadInto(typeof(T));
        }

        /// <summary>
        /// Reads the current appSettings section into a new instance of the model class type specified.
        /// </summary>
        /// <typeparam name="T">The type of model DTO to read into.</typeparam>
        /// <param name="errors">When an errors collection is supplied, then throwing is suppressed and instead the errors collection is populated.</param>
        /// <returns>
        /// An instance of the type specified.
        /// </returns>
        public T ReadInto<T>(ICollection<ConfigurationModelValidationError> errors) where T : new()
        {
            return (T)ReadInto(typeof(T), errors);
        }

        /// <summary>
        /// Reads the current appSettings section into a new instance of the model class type specified.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>
        /// An instance of the type specified.
        /// </returns>
        /// <exception cref="ArgumentNullException">modelType</exception>
        /// <exception cref="Evoq.Configuration.InvalidConfigurationModelException"></exception>
        /// <exception cref="InvalidConfigurationModelException"></exception>
        public object ReadInto(Type modelType)
        {
            return this.ReadInto(modelType, null);
        }

        /// <summary>
        /// Reads the current appSettings section into a new instance of the model class type specified.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="errors">When an errors collection is supplied, then throwing is suppressed and instead the errors collection is populated.</param>
        /// <returns>
        /// An instance of the type specified.
        /// </returns>
        /// <exception cref="ArgumentNullException">modelType</exception>
        /// <exception cref="Evoq.Configuration.InvalidConfigurationModelException">
        /// </exception>
        /// <exception cref="InvalidConfigurationModelException"></exception>
        public object ReadInto(Type modelType, ICollection<ConfigurationModelValidationError> errors)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            bool throwOnValidationErrors = (errors == null); // If we were not supplied with a bucket in which to stick errors, we must throw.

            var modelMappings = ModelMappingDictionary.CreateFrom(modelType);

            if (modelMappings.Any())
            {
                object model;

                model = this.CreateModelInstanceOrThrow(modelType);

                foreach (var mapping in modelMappings.Values)
                {
                    try
                    {
                        // SetValue will throw given the wrong type for the model property.

                        object sourceValue = this.GetSourceValue(mapping);

                        mapping.PropertyInfo.SetValue(model, sourceValue);
                    }                    
                    catch (InvalidOperationException notValidException) when (notValidException.Message.Contains("not a valid"))
                    {
                        // Invalid type cast/convert.

                        if (throwOnValidationErrors)
                        {
                            throw new InvalidConfigurationModelException($"Cannot read the settings into {modelType.Name}.{mapping.ModelPropertyName}. {notValidException.Message}", notValidException);
                        }
                        else
                        {
                            errors.Add(new ConfigurationModelValidationError(notValidException.Message, new string[] { mapping.ModelPropertyName }));
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                        // Missing. We can swallow the problem here and allow later validation step to deal with it.                        
                    }
                    catch (InvalidOperationException)
                    {
                        // Missing. We can swallow the problem here and allow later validation step to deal with it.                        
                    }
                }

                IEnumerable<ConfigurationModelValidationError> validationErrors = null;
                bool isValid = this.ModelValidator.TryValidateModel(model, out validationErrors);
                if (!isValid)
                {
                    // Invalid.

                    if (throwOnValidationErrors)
                    {
                        string message;

                        if (validationErrors.Count() == 1)
                        {
                            message = $"Cannot read the settings into {modelType.Name}. {validationErrors.First().ErrorMessage}";
                        }
                        else
                        {
                            message = $"Cannot read the settings into {modelType.Name}. There are {validationErrors.Count()} validation errors.";
                        }

                        throw new InvalidConfigurationModelException(message)
                        {
                            ValidationErrors = validationErrors
                        };
                    }
                    else
                    {
                        foreach (var error in validationErrors)
                            errors.Add(error);
                    }
                }

                return model;
            }
            else
            {
                throw new InvalidConfigurationModelException($"Cannot read the appSettings section. The model class {modelType.Name} has no public properties.");
            }
        }

        /// <summary>
        /// Gets the value from the source of data.
        /// </summary>
        /// <param name="mapping">The mapping information used to lookup the source value.</param>
        /// <param name="sourceValue">The value at source.</param>
        /// <returns>True if the key existed.</returns>
        protected abstract object GetSourceValue(ModelMapping mapping);

        /// <summary>
        /// Creates a new instance of the object model type.
        /// </summary>
        protected object CreateModelInstanceOrThrow(Type modelType)
        {
            object model;
            try
            {
                model = Activator.CreateInstance(modelType);
            }
            catch (Exception ex)
            {
                throw new InvalidConfigurationModelException($"Cannot populate the object model because the model's type {modelType.Name} cannot be instantiated. See inner exception.", ex);
            }

            return model;
        }
    }
}
