using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestGM
{

    [Test]
    public void GMInit()
    {
        GameObject go = new GameObject();
        go.AddComponent<GameManager>();
        
        Assert.IsNotNull(GameManager.Instance);
        Assert.Zero(GameManager.Instance.Timer);
        Assert.AreEqual(GameContext.Hub, GameManager.Instance.Context);
        Assert.AreEqual(GameState.Menu, GameManager.Instance.State);
    }
}
