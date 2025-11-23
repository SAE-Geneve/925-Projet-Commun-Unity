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
        GameObject player = GameObject.Find("PlayerGrab");
        Assert.IsNotNull(player);
        
        yield return null;
    }

    #region Controller

    [UnityTest]
    public IEnumerator TestGrab()
    {
        GameObject box = GameObject.Find("White");
        GameObject player = GameObject.Find("PlayerGrab");
        Controller controller = player.GetComponent<Controller>();
        
        Assert.IsNotNull(player);
        Assert.IsNotNull(box);
        
        yield return new WaitForSeconds(1f);
        
        controller.CatchStart();

        Assert.AreEqual(GameObject.Find("CatchPoint"), box.transform.parent.gameObject);
        
        controller.Drop();
        
        Assert.IsNull(box.transform.parent);
    }

    #endregion

}