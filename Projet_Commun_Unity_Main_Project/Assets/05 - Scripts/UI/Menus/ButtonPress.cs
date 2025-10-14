using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class ButtonPress : MonoBehaviour
{
    //Unoptimized way to load scenes but it works
    private enum SceneList
    {
        UI_MainMenu, 
        UI_OptionsMenu,
        UI_ScenePause
    };
    [Header("Scene Changes")]
    [SerializeField] SceneList sceneToLoad;
    
    //Shouldn't be universal (Like main menu does not care about ingame canvas)
    [Header("Game Canvas")]
    [SerializeField] Canvas currentCanvas=null;
    [SerializeField] Canvas newCanvas=null;
    
    //NEED TO BUILD BEFORE BEING ABLE TO USE SCENE COUNT
    /*[SerializeField] public List<String> SceneList;

    void Awake()
    {
        for (int i = 0; i<SceneManager.sceneCount; i++)
        {
            Debug.Log(SceneManager.GetSceneAt(i).name);
            SceneList.Add(SceneManager.GetSceneAt(i).name);
        }
    }*/
        
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneToLoad.ToString());
    }
    public void ChangeCanvas()
    {
        currentCanvas.enabled = false;
        newCanvas.enabled = true;
    }
}
