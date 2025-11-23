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
        SceneManager.UnloadSceneAsync("Test3C");
    }
    
    [UnityTest]
    public IEnumerator PlayerNotNull()
    {
        GameObject player = GameObject.Find("CharacterGrab");
        Assert.IsNotNull(player);
        
        yield return null;
    }

    #region Controller

    [UnityTest]
    public IEnumerator TestGrab()
    {
        GameObject box = GameObject.Find("PropGrab");
        GameObject player = GameObject.Find("CharacterGrab");
        GameObject catchPoint = GameObject.Find("CatchPoint");
        Controller controller = player.GetComponent<Controller>();
        
        Assert.IsNotNull(box);
        Assert.IsNotNull(player);
        Assert.IsNotNull(catchPoint);
        Assert.IsNotNull(controller);
        
        yield return new WaitForSeconds(1f);
        
        controller.CatchStart();

        Assert.AreEqual(catchPoint, box.transform.parent.gameObject);
        
        controller.Drop();
        
        Assert.AreNotEqual(catchPoint, box.transform.parent.gameObject);
    }

    #endregion

}