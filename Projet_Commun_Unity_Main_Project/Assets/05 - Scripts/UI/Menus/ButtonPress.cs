using UnityEngine;
using UnityEngine.SceneManagement;

//[ExecuteInEditMode]
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
    
    //Shouldn't be universal (Like main menu does not care about in-game canvas)
    [Header("Game Canvas")]
    [SerializeField] Canvas currentCanvas;
    [SerializeField] Canvas newCanvas;
    
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
