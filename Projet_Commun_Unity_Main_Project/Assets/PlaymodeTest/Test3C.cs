using System.Collections;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Test3C
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadSceneAsync("Test3C");
    }
    
    [TearDown]
    public void TearDown()
    {
        SceneManager.UnloadSceneAsync("Test3C");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestGrab()
    {
        GameObject box = GameObject.Find("White");

        GameObject pl = GameObject.Find("NewPlayer");
        Assert.IsNotNull(pl);
        Controller c = pl.GetComponent<Controller>();
        Assert.IsNotNull(c);
        
        yield return new WaitForSeconds(1);
        c.CatchStart();

        Assert.AreEqual(GameObject.Find("CatchPoint"), box.transform.parent.gameObject);
        yield return null;
    }
}
