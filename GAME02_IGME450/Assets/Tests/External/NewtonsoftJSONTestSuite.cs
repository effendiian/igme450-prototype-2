using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Tests
{
           
    /// <summary>
    /// TestJSON object.
    /// </summary>
    public class TestJSON
    {
        public string Value;
    }

    /// <summary>
    /// TestSuite for testing external library added to Unity.
    /// </summary>
    public class NewtonsoftJSONTestSuite
    {

        /// <summary>
        /// Success message.
        /// </summary>
        private const string MSG_SUCCESS = "SUCCESS";

        /// <summary>
        /// Failure message.
        /// </summary>
        private const string MSG_FAILURE = "FAILURE";

        /// <summary>
        /// Success JSON object.
        /// </summary>
        public TestJSON json_success;

        /// <summary>
        /// Failure JSON object.
        /// </summary>
        public TestJSON json_failure;

        /// <summary>
        /// Setup before each test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            json_success = new TestJSON() { Value = MSG_SUCCESS };
            json_failure = new TestJSON() { Value = MSG_FAILURE };
        }

        /// <summary>
        /// TearDown after each test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            json_success.Value = "";
            json_success = null;

            json_failure.Value = "";
            json_failure = null;
        }

        /// <summary>
        /// JSON Serialization test.
        /// </summary>
        [Test]
        public void JSONSerialization()
        {
            TestJSON json_test = new TestJSON();
            json_test.Value = MSG_SUCCESS;

            string json_actual = JsonConvert.SerializeObject(json_test);
            string json_expected = JsonConvert.SerializeObject(json_success);
            Assert.AreEqual(json_expected, json_actual);
        }

        /// <summary>
        /// JSON Deserialization test.
        /// </summary>
        [Test]
        public void JSONDeserialization()
        {
            // Construct a JSON success object.
            string json = @"{
    'Value': '" + MSG_SUCCESS + @"'
}";

            // Deserialize the test object.
            TestJSON json_test = JsonConvert.DeserializeObject<TestJSON>(json);

            // Check if values are equal.
            Assert.AreEqual(json_success.Value, json_test.Value);
            Assert.AreNotEqual(json_failure.Value, json_test.Value);
        }

        /// <summary>
        /// JSON manipulation test.
        /// </summary>
        [Test]
        public void JSONModification()
        {
            // Construct a JSON success object.
            string json = @"{
    'Value': '" + MSG_FAILURE + @"'
}";

            // Deserialize the test object.
            TestJSON json_test = JsonConvert.DeserializeObject<TestJSON>(json);
            json_test.Value = MSG_SUCCESS;

            // Check if values are equal.
            Assert.AreEqual(json_success.Value, json_test.Value);
            Assert.AreNotEqual(json_failure.Value, json_test.Value);
        }

        /// <summary>
        /// Determine if <see cref="Newtonsoft.Json"/> converts LINQ statements correctly.
        /// </summary>
        [Test]
        public void LINQtoJSON()
        {
            JObject json_test = new JObject();
            JValue json_value = new JValue(MSG_SUCCESS);
            json_test["Value"] = json_value;

            JToken json_actual = json_test["Value"];
            JToken json_expected = new JValue(json_success.Value);

            Debug.Log($"Expected: '{json_expected}' | Actual: '{json_actual}'");
            Assert.AreEqual(json_expected.Value<string>(), json_actual.Value<string>());
        }

    }
}
