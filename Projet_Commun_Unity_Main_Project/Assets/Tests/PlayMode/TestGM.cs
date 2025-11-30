using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestGM
{

    [Test]
    public void Initialization()
    {
        GameObject go = new GameObject();
        var gm = go.AddComponent<GameManager>();
        
        Assert.IsNotNull(gm);
        Assert.Zero(gm.Timer);
        Assert.AreEqual(GameContext.Hub, gm.Context);
        Assert.AreEqual(GameState.Menu, gm.State);
    }

    [Test]
    public void SwitchState()
    {
        GameObject go = new GameObject();
        var gm = go.AddComponent<GameManager>();
        
        Assert.AreEqual(GameState.Menu, gm.State);
        gm.SwitchState(GameState.Playing);
        Assert.AreEqual(GameState.Playing, gm.State);
        gm.SwitchState(GameState.Playing);
        
        LogAssert.Expect($"Already in Playing state, cannot change");
    }

    [Test]
    public void Cinematic()
    {
        GameObject go = new GameObject();
        var gm = go.AddComponent<GameManager>();
        
        gm.StartCinematic();
        
        LogAssert.Expect("Can only start cinematic when the game is in playing state");
        
        gm.SwitchState(GameState.Playing);
        Assert.AreEqual(GameState.Playing, gm.State);
        gm.StartCinematic();
        Assert.AreEqual(GameState.Cinematic, gm.State);
    }

    #region Missions

    [Test]
    public void StartMission()
    {
        GameObject go = new GameObject();
        var gm = go.AddComponent<GameManager>();
        
        Mission m = new Mission();
        
        gm.StartMission(m);
        LogAssert.Expect("Mission can only be started when game is playing in the hub");
        
        gm.SwitchState(GameState.Playing);
        gm.StartMission(m);
        
        Assert.AreEqual(GameContext.Mission, gm.Context);
        Assert.AreEqual(gm.CurrentMission, m);
        
        gm.StartMission(m);
        LogAssert.Expect("Mission can only be started when game is playing in the hub");
    }

    [Test]
    public void StopMission()
    {
        GameObject go = new GameObject();
        var gm = go.AddComponent<GameManager>();
        
        Mission m = new Mission();
        
        gm.StopMission();
        LogAssert.Expect("Mission can only be stopped when game is playing in a mission");
        
        gm.SwitchState(GameState.Playing);
        
        gm.StopMission();
        LogAssert.Expect("Mission can only be stopped when game is playing in a mission");
        
        gm.StartMission(m);
        Assert.AreEqual(GameContext.Mission, gm.Context);
        gm.StopMission();
        Assert.IsNull(gm.CurrentMission);
        
        gm.StopMission();
        LogAssert.Expect("Mission can only be stopped when game is playing in a mission");
    }

    #endregion

    [Test]
    public void Pause()
    {
        GameObject go = new GameObject();
        var gm = go.AddComponent<GameManager>();
        
        gm.PauseTrigger();
        LogAssert.Expect("Can only pause when playing or cinematic");
        
        gm.SwitchState(GameState.Playing);
        Assert.AreEqual(GameState.Playing, gm.State);
        gm.PauseTrigger();
        Assert.AreEqual(GameState.Paused, gm.State);
        gm.PauseTrigger();
        Assert.AreEqual(GameState.Playing, gm.State);
        
        gm.SwitchState(GameState.Cinematic);
        Assert.AreEqual(GameState.Cinematic, gm.State);
        gm.PauseTrigger();
        Assert.AreEqual(GameState.Paused, gm.State);
        gm.PauseTrigger();
        Assert.AreEqual(GameState.Cinematic, gm.State);
    }

    [Test]
    public void Disconnection()
    {
        GameObject go = new GameObject();
        var gm = go.AddComponent<GameManager>();

        gm.OnPlayerDisconnected();
        LogAssert.Expect("Can only start the disconnection timer when the game is in playing/cinematic state");

        gm.OnPlayerReconnected();
        LogAssert.Expect("Can only reconnect when the game is in disconnected state");
        
        gm.SwitchState(GameState.Cinematic);
        gm.OnPlayerDisconnected();
        
        Assert.AreEqual(GameState.Disconnected, gm.State);
        
        gm.OnPlayerReconnected();
        
        Assert.AreEqual(GameState.Cinematic, gm.State);
    }
}
