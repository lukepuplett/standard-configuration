using Evoq.Configuration;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Contains extensions to types in Microsoft.Extensions.Configuration.
    /// </summary>
    public static class MicrosoftExtensionsConfigurationExtensions
    {
        /// <summary>
        /// Performs as per Get but also validates
        /// </summary>
        /// <typeparam name="T">The type to bind configuration settings to.</typeparam>
        /// <param name="configuration"></param>
        /// <param name="model">May contain the model.</param>
        /// <param name="errors">May contain validation errors.</param>        
        /// <param name="configureOptions">Optional action for configuring options.</param>
        /// <returns>True if the bound model is valid.</returns>
        public static bool GetAndValidate<T>(this IConfiguration configuration, out T model, out IEnumerable<ConfigurationModelValidationError> errors, Action<BinderOptions> configureOptions = null)
        {
            try
            {
                if (configureOptions == null)
                {
                    model = configuration.Get<T>();
                }
                else
                {
                    model = configuration.Get<T>(configureOptions);
                }

                if (model == null)
                {
                    var error = new ConfigurationModelValidationError("The configuration was not found.", new string[0]);
                    errors = new[] { error };
                    return false;
                }
                else
                {
                    var validator = new ConfigurationModelValidator();
                    return validator.TryValidateModel(model, out errors);
                }
            }
            catch (InvalidOperationException invalidOperation) when (invalidOperation.GetBaseException() is FormatException formatException)
            {
                var error = new ConfigurationModelValidationError(formatException.Message, new string[0]);
                errors = new[] { error };
                model = default(T);
                return false;
            }
        }
    }
}
