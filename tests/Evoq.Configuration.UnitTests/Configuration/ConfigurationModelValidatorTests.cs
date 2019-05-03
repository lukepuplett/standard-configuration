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

            [MaxLength(1)]
            public string SingleLetter { get; set; }

            [ValidateObject]
            public AnotherModel AnotherModel { get; set; }
        }

        class AnotherModel
        {
            [MaxLength(1)]
            public string SingleLetter { get; set; }
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

        [TestMethod]
        public void ConfigurationModelValidator_TryValidateObject__when__another_single_error__then__returns_false_and_well_formed_error()
        {
            ConfigurationModelValidator v = new ConfigurationModelValidator();

            IEnumerable<ConfigurationModelValidationError> errors;
            bool isValid = v.TryValidateModel(
                new MyModel()
                {
                    Max5Characters = "12345",
                    SingleLetter = "Oops"

                }, out errors);

            Assert.IsFalse(isValid); // !
            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("The field SingleLetter must be a string or array type with a maximum length of '1'.", errors.First().ErrorMessage);
            Assert.AreEqual(nameof(MyModel.SingleLetter), errors.First().MemberNames.First());
        }

        [TestMethod]
        public void ConfigurationModelValidator_TryValidateObject__when__a_nested_object_is_invalid__then__returns_false_and_well_formed_error()
        {
            ConfigurationModelValidator v = new ConfigurationModelValidator();

            IEnumerable<ConfigurationModelValidationError> errors;
            bool isValid = v.TryValidateModel(
                new MyModel()
                {
                    Max5Characters = "12345",
                    SingleLetter = "a",
                    AnotherModel = new AnotherModel() { SingleLetter = "Oops" }

                }, out errors);

            Assert.IsFalse(isValid); // !
            Assert.IsNotNull(errors);
            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("The field AnotherModel references an object that failed validation.", errors.First().ErrorMessage);
            Assert.AreEqual(nameof(MyModel.AnotherModel), errors.First().MemberNames.First());
        }
    }
}
