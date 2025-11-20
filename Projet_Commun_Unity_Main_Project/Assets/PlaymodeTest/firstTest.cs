using System;
using System.Collections;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class firstTest
{

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Lost Luggage");
    }

    [TearDown]
    public void TearDown()
    {
        SceneManager.UnloadSceneAsync("Lost Luggage");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator firstTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        var gameObject = GameObject.Find("Lost luggage");
        
        Assert.That(gameObject, Is.Not.Null);
        yield return null;
    }
}
