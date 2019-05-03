using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

//
// Note: When DataAnnotations are officially added to .NET Standard, remove the reference to the package.
//
// https://github.com/dotnet/standard/issues/450
//

namespace Evoq.Configuration
{
    /// <summary>
    /// Validates configuration models using DataAnnotations attributes.
    /// </summary>
    public class ConfigurationModelValidator
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationModelValidator"/> class.
        /// </summary>
        /// <param name="serviceProvider">Optional service provider.</param>
        public ConfigurationModelValidator(IServiceProvider serviceProvider = null)
        {
            this.ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// The service provider which is flowed to validation attributes.
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Tries to validate the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="errors">The errors.</param>        
        /// <returns>True if there are no errors.</returns>
        /// <exception cref="ArgumentNullException">model</exception>
        public virtual bool TryValidateModel(object model, out IEnumerable<ConfigurationModelValidationError> errors)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }


            var validationContext = new ValidationContext(model, serviceProvider: this.ServiceProvider, items: null);
            var results = new List<ValidationResult>();

            if (Validator.TryValidateObject(model, validationContext, results, true))
            {
                // Is valid!

                errors = new ConfigurationModelValidationError[] { }; // Empty is better than null.
                return true;
            }
            else
            {
                errors = results.Select(r => ConvertToError(r)).ToArray();
                return false;
            }
        }

        private static ConfigurationModelValidationError ConvertToError(ValidationResult r)
        {
            return new ConfigurationModelValidationError(r.ErrorMessage, r.MemberNames);
        }
    }
}
