using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Test3C
{
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        SceneManager.LoadScene("Test3C");
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        SceneManager.UnloadSceneAsync("Test3C");
    }
    
    #region Movement
    
    [UnityTest]
    public IEnumerator Move()
    {
        PlayerMovement playerMovement = GameObject.Find("CharacterMove").GetComponent<PlayerMovement>();
        DebugTriggerController debugTriggerController = GameObject.Find("TargetMove").GetComponent<DebugTriggerController>();
        
        Assert.IsNotNull(playerMovement);
        Assert.IsNotNull(debugTriggerController);
        
        yield return new WaitForSeconds(1f);
        
        playerMovement.SetMovement(Vector2.up);
        
        yield return new WaitForSeconds(2f);
        
        playerMovement.SetMovement(Vector2.zero);
        
        Assert.IsTrue(debugTriggerController.Success);
    }
    
    [UnityTest]
    public IEnumerator Dash()
    {
        PlayerMovement playerMovement = GameObject.Find("CharacterDash").GetComponent<PlayerMovement>();
        DebugTriggerController debugTriggerController = GameObject.Find("TargetDash").GetComponent<DebugTriggerController>();
        
        Assert.IsNotNull(playerMovement);
        Assert.IsNotNull(debugTriggerController);
        
        yield return new WaitForSeconds(1f);
        
        playerMovement.Dash();
        
        yield return new WaitForSeconds(2f);
        
        Assert.IsTrue(debugTriggerController.Success);
    }

    #endregion

    #region Controller

    [UnityTest]
    public IEnumerator GrabDrop()
    {
        GameObject box = GameObject.Find("PropGrab");
        GameObject player = GameObject.Find("CharacterGrab");
        GameObject catchPoint = player.transform.Find("CatchPoint").gameObject;
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
    
    [UnityTest]
    public IEnumerator GrabThrow()
    {
        GameObject box = GameObject.Find("PropThrow");
        GameObject player = GameObject.Find("CharacterThrow");
        GameObject catchPoint = player.transform.Find("CatchPoint").gameObject;
        Controller controller = player.GetComponent<Controller>();
        
        Assert.IsNotNull(box);
        Assert.IsNotNull(player);
        Assert.IsNotNull(catchPoint);
        Assert.IsNotNull(controller);
        
        yield return new WaitForSeconds(1f);
        
        controller.CatchStart();

        Assert.AreEqual(catchPoint, box.transform.parent.gameObject);
        
        controller.CatchStart();
        yield return new WaitForSeconds(0.5f);
        controller.CatchCanceled();
        
        Assert.AreNotEqual(catchPoint, box.transform.parent.gameObject);
        
        yield return new WaitForSeconds(2f);
        
        LogAssert.Expect(LogType.Log, "Task Throw done!");
    }
    
    [UnityTest]
    public IEnumerator GrabInteract()
    {
        GameObject box = GameObject.Find("PropInteract");
        GameObject player = GameObject.Find("CharacterInteract");
        GameObject catchPoint = player.transform.Find("CatchPoint").gameObject;
        PlayerController playerController = player.GetComponent<PlayerController>();
        
        Assert.IsNotNull(box);
        Assert.IsNotNull(player);
        Assert.IsNotNull(catchPoint);
        Assert.IsNotNull(playerController);
        
        yield return new WaitForSeconds(1f);
        
        playerController.CatchStart();

        Assert.AreEqual(catchPoint, box.transform.parent.gameObject);
        
        playerController.TryInteract();
        
        LogAssert.Expect(LogType.Log, "Interact with prop");
    }
    
    [UnityTest]
    public IEnumerator DestroyGrab()
    {
        GameObject box = GameObject.Find("PropDestroyGrab");
        GameObject player = GameObject.Find("CharacterDestroyGrab");
        GameObject catchPoint = player.transform.Find("CatchPoint").gameObject;
        PlayerController playerController = player.GetComponent<PlayerController>();
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        
        Assert.IsNotNull(box);
        Assert.IsNotNull(player);
        Assert.IsNotNull(catchPoint);
        Assert.IsNotNull(playerController);
        Assert.IsNotNull(playerMovement);
        
        yield return new WaitForSeconds(1f);
        
        playerController.CatchStart();

        Assert.AreEqual(catchPoint, box.transform.parent.gameObject);
        
        playerMovement.SetMovement(Vector2.up);
        
        yield return new WaitForSeconds(2f);
        
        LogAssert.Expect(LogType.Log, "Task Destroy done!");
        Assert.IsTrue(playerController.InteractableGrabbed == null);
    }

    #endregion
}