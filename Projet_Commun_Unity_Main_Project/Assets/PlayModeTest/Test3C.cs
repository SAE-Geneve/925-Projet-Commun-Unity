using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Test3C
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Test3C");
    }
    
    [TearDown]
    public void TearDown()
    {
        SceneManager.UnloadScene("Test3C");
    }
    
    [UnityTest]
    public IEnumerator PlayerNotNull()
    {
        GameObject player = GameObject.Find("NewPlayer");
        Assert.IsNotNull(player);
        
        yield return null;
    }
    
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