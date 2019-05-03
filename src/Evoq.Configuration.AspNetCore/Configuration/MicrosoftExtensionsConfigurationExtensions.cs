using Evoq.Configuration;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration
{
    public static class MicrosoftExtensionsConfigurationExtensions
    {
        /// <summary>
        /// Performs as per Get but also validates
        /// </summary>
        /// <typeparam name="T">The type to bind configuration settings to.</typeparam>        
        /// <param name="configureOptions">Optional action for configuring options.</param>
        /// <returns>True if the bound model is valid.</returns>
        public static bool GetAndValidate<T>(this IConfiguration configuration, out IEnumerable<ConfigurationModelValidationError> errors, Action<BinderOptions> configureOptions = null)
        {
            T model;
            if (configureOptions == null)
            {
                model = configuration.Get<T>();
            }
            else
            {
                model = configuration.Get<T>(configureOptions);
            }

            var validator = new ConfigurationModelValidator();
            return validator.TryValidateModel(model, out errors);
        }
    }
}
