using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class ButtonPress : MonoBehaviour
{
    //Unoptimized way to load scenes but it works
    private enum SceneList
    {
        UI_Templates, 
        UI_SceneChangeTest,
        UI_SceneChangeTest2
    };
    
    [SerializeField] SceneList sceneToLoad;
    
    //NEED TO BUILD BEFORE BEING ABLE TO USE SCENE COUNT
    /*[SerializeField] public List<String> SceneList;

    void Awake()
    {
        for (int i = 0; i<SceneManager.sceneCountInBuildSettings; i++)
        {
            Debug.Log(SceneManager.GetSceneAt(i).name);
            SceneList.Add(SceneManager.GetSceneAt(i).name);
        }
    }*/
        
    public void MainMenuStart()
    {
        SceneManager.LoadScene(sceneToLoad.ToString());
    }
}
