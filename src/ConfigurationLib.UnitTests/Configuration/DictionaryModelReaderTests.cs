using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evoq.Configuration
{
    [TestClass]
    public class DictionaryModelReaderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DictionaryModelReader__ctor__when__empty_dictionary__then__ArgumentException()
        {
            var modelReader = new DictionaryModelReader(new Dictionary<string, object>() { });

            Assert.IsNotNull(modelReader);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidConfigurationModelException))]
        public void DictionaryModelReader__ctor__when__empty_dictionary_and_Needed_field__then__InvalidConfigurationModelException()
        {
            var modelReader = new DictionaryModelReader(new Dictionary<string, object>()
            {
                { "NotExistingInModel", "Yellow" }
            });

            Assert.IsNotNull(modelReader);

            var model = modelReader.ReadInto<DummyAppSettingsModelWithRequiredAndOptional>();
        }

        [TestMethod]
        public void DictionaryModelReader__ctor__when__good_dictionary_and_Needed_field__then__model_filled()
        {
            var modelReader = new DictionaryModelReader(new Dictionary<string, object>()
            {
                { "needed", "Yellow" }
            });

            Assert.IsNotNull(modelReader);

            var model = modelReader.ReadInto<DummyAppSettingsModelWithRequiredAndOptional>();

            Assert.AreEqual("Yellow", model.Needed);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidConfigurationModelException))]
        public void DictionaryModelReader__ctor__when__good_dictionary_and_Needed_field_with_case_sensitive__then__InvalidConfigurationModelException()
        {
            var modelReader = new DictionaryModelReader(new Dictionary<string, object>()
            {
                { "needed", "Yellow" }
            });

            modelReader.KeyComparer = StringComparison.CurrentCulture;

            Assert.IsNotNull(modelReader);

            var model = modelReader.ReadInto<DummyAppSettingsModelWithRequiredAndOptional>();
        }
    }
}
