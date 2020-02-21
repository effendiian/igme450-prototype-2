using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using RSG;

namespace Tests
{

    /// <summary>
    /// TestSuite for testing external library added to Unity.
    /// </summary>
    public class RSGPromisesTestSuite
    {

        /// <summary>
        /// Success message string.
        /// </summary>
        private const string MSG_SUCCESS = "SUCCESS";

        /// <summary>
        /// Empty message string.
        /// </summary>
        private const string MSG_EMPTY = "EMPTY";

        /// <summary>
        /// Message for the exception.
        /// </summary>
        private const string MSG_EXCEPTION = "This is an intentional exception!";

        /// <summary>
        /// Test <see cref="Promise"/> object. Resolves with a string.
        /// </summary>
        private Promise<string> promise;

        /// <summary>
        /// Return value from the promise.
        /// </summary>
        private string returnValue;

        [SetUp]
        public void Setup()
        {
            // Setup the promise.
            promise = new Promise<string>();
            returnValue = MSG_EMPTY;
            promise.Done(result => {
                returnValue = result;
            }, HandlePromiseException);
        }

        [TearDown]
        public void TearDown()
        {
            // Mark for GC deletion.
            promise = null;
            returnValue = null;
        }

        /// <summary>
        /// Determine if <see cref="RSG.Promises"/> resolve properly.
        /// </summary>
        [UnityTest]
        public IEnumerator PromisesResolveSuccessfully()
        {
            Debug.Log("Test - Promise.Resolve()");
            yield return new WaitForSeconds(0.1f);
            promise.Resolve(MSG_SUCCESS);
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(returnValue, MSG_SUCCESS);
        }

        /// <summary>
        /// Determine if <see cref="RSG.Promises"/> reject properly.
        /// </summary>
        [UnityTest]
        public IEnumerator PromisesRejectSuccessfully()
        {
            Debug.Log("Test - Promise.Reject()");
            yield return new WaitForSeconds(0.1f);
            promise.Reject(new System.Exception(MSG_EXCEPTION));
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(returnValue, MSG_EMPTY);
        }

        /// <summary>
        /// Handle unhandled exceptions in a promise.
        /// </summary>
        /// <param name="ex"></param>
        private void HandlePromiseException(System.Exception ex)
        {
            switch (ex.Message)
            {
                case MSG_EXCEPTION:
                    Debug.Log(ex);
                    break;
                default:
                    Debug.LogError(ex);
                    break;
            }
        }

    }
}
