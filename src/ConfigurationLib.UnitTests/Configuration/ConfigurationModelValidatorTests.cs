using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evoq.Configuration
{
    [TestClass]
    public class ConfigurationModelValidatorTests
    {
        class MyModel
        {
            [StringLength(5)]
            public string Max5Characters { get; set; }
        }

        [TestMethod]
        public void ConfigurationModelValidator_TryValidateObject__when__no_errors__then__returns_false()
        {
            ConfigurationModelValidator v = new ConfigurationModelValidator();

            IEnumerable<ConfigurationModelValidationError> errors;
            bool isValid = v.TryValidateModel(
                new MyModel()
                {
                    Max5Characters = "1234"

                }, out errors);

            Assert.IsTrue(isValid);
            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Count());
        }


        [TestMethod]
        public void ConfigurationModelValidator_TryValidateObject__when__single_error__then__returns_false_and_well_formed_error()
        {
            ConfigurationModelValidator v = new ConfigurationModelValidator();

            IEnumerable<ConfigurationModelValidationError> errors;
            bool isValid = v.TryValidateModel(
                new MyModel()
                {
                    Max5Characters = "123456789"

                }, out errors);

            Assert.IsFalse(isValid); // !
            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("The field Max5Characters must be a string with a maximum length of 5.", errors.First().ErrorMessage);
            Assert.AreEqual(nameof(MyModel.Max5Characters), errors.First().MemberNames.First());
        }
    }
}
