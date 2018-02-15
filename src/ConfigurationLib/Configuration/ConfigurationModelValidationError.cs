using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evoq.Configuration
{
    /// <summary>
    /// Holds the result from validating a configuration model.
    /// </summary>
    public sealed class ConfigurationModelValidationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationModelValidationError"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="memberNames">The member names.</param>
        public ConfigurationModelValidationError(string errorMessage, IEnumerable<string> memberNames)
        {
            this.ErrorMessage = errorMessage;
            this.MemberNames = memberNames;
        }

        /// <summary>
        /// Gets the member names that have an error.
        /// </summary>
        public IEnumerable<string> MemberNames { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string ErrorMessage { get; }
    }
}
