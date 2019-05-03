using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evoq.Configuration
{
    [TestClass]
    public class MicrosoftExtensionsConfigurationExtensions
    {
        [TestMethod]
        public void IConfiguration_GetAndValidate__when__required_and_supplied__then__true_no_errors()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>()
            {
                { "RequiredString", "This value is needed." }
            });
                        
            bool isValid = builder.Build().GetAndValidate(out DummyAppSettingsModel model, out IEnumerable<ConfigurationModelValidationError> errors);

            Assert.IsTrue(isValid);
            Assert.IsFalse(errors.Any());
        }

        [TestMethod]
        public void IConfiguration_GetAndValidate__when__required_and_empty__then__false_with_errors()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>()
            {
                { "RequiredString", "" }
            });
                        
            bool isValid = builder.Build().GetAndValidate(out DummyAppSettingsModel model, out IEnumerable<ConfigurationModelValidationError> errors);

            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Any());
            Assert.AreEqual("The RequiredString field is required.", errors.First().ErrorMessage);
            Assert.AreEqual("RequiredString", errors.Single().MemberNames.Single());
        }

        [TestMethod]
        public void IConfiguration_GetAndValidate__when__required_number_is_null__then__false_with_errors()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>()
            {
                { "RequiredString", "I am required." },
                { "RequiredNumber", "String!" }
            });

            bool isValid = builder.Build().GetAndValidate(out DummyAppSettingsModel model, out IEnumerable<ConfigurationModelValidationError> errors);

            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Any());
            Assert.AreEqual("Input string was not in a correct format.", errors.First().ErrorMessage);            

        }

        [TestMethod]
        public void IConfiguration_GetAndValidate__when__required_and_missing__then__false_with_errors()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>()
            {
                
            });
                        
            bool isValid = builder.Build().GetAndValidate(out DummyAppSettingsModel model, out IEnumerable<ConfigurationModelValidationError> errors);

            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Any());
            Assert.AreEqual("The configuration was not found.", errors.First().ErrorMessage);
        }

        [TestMethod]
        public void IConfiguration_GetAndValidate__when__max_length_and_long_string__then__false_with_errors()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>()
            {
                { "RequiredString", "This value is needed." },
                { "ShortString", "This value is too long." }
            });
                        
            bool isValid = builder.Build().GetAndValidate(out  DummyAppSettingsModel model, out IEnumerable<ConfigurationModelValidationError> errors);

            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Any());
            Assert.AreEqual("The field ShortString must be a string or array type with a maximum length of '4'.", errors.First().ErrorMessage);
            Assert.AreEqual("ShortString", errors.Single().MemberNames.Single());
        }
    }
}
