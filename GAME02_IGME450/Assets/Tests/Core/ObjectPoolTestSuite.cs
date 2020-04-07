using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    
    /// <summary>
    /// TestSuite for testing core library functions.
    /// </summary>
    public class ObjectPoolTestSuite
    {

        /// <summary>
        /// Test GameObject.
        /// </summary>
        private GameObject testPrefab = null;

        /// <summary>
        /// Test pool reference.
        /// </summary>
        private GameObjectPool testPool = null;

        /// <summary>
        /// Collection storing test objects.
        /// </summary>
        private List<GameObject> testObjects = null;

        /// <summary>
        /// Setup the test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            testObjects = new List<GameObject>();
            testPrefab = new GameObject("Test Prefab Object");
            testPool = null;
        }

        /// <summary>
        /// TearDown the test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            foreach(GameObject go in testObjects)
            {
                GameObject.Destroy(go);
            }
            testObjects = null;

            GameObject.Destroy(testPrefab);
            testPrefab = null;

            testPool.Clear();
            testPool = null;
        }

        /// <summary>
        /// Pool size of 1 works.
        /// </summary>
        [Test]
        public void PoolOfOne()
        {
            // Create the object pool.
            testPool = new GameObjectPool(testPrefab, 1);

            // Test pool should be non-null.
            Assert.IsNotNull(testPool);
            // Test pool should contain 1 item.
            Assert.AreEqual(1, testPool.Size);
        }

        /// <summary>
        /// Pool size of 1 with expansion works.
        /// </summary>
        [Test]
        public void PoolOfOneWithExpansion()
        {
            // Create the object pool.
            testPool = new GameObjectPool(testPrefab, 1, true);

            // Test pool should be non-null.
            Assert.IsNotNull(testPool);
            // Test pool should contain 1 item.
            Assert.AreEqual(1, testPool.Size);

            // Get one object from the pool.
            testObjects.Add(testPool.GetPooledObject());
            Assert.IsNotNull(testObjects[0]);
            Assert.IsFalse(testObjects[0].activeInHierarchy);
            Assert.AreEqual(1, testPool.Size);
            testObjects[0].SetActive(true);

            // Get one object from the pool.
            testObjects.Add(testPool.GetPooledObject());
            Assert.IsNotNull(testObjects[1]);
            Assert.IsFalse(testObjects[1].activeInHierarchy);
            Assert.AreEqual(2, testPool.Size);
        }

        /// <summary>
        /// Pool size of 1 without expansion.
        /// </summary>
        [Test]
        public void PoolOfOneWithoutExpansion()
        {
            // Create the object pool.
            testPool = new GameObjectPool(testPrefab, 1, false);

            // Test pool should be non-null.
            Assert.IsNotNull(testPool);
            // Test pool should contain 1 item.
            Assert.AreEqual(1, testPool.Size);

            // Get one object from the pool.
            testObjects.Add(testPool.GetPooledObject());
            Assert.IsNotNull(testObjects[0]);
            Assert.IsFalse(testObjects[0].activeInHierarchy);
            Assert.AreEqual(1, testPool.Size);
            testObjects[0].SetActive(true);

            // Get one object from the pool.
            testObjects.Add(testPool.GetPooledObject());
            Assert.IsNull(testObjects[1]);
            Assert.AreEqual(1, testPool.Size);
        }

        /// <summary>
        /// Pool size of 0 works with expansion.
        /// </summary>
        [Test]
        public void PoolOfZeroWithExpansion()
        {
            // Create the object pool.
            testPool = new GameObjectPool(testPrefab, 0, true);

            // Test pool should be non-null.
            Assert.IsNotNull(testPool);
            // Test pool should contain no items.
            Assert.AreEqual(0, testPool.Size);

            // Get one object from the pool.
            testObjects.Add(testPool.GetPooledObject());
            Assert.IsNotNull(testObjects[0]);
            Assert.IsFalse(testObjects[0].activeInHierarchy);
            Assert.AreEqual(1, testPool.Size);
        }

        /// <summary>
        /// Pool size of 0 works without expansion.
        /// </summary>
        [Test]
        public void PoolOfZeroWithoutExpansion()
        {
            // Create the object pool.
            testPool = new GameObjectPool(testPrefab, 0, false);

            // Test pool should be non-null.
            Assert.IsNotNull(testPool);
            // Test pool should contain no items.
            Assert.AreEqual(0, testPool.Size);
            
            // Get one object from the pool.
            testObjects.Add(testPool.GetPooledObject());
            Assert.IsNull(testObjects[0]);
            Assert.AreEqual(0, testPool.Size);
        }

        /// <summary>
        /// Pool size of 10 works with expansion.
        /// </summary>
        [Test]
        public void PoolOfTenWithExpansion()
        {
            // Create the object pool.
            testPool = new GameObjectPool(testPrefab, 10, true);

            // Test pool should be non-null.
            Assert.IsNotNull(testPool);
            // Test pool should contain 10 items.
            Assert.AreEqual(10, testPool.Size);

            // Assert each item requested.
            for (int i = 0; i < 10; i++)
            {
                // Get one object from the pool.
                testObjects.Add(testPool.GetPooledObject());
                Assert.IsNotNull(testObjects[i]);
                Assert.IsFalse(testObjects[i].activeInHierarchy);
                Assert.AreEqual(10, testPool.Size);
                testObjects[i].SetActive(true);
            }

            // Get one object from the pool.
            testObjects.Add(testPool.GetPooledObject());
            Assert.IsNotNull(testObjects[10]);
            Assert.IsFalse(testObjects[10].activeInHierarchy);
            Assert.AreEqual(11, testPool.Size);
        }

        /// <summary>
        /// Pool size of 10 works without expansion.
        /// </summary>
        [Test]
        public void PoolOfTenWithoutExpansion()
        {
            // Create the object pool.
            testPool = new GameObjectPool(testPrefab, 10, false);

            // Test pool should be non-null.
            Assert.IsNotNull(testPool);
            // Test pool should contain 10 items.
            Assert.AreEqual(10, testPool.Size);

            // Assert each item requested.
            for(int i = 0; i < 10; i++)
            {
                // Get one object from the pool.
                testObjects.Add(testPool.GetPooledObject());
                Assert.IsNotNull(testObjects[i]);
                Assert.IsFalse(testObjects[i].activeInHierarchy);
                Assert.AreEqual(10, testPool.Size);
                testObjects[i].SetActive(true);
            }

            // Get one more object from the pool.
            testObjects.Add(testPool.GetPooledObject());
            Assert.IsNull(testObjects[10]);
            Assert.AreEqual(10, testPool.Size);
        }


    }

}

